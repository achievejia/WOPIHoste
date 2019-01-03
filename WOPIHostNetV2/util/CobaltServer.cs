using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Cobalt;
using Newtonsoft.Json;
using NLog;
using WOPIHostNetV2.model;

namespace WOPIHostNetV2.util
{
    public class CobaltServer
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private HttpListener m_listener;
        private string _listenerURL;
        private DocumentUtil documentUtil;

        public CobaltServer(string listenerURL)
        {
            _listenerURL = listenerURL;
        }

        public void Start()
        {
            m_listener = new HttpListener();
            m_listener.Prefixes.Add($"http://{_listenerURL}/");
            m_listener.Start();
            m_listener.BeginGetContext(new AsyncCallback(ProcessRequest), m_listener);

            logger.Info($"Suboc监听服务已启动，监听地址为：http://{_listenerURL}/");
        }

        public void Stop()
        {
            m_listener.Stop();

            logger.Info($"Suboc监听服务已停止");
        }

        private void ErrorResponse(HttpListenerContext context, string errmsg)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(errmsg);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = @"application/json";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Close();
        }

        private void ProcessRequest(IAsyncResult result)
        {
            try
            {
                logger.Info($"监听中......");
                var listener = (HttpListener)result.AsyncState;
                var context = listener.EndGetContext(result);
                try
                {
                    var listStr = new List<string>();
                    foreach (var item in context.Request.Headers.AllKeys)
                    {
                        listStr.Add(context.Request.Headers[item]);
                    }
                    logger.Info($"监听内容：HTTP请求方法为【{context.Request.HttpMethod}】，HTTP请求绝对路径为【{context.Request.Url.AbsolutePath}】，HTTP请求头为【{JsonConvert.SerializeObject(context.Request.Headers.AllKeys)}】值为【{JsonConvert.SerializeObject(listStr)}】");
                    var stringarr = context.Request.Url.AbsolutePath.Split('/');
                    var access_token = context.Request.QueryString["access_token"];

                    if (stringarr.Length < 3 || access_token == null)
                    {
                        logger.Info($"请求参数无效，参数为：{stringarr}，token参数为：{access_token}");
                        ErrorResponse(context, @"无效的请求参数");
                        m_listener.BeginGetContext(new AsyncCallback(ProcessRequest), m_listener);
                        return;
                    }

                    documentUtil = new DocumentUtil();
                    //string fileId = stringarr[3];
                    var docInfo = documentUtil.GetInfo(stringarr[3]);
                    logger.Info($"文件信息为：{JsonConvert.SerializeObject(docInfo)}");
                    var filename = docInfo.fileName;

                    EditSession editSession = CobaltSessionManager.Instance.GetSession(docInfo.uuid);
                    if (editSession == null)
                    {
                        var fileExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        if (fileExt.ToLower().Contains("xlsx"))
                            editSession = new FileSession(docInfo.uuid, docInfo.filePath, docInfo.loginUser, docInfo.author, false);
                        else
                            editSession = new CobaltSession(docInfo.uuid, docInfo.filePath, docInfo.loginUser, docInfo.author, false);
                        CobaltSessionManager.Instance.AddSession(editSession);
                    }

                    if (stringarr.Length == 4 && context.Request.HttpMethod.Equals(@"GET"))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            var json = new DataContractJsonSerializer(typeof(WopiCheckFileInfo));
                            json.WriteObject(memoryStream, editSession.GetCheckFileInfo(access_token));
                            memoryStream.Flush();
                            memoryStream.Position = 0;
                            StreamReader streamReader = new StreamReader(memoryStream);
                            var jsonResponse = Encoding.UTF8.GetBytes(streamReader.ReadToEnd());

                            context.Response.AddHeader("Cache-Control", "no-cache");
                            context.Response.AddHeader("Pragma", "no-cache");
                            context.Response.AddHeader("Expires", "-1");
                            context.Response.ContentType = @"application/json";
                            context.Response.ContentLength64 = jsonResponse.Length;
                            context.Response.OutputStream.Write(jsonResponse, 0, jsonResponse.Length);
                            context.Response.OutputStream.Flush();
                            context.Response.StatusCode = (int)HttpStatusCode.OK;

                            context.Response.Close();
                        }
                    }
                    else if (stringarr.Length == 5 && stringarr[4].Equals(@"contents"))
                    {
                        if (context.Request.HttpMethod.Equals(@"POST"))
                        {
                            logger.Info($"进入contents方法，调用了保存。");

                            var ms = new MemoryStream();
                            context.Request.InputStream.CopyTo(ms);
                            editSession.Save(ms.ToArray());
                            context.Response.ContentLength64 = 0;
                            context.Response.ContentType = @"text/html";
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        }
                        else
                        {
                            var content = editSession.GetFileContent();
                            context.Response.ContentType = @"application/octet-stream";
                            context.Response.ContentLength64 = content.Length;
                            context.Response.OutputStream.Write(content, 0, content.Length);
                            context.Response.OutputStream.Flush();
                        }
                        //context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod.Equals(@"POST") &&
                        context.Request.Headers["X-WOPI-Override"].Equals("COBALT"))
                    {
                        var ms = new MemoryStream();
                        context.Request.InputStream.CopyTo(ms);
                        AtomFromByteArray atomRequest = new AtomFromByteArray(ms.ToArray());
                        RequestBatch requestBatch = new RequestBatch();

                        Object ctx;
                        ProtocolVersion protocolVersion;

                        requestBatch.DeserializeInputFromProtocol(atomRequest, out ctx, out protocolVersion);
                        editSession.ExecuteRequestBatch(requestBatch);

                        foreach (Request request in requestBatch.Requests)
                        {
                            if (request.GetType() == typeof(PutChangesRequest) &&
                                request.PartitionId == FilePartitionId.Content)
                            {

                                editSession.Save();
                                editSession.fileinfo = new FileInfo(docInfo.filePath);
                            }
                        }
                        var response = requestBatch.SerializeOutputToProtocol(protocolVersion);

                        context.Response.Headers.Add("X-WOPI-CorellationID", context.Request.Headers["X-WOPI-CorrelationID"]);
                        context.Response.Headers.Add("request-id", context.Request.Headers["X-WOPI-CorrelationID"]);
                        context.Response.ContentType = @"application/octet-stream";
                        context.Response.ContentLength64 = response.Length;
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        response.CopyTo(context.Response.OutputStream);
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod.Equals(@"POST") &&
                        (context.Request.Headers["X-WOPI-Override"].Equals("LOCK") ||
                        context.Request.Headers["X-WOPI-Override"].Equals("UNLOCK") ||
                        context.Request.Headers["X-WOPI-Override"].Equals("REFRESH_LOCK"))
                        )
                    {
                        context.Response.ContentLength64 = 0;
                        context.Response.ContentType = @"text/html";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.Close();
                    }
                    else
                    {
                        logger.Info($"请求参数无效，参数为：{stringarr}，token参数为：{access_token}");
                        ErrorResponse(context, @"无效的请求参数");
                    }
                    logger.Info($"当前请求处理完成...");
                }
                catch (Exception ex)
                {
                    logger.Error($"请求处理发生异常：{ex.Message}");
                }
                m_listener.BeginGetContext(new AsyncCallback(ProcessRequest), m_listener);
            }
            catch (Exception ex)
            {
                logger.Error($"获取请求时发生异常：{ex.Message}");
                return;
            }
        }

        public void StreamToFile(Stream stream, string fileName)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["FileSavePath"];
            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(bytes);
                }
            }
        }
    }
}
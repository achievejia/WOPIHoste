using System;
using Newtonsoft.Json;
using WOPIHostNet.model;

namespace WOPIHostNet.util
{
    public class DocumentUtil
    {
        private HttpUtil httpUtil = new HttpUtil();

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="uuid">文件Id</param>
        /// <returns></returns>
        public documentInfo GetInfo(string uuid)
        {
            try
            {
                var url = System.Configuration.ConfigurationManager.AppSettings["InfoURL"];
                var result = httpUtil.DoGet(url + $"?uuid={uuid}");
                Console.WriteLine("请求返回结果：" + result);
                var info = JsonConvert.DeserializeObject<documentInfo>(result);
                info.uuid = uuid;
                return info;
            }
            catch (Exception e)
            {
                Console.WriteLine("请求异常：" + e.Message);
                return null;
            }
            
        }
    }
}
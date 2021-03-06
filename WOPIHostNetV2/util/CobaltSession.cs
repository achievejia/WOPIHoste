﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cobalt;
using Newtonsoft.Json;
using WOPIHostNetV2.model;

namespace WOPIHostNetV2.util
{
    public class CobaltSession : EditSession
    {
        private readonly CobaltFile m_cobaltFile;
        private readonly DisposalEscrow m_disposal;

        /// <summary>
        /// 查看Session
        /// </summary>
        /// <param name="sessionId">当前session Id</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="login">当前登录用户</param>
        /// <param name="name">文件所属人（作者）</param>
        /// <param name="isAnonymous">是否匿名</param>
        public CobaltSession(string sessionId, string filePath, string login = "Anonymous", string name = "Anonymous", bool isAnonymous = true)
            : base(sessionId, filePath, login, name, isAnonymous)
        {
            m_disposal = new DisposalEscrow(m_sessionId);

            CobaltFilePartitionConfig content = new CobaltFilePartitionConfig();
            content.IsNewFile = true;
            content.HostBlobStore = new TemporaryHostBlobStore(new TemporaryHostBlobStore.Config(), m_disposal, m_sessionId + @".Content");
            content.cellSchemaIsGenericFda = true;
            content.CellStorageConfig = new CellStorageConfig();
            content.Schema = CobaltFilePartition.Schema.ShreddedCobalt;
            content.PartitionId = FilePartitionId.Content;

            CobaltFilePartitionConfig coauth = new CobaltFilePartitionConfig();
            coauth.IsNewFile = true;
            coauth.HostBlobStore = new TemporaryHostBlobStore(new TemporaryHostBlobStore.Config(), m_disposal, m_sessionId + @".CoauthMetadata");
            coauth.cellSchemaIsGenericFda = false;
            coauth.CellStorageConfig = new CellStorageConfig();
            coauth.Schema = CobaltFilePartition.Schema.ShreddedCobalt;
            coauth.PartitionId = FilePartitionId.CoauthMetadata;

            CobaltFilePartitionConfig wacupdate = new CobaltFilePartitionConfig();
            wacupdate.IsNewFile = true;
            wacupdate.HostBlobStore = new TemporaryHostBlobStore(new TemporaryHostBlobStore.Config(), m_disposal, m_sessionId + @".WordWacUpdate");
            wacupdate.cellSchemaIsGenericFda = false;
            wacupdate.CellStorageConfig = new CellStorageConfig();
            wacupdate.Schema = CobaltFilePartition.Schema.ShreddedCobalt;
            wacupdate.PartitionId = FilePartitionId.WordWacUpdate;

            Dictionary<FilePartitionId, CobaltFilePartitionConfig> partitionConfs = new Dictionary<FilePartitionId, CobaltFilePartitionConfig>();
            partitionConfs.Add(FilePartitionId.Content, content);
            partitionConfs.Add(FilePartitionId.WordWacUpdate, wacupdate);
            partitionConfs.Add(FilePartitionId.CoauthMetadata, coauth);

            m_cobaltFile = new CobaltFile(m_disposal, partitionConfs, new CobaltHostLockingStore(this), null);

            if (m_fileinfo.Exists)
            {
                //String appdata_path = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                String appdata_path = @"C:\temp";
                appdata_path = Path.Combine(appdata_path, @"WopiCobaltHost");
                logger.Info($"临时文件家路径：{appdata_path}");
                if (!Directory.Exists(appdata_path))
                    Directory.CreateDirectory(appdata_path);
                String cache_file = Path.Combine(appdata_path, m_fileinfo.Name);
                if (File.Exists(cache_file))
                    File.Delete(cache_file);
                File.Copy(m_fileinfo.FullName, cache_file, true);
                var file_atom = FileAtom.FromExisting(cache_file, m_disposal);
                Cobalt.Metrics o1;
                m_cobaltFile.GetCobaltFilePartition(FilePartitionId.Content).SetStream(RootId.Default.Value, file_atom, out o1);
                m_cobaltFile.GetCobaltFilePartition(FilePartitionId.Content).GetStream(RootId.Default.Value).Flush();
            }
        }



        override public byte[] GetFileContent()
        {
            MemoryStream ms = new MemoryStream();
            new GenericFda(m_cobaltFile.CobaltEndpoint, null).GetContentStream().CopyTo(ms);
            return ms.ToArray();
        }
        override public void Save()
        {
            lock (m_fileinfo)
            {
                using (FileStream fileStream = m_fileinfo.Open(FileMode.Truncate))
                {
                    new GenericFda(m_cobaltFile.CobaltEndpoint, null).GetContentStream().CopyTo(fileStream);
                }
            }
        }

        override public void ExecuteRequestBatch(RequestBatch requestBatch)
        {
            m_cobaltFile.CobaltEndpoint.ExecuteRequestBatch(requestBatch);
            m_lastUpdated = DateTime.Now;
        }

        override public void Dispose()
        {
            m_disposal.Dispose();
        }
        override public WopiCheckFileInfo GetCheckFileInfo(string operType)
        {
            WopiCheckFileInfo cfi = null;
            
            //当是否禁止编辑为false，是否禁止预览为ture时，此时的状态为编辑状态
            switch (converBS64(operType))
            {
                case "1":
                    cfi = new WopiCheckFileInfo();

                    cfi.BaseFileName = m_fileinfo.Name;
                    cfi.OwnerId = m_login;
                    cfi.UserFriendlyName = m_name;

                    lock (m_fileinfo)
                    {
                        if (m_fileinfo.Exists)
                        {
                            cfi.Size = m_fileinfo.Length;
                        }
                        else
                        {
                            cfi.Size = 0;
                        }
                    }

                    cfi.Version = m_fileinfo.LastWriteTimeUtc.ToString("s");

                    cfi.SupportsCoauth = true;
                    cfi.SupportsCobalt = true;
                    cfi.SupportsFolders = true;
                    cfi.SupportsLocks = true;
                    cfi.SupportsScenarioLinks = false;
                    cfi.SupportsSecureStore = false;
                    cfi.SupportsUpdate = true;
                    cfi.DisablePrint = true;
                    cfi.DisableTranslation = true;
                    cfi.DisableBrowserCachingOfUserContent = true;
                    cfi.ReadOnly = false;
                    cfi.UserCanWrite = true;
                    cfi.WebEditingDisabled = false;
                    cfi.RestrictedWebViewOnly = false;

                    logger.Info($"编辑状态");
                    break;
                case "3":
                    cfi = new WopiCheckFileInfo();

                    cfi.BaseFileName = m_fileinfo.Name;
                    cfi.OwnerId = m_login;
                    cfi.UserFriendlyName = m_name;

                    lock (m_fileinfo)
                    {
                        if (m_fileinfo.Exists)
                        {
                            cfi.Size = m_fileinfo.Length;
                        }
                        else
                        {
                            cfi.Size = 0;
                        }
                    }

                    cfi.Version = m_fileinfo.LastWriteTimeUtc.ToString("s");

                    cfi.SupportsCoauth = true;
                    cfi.SupportsCobalt = true;
                    cfi.SupportsFolders = true;
                    cfi.SupportsLocks = true;
                    cfi.SupportsScenarioLinks = false;
                    cfi.SupportsSecureStore = false;
                    cfi.SupportsUpdate = true;
                    cfi.DisablePrint = true;
                    cfi.DisableTranslation = true;
                    cfi.DisableBrowserCachingOfUserContent = true;
                    cfi.ReadOnly = false;
                    cfi.UserCanWrite = true;
                    cfi.WebEditingDisabled = false;
                    cfi.RestrictedWebViewOnly = false;

                    logger.Info($"批注状态");
                    break;
                default:
                    cfi = new WopiCheckFileInfo();

                    cfi.BaseFileName = m_fileinfo.Name;
                    cfi.OwnerId = m_login;
                    cfi.UserFriendlyName = m_name;

                    lock (m_fileinfo)
                    {
                        if (m_fileinfo.Exists)
                        {
                            cfi.Size = m_fileinfo.Length;
                        }
                        else
                        {
                            cfi.Size = 0;
                        }
                    }

                    cfi.Version = m_fileinfo.LastWriteTimeUtc.ToString("s");

                    cfi.SupportsCoauth = true;
                    cfi.SupportsCobalt = true;
                    cfi.SupportsFolders = true;
                    cfi.SupportsLocks = true;
                    cfi.SupportsScenarioLinks = false;
                    cfi.SupportsSecureStore = false;
                    cfi.SupportsUpdate = true;
                    cfi.DisablePrint = true;
                    cfi.DisableTranslation = true;
                    cfi.DisableBrowserCachingOfUserContent = true;
                    cfi.ReadOnly = true;
                    cfi.UserCanWrite = true;
                    cfi.WebEditingDisabled = true;
                    cfi.RestrictedWebViewOnly = true;

                    logger.Info($"只读状态");
                    break;
            }
            logger.Info($"该文件信息为：{JsonConvert.SerializeObject(cfi)}");
            return cfi;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            var s = Encoding.GetEncoding("UTF-8").GetString(Convert.FromBase64String(result));
            return s;
        }

        private static string converBS64(string temp)
        {
            var result = "";
            temp = temp + "==";
            switch (temp)
            {
                case "MQ==":
                    result = "1";
                    break;
                case "Mg==":
                    result = "2";
                    break;
                default:
                    result = "3";
                    break;
            }
            return result;
        }
    }
}
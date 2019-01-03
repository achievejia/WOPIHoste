using Newtonsoft.Json;
using System;
using System.IO;

namespace WOPIHostNetV2.model
{
    public class FileSession : EditSession
    {
        /// <summary>
        /// 文件session
        /// </summary>
        /// <param name="sessionId">当前session Id</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="login">当前登录用户</param>
        /// <param name="name">文件所属人（作者）</param>
        /// <param name="isAnonymous">是否匿名</param>
        public FileSession(string sessionId, string filePath, string login = "Anonymous", string name = "Anonymous", bool isAnonymous = true)
            : base(sessionId, filePath, login, name, isAnonymous)
        { }

        override public WopiCheckFileInfo GetCheckFileInfo(string operType)
        {
            WopiCheckFileInfo cfi = new WopiCheckFileInfo();

            cfi.BaseFileName = m_fileinfo.Name;
            cfi.OwnerId = m_name;
            cfi.UserFriendlyName = m_login;

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

            cfi.Version = DateTime.Now.ToString("s");
            cfi.SupportsCoauth = true;
            cfi.SupportsCobalt = false;
            cfi.SupportsFolders = true;
            cfi.SupportsLocks = true;
            cfi.SupportsScenarioLinks = false;
            cfi.SupportsSecureStore = false;
            cfi.SupportsUpdate = true;
            cfi.UserCanWrite = true;

            return cfi;

            //WopiCheckFileInfo cfi = null;

            //当是否禁止编辑为false，是否禁止预览为ture时，此时的状态为编辑状态
            //if (!mFileInfo.IsDisableEdit && mFileInfo.IsDisablePreview)
            //{
            //    cfi = new WopiCheckFileInfo();

            //    cfi.BaseFileName = m_fileinfo.Name;
            //    cfi.OwnerId = m_login;
            //    cfi.UserFriendlyName = m_name;

            //    lock (m_fileinfo)
            //    {
            //        if (m_fileinfo.Exists)
            //        {
            //            cfi.Size = m_fileinfo.Length;
            //        }
            //        else
            //        {
            //            cfi.Size = 0;
            //        }
            //    }

            //    cfi.Version = m_fileinfo.LastWriteTimeUtc.ToString("s");

            //    cfi.SupportsCoauth = true;
            //    cfi.SupportsCobalt = true;
            //    cfi.SupportsFolders = true;
            //    cfi.SupportsLocks = true;
            //    cfi.SupportsScenarioLinks = false;
            //    cfi.SupportsSecureStore = false;
            //    cfi.SupportsUpdate = true;
            //    cfi.DisablePrint = true;
            //    cfi.DisableTranslation = true;
            //    cfi.DisableBrowserCachingOfUserContent = true;
            //    cfi.ReadOnly = false;
            //    cfi.UserCanWrite = true;
            //    cfi.WebEditingDisabled = false;
            //    cfi.RestrictedWebViewOnly = false;

            //    logger.Info($"编辑状态");
            //}
            //else if (!mFileInfo.IsDisableEdit && !mFileInfo.IsDisablePreview) //当是否禁止编辑为false，是否禁止预览为false时，此时的状态为批注状态
            //{
            //    cfi = new WopiCheckFileInfo();

            //    cfi.BaseFileName = m_fileinfo.Name;
            //    cfi.OwnerId = m_login;
            //    cfi.UserFriendlyName = m_name;

            //    lock (m_fileinfo)
            //    {
            //        if (m_fileinfo.Exists)
            //        {
            //            cfi.Size = m_fileinfo.Length;
            //        }
            //        else
            //        {
            //            cfi.Size = 0;
            //        }
            //    }

            //    cfi.Version = m_fileinfo.LastWriteTimeUtc.ToString("s");

            //    cfi.SupportsCoauth = true;
            //    cfi.SupportsCobalt = true;
            //    cfi.SupportsFolders = true;
            //    cfi.SupportsLocks = true;
            //    cfi.SupportsScenarioLinks = false;
            //    cfi.SupportsSecureStore = false;
            //    cfi.SupportsUpdate = true;
            //    cfi.DisablePrint = true;
            //    cfi.DisableTranslation = true;
            //    cfi.DisableBrowserCachingOfUserContent = true;
            //    cfi.ReadOnly = false;
            //    cfi.UserCanWrite = true;
            //    cfi.WebEditingDisabled = false;
            //    cfi.RestrictedWebViewOnly = false;

            //    logger.Info($"批注状态");
            //}
            //else if (mFileInfo.IsDisableEdit && !mFileInfo.IsDisablePreview) //当是否禁止编辑为true，是否禁止预览为false时，此时的状态为只读状态
            //{
            //    cfi = new WopiCheckFileInfo();

            //    cfi.BaseFileName = m_fileinfo.Name;
            //    cfi.OwnerId = m_login;
            //    cfi.UserFriendlyName = m_name;

            //    lock (m_fileinfo)
            //    {
            //        if (m_fileinfo.Exists)
            //        {
            //            cfi.Size = m_fileinfo.Length;
            //        }
            //        else
            //        {
            //            cfi.Size = 0;
            //        }
            //    }

            //    cfi.Version = m_fileinfo.LastWriteTimeUtc.ToString("s");

            //    cfi.SupportsCoauth = true;
            //    cfi.SupportsCobalt = true;
            //    cfi.SupportsFolders = true;
            //    cfi.SupportsLocks = true;
            //    cfi.SupportsScenarioLinks = false;
            //    cfi.SupportsSecureStore = false;
            //    cfi.SupportsUpdate = true;
            //    cfi.DisablePrint = true;
            //    cfi.DisableTranslation = true;
            //    cfi.DisableBrowserCachingOfUserContent = true;
            //    cfi.ReadOnly = true;
            //    cfi.UserCanWrite = true;
            //    cfi.WebEditingDisabled = true;
            //    cfi.RestrictedWebViewOnly = true;

            //    logger.Info($"只读状态");
            //}
            //logger.Info($"该文件信息为：{JsonConvert.SerializeObject(cfi)}");
            //return cfi;
        }
        override public byte[] GetFileContent()
        {
            MemoryStream ms = new MemoryStream();
            lock (m_fileinfo)
            {
                using (FileStream fileStream = m_fileinfo.OpenRead())
                {
                    fileStream.CopyTo(ms);
                }
            }
            return ms.ToArray();
        }
        override public void Save(byte[] new_content)
        {
            lock (m_fileinfo)
            {
                using (FileStream fileStream = m_fileinfo.Open(FileMode.Truncate))
                {
                    fileStream.Write(new_content, 0, new_content.Length);
                }
            }
            m_lastUpdated = DateTime.Now;
        }
    }
}
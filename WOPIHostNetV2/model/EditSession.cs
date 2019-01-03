using System;
using System.IO;
using Cobalt;
using NLog;

namespace WOPIHostNetV2.model
{
    abstract public class EditSession
    {
        public readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected readonly string m_sessionId;
        protected readonly string m_login;
        protected readonly string m_name;
        protected readonly bool m_isAnonymous;
        protected FileInfo m_fileinfo;
        protected DateTime m_lastUpdated;
        /// <summary>
        /// 编辑Session
        /// </summary>
        /// <param name="sessionId">当前session Id</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="login">当前登录用户</param>
        /// <param name="name">文件所属人（作者）</param>
        /// <param name="isAnonymous">是否匿名</param>
        public EditSession(string sessionId, string filePath, string login, string name, bool isAnonymous)
        {
            m_sessionId = sessionId;
            m_fileinfo = new FileInfo(filePath);
            m_name = name;
            m_login = login;
            m_isAnonymous = isAnonymous;
        }

        public string SessionId
        {
            get { return m_sessionId; }
            set { }
        }

        public string Login
        {
            get { return m_login; }
            set { }
        }

        public string Name
        {
            get { return m_name; }
            set { }
        }

        public bool IsAnonymous
        {
            get { return m_isAnonymous; }
            set { }
        }

        public FileInfo fileinfo { get { return m_fileinfo; } set { m_fileinfo = value; } }

        public DateTime LastUpdated
        {
            get { return m_lastUpdated; }
            set { }
        }
       
        public long FileLength
        {
            get
            {
                return m_fileinfo.Length;
            }
        }
        abstract public WopiCheckFileInfo GetCheckFileInfo(string operType);
        abstract public byte[] GetFileContent();
        virtual public void Save(byte[] new_content) { }
        virtual public void Dispose() { }
        virtual public void Save() { }
        virtual public void ExecuteRequestBatch(RequestBatch requestBatch) { }
    }
}
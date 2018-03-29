using System;
using System.IO;
using Cobalt;

namespace WOPIHostNet.model
{
    abstract public class EditSession
    {
        protected readonly string m_sessionId;
        protected readonly string m_login;
        protected readonly string m_name;
        protected readonly string m_email;
        protected readonly bool m_isAnonymous;
        protected readonly FileInfo m_fileinfo;
        protected DateTime m_lastUpdated;
        protected string m_operType;
        public EditSession(string sessionId, string filePath, string login, string name, string email, bool isAnonymous, string operType)
        {
            m_sessionId = sessionId;
            m_fileinfo = new FileInfo(filePath);
            m_name = name;
            m_login = login;
            m_email = email;
            m_isAnonymous = isAnonymous;
            m_operType = operType;
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

        public string Email
        {
            get { return m_email; }
            set { }
        }

        public bool IsAnonymous
        {
            get { return m_isAnonymous; }
            set { }
        }

        public DateTime LastUpdated
        {
            get { return m_lastUpdated; }
            set { }
        }

        public string OperType
        {
            get { return m_operType; }
            set { }
        }
        public long FileLength
        {
            get
            {
                return m_fileinfo.Length;
            }
        }
        abstract public WopiCheckFileInfo GetCheckFileInfo();
        abstract public byte[] GetFileContent();
        virtual public void Save(byte[] new_content) { }
        virtual public void Dispose() { }
        virtual public void Save() { }
        virtual public void ExecuteRequestBatch(RequestBatch requestBatch) { }
    }
}
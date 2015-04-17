using MVVM;

namespace HiveQuery.Data
{
    public class Connection : Observer
    {
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { Set(() => Name, ref m_Name, value); }
        }

        private string m_HiveIP;

        public string HiveIP
        {
            get { return m_HiveIP; }
            set { Set(() => HiveIP, ref m_HiveIP, value); }
        }

        private int m_HivePort;

        public int HivePort
        {
            get { return m_HivePort; }
            set { Set(() => HivePort, ref m_HivePort, value); }
        }

        private string m_SolrUrl;

        public string SolrUrl
        {
            get { return m_SolrUrl; }
            set { Set(() => SolrUrl, ref m_SolrUrl, value); }
        }

        private bool m_IsLimitHive;

        public bool IsLimitHive
        {
            get { return m_IsLimitHive; }
            set { Set(() => IsLimitHive, ref m_IsLimitHive, value); }
        }

        private string m_UserName;

        public string UserName
        {
            get { return m_UserName; }
            set { Set(() => UserName, ref m_UserName, value); }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set { Set(() => Password, ref m_Password, value); }
        }
    }
}
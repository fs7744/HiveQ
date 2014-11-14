using MVVM;

namespace HiveQuery.Data
{
    public class HiveConnection : Observer
    {
        private string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { Set(() => Name, ref m_Name, value); }
        }

        private string m_IP;

        public string IP
        {
            get { return m_IP; }
            set { Set(() => IP, ref m_IP, value); }
        }

        private int m_Port;

        public int Port
        {
            get { return m_Port; }
            set { Set(() => Port, ref m_Port, value); }
        }

        private bool m_IsLimit;

        public bool IsLimit
        {
            get { return m_IsLimit; }
            set { Set(() => IsLimit, ref m_IsLimit, value); }
        }
    }
}
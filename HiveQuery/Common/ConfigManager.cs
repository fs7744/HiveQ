using HiveQuery.Data;
using Jil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;

namespace HiveQuery.Common
{
    public sealed class ConfigManager
    {
        public static readonly ConfigManager Instance = new ConfigManager();

        private string m_jsonPath;
        public Config Config { get; private set; }

        public void Init(string jsonPath)
        {
            m_jsonPath = jsonPath;
            Config = JSON.Deserialize<Config>(File.ReadAllText(m_jsonPath));
        }

        public void Save()
        {
            File.WriteAllText(m_jsonPath, JSON.Serialize(Config));
        }

        public void Save(ObservableCollection<HiveConnection> settingsConnections)
        {
            Config.Connections = settingsConnections;
            Save();
        }
    }
}

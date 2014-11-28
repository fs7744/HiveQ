using MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HiveQuery.Data
{
    public class Config : Observer
    {
        public ObservableCollection<Connection> Connections { get; set; }

        public string UsingHiveWay { get; set; }

        public List<string> OnlyShowHiveDataBase { get; set; }

        public List<string> LimitKeys { get; set; }
    }
}
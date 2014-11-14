using HiveQuery.Common;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using HiveQuery.Data;

namespace HiveQuery.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigManager.Instance.Init(string.Format("{0}app.json", AppDomain.CurrentDomain.BaseDirectory));
            Assembly thisassembly = Assembly.GetExecutingAssembly();
            using (var stream = thisassembly.GetManifestResourceStream("HiveQuery.Hive.xshd"))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    var syntaxDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    HighlightingManager.Instance.RegisterHighlighting("Hive", new string[2] { "q", "Q" }, syntaxDefinition);
                }
            }
            using (var stream = thisassembly.GetManifestResourceStream("HiveQuery.Hive.xshd"))
            {
                var doc = XDocument.Load(stream);
                HiveManager.Instance.AddKeyWords(doc.Descendants(XNamespace.Get("http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008") + "Word")
                    .Select(i => i.Value.ToUpper()).Distinct().Select(i=> new CompletionData(i)));
            }
            //ConfigManager.Instance.Config = new Data.Config();
            //ConfigManager.Instance.Config.Connections = new System.Collections.ObjectModel.ObservableCollection<Data.HiveConnection>();
            //ConfigManager.Instance.Config.Connections.Add(new Data.HiveConnection() { IP = "192.168.192.1", Name = "Gdev", Port = 99, IsLimit = true });
            //ConfigManager.Instance.Config.UsingHiveWay = "Hive";
            //ConfigManager.Instance.Save();
            base.OnStartup(e);
        }
    }
}
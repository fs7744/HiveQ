using HiveQuery.Common;
using HiveQuery.Data;
using HiveQuery.DataProvider;
using HiveQuery.FileWriter;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HiveQuery.ViewMode
{
    public class MainViewModel : MetroViewModelBase
    {
        private string m_TitleFormat = "{0} {1}";

        private bool IsSettingsChanged = false;

        private CancellationTokenSource m_Token;

        #region Command

        public ICommand SettingsCommand { get; private set; }

        public ICommand DeleteConnectionCommand { get; private set; }

        public ICommand SettingsChangedCommand { get; private set; }

        public ICommand TextChangedCommand { get; private set; }

        public ICommand OpenFileCommand { get; private set; }

        public ICommand SavedCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand RunCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public ICommand ExportCommand { get; private set; }

        #endregion Command

        #region property

        private bool m_IsOpenSettings;

        public bool IsOpenSettings
        {
            get { return m_IsOpenSettings; }
            set { Set(() => IsOpenSettings, ref m_IsOpenSettings, value); }
        }

        private ObservableCollection<Connection> m_Connections;

        public ObservableCollection<Connection> Connections
        {
            get { return m_Connections; }
            set { Set(() => Connections, ref m_Connections, value); }
        }

        private ObservableCollection<Connection> m_SettingsConnections;

        public ObservableCollection<Connection> SettingsConnections
        {
            get { return m_SettingsConnections; }
            set { Set(() => SettingsConnections, ref m_SettingsConnections, value); }
        }

        private Connection m_UsingConnection;

        public Connection UsingConnection
        {
            get { return m_UsingConnection; }
            set
            {
                if (Set(() => UsingConnection, ref m_UsingConnection, value))
                    InitDataBase(m_UsingConnection);
            }
        }

        private List<string> m_DataProviders;

        public List<string> DataProviders
        {
            get { return m_DataProviders; }
            set { Set(() => DataProviders, ref m_DataProviders, value); }
        }

        private string m_UsingDataProvider;

        public string UsingDataProvider
        {
            get { return m_UsingDataProvider; }
            set
            {
                if (Set(() => UsingDataProvider, ref m_UsingDataProvider, value))
                {
                    InitDataBase(m_UsingConnection);
                }
            }
        }

        private string m_Statement;

        public string Statement
        {
            get { return m_Statement; }
            set { Set(() => Statement, ref m_Statement, value); }
        }

        private string m_FileName;

        public string FileName
        {
            get { return m_FileName; }
            set { Set(() => FileName, ref m_FileName, value); }
        }

        private string m_FilePath;

        public string FilePath
        {
            get { return m_FilePath; }
            set { Set(() => FilePath, ref m_FilePath, value); }
        }

        private string m_TextStatement;

        public string TextStatement
        {
            get { return m_TextStatement; }
            set { Set(() => TextStatement, ref m_TextStatement, value); }
        }

        private List<DataTable> m_Result;

        public List<DataTable> Result
        {
            get { return m_Result; }
            set { Set(() => Result, ref m_Result, value); }
        }

        private bool m_IsLoading;

        public bool IsLoading
        {
            get { return m_IsLoading; }
            set { Set(() => IsLoading, ref m_IsLoading, value); }
        }

        private List<CompletionData> m_CompletionData;

        public List<CompletionData> CompletionData
        {
            get { return m_CompletionData; }
            set { Set(() => CompletionData, ref m_CompletionData, value); }
        }

        private List<Tuple<string, List<CompletionData>>> m_CompletionDataBase;

        public List<Tuple<string, List<CompletionData>>> CompletionDataBase
        {
            get { return m_CompletionDataBase; }
            set { Set(() => CompletionDataBase, ref m_CompletionDataBase, value); }
        }

        #endregion property

        #region ctor

        public MainViewModel()
        {
            SettingsCommand = new Command(ExecuteSettingsCommand);
            DeleteConnectionCommand = new Command<Connection>(ExecuteDeleteConnectionCommand);
            SettingsChangedCommand = new Command(ExecuteSettingsChangedCommand);
            TextChangedCommand = new Command(ExecuteTextChangedCommand);
            OpenFileCommand = new Command(ExecuteOpenFileCommand);
            SavedCommand = new Command(ExcuteSavedCommand);
            RunCommand = new Command(ExcuteRunCommand);
            StopCommand = new Command(ExcuteStopCommand);
            ExportCommand = new Command<string>(ExcuteExportCommand);
            DataProviders = DataProviderFactory.Instance.GetKeys().ToList();
            UsingDataProvider = DataProviders.FirstOrDefault();
            Connections = new ObservableCollection<Connection>(ConfigManager.Instance.Config.Connections);
            UsingConnection = Connections.FirstOrDefault();
            SetFileName(false);
            FileName = string.Format(m_TitleFormat, string.Empty, string.Empty);
        }

        #endregion ctor

        #region ExecuteCommand

        private void ExcuteExportCommand(string format)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = GetFileFilter(format);
            if (sfd.ShowDialog() == true)
            {
                var ex = FileWriterFactory.Write(sfd.FileName, format, Result);
                if (ex != null)
                    ShowMessageAsync("", ex.Message, MessageDialogStyle.Affirmative);
            }
        }

        private void ExcuteRunCommand()
        {
            if (UsingConnection == null)
            {
                ShowMessageAsync("Info", "Please choose one connection !", MessageDialogStyle.Affirmative);
                return;
            }

            if (!IsLoading && !string.IsNullOrWhiteSpace(TextStatement))
            {
                IsLoading = true;
                if (UsingConnection.IsLimitHive && !CheckLimit(TextStatement, ConfigManager.Instance.Config.LimitKeys))
                {
                    IsLoading = false;
                    return;
                }
                m_Token = new CancellationTokenSource();
                Task.Factory.StartNew(async () =>
                {
                    var statement = string.IsNullOrEmpty(Statement) ? TextStatement : Statement;
                    if (!string.IsNullOrEmpty(statement))
                        Result = await DataProviderFactory.Instance.Execute(UsingDataProvider, statement, UsingConnection, m_Token);
                    IsLoading = false;
                }, m_Token.Token);
            }
        }

        private void ExcuteStopCommand()
        {
            if (IsLoading)
            {
                if (m_Token != null)
                    m_Token.Cancel();
                IsLoading = false;
            }
        }

        private void ExcuteSavedCommand()
        {
            Stream stream = null;
            if (!string.IsNullOrWhiteSpace(TextStatement))
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (sfd.ShowDialog() == true)
                    {
                        FilePath = sfd.FileName;
                        stream = sfd.OpenFile();
                    }
                }
                else
                    stream = File.OpenWrite(FilePath);
                if (string.IsNullOrWhiteSpace(FilePath)) return;
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(TextStatement);
                    writer.Flush();
                }
                SetFileName(false);
            }
        }

        private void ExecuteOpenFileCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
                if (string.IsNullOrWhiteSpace(FilePath)) return;
                using (var reader = new StreamReader(ofd.OpenFile()))
                {
                    TextStatement = reader.ReadToEnd();
                }
                SetFileName(false);
            }
        }

        private void ExecuteTextChangedCommand()
        {
            SetFileName(true);
        }

        private async void ExecuteSettingsChangedCommand()
        {
            if (!IsOpenSettings && IsSettingsChanged)
            {
                var result = await ShowMessageAsync(string.Empty, "The settings changed, do you want to save ?");
                if (result == MessageDialogResult.Affirmative)
                {
                    SettingsConnections.Where(i => string.IsNullOrWhiteSpace(i.HiveIP)
                        && string.IsNullOrWhiteSpace(i.Name)).ToList()
                        .ForEach(i => SettingsConnections.Remove(i));
                    ConfigManager.Instance.Save(SettingsConnections);
                    Connections.Clear();
                    foreach (var item in SettingsConnections)
                    {
                        Connections.Add(item);
                    }
                }
            }
        }

        private void ExecuteSettingsCommand()
        {
            IsOpenSettings = !IsOpenSettings;
            if (IsOpenSettings)
            {
                IsSettingsChanged = false;
                var temp = ConfigManager.Instance.Config.Clone();
                SettingsConnections = temp.Connections;
                Helper.WaitChanged(SettingsConnections, () => IsSettingsChanged = true);
            }
        }

        private void ExecuteDeleteConnectionCommand(Connection connection)
        {
            SettingsConnections.Remove(connection);
        }

        #endregion ExecuteCommand

        #region private Method

        private void InitDataBase(Connection conn)
        {
            if (m_UsingConnection == null || string.IsNullOrWhiteSpace(UsingDataProvider)) return;
            Task.Factory.StartNew(() =>
            {
                var provider = DataProviderFactory.Instance.GetProvider(UsingDataProvider);
                if (provider is HiveDataProvider)
                {
                    var data = (provider as HiveDataProvider).GetCompletionData(UsingConnection);
                    CompletionData = data.Item1;
                    CompletionDataBase = data.Item2;
                }
            });
        }

        private string GetFileFilter(string format)
        {
            string result = string.Empty;
            switch (format)
            {
                case "CSV":
                    result = "CSV files (.csv)|*.csv";
                    break;

                case "XLS":
                    result = "XLS files (.xls)|*.xls";
                    break;

                case "XLSX":
                    result = "XLSX files (.xlsx)|*.xlsx";
                    break;
            }
            return result;
        }

        private bool CheckLimit(string HiveText, List<string> list)
        {
            if (list.IsEmpty()) return true;
            var str = HiveText.ToLower();
            var key = list.FirstOrDefault(i => str.Contains(i.ToLower()));
            if (key != null)
                ShowMessageAsync("Waring", string.Format("The {0} is limit", key), MessageDialogStyle.Affirmative);
            return key == null;
        }

        private void SetFileName(bool isNoSave)
        {
            FileName = string.Format(m_TitleFormat, isNoSave ? "*" : string.Empty, FilePath);
        }

        #endregion private Method
    }
}
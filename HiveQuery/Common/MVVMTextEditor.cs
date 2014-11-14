using HiveQuery.Data;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace HiveQuery.Common
{
    public class MVVMTextEditor : TextEditor
    {
        private CompletionWindow m_CompletionWindow;

        public MVVMTextEditor()
        {
            TextArea.SelectionChanged += TextArea_SelectionChanged;
            TextChanged += TextEditor_TextChanged;
            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();
            foldingManager = FoldingManager.Install(TextArea);
            foldingStrategy = new BraceFoldingStrategy();
        }

        private FoldingManager foldingManager;
        private AbstractFoldingStrategy foldingStrategy;

        private void foldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (foldingStrategy != null)
            {
                foldingStrategy.UpdateFoldings(foldingManager, Document);
            }
        }

        private void TextEditor_TextChanged(object sender, System.EventArgs e)
        {
            TextInfo = Text;
            var str = Document.GetText(0, TextArea.Caret.Offset);
            SetCompletion(str);
        }

        private void SetCompletion(string str)
        {
            if (m_CompletionWindow != null)
                m_CompletionWindow.Close();
            if (string.IsNullOrWhiteSpace(str)) return;
            var match = HiveQuery.Data.CompletionData.DotReg.Match(str);
            if (!match.Success) return;
            str = match.Value;
            var showDataBase = str.Contains(".");

            m_CompletionWindow = new CompletionWindow(TextArea);

            var data = !showDataBase ? GetCompletionKey(str) : GetCompletionDataBase(str);
            if (data == null) return;
            foreach (var item in data)
            {
                m_CompletionWindow.CompletionList.CompletionData.Add(item);
            }
            if (m_CompletionWindow.CompletionList.CompletionData.Count > 0)
                m_CompletionWindow.Show();
        }

        private IEnumerable<CompletionData> GetCompletionKey(string str)
        {
            return CompletionKey.IsEmpty() ? null
                : CompletionKey.Where(i => string.IsNullOrWhiteSpace(str) ? false
                    : i.LowerText.StartsWith(str.ToLower()) && i.LowerText.Length != str.Length);
        }

        private IEnumerable<CompletionData> GetCompletionDataBase(string str)
        {
            IEnumerable<CompletionData> result = null;

            if (CompletionDataBase != null && CompletionDataBase.Count > 0)
            {
                var keys = str.Split(new char[] { '.' });
                if (keys.IsEmpty()) return result;
                var name = keys[keys.Length <= 2 ? 0 : keys.Length - 2].ToLower();
                var pattern = keys.Length >= 2 ? keys[keys.Length - 1].ToLower() : string.Empty;
                var key = CompletionDataBase.FirstOrDefault(i => i.Item1.ToLower().StartsWith(name));
                if (key != null)
                {
                    result = pattern == string.Empty ? key.Item2 : key.Item2.Where(i => i.LowerText.StartsWith(pattern) && i.Text.Length != pattern.Length);
                }
            }
            return result;
        }

        private void TextArea_SelectionChanged(object sender, EventArgs e)
        {
            SelectedTextInfo = SelectedText;
        }

        public string SelectedTextInfo
        {
            get { return (string)GetValue(SelectedTextInfoProperty); }
            set { SetValue(SelectedTextInfoProperty, value); }
        }

        public static readonly DependencyProperty SelectedTextInfoProperty =
            DependencyProperty.Register("SelectedTextInfo", typeof(string), typeof(MVVMTextEditor), new PropertyMetadata(string.Empty));

        public string TextInfo
        {
            get { return (string)GetValue(TextInfoProperty); }
            set { SetValue(TextInfoProperty, value); }
        }

        public static readonly DependencyProperty TextInfoProperty =
            DependencyProperty.Register("TextInfo", typeof(string), typeof(MVVMTextEditor), new PropertyMetadata(string.Empty, (o, e) =>
             {
                 var control = o as MVVMTextEditor;
                 if (control != null && (string)e.NewValue != control.Text)
                     control.Text = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
             }));

        public List<CompletionData> CompletionKey
        {
            get { return (List<CompletionData>)GetValue(CompletionKeyProperty); }
            set { SetValue(CompletionKeyProperty, value); }
        }

        public static readonly DependencyProperty CompletionKeyProperty =
            DependencyProperty.Register("CompletionKey", typeof(List<CompletionData>), typeof(MVVMTextEditor), new PropertyMetadata(null));

        public List<Tuple<string, List<CompletionData>>> CompletionDataBase
        {
            get { return (List<Tuple<string, List<CompletionData>>>)GetValue(CompletionDataBaseProperty); }
            set { SetValue(CompletionDataBaseProperty, value); }
        }

        public static readonly DependencyProperty CompletionDataBaseProperty =
            DependencyProperty.Register("CompletionDataBase", typeof(List<Tuple<string, List<CompletionData>>>), typeof(MVVMTextEditor), new PropertyMetadata(null));
    }
}
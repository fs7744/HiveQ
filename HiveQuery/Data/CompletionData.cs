using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace HiveQuery.Data
{
    public class CompletionData : ICompletionData
    {
        public object Content
        {
            get
            {
                return Text;
            }
        }

        public object Description
        {
            get
            {
                return Text;
            }
        }

        public ImageSource Image
        {
            get
            {
                return null;
            }
        }

        public double Priority
        {
            get
            {
                return 0;
            }
        }

        public string Text
        {
            get;
            private set;
        }

        public string LowerText
        {
            get;
            private set;
        }

        public CompletionData(string text)
        {
            Text = text;
            LowerText = string.IsNullOrWhiteSpace(text) ? text : text.ToLower();
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var str = textArea.Document.GetText(0, textArea.Caret.Offset);
            if (string.IsNullOrEmpty(str)) return;
            var showDataBase = str.EndsWith(".");
            if (showDataBase) textArea.Document.Replace(completionSegment, Text);
            else
            {
                var match = Reg.Match(str);
                if (!match.Success) return;
                textArea.Document.Replace(new AnchorSegment(textArea.Document, completionSegment.Offset - match.Value.Length, match.Value.Length), this.Text);
            }
        }

        public static Regex Reg = new Regex("[_0-9A-Z-a-z]+$", RegexOptions.RightToLeft);
        public static Regex DotReg = new Regex("[_.0-9A-Z-a-z]+$", RegexOptions.RightToLeft);
    }
}
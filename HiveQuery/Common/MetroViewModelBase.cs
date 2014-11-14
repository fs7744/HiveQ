using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MVVM;
using System.Threading.Tasks;

namespace HiveQuery.Common
{
    public class MetroViewModelBase : ViewModelBase
    {
        public MetroWindow Window { get; set; }

        protected override void Cleanup()
        {
            Window = null;
        }

        public Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.AffirmativeAndNegative)
        {
            return DialogManager.ShowMessageAsync(Window, title, message, style: style);
        }
    }
}
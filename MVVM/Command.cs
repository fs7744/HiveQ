using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MVVM
{
    public class Command : ICommand
    {
        private Action m_Execute;
        private Func<bool> m_CanExecute;

        public Command(Action execute) : this(execute, null)
        {
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            m_Execute = execute;
            m_CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return m_CanExecute != null ? m_CanExecute() : true;
        }

        public void Execute(object parameter)
        {
            if (m_Execute != null)
                m_Execute();
        }
    }

    public class Command<T> : ICommand
    {
        private Action<T> m_Execute;
        private Func<T, bool> m_CanExecute;

        public Command(Action<T> execute) : this(execute, null)
        {
        }

        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            m_Execute = execute;
            m_CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return m_CanExecute != null ? m_CanExecute((T)parameter) : true;
        }

        public void Execute(object parameter)
        {
            if (m_Execute != null && parameter is T)
                m_Execute((T)parameter);
        }
    }

    public class EventCommand : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(EventCommand), new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventCommand), new PropertyMetadata(null));

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }

            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty);
            }

            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        protected override void Invoke(object parameter)
        {
            var command = Command;
            var param = CommandParameter != null ? CommandParameter : parameter;
            if (command != null && command.CanExecute(param))
                command.Execute(param);
        }
    }
}
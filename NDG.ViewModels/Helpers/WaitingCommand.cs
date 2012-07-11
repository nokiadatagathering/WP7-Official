using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NDG.ViewModels.Helpers
{

    public class WaitingCommand : ICommand
    {
        private Action execute;
        public event EventHandler CanExecuteChanged;
        private Func<bool> canExecute;
        private bool isExecutedNow = false;

        public bool IsExecutedNow
        {
            get { return this.isExecutedNow; }
            set { this.isExecutedNow = value; this.RaiseCanExecuteChanged(); }
        }

        public WaitingCommand(Action execute)
            : this(execute, null)
        {
        }

        public WaitingCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            bool result = !this.IsExecutedNow;
            if (this.canExecute != null)
            {
                result = result && this.canExecute();
            }
            
            return result;
        }

        public void Execute(object parameter)
        {
            this.IsExecutedNow = true;
            if (this.execute != null)
            {
                this.execute();
            }
        }
    }

    public class WaitingCommand<T> : ICommand
    {
        private Action<T> execute;
        public event EventHandler CanExecuteChanged;
        private Predicate<T> canExecute;
        private bool isExecutedNow = false;

        public bool IsExecutedNow
        {
            get { return this.isExecutedNow; }
            set { this.isExecutedNow = value; this.RaiseCanExecuteChanged(); }
        }

        public WaitingCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public WaitingCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            bool result = !this.IsExecutedNow;
            if (this.canExecute != null)
            {
                result = result && this.canExecute((T)parameter);
            }

            return result;
        }

        public void Execute(object parameter)
        {
            if (this.execute != null && !this.IsExecutedNow)
            {
                this.IsExecutedNow = true;
                this.execute((T)parameter);
            }
        }
    }
}

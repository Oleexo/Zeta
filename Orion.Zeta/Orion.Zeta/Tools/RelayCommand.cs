using System;
using System.Windows.Input;

namespace Orion.Zeta.Tools
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Action<object> methodToExecute;
        private Func<bool> canExecuteEvaluator;
        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
            : this(obj => methodToExecute(), canExecuteEvaluator)
        {
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public RelayCommand(Action<object> methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public RelayCommand(Action<object> methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                bool result = this.canExecuteEvaluator.Invoke();
                return result;
            }
        }
        public void Execute(object parameter)
        {
            this.methodToExecute.Invoke(parameter);
        }
    }
}
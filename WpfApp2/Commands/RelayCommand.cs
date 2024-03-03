using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        private Func<Task<string>> getAllDataFromJsonApi;
        private Action check;
        private Func<object?, Task<bool>> check1;
        private Action getAllDataFromJsonApi1;
        private Func<object?, Task<bool>> check2;
        private Action getAllDataFromJsonApi2;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Func<Task<string>> getAllDataFromJsonApi, Action check)
        {
            this.getAllDataFromJsonApi = getAllDataFromJsonApi;
            this.check = check;
        }

        public RelayCommand(Func<Task<string>> getAllDataFromJsonApi, Func<object?, Task<bool>> check1)
        {
            this.getAllDataFromJsonApi = getAllDataFromJsonApi;
            this.check1 = check1;
        }

        public RelayCommand(Action getAllDataFromJsonApi1, Func<object?, Task<bool>> check2)
        {
            this.getAllDataFromJsonApi1 = getAllDataFromJsonApi1;
            this.check2 = check2;
        }

        public RelayCommand(Action getAllDataFromJsonApi2)
        {
            this.getAllDataFromJsonApi2 = getAllDataFromJsonApi2;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter)
            => _execute(parameter);
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MultipleChoiceGUI.Commands
{
    internal class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action _execute;
        private readonly Func<object, bool> _canExecute;

        public Command(Action execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public Command(Action execute) : this(execute, (object obj) => true)
        {
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}

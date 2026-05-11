using MultipleChoiceGUI.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        protected Action CloseAction { get; init; }
        public ICommand CloseCommand { get; init; }
        public WindowViewModel(Action closeAction)
        {
            CloseAction = closeAction;
            CloseCommand = new Command(CloseAction);
        }
    }
}

using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
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
        public WindowViewModel(IActionableWindow actionableWindow)
        {
            CloseAction = actionableWindow.CloseAction;
            CloseCommand = new Command(CloseAction);
        }
    }
}

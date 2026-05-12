using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.NewTopic
{
    public class NewTopicViewModel : BaseViewModel
    {
        Manager _manager;
        IActionableWindow _actionableWindow;
        public String TopicName
        {
            get => Get<String>();
            set => Set(value);
        }

        public ICommand AddNewTopic { get; init; }
        public ICommand CloseCommand { get; init; }

        public NewTopicViewModel(Manager manager, IActionableWindow window)
        {
            AddNewTopic = new Command(OnAddNewTopic);
            CloseCommand = new Command(window.CloseAction);
            _manager = manager;
            _actionableWindow = window;
        }

        public void OnAddNewTopic()
        {
            if (!String.IsNullOrWhiteSpace(TopicName))
            {
                int id = _manager.InsertTopic(TopicName);
                MessageBox.Show("Toevoegen succesvol", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                _actionableWindow.CloseAction();
            }
            else
            {
                MessageBox.Show("Geef een naam in a.u.b.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

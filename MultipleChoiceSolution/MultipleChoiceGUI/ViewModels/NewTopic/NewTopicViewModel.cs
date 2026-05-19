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
    public class NewTopicViewModel : WindowViewModel
    {
        Manager _manager;
        public String TopicName
        {
            get => Get<String>();
            set => Set(value);
        }

        public ICommand AddNewTopic { get; init; }

        public NewTopicViewModel(Manager manager, IActionableWindow window) : base(window)
        {
            AddNewTopic = new Command(OnAddNewTopic);
            _manager = manager;
        }

        public void OnAddNewTopic()
        {
            List<string> errors = _manager.InsertTopic(TopicName);
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join('\n',errors), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Toevoegen succesvol", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction();
            }
        }
    }
}

using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
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
        public String TopicName
        {
            get => Get<String>();
            set => Set(value);
        }

        public ICommand AddNewTopic { get; init; }

        public NewTopicViewModel(Manager manager)
        {
            AddNewTopic = new Command(OnAddNewTopic);
            _manager = manager;
        }

        public void OnAddNewTopic()
        {
            if (!String.IsNullOrWhiteSpace(TopicName))
            {
                _manager.InsertTopic(TopicName);
            }
            else
            {
                MessageBox.Show("Geef een naam in a.u.b.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

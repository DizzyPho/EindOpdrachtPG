using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.NewQuiz
{
    public class NewQuizViewModel : WindowViewModel
    {
        Manager _manager;
        public ICommand GenerateQuizCommand { get; init; }
        public NewQuizViewModel(Manager manager, IActionableWindow window) : base(window)
        {
            _manager = manager;
            GenerateQuizCommand = new Command(OnGenerateQuiz);

            TopicList = _manager.GetNonEmptyTopics()
                                .Select(t =>  new TopicQuestionsAmountViewModel(t))
                                .ToList();
        }

        public string Name 
        {
            get => Get<String>();
            set => Set(value);
        }
        public List<TopicQuestionsAmountViewModel> TopicList { get; set; }

        private void OnGenerateQuiz()
        {
            Dictionary<Topic, int> questionAmountByTopic = TopicList.Where(t => t.IsChecked)
                                                                    .Select(t => new KeyValuePair<Topic, int>(t.Topic, t.Count))
                                                                    .ToDictionary();
          
            List<string> errors = _manager.GenerateQuiz(questionAmountByTopic, Name);
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join('\n', errors), "Er liep iets mis.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CloseAction();
            }
        }
    }
}

using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.NewQuiz
{
    public class NewQuizViewModel : BaseViewModel
    {
        IActionableWindow _actionableWindow;
        Manager _manager;
        public ICommand GenerateQuizCommand { get; init; }
        public ICommand CloseCommand { get; init; }
        public NewQuizViewModel(Manager manager, IActionableWindow window)
        {
            _manager = manager;
            _actionableWindow = window;
            GenerateQuizCommand = new Command(OnGenerateQuiz);
            CloseCommand = new Command(window.CloseAction);

            TopicList = _manager.GetTopics()
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
          
            _manager.GenerateQuiz(questionAmountByTopic, Name);
            _actionableWindow.CloseAction();
        }
    }
}

using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.NewQuiz
{
    public class NewQuizViewModel : BaseViewModel
    {
        Manager _manager;
        public ICommand GenerateQuizCommand { get; init; }
        public NewQuizViewModel(Manager manager)
        {
            _manager = manager;
            GenerateQuizCommand = new Command(OnGenerateQuiz);
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
        }
    }
}

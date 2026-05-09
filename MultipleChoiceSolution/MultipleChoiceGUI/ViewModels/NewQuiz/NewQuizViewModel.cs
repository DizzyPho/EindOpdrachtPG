using MultipleChoiceBL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.NewQuiz
{
    public class NewQuizViewModel : BaseViewModel
    {
        Manager _manager;
        public NewQuizViewModel(Manager manager)
        {
            _manager = manager;
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
    }
}

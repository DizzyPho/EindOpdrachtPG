using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.ViewModels.ImportQuestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.NewQuestion
{
    public class NewQuestionViewModel : BaseViewModel
    {
        Manager _manager;
        public NewQuestionViewModel(Manager manager)
        {
            _manager = manager;

            TopicList = _manager.GetTopics()
                                .Select(topic => new TopicViewModel(topic))
                                .ToList();
        }
        public String QuestionText
        {
            get => Get<String>(); set => Set(value);
        }
        public List<TopicViewModel> TopicList
        {
            get => Get<List<TopicViewModel>>();
            set => Set(value);
        }
    }
}

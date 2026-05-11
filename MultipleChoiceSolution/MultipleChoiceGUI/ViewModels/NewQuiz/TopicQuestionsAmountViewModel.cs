using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.NewQuiz
{
    public class TopicQuestionsAmountViewModel : BaseViewModel
    {
        public TopicQuestionsAmountViewModel(Topic topic)
        {
            Name = topic.Name;
            IsChecked = false;
            Topic = topic;
        }

        public Topic Topic { get; init; }

        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }

        public string Name { get; set; }
        public int Count 
        {
            get => Get<int>();
            set
            {
                Set(value);
                IsChecked = value > 0;
            }
        }
    }
}

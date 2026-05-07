using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class TopicViewModel : BaseViewModel
    {
        public Topic Topic
        {
            get => Get<Topic>(); 
            set => Set(value);
        }
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }
        public TopicViewModel(Topic topic)
        {
            Topic = topic;
            IsChecked = false;
        }
    }
}

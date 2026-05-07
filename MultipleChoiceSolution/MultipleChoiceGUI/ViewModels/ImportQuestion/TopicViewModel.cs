using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class TopicViewModel : BaseViewModel
    {
        public TopicDTO Topic
        {
            get => Get<TopicDTO>(); 
            set => Set(value);
        }
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }
        public TopicViewModel(TopicDTO topic)
        {
            Topic = topic;
            IsChecked = false;
        }
    }
}

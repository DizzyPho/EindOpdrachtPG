using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class ImportQuestionViewModel : BaseViewModel
    {
        public ImportQuestionViewModel(List<TopicDTO> topicList)
        {
            TopicList = topicList;
        }
        public List<TopicDTO> TopicList
        {
            get => Get<List<TopicDTO>>();
            set => Set(value);
        }

        public String FilePath
        {
            get => Get<String>();
            set => Set(value);
        }
    }
}

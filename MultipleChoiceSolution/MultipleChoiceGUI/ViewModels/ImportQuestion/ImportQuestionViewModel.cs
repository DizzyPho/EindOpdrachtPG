using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class ImportQuestionViewModel : BaseViewModel
    {
        Manager _manager;
        public ImportQuestionViewModel(Manager manager)
        {
            _manager = manager;
            TopicList = _manager.GetTopics();
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

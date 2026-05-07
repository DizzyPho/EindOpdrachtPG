using Microsoft.Win32;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class ImportQuestionViewModel : BaseViewModel
    {
        Manager _manager;
        public ImportQuestionViewModel(Manager manager)
        {
            _manager = manager;
            TopicList = _manager.GetTopics();
            SelectFileCommand = new Command(OnSelectFile);
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

        public ICommand SelectFileCommand { get; init; }

        public void OnSelectFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.ShowDialog();
            FilePath = ofd.FileName;
        }
    }
}

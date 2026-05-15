using Microsoft.Win32;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Config;
using MultipleChoiceGUI.Interfaces;
using MultipleChoiceUtil.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class ImportQuestionViewModel : WindowViewModel
    {
        Manager _manager;
        ImportManager _importManager;
        MessageManager _messageManager;
        public ImportQuestionViewModel(Manager manager, MessageManager messageManager, IActionableWindow window) : base(window)
        {
            _manager = manager;
            _messageManager = messageManager;
            
            TopicList = _manager.GetTopics()
                                .Select(topic => new TopicViewModel(topic))
                                .ToList();
            SelectFileCommand = new Command(OnSelectFile);
            ImportQuestionsCommand = new Command(OnImportQuestions);
            FormatOptions = new List<FormatViewModel>
            {
                new FormatViewModel("Oplossingssleutel aan einde van bestand", "CorrectionAtEnd"),
                new FormatViewModel("Oplossing na vraag", "CorrectionAfterQuestion")
            };
        }
        public List<TopicViewModel> TopicList
        {
            get => Get<List<TopicViewModel>>();
            set => Set(value);
        }
        public List<FormatViewModel> FormatOptions { get; set; } 

        public String FilePath
        {
            get => Get<String>();
            set => Set(value);
        }

        public ICommand SelectFileCommand { get; init; }
        public ICommand ImportQuestionsCommand { get; init; }
        public void OnSelectFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.ShowDialog();
            FilePath = ofd.FileName;
        }

        public void OnImportQuestions()
        {
            string fileFormat = FormatOptions.Single(f => f.IsChecked).Option;
            _importManager = new ImportManager(RepoFactory.Create(ConfigurationService.GetConnectionString("SQLServerConnection"),
                                                                  _messageManager,
                                                                  ConfigurationService.GetSetting("databaseType")),
                                                                  FileReaderFactory.Create(fileFormat));

            List<int> topicIds = TopicList.Where(t => t.IsChecked).Select(t => t.Topic.Id).ToList();
            _importManager.ImportQuestions(FilePath, topicIds);

            MessageBox.Show("Import succesvol", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseAction();
        }
    }
}

using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Config;
using MultipleChoiceGUI.ViewModels;
using MultipleChoiceGUI.ViewModels.QuestionInfo;
using MultipleChoiceUtil.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultipleChoiceGUI.Windows
{
    /// <summary>
    /// Interaction logic for QuestionsWindow.xaml
    /// </summary>
    public partial class QuestionsWindow : Window
    {
        Manager _manager;
        ObservableCollection<TopicDTO> _topics;
        QuestionInfoViewModel _viewModel;
        public QuestionsWindow()
        {
            InitializeComponent();
            _manager = new Manager(RepoFactory.Create(ConfigurationService.GetConnectionString("SQLServerConnection"),
                                                      ConfigurationService.GetSetting("databaseType")));
            _topics = new ObservableCollection<TopicDTO>(_manager.GetTopics());
            ComboBoxTopics.ItemsSource = _topics;

            _viewModel = new QuestionInfoViewModel(_manager);
            DataContext = _viewModel;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}

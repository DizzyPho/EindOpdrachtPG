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
        ObservableCollection<QuestionDTO> _questions;
        QuestionInfoViewModel _viewModel;
        public QuestionsWindow()
        {
            InitializeComponent();
            _manager = new Manager(RepoFactory.Create(ConfigurationService.GetConnectionString("SQLServerConnection"),
                                                      ConfigurationService.GetSetting("databaseType")));
            _topics = new ObservableCollection<TopicDTO>(_manager.GetTopics());
            ComboBoxTopics.ItemsSource = _topics;
            _questions = new ObservableCollection<QuestionDTO>();
            ListBoxQuestions.ItemsSource = _questions;

            _viewModel = new QuestionInfoViewModel();
            DataContext = _viewModel;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBoxTopics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ComboBoxTopics.SelectedItem;

            if(selectedItem == null)
            {
                return;
            }
            TopicDTO selectedTopic = (TopicDTO)selectedItem;
            _questions = new ObservableCollection<QuestionDTO>(_manager.GetQuestionDTOs(selectedTopic.Id));
            ListBoxQuestions.ItemsSource = _questions;
        }

        private void ListBoxQuestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is not ListBox)
            {
                return;
            }
            QuestionDTO selected = (QuestionDTO)ListBoxQuestions.SelectedItem;
            Question question = _manager.GetQuestion(selected.Id);
            _viewModel.QuestionText = question.QuestionText;
            ListBoxAnswers.ItemsSource = question.GetAnswers();
        }
    }
}

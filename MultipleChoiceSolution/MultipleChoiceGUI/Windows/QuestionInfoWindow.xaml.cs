using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Config;
using MultipleChoiceGUI.Interfaces;
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
    public partial class QuestionsWindow : Window, IActionableWindow
    {
        Manager _manager;
        QuestionInfoViewModel _viewModel;
        public QuestionsWindow(Manager manager)
        {
            InitializeComponent();
            _manager = manager;

            _viewModel = new QuestionInfoViewModel(_manager, this);
            DataContext = _viewModel;
        }
        public void CloseAction()
        {
            Close();
        }

        private void NewTopic_Click(object sender, RoutedEventArgs e)
        {
            NewTopicWindow ntw = new NewTopicWindow(_manager);
            ntw.ShowDialog();
        }

        private void ImportQuestions_Click(object sender, RoutedEventArgs e)
        {
            ImportQuestionsWindow iqw = new ImportQuestionsWindow(_manager);
            iqw.ShowDialog();
        }
        private void NewQuestion_Click(object sender, RoutedEventArgs e)
        {
            NewQuestionWindow nqw = new NewQuestionWindow(_manager);
            nqw.ShowDialog();
        }
    }
}

using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Interfaces;
using MultipleChoiceGUI.ViewModels.QuizInfo;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for QuizInfoWindow.xaml
    /// </summary>
    public partial class QuizInfoWindow : Window, IActionableWindow
    {
        Manager _manager;
        QuizInfoViewModel _viewModel;
        public QuizInfoWindow(Manager manager, MessageManager messageManager)
        {
            InitializeComponent();
            _manager = manager;
            _viewModel = new QuizInfoViewModel(_manager, messageManager);
            DataContext = _viewModel;
        }

        public void CloseAction()
        {
            Close();
        }

        private void NewQuiz_Click(object sender, RoutedEventArgs e)
        {
            NewQuizWindow newQuizWindow = new NewQuizWindow(_manager);
            newQuizWindow.Show();
        }

        private void SolveQuiz_Click(object sender, RoutedEventArgs e)
        {
            SolveQuizWindow sqw = new SolveQuizWindow(_viewModel.SelectedQuiz.Id, _manager);
            sqw.ShowDialog();
        }

        private void BulkInput_Click(object sender, RoutedEventArgs e)
        {
            BulkAnswerWindow baw = new BulkAnswerWindow(_viewModel.SelectedQuiz.Id, _manager);
            baw.ShowDialog();
        }
    }
}

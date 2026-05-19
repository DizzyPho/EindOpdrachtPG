using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Interfaces;
using MultipleChoiceGUI.ViewModels.SolveQuiz;
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
    /// Interaction logic for SolveQuizWindow.xaml
    /// </summary>
    public partial class SolveQuizWindow : Window, IActionableWindow
    {
        public SolveQuizWindow(int quizId, Manager manager)
        {
            InitializeComponent();
            DataContext = new QuizViewModel(quizId, manager, this);
        }
        public SolveQuizWindow(AnswerSetDTO answerSet, Manager manager)
        {
            InitializeComponent();
            DataContext = new QuizViewModel(answerSet, manager, this);
        }
        public void CloseAction()
        {
            Close();
        }
    }
}

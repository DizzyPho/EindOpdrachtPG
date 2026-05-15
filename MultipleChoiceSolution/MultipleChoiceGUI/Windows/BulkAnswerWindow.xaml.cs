using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Interfaces;
using MultipleChoiceGUI.ViewModels.BulkAnswer;
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
    /// Interaction logic for BulkAnswerWindow.xaml
    /// </summary>
    public partial class BulkAnswerWindow : Window, IActionableWindow
    {
        public BulkAnswerWindow(int quizId, Manager manager)
        {
            InitializeComponent();
            DataContext = new BulkAnswerViewModel(quizId, manager, this); 
        }

        public void CloseAction()
        {
            Close();
        }
    }
}

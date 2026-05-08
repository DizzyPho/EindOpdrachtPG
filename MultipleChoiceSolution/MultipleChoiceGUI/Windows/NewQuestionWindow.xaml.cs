using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.ViewModels.NewQuestion;
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
    /// Interaction logic for NewQuestionWindow.xaml
    /// </summary>
    public partial class NewQuestionWindow : Window
    {
        public NewQuestionWindow(Manager manager)
        {
            InitializeComponent();
            DataContext = new NewQuestionViewModel(manager);
        }
    }
}

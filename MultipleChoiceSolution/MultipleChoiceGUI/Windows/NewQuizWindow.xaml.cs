using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.ViewModels.NewQuiz;
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
    /// Interaction logic for NewQuizWindow.xaml
    /// </summary>
    public partial class NewQuizWindow : Window
    {
        public NewQuizWindow(Manager manager)
        {
            InitializeComponent();
            DataContext = new NewQuizViewModel(manager, () => this.Close());
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

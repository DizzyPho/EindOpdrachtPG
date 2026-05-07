using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.ViewModels.ImportQuestion;
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
    /// Interaction logic for ImportQuestionsWindow.xaml
    /// </summary>
    public partial class ImportQuestionsWindow : Window
    {
        public ImportQuestionsWindow(Manager manager)
        {
            InitializeComponent();
            DataContext = new ImportQuestionViewModel(manager);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

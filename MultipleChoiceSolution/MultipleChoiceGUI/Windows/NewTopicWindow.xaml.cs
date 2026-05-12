using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Interfaces;
using MultipleChoiceGUI.ViewModels.NewTopic;
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
    /// Interaction logic for NewTopicWindow.xaml
    /// </summary>
    public partial class NewTopicWindow : Window, IActionableWindow
    {
        public NewTopicWindow(Manager manager)
        {
            InitializeComponent();
            DataContext = new NewTopicViewModel(manager, this);
        }

        public void CloseAction()
        {
            Close();
        }
    }
}

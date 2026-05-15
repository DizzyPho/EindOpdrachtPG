using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Config;
using MultipleChoiceGUI.Windows;
using MultipleChoiceUtil.Factories;
using System.Configuration;
using System.Text;
using System.Windows;

namespace MultipleChoiceGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Manager _manager;
        MessageManager _messageManager;
        IQuizFileWriter _fileWriter;
        public MainWindow()
        {
            InitializeComponent();          

            _manager = new Manager(RepoFactory.Create(ConfigurationService.GetConnectionString("SQLServerConnection"),
                                          ConfigurationService.GetSetting("databaseType")));
            _messageManager = new MessageManager();
            _fileWriter = FileWriterFactory.Create(); 

        }

        private void ButtonQuestions_Click(object sender, RoutedEventArgs e)
        {
            QuestionsWindow qw = new QuestionsWindow(_manager, _messageManager);
            qw.ShowDialog();
        }

        private void ButtonQuiz_Click(object sender, RoutedEventArgs e)
        {
            QuizInfoWindow qiw = new QuizInfoWindow(_manager, _messageManager, _fileWriter);
            qiw.ShowDialog();
        }
    }
}
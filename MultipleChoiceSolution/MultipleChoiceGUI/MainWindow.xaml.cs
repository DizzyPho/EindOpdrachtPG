using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Interfaces;
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
        public MainWindow()
        {
            InitializeComponent();
            
            IQuizRepository quizRepository = RepoFactory.Create(ConfigurationService.GetConnectionString("SQLServerConnection"),
                                                                ConfigurationService.GetSetting("databaseType"));
            IFileReader reader = FileReaderFactory.Create("CorrectionAtEnd");
            //List<Question> q = reader.Read("./Data/Muziek80s.txt");
            //quizRepository.ImportQuestions(q, [1,2,3]);

        }

        private void ButtonQuestions_Click(object sender, RoutedEventArgs e)
        {
            QuestionsWindow qw = new QuestionsWindow();
            qw.ShowDialog();
        }

        private void ButtonQuiz_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
using Microsoft.Win32;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceBL.Messages;
using MultipleChoiceGUI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.QuizInfo
{
    public class QuizInfoViewModel : BaseViewModel
    {
        Manager _manager;
        public QuizInfoViewModel(Manager manager, MessageManager messageManager)
        {
            _manager = manager;
            Quizzes = new ObservableCollection<QuizDTO>(_manager.GetQuizDTOs());
            ExportCommand = new Command(OnExport);

            if (Quizzes.Count > 0)
            {
                SelectedQuiz = Quizzes.First();
            }

            messageManager.Register<NewQuizMessage>(this, (o, message) => Quizzes.Add(message.QuizDTO));
            messageManager.Register<NewResultMessage>(this, (o, message) => ProcessResultMessage(message));
        }

        public ObservableCollection<QuizDTO> Quizzes
        {
            get => Get<ObservableCollection<QuizDTO>>();
            set => Set(value);
        }

        public ObservableCollection<ResultViewModel> Results
        {
            get => Get<ObservableCollection<ResultViewModel>>();
            set => Set(value);
        }

        public QuizDTO SelectedQuiz
        {
            get => Get<QuizDTO>(); 
            set 
            {
                Set(value);
                Results = GetQuizResults(value);
            }
        }
        public ResultViewModel SelectedUserResult
        {
            get => Get<ResultViewModel>();
            set => Set(value);
        }
        public ICommand ExportCommand { get; init; }
        public void OnExport()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if(saveFileDialog.ShowDialog() == true)
            {
                _manager.SaveQuiz(_manager.GetQuiz(SelectedQuiz.Id), saveFileDialog.FileName);
                MessageBox.Show("Export gelukt!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public ObservableCollection<ResultViewModel> GetQuizResults(QuizDTO quiz)
        {
            var results = _manager.GetResultDTOs(quiz.Id).Select(r => new ResultViewModel(r, quiz.QuestionCount));
            return new ObservableCollection<ResultViewModel>(results);
        }

        public void ProcessResultMessage(NewResultMessage message)
        {
            if(message.QuizId == SelectedQuiz.Id)
            {
                Results.Add(new ResultViewModel(message.Result, SelectedQuiz.QuestionCount));
            }
        }
    }
}

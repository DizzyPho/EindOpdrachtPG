using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceBL.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuizInfo
{
    public class QuizInfoViewModel : BaseViewModel
    {
        Manager _manager;
        public QuizInfoViewModel(Manager manager, MessageManager messageManager)
        {
            _manager = manager;
            Quizzes = new ObservableCollection<QuizDTO>(_manager.GetQuizDTOs());
            if (Quizzes.Count > 0)
            {
                SelectedQuiz = Quizzes.First();
            }

            messageManager.Register<NewQuizMessage>(this, (o, message) => Quizzes.Add(message.QuizDTO));
        }

        public ObservableCollection<QuizDTO> Quizzes
        {
            get => Get<ObservableCollection<QuizDTO>>();
            set => Set(value);
        }

        public QuizDTO SelectedQuiz
        {
            get => Get<QuizDTO>(); 
            set => Set(value);
        }
    }
}

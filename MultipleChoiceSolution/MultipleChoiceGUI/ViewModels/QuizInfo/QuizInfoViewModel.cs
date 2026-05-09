using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuizInfo
{
    public class QuizInfoViewModel : BaseViewModel
    {
        Manager _manager;
        public QuizInfoViewModel(Manager manager)
        {
            _manager = manager;
            quizzes = new ObservableCollection<QuizDTO>(_manager.GetQuizDTOs());
            if (quizzes.Count > 0)
            {
                SelectedQuiz = quizzes.First();
            }
        }

        public ObservableCollection<QuizDTO> quizzes
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

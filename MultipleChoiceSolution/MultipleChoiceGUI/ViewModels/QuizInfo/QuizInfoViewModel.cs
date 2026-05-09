using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuizInfo
{
    public class QuizInfoViewModel : BaseViewModel
    {
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

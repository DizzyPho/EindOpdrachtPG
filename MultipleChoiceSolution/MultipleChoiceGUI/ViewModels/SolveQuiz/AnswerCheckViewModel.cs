using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.SolveQuiz
{
    public class AnswerCheckViewModel : BaseViewModel
    {
        public AnswerCheckViewModel(Answer answer)
        {
            AnswerText = answer.AnswerText;
            IsCorrect = answer.IsCorrect;
            IsChecked = false;
        }

        public String AnswerText { get; init; }
        public bool IsCorrect { get; init; }
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }
    }
}

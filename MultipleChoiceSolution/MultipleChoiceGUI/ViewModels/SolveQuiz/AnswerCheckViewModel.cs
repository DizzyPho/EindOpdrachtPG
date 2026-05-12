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
            Id = answer.Id;
            AnswerText = answer.AnswerText;
            IsCorrect = answer.IsCorrect;
            IsChecked = false;
        }
        public int? Id { get; init; }
        public String AnswerText { get; init; }
        public bool IsCorrect { get; init; }
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }
    }
}

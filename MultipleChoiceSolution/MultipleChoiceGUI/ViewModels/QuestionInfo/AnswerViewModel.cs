using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuestionInfo
{
    public class AnswerViewModel : BaseViewModel
    {
        public String Text
        {
            get => Get<string>();
            set => Set(value);
        }

        public char Mark
        {
            get => Get<char>();
            set => Set(value);
        }

        public AnswerViewModel(Answer answer)
        {
            Mark = answer.IsCorrect ? '\u2713' : ' ';
            Text = answer.AnswerText;
        }
    }
}

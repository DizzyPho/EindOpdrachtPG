using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace MultipleChoiceGUI.ViewModels.SolveQuiz
{
    public class AnswerCheckViewModel : BaseViewModel
    {
        public AnswerCheckViewModel(Answer answer)
        {
            Id = (int)answer.Id;
            AnswerText = answer.AnswerText;
            IsCorrect = answer.IsCorrect;
            IsChecked = false;
            TextColour = new SolidColorBrush(Colors.Black);
        }
        public int Id { get; init; }
        public String AnswerText { get; init; }
        public bool IsCorrect { get; init; }
        public SolidColorBrush TextColour
        {
            get => Get<SolidColorBrush>();
            set => Set(value);
        }
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }

        public void SetRed()
        {
            TextColour = new SolidColorBrush(Colors.Red);
        }
        public void SetGreen()
        {
            TextColour = new SolidColorBrush(Colors.Green);
        }
    }
}

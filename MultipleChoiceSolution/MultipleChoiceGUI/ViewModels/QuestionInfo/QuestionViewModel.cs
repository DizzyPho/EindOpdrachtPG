using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuestionInfo
{
    public class QuestionViewModel : BaseViewModel
    {
        public QuestionViewModel(QuestionDTO questionDTO)
        {
            Id = questionDTO.Id;
            QuestionText = questionDTO.Question;
            IsEnabled = questionDTO.IsEnabled;
        }
        public int Id { get; init; }
        public String QuestionText
        {
            get => Get<string>();
            set => Set(value);
        }
        public bool IsEnabled
        {
            get => Get<bool>();
            set => Set(value);
        }
    }
}

using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuestionInfo
{
    public class QuestionViewModel : BaseViewModel
    {
        Manager _manager;
        bool isInitPhase = true;
        public QuestionViewModel(QuestionDTO questionDTO, Manager manager)
        {
            _manager = manager;
            Id = questionDTO.Id;
            QuestionText = questionDTO.Question;
            IsEnabled = questionDTO.IsEnabled;
            isInitPhase = false;
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
            set
            {
                if(!isInitPhase) 
                    _manager.SetQuestionEnabled(Id, value);
                Set(value);
            }
        }
    }
}

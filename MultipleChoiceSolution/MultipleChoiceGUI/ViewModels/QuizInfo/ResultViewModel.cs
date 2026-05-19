using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.QuizInfo
{
    public class ResultViewModel : BaseViewModel
    {
        public ResultViewModel(ResultDTO resultDTO, int questionCount)
        {
            UserId = resultDTO.UserId;
            Score = $"{resultDTO.Score} / {questionCount}";
        }

        public int UserId
        {
            get => Get<int>();
            set => Set(value);
        }
        public string Score
        {
            get => Get<string>();
            set => Set(value);
        }
    }
}

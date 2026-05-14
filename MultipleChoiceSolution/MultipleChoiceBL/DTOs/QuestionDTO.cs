using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public record struct QuestionDTO
    {
        public QuestionDTO(int id, string question, bool isEnabled)
        {
            Id = id;
            Question = question;
            IsEnabled = isEnabled;
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsEnabled { get; set; }
        public override string? ToString()
        {
            return Question;
        }
    }
}

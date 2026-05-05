using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public record struct QuestionDTO
    {
        public QuestionDTO(int id, string question)
        {
            Id = id;
            Question = question;
        }

        public int Id { get; set; }
        public string Question { get; set; }

    }
}

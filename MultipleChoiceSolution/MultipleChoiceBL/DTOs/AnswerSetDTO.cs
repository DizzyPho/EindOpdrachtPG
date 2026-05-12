using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public class AnswerSetDTO
    {
        public AnswerSetDTO(int quizId, int userId, Dictionary<int, List<int>> questionAnswers)
        {
            UserId = userId;
            QuestionAnswers = questionAnswers;
        }

        public int UserId { get; set; }
        public Dictionary<int, List<int>> QuestionAnswers { get; set; }
    }
}

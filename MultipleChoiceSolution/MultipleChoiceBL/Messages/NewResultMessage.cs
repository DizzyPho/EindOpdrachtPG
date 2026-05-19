using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Messages
{
    public class NewResultMessage
    {
        public NewResultMessage(AnswerSetDTO answerSet)
        {
            QuizId = answerSet.QuizId;
            Result = new ResultDTO(answerSet.UserId, answerSet.Score);
        }

        public int QuizId { get; init; }
        public ResultDTO Result { get; init; }
    }
}

using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Messages
{
    public class NewQuizMessage
    {
        public NewQuizMessage(QuizDTO quizDTO)
        {
            QuizDTO = quizDTO;
        }

        public QuizDTO QuizDTO { get; init; }
    }
}

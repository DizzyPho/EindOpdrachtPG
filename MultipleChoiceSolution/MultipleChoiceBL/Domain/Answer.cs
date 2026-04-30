using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class Answer
    {
        private Answer(string answerText, bool isCorrect)
        {
            AnswerText = answerText;
            IsCorrect = isCorrect;
        }

        public static bool TryCreate(string answerText, bool isCorrect, out FactoryResult<Answer> result)
        {
            List<String> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(answerText)) errors.Add("Answer cannot be empty.");

            if (errors.Count > 0)
            {
                result = new FactoryResult<Answer>(errors);
                return false;
            }
            else
            {
                result = new FactoryResult<Answer>(new Answer(answerText, isCorrect));
                return false;
            }
        }
        public string AnswerText { get; init; }
        public bool IsCorrect { get; init; }
    }
}

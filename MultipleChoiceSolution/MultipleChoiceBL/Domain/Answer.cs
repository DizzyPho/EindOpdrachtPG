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
        private Answer(int? id, string answerText, bool isCorrect) : this(answerText, isCorrect)
        {
            Id = id;
        }

        public static bool TryCreate(string answerText, bool isCorrect, out FactoryResult<Answer> result)
        {
            return TryCreate(null, answerText, isCorrect, out result);
        }

        public static bool TryCreate(int? id, string answerText, bool isCorrect, out FactoryResult<Answer> result)
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
                result = new FactoryResult<Answer>(new Answer(id, answerText, isCorrect));
                return true;
            }
        }

        public int? Id { get; init; }
        public string AnswerText { get; init; }
        public bool IsCorrect { get; private set; }

        public void SetCorrect()
        {
            IsCorrect = true;
        }

        public void SetIncorrect()
        {
            IsCorrect = false;
        }

        public override string? ToString()
        {
            return AnswerText;
        }
    }
}

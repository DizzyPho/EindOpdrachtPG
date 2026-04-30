using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class Question
    {
        private Question(string questionText, List<Answer> answers)
        {
            QuestionText = questionText;
            Answers = answers;
        }
        public static bool TryCreate(string questionText, List<Answer> answers, out FactoryResult<Question> result)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(questionText)) errors.Add("Question text cannot be empty.");
            if (answers == null || answers.Count < 2)
            {
                errors.Add("Question must have at least 2 answers.");
            }
            else
            {
                if (!answers.Any(a => a.IsCorrect)) errors.Add("Please provide at least 1 correct answer.");
            }

            if (errors.Count > 0)
            {
                result = new FactoryResult<Question>(errors);
                return false;
            }
            else
            {
                result = new FactoryResult<Question>(new Question(questionText, answers));
                return true;
            }
        }

        public string QuestionText { get; init; }

        private List<Answer> Answers { get; init; }


        public IReadOnlyList<Answer> GetAnswers()
        {
            return Answers;
        }
    }
}

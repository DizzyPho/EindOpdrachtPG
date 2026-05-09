using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class Question
    {
        internal Question(string questionText, List<Answer> answers, int? id) : this(questionText, answers)
        {
            Id = id;
        }
        internal Question(string questionText, List<Answer> answers)
        {
            QuestionText = questionText;
            Answers = answers;
        }
        public static bool TryCreate(string questionText, List<Answer> answers, out FactoryResult<Question> result)
        {
            return TryCreate(questionText, answers, null, out result);
        }

        public static bool TryCreate(string questionText, List<Answer> answers, int? id,out FactoryResult<Question> result)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(questionText)) errors.Add("Question text cannot be empty.");
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
                result = new FactoryResult<Question>(new Question(questionText, answers, id));
                return true;
            }
        }
        public int? Id { get; init; }

        public string QuestionText { get; init; }

        private List<Answer> Answers { get; init; }


        public IReadOnlyList<Answer> GetAnswers()
        {
            return Answers;
        }

        public override bool Equals(object? obj)
        {
            return obj is Question question &&
                   Id == question.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
        // fisher yates shuffle, https://stackoverflow.com/questions/273313/randomize-a-listt
        // take parameter random to allow setting seed
        public void ShuffleAnswers(Random random)
        {
            int n = Answers.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Answer value = Answers[k];
                Answers[k] = Answers[n];
                Answers[n] = value;
            }
        }
    }
}

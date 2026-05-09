using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class Quiz
    {
        private Quiz(string name, DateTime creationDate, int seed)
        {
            Name = name;
            CreationDate = creationDate;
            Questions = new List<Question>();
            Seed = seed;
        }

        public static bool TryCreate(string name, DateTime creationDate, int seed, List<Question> questions, out FactoryResult<Quiz> result)
        {
            List<String> errors = new List<String>();

            if (string.IsNullOrWhiteSpace(name)) errors.Add("Name cannot be empty.");
            if (creationDate > DateTime.Now) errors.Add($"Invalid date '{creationDate}', cannot be in the future.");
            if (questions.Count == 0 && questions == null) errors.Add("Add at least 1 question to the quiz.");

            if (errors.Count > 0)
            {
                result = new FactoryResult<Quiz>(errors);
                return false;
            }
            else
            {
                result = new FactoryResult<Quiz>(new Quiz(name, creationDate, seed));
                return true;
            }
        }
        public String Name { get; init; }

        public DateTime CreationDate { get; init; }
        public int Seed { get; init; }
        public List<Question> Questions { get; init; }

    }
}

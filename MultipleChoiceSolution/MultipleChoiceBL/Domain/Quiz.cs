using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class Quiz
    {
        private Quiz(int? id, string name, int seed, List<Question> questions) : this(name, seed, questions)
        {
            Id = id;
        }
        private Quiz(string name, int seed, List<Question> questions)
        {
            Name = name;
            Questions = questions;
            Seed = seed;
        }
        public static bool TryCreate(int? id, string name, int seed, List<Question> questions, out FactoryResult<Quiz> result)
        {
            List<String> errors = new List<String>();

            if (string.IsNullOrWhiteSpace(name)) errors.Add("Name cannot be empty.");
            if (questions.Count == 0 && questions == null) errors.Add("Add at least 1 question to the quiz.");

            if (errors.Count > 0)
            {
                result = new FactoryResult<Quiz>(errors);
                return false;
            }
            else
            {
                result = new FactoryResult<Quiz>(new Quiz(id, name, seed, questions));
                return true;
            }
        }
        public static bool TryCreate(string name, int seed, List<Question> questions, out FactoryResult<Quiz> result)
        {
            return TryCreate(null, name, seed, questions, out result);
        }
        public String Name { get; init; }
        public int Seed { get; init; }
        public List<Question> Questions { get; init; }

        public int? Id { get; init; }

    }
}

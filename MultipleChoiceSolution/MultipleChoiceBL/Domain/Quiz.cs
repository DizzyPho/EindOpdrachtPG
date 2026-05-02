using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class Quiz
    {
        private Quiz(string name, DateTime creationDate)
        {
            Name = name;
            CreationDate = creationDate;
            Questions = new HashSet<Question>();
        }

        public static bool TryCreate(string name, DateTime creationDate, out FactoryResult<Quiz> result)
        {
            List<String> errors = new List<String>();

            if (string.IsNullOrWhiteSpace(name)) errors.Add("Name cannot be empty.");
            if (creationDate > DateTime.Now) errors.Add($"Invalid date '{creationDate}', cannot be in the future.");

            if (errors.Count > 0)
            {
                result = new FactoryResult<Quiz>(errors);
                return false;
            }
            else
            {
                result = new FactoryResult<Quiz>(new Quiz(name, creationDate));
                return true;
            }
        }
        public String Name { get; init; }

        public DateTime CreationDate { get; init; }
        public HashSet<Question> Questions { get; init; }

        public bool AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);
            return Questions.Add(question);
        }

    }
}

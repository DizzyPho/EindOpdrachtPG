using MultipleChoiceBL.Interfaces;
using MultipleChoiceDL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceUtil.Factories
{
    public static class RepoFactory
    {
        public static IQuizRepository Create(string connectionString, string type)
        {
            IQuizRepository repo = type switch
            {
                "SQLS" => new QuizRepositorySQLS(connectionString),
                _ => throw new ArgumentException("Invalid DB type")
            };
            return repo;
        }
    }
}

using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceDL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceUtil.Factories
{
    public static class RepoFactory
    {
        public static IQuizRepository Create(string connectionString, MessageManager messageManager, string type)
        {
            IQuizRepository repo = type switch
            {
                "SQLS" => new QuizRepositorySQLS(connectionString, messageManager),
                _ => throw new ArgumentException("Invalid DB type")
            };
            return repo;
        }
    }
}

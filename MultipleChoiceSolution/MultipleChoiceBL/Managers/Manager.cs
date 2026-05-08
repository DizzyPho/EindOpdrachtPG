using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Managers
{
    public class Manager
    {
        private IQuizRepository _repository;
        public Manager(IQuizRepository repository) 
        {
            _repository = repository;
        }

        public Question GetQuestion(int questionId)
        {
            return _repository.GetQuestion(questionId);
        }

        public List<QuestionDTO> GetQuestionDTOs(int topicId)
        {
            return _repository.GetQuestionDTOs(topicId);
        }

        public List<Topic> GetTopics()
        {
            return _repository.GetTopics();
        }

        public void InsertQuestion(Question question, List<int> topicIds)
        {
            _repository.ImportQuestions(new List<Question> { question }, topicIds);
        }

        public int InsertTopic(string topicName)
        {
            return _repository.InsertTopic(topicName);
        }
    }
}

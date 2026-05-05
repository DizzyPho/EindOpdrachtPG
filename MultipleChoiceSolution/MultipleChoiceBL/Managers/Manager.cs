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

        public List<QuestionDTO> GetQuestionDTOs(int topicId)
        {
            return _repository.GetQuestionDTOs(topicId);
        }

        public List<TopicDTO> GetTopics()
        {
            return _repository.GetTopics();
        }
    }
}

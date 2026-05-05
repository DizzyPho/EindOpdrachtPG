using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Interfaces
{
    public interface IQuizRepository
    {
        public List<TopicDTO> GetTopics();
        public void ImportQuestions(List<Question> questions, List<int> topicIds);
    }
}

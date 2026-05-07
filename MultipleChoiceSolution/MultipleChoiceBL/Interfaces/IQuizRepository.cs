using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Interfaces
{
    public interface IQuizRepository
    {
        Question GetQuestion(int questionId);
        List<QuestionDTO> GetQuestionDTOs(int topicId);
        public List<TopicDTO> GetTopics();
        public void ImportQuestions(List<Question> questions, List<int> topicIds);
        public int InsertTopic(string topicName);
    }
}

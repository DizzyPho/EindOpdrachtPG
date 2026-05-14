using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Interfaces
{
    public interface IQuizRepository
    {
        public Quiz GetQuiz(int id);
        Question GetQuestion(int questionId);
        List<QuestionDTO> GetQuestionDTOs(int topicId);
        public List<Topic> GetTopics();
        public void ImportQuestions(List<Question> questions, List<int> topicIds);
        public int InsertTopic(string topicName);

        public Dictionary<int, List<int>> GetQuestionIdsByTopic();
        public List<Question> GetQuestions(List<int> questionIds);

        public int InsertQuiz(Quiz quiz);
        public List<QuizDTO> GetQuizDTOs();
        public void SubmitAnswerSets(List<AnswerSetDTO> answerSets);
    }
}

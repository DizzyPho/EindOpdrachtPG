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
        public List<QuestionDTO> ImportQuestions(List<Question> questions, List<int> topicIds);
        public Topic InsertTopic(string topicName);

        public Dictionary<int, List<int>> GetQuestionIdsByTopic();
        public List<Question> GetQuestions(List<int> questionIds);

        public int InsertQuiz(Quiz quiz);
        public List<QuizDTO> GetQuizDTOs();
        public void SubmitAnswerSets(List<AnswerSetDTO> answerSets);
        public void InsertUsersIfNotExists(IEnumerable<int> ids);
        public List<string> GetQuestionTopics(int questionId);
        public void SetQuestionEnabled(int id, bool isEnabled);
        public List<ResultDTO> GetResultDTOs(int quizId);
        public AnswerSetDTO GetAnswerSet(int userId, int quizId);
    }
}

using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Managers
{
    public class Manager
    {
        private IQuizRepository _repository;
        private IQuizFileWriter _fileWriter;
        private MessageManager _messageManager;
        public Manager(IQuizRepository repository, IQuizFileWriter fileWriter) 
        {
            _repository = repository;
            _fileWriter = fileWriter;
            _messageManager = new MessageManager();
        }

        public void SaveQuiz(Quiz quiz, string path)
        {
            _fileWriter.SaveQuiz(quiz, path);
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

        public List<QuizDTO> GetQuizDTOs()
        {
            return _repository.GetQuizDTOs();
        }

        public void InsertQuestion(Question question, List<int> topicIds)
        {
            _repository.ImportQuestions(new List<Question> { question }, topicIds);
        }

        public void InsertTopic(string topicName)
        {
            int id = _repository.InsertTopic(topicName);
            Topic topic = new Topic(id, topicName);
            _messageManager.Send<NewTopicMessage>(new NewTopicMessage(topic));
        }

        public Quiz GenerateQuiz(Dictionary<Topic, int> questionAmounts, string quizName)
        {
            QuizGenerator generator = new QuizGenerator(_repository);
            Quiz quiz = generator.GenerateQuiz(questionAmounts, quizName);

            int id = _repository.InsertQuiz(quiz);
            
            //QuizDTO dto = new QuizDTO(id, quiz.Name, quiz.Questions.Count, _repository.GetQuestionTopics(id));
            //_messageManager.Send<NewQuizMessage>(new NewQuizMessage(dto));


            return quiz;
        }

        public Quiz GetQuiz(int id)
        {
            return _repository.GetQuiz(id);
        }

        public void SubmitAnswers(AnswerSetDTO answerSet)
        {
            _repository.InsertUsersIfNotExists([answerSet.UserId]);
            _repository.SubmitAnswerSets([answerSet]);
        }
        public void SubmitAnswerSets(List<AnswerSetDTO> answerSets)
        {
            foreach (AnswerSetDTO answerSet in answerSets)
            {
                
            }
            _repository.InsertUsersIfNotExists(answerSets.Select(a => a.UserId));
            _repository.SubmitAnswerSets(answerSets);
        }

        public void SetQuestionEnabled(int id, bool isEnabled)
        {
            _repository.SetQuestionEnabled(id, isEnabled);
        }
    }
}

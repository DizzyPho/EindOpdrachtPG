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
        public List<Topic> GetNonEmptyTopics()
        {
            return _repository.GetNonEmptyTopics();
        }

        public List<QuizDTO> GetQuizDTOs()
        {
            return _repository.GetQuizDTOs();
        }

        public void InsertQuestion(Question question, List<int> topicIds)
        {
            List<QuestionDTO> questionDTOs = _repository.ImportQuestions(new List<Question> { question }, topicIds);
            _messageManager.Send<QuestionsImportedMessage>(new QuestionsImportedMessage(topicIds, questionDTOs));
        }
        // return true if succesfull
        public List<string> InsertTopic(string topicName)
        {
            List<String> errors = new List<string>();
            if (String.IsNullOrWhiteSpace(topicName)) errors.Add("Geef een geldige naam.");
            if (errors.Count > 0) return errors;

            Topic topic = _repository.InsertTopic(topicName);
            if(topic != null)
            {
                _messageManager.Send<NewTopicMessage>(new NewTopicMessage(topic));
            }
            else
            {
                errors.Add("Er liep iets mis. Bestaat dit onderwerp al?");
            }
            return errors;
        }

        public List<string> GenerateQuiz(Dictionary<Topic, int> questionAmounts, string quizName)
        {
            List<string> errors = new List<string>();
            if (questionAmounts == null || questionAmounts.Keys.Count < 1) errors.Add("Selecteer minstens een onderwerp"); 
            if (string.IsNullOrWhiteSpace(quizName)) errors.Add("Geef een geldige naam."); 


            if(errors.Count == 0) 
            {
                QuizGenerator generator = new QuizGenerator(_repository);
                Quiz quiz = generator.GenerateQuiz(questionAmounts, quizName);

                int id = _repository.InsertQuiz(quiz);
            
                QuizDTO dto = _repository.GetQuizDTO(id);
                _messageManager.Send<NewQuizMessage>(new NewQuizMessage(dto));
            }

            return errors;
        }

        public Quiz GetQuiz(int id)
        {
            return _repository.GetQuiz(id);
        }

        public void SubmitAnswers(AnswerSetDTO answerSet)
        {
            _repository.InsertUsersIfNotExists([answerSet.UserId]);
            _repository.SubmitAnswerSets([answerSet]);
            _messageManager.Send<NewResultMessage>(new NewResultMessage(answerSet));
        }
        public void SubmitAnswerSets(List<AnswerSetDTO> answerSets)
        {
            _repository.InsertUsersIfNotExists(answerSets.Select(a => a.UserId));
            _repository.SubmitAnswerSets(answerSets);
            foreach (AnswerSetDTO answerSet in answerSets)
            {
                _messageManager.Send<NewResultMessage>(new NewResultMessage(answerSet));
            }
        }

        public void SetQuestionEnabled(int id, bool isEnabled)
        {
            _repository.SetQuestionEnabled(id, isEnabled);
        }

        public List<ResultDTO> GetResultDTOs(int quizId)
        {
            return _repository.GetResultDTOs(quizId);
        }
        public AnswerSetDTO GetAnswerSet(int userId, int quizId)
        {
            return _repository.GetAnswerSet(userId, quizId);
        }
    }
}

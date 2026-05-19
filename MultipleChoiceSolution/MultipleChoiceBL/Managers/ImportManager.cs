using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Managers
{
    public class ImportManager
    {
        private IQuizRepository _repository;
        private IFileReader _reader;
        private MessageManager _messageManager;

        public ImportManager(IQuizRepository repository, IFileReader reader, MessageManager messageManager)
        {
            _repository = repository;
            _reader = reader;
            _messageManager = messageManager;
        }

        public List<string> ImportQuestions(string path, List<int> topicIds)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(path)) errors.Add("Selecteer een geldig bestand");
            if (topicIds.Count == 0) errors.Add("Selecteer minstens een onderwerp");

            if (errors.Count == 0)
            {
                List<Question> questions = _reader.Read(path);
                List<QuestionDTO> questionDTOs = _repository.ImportQuestions(questions, topicIds);
                _messageManager.Send<QuestionsImportedMessage>(new QuestionsImportedMessage(topicIds, questionDTOs));
            }
            return errors;
        }
    }
}

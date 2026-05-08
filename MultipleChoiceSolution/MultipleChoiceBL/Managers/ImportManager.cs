using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Managers
{
    public class ImportManager
    {
        private IQuizRepository _repository;
        private IFileReader _reader;

        public ImportManager(IQuizRepository repository, IFileReader reader)
        {
            _repository = repository;
            _reader = reader;
        }

        public void ImportQuestions(string path, List<int> topicIds)
        {
            List<Question> questions = _reader.Read(path);
            _repository.ImportQuestions(questions, topicIds);
        }
    }
}

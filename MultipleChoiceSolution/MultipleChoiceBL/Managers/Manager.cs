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


    }
}

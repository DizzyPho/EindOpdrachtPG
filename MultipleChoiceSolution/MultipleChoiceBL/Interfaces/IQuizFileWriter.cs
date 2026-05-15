using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Interfaces
{
    public interface IQuizFileWriter
    {
        public void SaveQuiz(Quiz quiz, string path);
    }
}

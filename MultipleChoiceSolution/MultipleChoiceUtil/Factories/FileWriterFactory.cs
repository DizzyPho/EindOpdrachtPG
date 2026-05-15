using MultipleChoiceBL.Interfaces;
using MultipleChoiceDL.FileWriters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceUtil.Factories
{
    public static class FileWriterFactory
    {
        public static IQuizFileWriter Create()
        {
            return new QuizFileWriterTXT();
        }
    }
}

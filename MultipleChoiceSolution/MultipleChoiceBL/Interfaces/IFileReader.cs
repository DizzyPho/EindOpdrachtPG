using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Interfaces
{
    public interface IFileReader
    {
        public List<Question> Read(string path);
    }
}

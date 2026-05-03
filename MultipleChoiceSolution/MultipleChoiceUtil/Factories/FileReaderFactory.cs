using MultipleChoiceBL.Interfaces;
using MultipleChoiceDL.FileReaders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceUtil.Factories
{
    public static class FileReaderFactory
    {
        public static IFileReader Create(string format)
        {
            IFileReader reader = format switch
            {
                "CorrectionAtEnd" => new FileReaderCorrectionAtEnd(),
                _ => throw new ArgumentException("Invalid FileReader format")
            };
            return reader;
        }
    }
}

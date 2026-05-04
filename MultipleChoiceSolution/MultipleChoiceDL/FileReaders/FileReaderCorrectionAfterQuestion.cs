using MultipleChoiceBL.Builders;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MultipleChoiceDL.FileReaders
{
    public class FileReaderCorrectionAfterQuestion : IFileReader
    {
        public List<Question> Read(string path)
        {

            bool readingQuestion = false;

            int currentLine = 1;
            List<Question> result = new List<Question>();

            using (StreamReader sr = new StreamReader(path))
            {
                string questionText = string.Empty;
                QuestionBuilder builder = new QuestionBuilder();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        readingQuestion = false;
                        continue;
                    }
                    else if (readingQuestion)
                    {
                        questionText += " " + line;
                    }
                    else if (Regex.IsMatch(line, @"^\d*\."))
                    {
                        questionText += line.Split(". ")[1];
                        readingQuestion = true;
                    }
                    else if (Regex.IsMatch(line, @"^[a-zA-Z]*\."))
                    {
                        string[] fields = line.Split(". ");
                        if (Answer.TryCreate(fields[1], false, out FactoryResult<Answer> factoryResult))
                        {
                            builder.AddAnswer(fields[0].ToUpper()[0], factoryResult.Result);
                        }
                    }
                    else if (Regex.IsMatch(line, @"^Correct:"))
                    {
                        char[] correctAnswers = line.Split(": ")[1].ToCharArray();
                        builder.SetCorrectAnswers(correctAnswers)
                               .SetQuestionText(questionText);

                        result.Add(builder.Build());
                        builder = new QuestionBuilder();
                    }
                    currentLine++;
                }
            }
            return result;
        }
    }
}

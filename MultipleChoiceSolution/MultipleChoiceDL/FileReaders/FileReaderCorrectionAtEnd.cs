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
    // for question files where answer sheet is attached at bottom of file
    public class FileReaderCorrectionAtEnd : IFileReader
    {
        public List<Question> Read(string path)
        {

            bool readingQuestion = false;
            bool readingAnswers = false;
            bool readingCorrections = false;

            int currentLine = 1;
            int currentCorrection = 0;
            List<QuestionBuilder> builders = new List<QuestionBuilder>();
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
                        if(readingAnswers)
                        {
                            builder.SetQuestionText(questionText);
                            builders.Add(builder);

                            builder = new QuestionBuilder();
                            questionText = string.Empty;
                        }
                        readingQuestion = false;
                        readingAnswers = false;
                        continue;
                    }
                    else if (readingCorrections)
                    {
                        builders[currentCorrection].SetCorrectAnswers(line.ToUpper().ToCharArray());
                        currentCorrection++;
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
                    else if (Regex.IsMatch(line, @"^[a-zA-Z]*\.") || readingAnswers)
                    {
                        string[] fields = line.Split(". ");
                        if (Answer.TryCreate(fields[1], false, out FactoryResult<Answer> factoryResult))
                        {
                            builder.AddAnswer(fields[0].ToUpper()[0], factoryResult.Result);
                        }
                        readingAnswers = true;
                    }
                    else if (line.ToLower().StartsWith("antwoorden"))
                    {
                        readingCorrections = true;
                    }

                    currentLine++;
                }
            }

            foreach (QuestionBuilder builder in builders)
            {
                result.Add(builder.Build());
            }

            return result;
        }
    }
}

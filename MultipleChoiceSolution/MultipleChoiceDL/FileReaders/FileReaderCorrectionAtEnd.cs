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
                List<Answer> currentQuestionAnswers = new List<Answer>();
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
                            currentQuestionAnswers = new List<Answer>();
                            questionText = string.Empty;
                        }
                        readingQuestion = false;
                        readingAnswers = false;
                        continue;
                    }
                    else if (readingCorrections)
                    {
                        foreach(char letter in line.ToCharArray())
                        {
                            builders[currentCorrection].SetCorrectAnswer(letter);
                        }
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
                        if (Answer.TryCreate(fields[1], false, out FactoryResult<Answer> factoryResult)) currentQuestionAnswers.Add(factoryResult.Result);
                        builder.AddAnswer(fields[0].ToUpper()[0], factoryResult.Result);
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

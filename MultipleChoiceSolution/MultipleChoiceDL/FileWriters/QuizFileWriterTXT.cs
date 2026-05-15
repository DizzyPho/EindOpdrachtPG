using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceDL.FileWriters
{
    public class QuizFileWriterTXT : IQuizFileWriter
    {
        public void SaveQuiz(Quiz quiz, string path)
        {

            int questionNumber = 1;
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(quiz.Name);
                sw.WriteLine();

                foreach (Question question in quiz.Questions)
                {
                    sw.WriteLine($"{questionNumber}. {question.QuestionText}");
                    sw.WriteLine();

                    char letter = 'A';
                    foreach(Answer answer in question.GetAnswers())
                    {
                        sw.WriteLine($"{letter}. {answer.AnswerText}");
                        letter++;
                    }

                    questionNumber++;
                }
            }
        }
    }
}

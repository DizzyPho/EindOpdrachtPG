using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public class AnswerSetDTO
    {
        public AnswerSetDTO(int userId, List<int> answerIds)
        {
            UserId = userId;
            AnswerIds = answerIds;
        }

        public int UserId { get; set; }
        public List<int> AnswerIds { get; set; }

        public static AnswerSetDTO StringToAnswerSet(Quiz quiz, string text)
        {
            string[] fields = text.Split(',');
            int.TryParse(fields[0], out int userId);
            string letters = fields[1];

            List<Question> questions = quiz.Questions;
            List<int> answerIds = new List<int>();

            for (int i = 0; i < letters.Count(); i++)
            {
                int index = LetterToInt(letters[i]);
                answerIds.Add((int)questions[i].GetAnswers()[index].Id);
            }

            return new AnswerSetDTO(userId, answerIds);
        }

        public static int LetterToInt(char letter)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return alphabet.IndexOf(letter);
        }
    }
}

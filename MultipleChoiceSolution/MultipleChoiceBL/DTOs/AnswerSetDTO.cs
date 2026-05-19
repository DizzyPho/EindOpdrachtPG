using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MultipleChoiceBL.DTOs
{
    public class AnswerSetDTO
    {
        public AnswerSetDTO(int quizId, int userId, Dictionary<int, List<int>> questionAnswers)
        {
            QuizId = quizId;
            UserId = userId;
            QuestionAnswers = questionAnswers;
        }
        public int QuizId { get; init; }
        public int UserId { get; init; }
        public int Score { get; init; }
        public Dictionary<int, List<int>> QuestionAnswers { get; init; }

        public static AnswerSetDTO StringToAnswerSet(Quiz quiz, string text)
        {
            string[] fields = text.Split(',');
            int.TryParse(fields[0], out int userId);
            string letters = fields[1];

            Dictionary<int, List<int>> questionAnswers = new Dictionary<int, List<int>>();
            List <Question> questions = quiz.Questions;

            for (int i = 0; i < letters.Count(); i++)
            {
                int index = LetterToInt(letters[i]);
                try
                {
                    Question question = questions[i];
                    IReadOnlyList<Answer> answers = question.GetAnswers();
                    int answerId = (int)answers[index].Id;
                    questionAnswers.Add((int)question.Id, [answerId]);
                }
                catch
                {
                    
                }
            }

            return new AnswerSetDTO((int)quiz.Id,userId, questionAnswers);
        }

        public static int LetterToInt(char letter)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return alphabet.IndexOf(letter);
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static AnswerSetDTO FromJson(string json)
        {
            return JsonSerializer.Deserialize<AnswerSetDTO>(json);
        }
    }
}

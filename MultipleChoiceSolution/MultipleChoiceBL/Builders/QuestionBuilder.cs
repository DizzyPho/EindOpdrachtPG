using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Builders
{
    public class QuestionBuilder
    {
        private Dictionary<char, Answer> _answersList;
        private string _questionText;

        public QuestionBuilder()
        {
            _answersList = new Dictionary<char, Answer>();
        }

        public QuestionBuilder SetQuestionText(string questionText)
        {
            _questionText = questionText;
            return this;
        }
        public QuestionBuilder AddAnswer(char letter, Answer answer)
        {
            _answersList[letter] = answer;
            return this;
        }
        public QuestionBuilder SetCorrectAnswer(char letter)
        {
            if(_answersList.TryGetValue(letter, out Answer answer))
            {
                answer.SetCorrect();
            }
            return this;
        }

        public QuestionBuilder SetCorrectAnswers(char[] letters)
        {
            foreach (char letter in letters)
            {
                SetCorrectAnswer(letter);
            }
            return this;
        }

        public Question Build()
        {
            return new Question(_questionText, _answersList.Values.ToList());
        }
    }
}

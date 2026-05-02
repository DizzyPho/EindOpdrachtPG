using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MultipleChoiceTest
{
    public class TestQuestion
    {
        private List<Answer> emptyAnswerSet;
        private List<Answer> validAnswerSet;
        private List<Answer> tooShortAnswerSet;
        private List<Answer> noCorrectAnswersSet;

        public TestQuestion()
        {
            Answer.TryCreate("Antwoord1", true, out FactoryResult<Answer> answer1);
            Answer.TryCreate("Antwoord2", false, out FactoryResult<Answer> answer2);
            Answer.TryCreate("Antwoord3", false, out FactoryResult<Answer> answer3);
            Answer.TryCreate("Antwoord4", false, out FactoryResult<Answer> answer4);
            emptyAnswerSet = new List<Answer>();
            validAnswerSet = [answer1.Result, answer2.Result, answer3.Result, answer4.Result];
            tooShortAnswerSet = [answer1.Result];
            noCorrectAnswersSet = [answer2.Result, answer3.Result, answer4.Result];
        }

        [Theory]
        [InlineData("Vraag1")]
        [InlineData("Vraag 1")]
        [InlineData(" Vraag 1")]
        [InlineData("Vraag 1 ")]
        [InlineData(" Vraag 1 ")]
        public void Test_TryCreate_Valid(string questionText)
        {
            bool isSucces = Question.TryCreate(questionText, validAnswerSet, out FactoryResult<Question> factory);
            Question q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.True(isSucces);
            Assert.Null(errors);
            Assert.NotNull(q);
            Assert.Equal(questionText, q.QuestionText);
            Assert.Equal(validAnswerSet, q.GetAnswers());
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void Test_TryCreate_Text_Empty(string text)
        {
            bool isSucces = Question.TryCreate(text, validAnswerSet, out FactoryResult<Question> factory);
            Question q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Question text cannot be empty.", errors);
        }

        [Fact]
        public void Test_TryCreate_TooFewAnswers()
        {
            bool isSucces = Question.TryCreate("Vraag 1", tooShortAnswerSet, out FactoryResult<Question> factory);
            Question q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Question must have at least 2 answers.", errors);
        }

        [Fact]

        public void Test_TryCreate_AnswerSetNullOrEmpty()
        {
            //check 1 : answer set null
            bool isSucces = Question.TryCreate("Vraag 1", null, out FactoryResult<Question> factory);
            Question q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Question must have at least 2 answers.", errors);

            //check 1 : answer set null
            isSucces = Question.TryCreate("Vraag 1", emptyAnswerSet, out factory);
            q = factory.Result;
            errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Question must have at least 2 answers.", errors);
        }
        // test if at least one of the answers provided is marked as the 'correct' answer to the question
        [Fact]
        public void Test_TryCreate_NoCorrectAnswersProvided()
        {
            bool isSucces = Question.TryCreate("Vraag 1", noCorrectAnswersSet, out FactoryResult<Question> factory);
            Question q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Please provide at least 1 correct answer.", errors);

        }
    }
}

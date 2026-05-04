using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MultipleChoiceTest
{
    public class TestQuiz
    {
        Question testQuestion1;
        Question testQuestion2;

        public TestQuiz()
        {
            Answer.TryCreate("Antwoord1", true, out FactoryResult<Answer> answer1);
            Answer.TryCreate("Antwoord2", false, out FactoryResult<Answer> answer2);
            Question.TryCreate("Vraag1", [answer1.Result, answer2.Result], out var factoryResult);
            testQuestion1 = factoryResult.Result;
            Question.TryCreate("Vraag2", [answer1.Result, answer2.Result], out factoryResult);
            testQuestion2 = factoryResult.Result;
        }

        [Theory]
        [InlineData("2026-5-1", "Test1")]
        [InlineData("2026-5-1", "Test 1")]
        [InlineData("2026-5-1", " Test 1")]
        [InlineData("2026-5-1", " Test 1 ")]
        [InlineData("2026-5-1", "Test 1 ")]
        public void Test_TryCreate_Name_Valid(DateTime creationDate, string name)
        {
            bool isSucces = Quiz.TryCreate(name, creationDate, out  var factory);

            Quiz q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.True(isSucces);
            Assert.NotNull(q);
            Assert.Null(errors);
            Assert.Equal(name, q.Name);
            Assert.NotNull(q.Questions);
        }

        [Theory]
        [InlineData("2026-5-1", "")]
        [InlineData("2026-5-1", " ")]
        [InlineData("2026-5-1", "   ")]
        [InlineData("2026-5-1", null)]
        [InlineData("2026-5-1", "\n")]
        public void Test_TryCreate_Name_Invalid(DateTime creationDate, string name)
        {
            bool isSucces = Quiz.TryCreate(name, creationDate, out var factory);

            Quiz q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Name cannot be empty.", errors);
        }
        [Fact]
        public void Test_TryCreate_Date_Valid()
        {
            // check 1 : today's date valid
            DateTime creationDate = DateTime.Now;
            bool isSucces = Quiz.TryCreate("Test 1", creationDate, out var factory);

            Quiz q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.True(isSucces);
            Assert.NotNull(q);
            Assert.Null(errors);
            Assert.Equal(creationDate, q.CreationDate);
            Assert.NotNull(q.Questions);

            // check 2 : dates before today

            creationDate = DateTime.Now.AddDays(-1);
            isSucces = Quiz.TryCreate("Test 1", creationDate, out factory);

            q = factory.Result;
            errors = factory.Errors;

            Assert.True(isSucces);
            Assert.NotNull(q);
            Assert.Null(errors);
            Assert.Equal(creationDate, q.CreationDate);
            Assert.NotNull(q.Questions);
        }

        [Fact]
        public void Test_TryCreate_Date_Invalid()
        {
            DateTime creationDate = DateTime.Now.AddDays(1);
            bool isSucces = Quiz.TryCreate("Test 1", creationDate, out var factory);

            Quiz q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.StartsWith("Invalid date", errors[0]);
        }

        [Fact]
        public void Test_AddQuestion_Valid()
        {
            Quiz.TryCreate("Test 1", DateTime.Now, out var factory);
            Quiz q = factory.Result;

            // check 1 : add first question
            Assert.True(q.AddQuestion(testQuestion1));
            Assert.Single(q.Questions);
            Assert.Contains(testQuestion1, q.Questions);

            // check 2 : add a second question
            Assert.True(q.AddQuestion(testQuestion2));
            Assert.Equal(2, q.Questions.Count);
            Assert.Contains(testQuestion2, q.Questions);
        }

        [Fact]
        public void Test_AddQuestion_Invalid()
        {
            Quiz.TryCreate("Test 1", DateTime.Now, out var factory);
            Quiz q = factory.Result;

            // check 1 : add null question
            Assert.Throws<ArgumentNullException>(() => q.AddQuestion(null));
            Assert.Empty(q.Questions);

            // check 2 : add duplicate question
            q.AddQuestion(testQuestion1);

            Assert.False(q.AddQuestion(testQuestion1));
            Assert.Single(q.Questions);
            Assert.Contains<Question> (testQuestion1, q.Questions);
        }
    }
}

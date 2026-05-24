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
        Question testQuestion3;

        List<Question> NoQuestionsList;
        List<Question> TwoQuestionsList;
        List<Question> ThreeQuestionsList;

        public TestQuiz()
        {
            Answer.TryCreate("Antwoord1", true, out FactoryResult<Answer> answer1);
            Answer.TryCreate("Antwoord2", false, out FactoryResult<Answer> answer2);
            Answer.TryCreate("Antwoord3", false, out FactoryResult<Answer> answer3);
            Question.TryCreate("Vraag1", [answer1.Result, answer2.Result], out var factoryResult);
            testQuestion1 = factoryResult.Result;
            Question.TryCreate("Vraag2", [answer1.Result, answer2.Result], out factoryResult);
            testQuestion2 = factoryResult.Result;
            Question.TryCreate("Vraag3", [answer1.Result, answer3.Result], out factoryResult);
            testQuestion3 = factoryResult.Result;

            NoQuestionsList = new List<Question>();
            TwoQuestionsList = [testQuestion1 , testQuestion2];
            ThreeQuestionsList = [testQuestion1 , testQuestion2 , testQuestion3];
        }

        [Theory]
        [InlineData("2026-5-1", "Test1")]
        [InlineData("2026-5-1", "Test 1")]
        [InlineData("2026-5-1", " Test 1")]
        [InlineData("2026-5-1", " Test 1 ")]
        [InlineData("2026-5-1", "Test 1 ")]
        public void Test_TryCreate_Name_Valid(DateTime creationDate, string name)
        {
            bool isSucces = Quiz.TryCreate(name, 1, TwoQuestionsList, out  var factory);

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
            bool isSucces = Quiz.TryCreate(name, 1, TwoQuestionsList, out var factory);

            Quiz q = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(q);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Name cannot be empty.", errors);
        }
   
    }
}

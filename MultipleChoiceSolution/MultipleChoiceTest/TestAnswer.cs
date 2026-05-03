using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;
using static System.Net.Mime.MediaTypeNames;

namespace MultipleChoiceTest
{
    public class TestAnswer
    {
        [Theory]
        [InlineData("TestAntwoord", true)]
        [InlineData("Test Antwoord", false)]
        [InlineData("TestAntwoord ", true)]
        [InlineData(" TestAntwoord", false)]
        [InlineData(" Test Antwoord ", true)]
        public void Test_TryCreate_Valid(string text, bool isAnswerCorrect)
        {
            bool isSucces = Answer.TryCreate(text, isAnswerCorrect, out FactoryResult<Answer> factory);
            Answer answer = factory.Result;

            Assert.True(isSucces);
            Assert.NotNull(answer);
            Assert.Null(factory.Errors);
            Assert.Equal(answer.AnswerText, text);
            Assert.Equal(answer.IsCorrect, isAnswerCorrect);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("   ")]
        [InlineData("\n")]
        [InlineData(null)]
        public void Test_Text_Empty(string text)
        {
            bool isSucces = Answer.TryCreate(text, true, out FactoryResult<Answer> factory);
            Answer answer = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(isSucces);
            Assert.Null(answer);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Answer cannot be empty.", errors);
        }

        [Fact]
        public void Test_SetCorrect()
        {
            // check 1 : IsCorrect was false before
            Answer.TryCreate("Test", false, out FactoryResult<Answer> factory);
            Answer answer = factory.Result;

            answer.SetCorrect();

            Assert.True(answer.IsCorrect);

            // check 2 : IsCorrect was already true
            answer.SetCorrect();
            Assert.True(answer.IsCorrect);
        }

        [Fact]
        public void Test_SetIncorrect()
        {
            // check 1 : IsCorrect was true before
            Answer.TryCreate("Test", true, out FactoryResult<Answer> factory);
            Answer answer = factory.Result;

            answer.SetIncorrect();

            Assert.False(answer.IsCorrect);

            // check 2 : IsCorrect was already true
            answer.SetIncorrect();
            Assert.False(answer.IsCorrect);
        }
    }

}

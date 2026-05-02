using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;

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
            bool IsSucces = Answer.TryCreate(text, isAnswerCorrect, out FactoryResult<Answer> factory);
            Answer answer = factory.Result;

            Assert.True(IsSucces);
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
            bool IsSucces = Answer.TryCreate(text, true, out FactoryResult<Answer> factory);
            Answer answer = factory.Result;
            List<String> errors = factory.Errors;

            Assert.False(IsSucces);
            Assert.Null(answer);
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Contains<String>("Answer cannot be empty.", errors);
        }
    }

}

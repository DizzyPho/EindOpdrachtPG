using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceTest
{
    public class TestQuiz
    {
        [Theory]
        [InlineData("2026-5-1", "Test1")]
        [InlineData("2026-5-2", "Test1")]
        [InlineData("2026-3-1", "Test1")]
        [InlineData("2022-3-1", "Test1")]
        [InlineData("2026-5-1", "Test 1")]
        [InlineData("2026-5-1", " Test 1")]
        [InlineData("2026-5-1", " Test 1 ")]
        [InlineData("2026-5-1", "Test 1 ")]
        public void Test_TryCreate_Valid(DateTime creationDate, string name)
        {
            bool isSucces = Quiz.TryCreate(name, creationDate, out  var factory);
        }
    }
}

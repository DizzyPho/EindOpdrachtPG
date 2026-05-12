using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.SolveQuiz
{
    public class FullQuestionViewModel : BaseViewModel
    {
        public FullQuestionViewModel(Question question)
        {
            QuestionText = question.QuestionText;
            QuestionId = question.Id;
            Answers = question.GetAnswers()
                              .Select(a => new AnswerCheckViewModel(a))
                              .ToList();
        }
        public List<AnswerCheckViewModel> Answers { get; set; }
        public string QuestionText { get; init; }
        public int? QuestionId { get; init; }
    }
}

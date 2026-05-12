using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.SolveQuiz
{
    public class QuizViewModel : WindowViewModel
    {
        Manager _manager;
        public QuizViewModel(int quizId, Manager manager, IActionableWindow window) : base(window)
        {
            _manager = manager;

            Quiz quiz = _manager.GetQuiz(quizId);
            Title = quiz.Name;

            Questions = quiz.Questions.Select(q => new FullQuestionViewModel(q))
                                      .ToList();
        }
        public List<FullQuestionViewModel> Questions { get; init; }
        public String Title { get; init; }
    }
}

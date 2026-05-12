using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.SolveQuiz
{
    public class QuizViewModel : BaseViewModel
    {
        Manager _manager;
        IActionableWindow _actionableWindow;
        public QuizViewModel(int quizId, Manager manager, IActionableWindow window)
        {
            _manager = manager;
            _actionableWindow = window;

            Quiz quiz = _manager.GetQuiz(quizId);
            Title = quiz.Name;
            Questions = quiz.Questions.Select(q => new FullQuestionViewModel(q))
                                      .ToList();
        }
        public List<FullQuestionViewModel> Questions { get; init; }
        public String Title { get; init; }
    }
}

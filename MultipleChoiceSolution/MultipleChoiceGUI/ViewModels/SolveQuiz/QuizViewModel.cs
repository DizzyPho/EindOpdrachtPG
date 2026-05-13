using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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

            SubmitCommand = new Command(OnSubmit);
        }
        public List<FullQuestionViewModel> Questions { get; init; }
        public String Title { get; init; }
        public int UserId 
        {
            get => Get<int>();
            set => Set(value);
        }

        public ICommand SubmitCommand { get; init; }

        public void OnSubmit()
        {
            List<int> selectedAnswerIds = Questions.SelectMany(q => q.Answers)
                                                   .Where(a => a.IsChecked)
                                                   .Select(a => a.Id)
                                                   .ToList();
            AnswerSetDTO answerSet = new AnswerSetDTO(UserId, selectedAnswerIds);
            _manager.SubmitAnswers(answerSet);
        }
    }
}

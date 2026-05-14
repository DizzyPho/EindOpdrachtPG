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
            QuizEnabled = true;

            Questions = quiz.Questions.Select(q => new FullQuestionViewModel(q))
                                      .ToList();

            SubmitCommand = new Command(OnSubmit);
        }
        public List<FullQuestionViewModel> Questions { get; init; }
        public String Title { get; init; }
        public String Score
        {
            get => Get<String>();
            set => Set(value);
        }
        public int UserId 
        {
            get => Get<int>();
            set => Set(value);
        }
        public bool QuizEnabled
        {
            get => Get<bool>();
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
            GiveFeedback();
            QuizEnabled = false;
            //_manager.SubmitAnswers(answerSet);
        }

        public void GiveFeedback()
        {
            int questionCount = Questions.Count;

            // count correct answers and set their color accordingly
            foreach (AnswerCheckViewModel answer in Questions.SelectMany(q => q.Answers))
            {
                ShowAnswerCorrect(answer);
            }

            int correctQuestions = Questions.Count(q => q.Answers
                                            .All(a => 
                                            {
                                                return a.IsCorrect == a.IsChecked;
                                            }));
            Score = $"Score: {correctQuestions} / {questionCount}";
        }

        public void ShowAnswerCorrect(AnswerCheckViewModel answer)
        {
            bool isChecked = answer.IsChecked;
            bool isCorrect = answer.IsCorrect;
            if(isChecked != isCorrect)
            {
                answer.SetRed();
            }
            else if (isChecked)
            {
                answer.SetGreen();
            }
        }
    }
}

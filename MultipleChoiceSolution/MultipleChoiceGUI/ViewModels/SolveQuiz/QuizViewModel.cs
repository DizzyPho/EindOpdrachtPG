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
        int _quizId;
        public QuizViewModel(int quizId, Manager manager, IActionableWindow window) : base(window)
        {
            _manager = manager;
            _quizId = quizId;

            Quiz quiz = _manager.GetQuiz(quizId);
            Title = quiz.Name;
            QuizEnabled = true;

            Questions = quiz.Questions.Select(q => new FullQuestionViewModel(q))
                                      .ToList();

            SubmitCommand = new Command(OnSubmit);
        }
        public List<FullQuestionViewModel> Questions { get; init; }
        public String Title { get; init; }
        private int _scoreNumber;
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
            Dictionary<int, List<int>> questionAnswers = new Dictionary<int, List<int>>();

            foreach (FullQuestionViewModel question in Questions)
            {
                List<int> selectedAnswerIds = question.Answers
                                                   .Where(a => a.IsChecked)
                                                   .Select(a => a.Id)
                                                   .ToList();
                questionAnswers.Add(question.QuestionId, selectedAnswerIds);

            }

            AnswerSetDTO answerSet = new AnswerSetDTO(_quizId, UserId, questionAnswers);
            GiveFeedback();
            QuizEnabled = false;
            _manager.SubmitAnswers(answerSet);
        }

        public void GiveFeedback()
        {
            int questionCount = Questions.Count;

            // count correct answers and set their color accordingly
            foreach (AnswerCheckViewModel answer in Questions.SelectMany(q => q.Answers))
            {
                ShowAnswerCorrect(answer);
            }

            _scoreNumber = CalculateScore();
            Score = $"Score: {_scoreNumber} / {questionCount}";
        }
        private int CalculateScore()
        {
            int correctQuestions = Questions.Count(q => q.Answers
                                            .All(a => a.IsCorrect == a.IsChecked));
            return correctQuestions;
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

using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.ViewModels.ImportQuestion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.NewQuestion
{
    public class NewQuestionViewModel : BaseViewModel
    {
        Manager _manager;
        public NewQuestionViewModel(Manager manager)
        {
            _manager = manager;

            TopicList = _manager.GetTopics()
                                .Select(topic => new TopicViewModel(topic))
                                .ToList();
            AnswerList = new ObservableCollection<AddAnswerViewModel>();
            AnswerList.Add(new AddAnswerViewModel());
            AnswerList.Add(new AddAnswerViewModel());

            AddAnswerCommand = new Command(OnAddAnswer);
            NewQuestionCommand = new Command(OnNewQuestion);
        }
        public ICommand AddAnswerCommand { get; init; }
        public ICommand NewQuestionCommand { get; init; }
        public String QuestionText
        {
            get => Get<String>(); set => Set(value);
        }
        public List<TopicViewModel> TopicList { get; set; }
        public ObservableCollection<AddAnswerViewModel> AnswerList
        {
            get => Get<ObservableCollection<AddAnswerViewModel>>();
            set => Set(value);
        }
        public void OnAddAnswer()
        {
            AnswerList.Add(new AddAnswerViewModel());
        }
        public void OnNewQuestion()
        {
            int answerNumber = 1;
            List<String> errors = new List<String>();
            List<Answer> answers = new List<Answer>();
            List<int> topicIds = TopicList.Where(t => t.IsChecked).Select(t => t.Topic.Id).ToList();
            if (answers.Count == 0)
            {
                errors.Add("Please select at least one category.");
            }

            foreach(AddAnswerViewModel answerViewModel in AnswerList)
            {
                if(Answer.TryCreate(answerViewModel.Text, answerViewModel.IsChecked, out FactoryResult<Answer> answerResult))
                {
                    answers.Add(answerResult.Result);
                }
                else
                {
                    errors.Add($"Antwoord {answerNumber}: {string.Join(", ", answerResult.Errors)}");
                }
                answerNumber++;
            }
            Question question = null;

            if(Question.TryCreate(QuestionText, answers, out FactoryResult<Question> questionResult))
            {
                question = questionResult.Result;
            }
            else
            {
                errors.Add(string.Join("\n", questionResult.Errors));
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join('\n', errors), "Er liep iets mis", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _manager.InsertQuestion(question, topicIds);
            }
        }
    }
}

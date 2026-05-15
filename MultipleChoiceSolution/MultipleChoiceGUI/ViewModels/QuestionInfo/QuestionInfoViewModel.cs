using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceBL.Messages;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.QuestionInfo
{
    public class QuestionInfoViewModel : WindowViewModel
    {
        private Manager _manager;
        public ObservableCollection<Topic> Topics
        {
            get => Get<ObservableCollection<Topic>>();
            set => Set(value);
        }
        public ObservableCollection<QuestionViewModel> QuestionList
        {
            get => Get<ObservableCollection<QuestionViewModel>>();
            set => Set(value);
        }
        public Topic SelectedTopic
        {
            get => Get<Topic>();
            set
            {
                Set(value);
                SelectedTopicChange(value);
            }
        }
        public QuestionViewModel SelectedQuestion
        {
            get => Get<QuestionViewModel>();
            set
            {
                Set(value);
                SelectedQuestionChange(value);
            }
        }
        public String QuestionText
        {
            get => Get<String>();
            set => Set(value);
        }
        public List<AnswerViewModel> SelectedQuestionAnswers
        {
            get => Get<List<AnswerViewModel>>();
            set => Set(value);
        }
        public QuestionInfoViewModel(Manager manager, MessageManager messageManager, IActionableWindow window) : base(window)
        {
            _manager = manager;
            Topics = new ObservableCollection<Topic>(_manager.GetTopics());

            messageManager.Register<NewTopicMessage>(this, (o, message) => Topics.Add(message.Topic));
            messageManager.Register<QuestionsImportedMessage>(this, (o, message) => OnNewQuestionsImported(message.TopicIds, message.Questions));
        }
        internal void SelectedQuestionChange(QuestionViewModel question)
        {
            if (question == null) return;
            QuestionText = question.QuestionText;
            var answers = _manager.GetQuestion(question.Id)
                                              .GetAnswers()
                                              .Select(answer => new AnswerViewModel(answer));
            SelectedQuestionAnswers = new List<AnswerViewModel>(answers);
        }

        internal void SelectedTopicChange(Topic topic)
        {
            QuestionList = new ObservableCollection<QuestionViewModel>
                           (_manager.GetQuestionDTOs(topic.Id)
                                    .Select(dto => new QuestionViewModel(dto, _manager))); 
        }

        private void OnNewQuestionsImported(List<int> topicIds, List<QuestionDTO> dtos)
        {
            if (topicIds.Contains(SelectedTopic.Id))
            {
                var newQuestions = dtos.Select(dto => new QuestionViewModel(dto, _manager));

                QuestionList = new ObservableCollection<QuestionViewModel>(QuestionList.Union(newQuestions));
                                           
            }
        }
    }
}

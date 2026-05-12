using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
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
        public List<QuestionDTO> QuestionList
        {
            get => Get<List<QuestionDTO>>();
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
        public QuestionDTO SelectedQuestion
        {
            get => Get<QuestionDTO>();
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
        public QuestionInfoViewModel(Manager manager, IActionableWindow window) : base(window)
        {
            _manager = manager;
            Topics = new ObservableCollection<Topic>(_manager.GetTopics());
        }
        internal void SelectedQuestionChange(QuestionDTO question)
        {
            QuestionText = question.Question;
            var answers = _manager.GetQuestion(question.Id)
                                              .GetAnswers()
                                              .Select(answer => new AnswerViewModel(answer));
            SelectedQuestionAnswers = new List<AnswerViewModel>(answers);
        }

        internal void SelectedTopicChange(Topic topic)
        {
            QuestionList = _manager.GetQuestionDTOs(topic.Id).ToList(); 
        }
    }
}

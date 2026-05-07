using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.QuestionInfo
{
    public class QuestionInfoViewModel : BaseViewModel
    {
        private Manager _manager;
        public List<QuestionDTO> QuestionList
        {
            get => Get<List<QuestionDTO>>();
            set => Set(value);
        }
        public TopicDTO SelectedTopic
        {
            get => Get<TopicDTO>();
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
        public List<Answer> SelectedQuestionAnswers
        {
            get => Get<List<Answer>>();
            set => Set(value);
        }
        public QuestionInfoViewModel(Manager manager)
        {
            _manager = manager;
        }
        internal void SelectedQuestionChange(QuestionDTO question)
        {
            QuestionText = question.Question;
            SelectedQuestionAnswers = _manager.GetQuestion(question.Id).GetAnswers().ToList();
        }

        internal void SelectedTopicChange(TopicDTO topic)
        {
            QuestionList = _manager.GetQuestionDTOs(topic.Id).ToList(); 
        }
    }
}

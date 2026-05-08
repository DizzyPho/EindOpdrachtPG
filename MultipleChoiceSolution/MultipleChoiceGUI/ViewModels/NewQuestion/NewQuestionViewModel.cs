using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.ViewModels.ImportQuestion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
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
        }
        public ICommand AddAnswerCommand { get; init; }
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


    }
}

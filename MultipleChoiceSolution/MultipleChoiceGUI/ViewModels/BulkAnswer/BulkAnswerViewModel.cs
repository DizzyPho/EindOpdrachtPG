using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceGUI.Commands;
using MultipleChoiceGUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MultipleChoiceGUI.ViewModels.BulkAnswer
{
    public class BulkAnswerViewModel : WindowViewModel
    {
        Manager _manager;
        IActionableWindow _actionableWindow;
        Quiz _quiz;
        public BulkAnswerViewModel(int quizId, Manager manager, IQuizFileWriter fileWriter, IActionableWindow actionableWindow) : base(actionableWindow)
        {
            _manager = manager;
            _quiz = _manager.GetQuiz(quizId);
            _actionableWindow = actionableWindow;
            SubmitCommand = new Command(OnSubmitAnswerSets);
        }

        public string BulkText
        {
            get => Get<String>();
            set => Set(value);
        }
        public ICommand SubmitCommand { get; init; }
        public void OnSubmitAnswerSets()
        {
            List<AnswerSetDTO> answerSets;
            try
            {
                answerSets = BulkTextToAnswerSets();
                _manager.SubmitAnswerSets(answerSets);
                _actionableWindow.CloseAction();
            }
            catch
            {
                MessageBox.Show("Er ging iets mis. Kijk de input na.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public List<AnswerSetDTO> BulkTextToAnswerSets()
        { 
            List<AnswerSetDTO> answerSets = new List<AnswerSetDTO>();

            string[] lines = BulkText.Split('\n');
            foreach (string line in lines)
            {
                answerSets.Add(AnswerSetDTO.StringToAnswerSet(_quiz, line));
            }
            return answerSets;
        }
    }
}

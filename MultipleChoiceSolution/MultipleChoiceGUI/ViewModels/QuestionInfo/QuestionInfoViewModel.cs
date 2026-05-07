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
        public String QuestionText
        {
            get => Get<String>();
            set => Set(value);
        }


        public QuestionInfoViewModel()
        {

        }
    }
}

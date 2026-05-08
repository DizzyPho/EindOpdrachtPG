using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.ImportQuestion
{
    public class FormatViewModel : BaseViewModel
    {
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }
        public string Text { get; set; }

        public string Option;

        public FormatViewModel(string text, string option)
        {
            Text = text;
            Option = option;
        }
    }
}

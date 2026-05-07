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

        public int _option;

        public FormatViewModel(string text, int option)
        {
            Text = text;
            _option = option;
        }
    }
}

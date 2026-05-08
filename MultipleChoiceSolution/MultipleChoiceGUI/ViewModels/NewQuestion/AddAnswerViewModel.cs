using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceGUI.ViewModels.NewQuestion
{

    public class AddAnswerViewModel : BaseViewModel
    {
        public String Text
        {
            get => Get<string>();
            set => Set(value);
        }
        public bool IsChecked
        {
            get => Get<bool>();
            set => Set(value);
        }


    }
    
}

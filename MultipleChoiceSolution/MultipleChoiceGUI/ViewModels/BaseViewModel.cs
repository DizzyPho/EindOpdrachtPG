using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MultipleChoiceGUI.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // store properties in dictionary
        private Dictionary<string, object> _properties = new Dictionary<string, object>();
        protected T Get<T>([CallerMemberName] string propertyName = "")
        {
            if (_properties.TryGetValue(propertyName, out var value) && value is T t)
            {
                return t;
            }
            else
            {
                return default;
            }
        }
        protected void Set(object value, [CallerMemberName] string propertyName = "")
        {
            _properties[propertyName] = value;
            if (_properties.ContainsKey(propertyName))
            {
                _properties[propertyName] = value;
            }
            else
            {
                _properties.Add(propertyName, value);
            }
            NotifyPropertyChanged(propertyName);
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}


using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sln_HttpClient.ViewModels
{
    public class KeyValueUCViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _key;

        public string Key
        {
            get { return _key; }
            set { _key = value; OnPropertyRaised(); }
        }
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyRaised(); }
        }
        private bool _IsChecked;

        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; OnPropertyRaised(); }
        }

        public ICommand CheckedChanged { get; set; }
        public ICommand ChangedCommand { get; set; }
        public event EventHandler ValueChangedEvent;
        public KeyValueUCViewModel()
        {
            ChangedCommand = new ActionCommand(x =>
              {
                  ValueChangedEvent?.Invoke(this, EventArgs.Empty);
              });
        }
    }
}

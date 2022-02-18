using Microsoft.Xaml.Behaviors.Core;
using sln_HttpClient.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace sln_HttpClient.ViewModels
{
    public class KeyValuePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        private KeyValueUC _selectedItem;
        public KeyValueUC SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyRaised(); }
        }

        public ObservableCollection<KeyValueUC> LbItems { get; set; } = new ObservableCollection<KeyValueUC>();
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand PlusBtnClickCommand { get; set; }
        public KeyValuePageViewModel()
        {

            //var data = new KeyValueUC();
            //(data.DataContext as KeyValueUCViewModel).IsChecked = true;
            ////(data.DataContext as KeyValueUCViewModel).Key = "";
            ////(data.DataContext as KeyValueUCViewModel).Value = "";
            //data.Width = 700;
            //LbItems.Add(data);
            PlusBtnClickCommand = new ActionCommand(x =>
            {

                var data = new KeyValueUC();
                data.Width = 700;
                (data.DataContext as KeyValueUCViewModel).IsChecked = true;
                (data.DataContext as KeyValueUCViewModel).ValueChangedEvent += KeyValuePageViewModel_ValueChangedEvent;
                LbItems.Add(data);
            });
        }
        public event EventHandler ValueChangedEvent;
        public void KeyValuePageViewModel_ValueChangedEvent(object? sender, EventArgs e)
        {
            ValueChangedEvent?.Invoke(this, e);
        }
    }
}

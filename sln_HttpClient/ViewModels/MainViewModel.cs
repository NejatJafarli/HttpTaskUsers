using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using sln_HttpClient.Command;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows;
using sln_HttpClient.Interfaces;

namespace sln_HttpClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public string URL { get; set; } = "http://localhost:45678/";
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private string _textName;

        public string TextName
        {
            get { return _textName; }
            set { _textName = value; OnPropertyRaised(); }
        }
        private string _textSurname;

        public string TextSurname
        {
            get { return _textSurname; }
            set { _textSurname = value; OnPropertyRaised(); }
        }
        public ObservableCollection<PostedUser> LbItems { get; set; } = new ObservableCollection<PostedUser>();
        public RelayCommand BtnGetCommand { get; set; }
        public RelayCommand BtnPostCommand { get; set; }
        public RelayCommand BtnPutCommand { get; set; }
        public RelayCommand LbDoubleClick { get; set; }
        public RelayCommand BtnDelCommand { get; set; }
        private PostedUser _selectedItem;

        public PostedUser SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyRaised(); }
        }

        public HttpClient Client { get; set; } = new HttpClient();
        public ListBox Lb { get; set; }
        public MainViewModel()
        {
            LbDoubleClick = new RelayCommand(s =>
             {
                 if (SelectedItem is not null)
                 {
                     TextName = SelectedItem.Name;
                     TextSurname = SelectedItem.Surname;
                 }
                 else
                 {
                     TextName = "";
                     TextSurname = "";
                 }
             });

            BtnDelCommand = new RelayCommand(async s =>
            {
                await DeleteMethodAsync();
                GetMethodAsync();
            });
            BtnGetCommand = new RelayCommand(async s =>
            {
                await GetMethodAsync();

            });
            BtnPutCommand = new RelayCommand(async s =>
            {
                await PutMethodAsync();
                GetMethodAsync();
            });
            BtnPostCommand = new RelayCommand(async s =>
            {
                await PostMethodAsync();
                GetMethodAsync();
            });
        }

        private async Task DeleteMethodAsync()
        {
            var t = await Client.DeleteAsync(URL + "?" + "Name=" + SelectedItem.Name + "&Surname=" + SelectedItem.Surname);
            if (t.StatusCode == System.Net.HttpStatusCode.OK)
                MessageBox.Show("User Deleted In Database");
            else if (t.StatusCode == System.Net.HttpStatusCode.NotFound)
                MessageBox.Show("User Not Found");
        }

        private async Task PutMethodAsync()
        {
            if (!string.IsNullOrWhiteSpace(TextName) && !string.IsNullOrWhiteSpace(TextSurname))
            {
                if (TextName == SelectedItem.Name && TextSurname == SelectedItem.Surname)
                {
                    MessageBox.Show("Changes Not Found");
                    return;
                }
                var t = await Client.PutAsync(URL, new StringContent(JsonSerializer.Serialize(new PostedUser[] { new PostedUser { Name = SelectedItem.Name, Surname = SelectedItem.Surname }, new PostedUser { Name = TextName, Surname = TextSurname } })));
                MessageBox.Show("Updated");
            }
            else
                MessageBox.Show("Name Or Surname Is Empty");
        }

        private async Task PostMethodAsync()
        {
            if (!string.IsNullOrWhiteSpace(TextName) && !string.IsNullOrWhiteSpace(TextSurname))
            {
                var t = await Client.PostAsync(URL, new StringContent(JsonSerializer.Serialize(new PostedUser { Name = TextName, Surname = TextSurname })));
                var text = await t.Content.ReadAsStringAsync();
                if (text == "1")
                {
                    MessageBox.Show("Added");
                    TextName = "";
                    TextSurname = "";
                }
                else if (text == "0")
                    MessageBox.Show("User Already Have In DB");
            }
            else
                MessageBox.Show("Name Or Surname Is Empty");
        }

        private async Task GetMethodAsync()
        {
            var t = await Client.GetAsync(URL);
            var Users = JsonSerializer.Deserialize<List<PostedUser>>(t.Content.ReadAsStringAsync().Result);
            LbItems.Clear();
            foreach (var User in Users)
                LbItems.Add(User);
        }
    }
}

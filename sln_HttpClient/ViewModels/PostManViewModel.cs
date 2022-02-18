using Microsoft.Xaml.Behaviors.Core;
using sln_HttpClient.Interfaces;
using sln_HttpClient.UserControls;
using sln_HttpClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace sln_HttpClient.ViewModels
{
    public class PostManViewModel : INotifyPropertyChanged
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
        public ObservableCollection<HttpMethod> CbItems { get; set; }

        private string _UrlText;

        public string UrlText
        {
            get { return _UrlText; }
            set { _UrlText = value; OnPropertyRaised(); }
        }

        public ICommand RequestTextChangedCommand { get; set; }
        public ICommand RightClickOnResponse { get; set; }
        private HttpMethod _cbSelected;
        public HttpMethod CbSelected
        {
            get { return _cbSelected; }
            set { _cbSelected = value; OnPropertyRaised(); }
        }
        private Visibility _visibility;

        public Visibility Visible
        {
            get { return _visibility; }
            set { _visibility = value; OnPropertyRaised(); }
        }

        public ICommand ParamsBtnClickCommand { get; set; }
        public ICommand HeadersBtnClickCommand { get; set; }
        public ICommand BodyBtnClickCommand { get; set; }

        public ICommand ResponseHeadersBtnClickCommand { get; set; }
        public ICommand ResponseBodyBtnClickCommand { get; set; }

        public ICommand SendBtnCommand { get; set; }

        public KeyValuePage ParamsView { get; set; } = new KeyValuePage();
        public KeyValuePage HeadersView { get; set; } = new KeyValuePage();
        public BodyUC Body { get; set; } = new BodyUC();

        public KeyValuePage ResponseHeadersView { get; set; } = new KeyValuePage();
        public BodyUC ResponseBody { get; set; } = new BodyUC();

        public HttpClient Client { get; set; } = new HttpClient();
        public PostManViewModel()
        {

            Visible = Visibility.Collapsed;

            CbItems = new ObservableCollection<HttpMethod>();
            CbItems.Add(HttpMethod.Get);
            CbItems.Add(HttpMethod.Post);
            CbItems.Add(HttpMethod.Put);
            CbItems.Add(HttpMethod.Delete);

            CbSelected = HttpMethod.Get;

            (ParamsView.DataContext as KeyValuePageViewModel).ValueChangedEvent += PostManViewModel_ValueChangedEvent; ;
            var UC = new KeyValueUC();
            UC.Width = 700;
            var convert = (UC.DataContext as KeyValueUCViewModel);
            convert.ValueChangedEvent += (ParamsView.DataContext as KeyValuePageViewModel).KeyValuePageViewModel_ValueChangedEvent;
            (ParamsView.DataContext as KeyValuePageViewModel).LbItems.Add(UC);

            DefaultHeadersAdd();

            BodyBtnClickCommand = new ActionCommand(x =>
              {
                  if (x is Frame frame)
                  {
                      //frame.Content= //create new page item for Body
                      frame.Content = Body;
                  }

              });

            ResponseBodyBtnClickCommand = new ActionCommand(x =>
            {
                if (x is Frame frame)
                    frame.Content = ResponseBody;
            });

            ResponseHeadersBtnClickCommand = new ActionCommand(x =>
            {
                if (x is Frame frame)
                    frame.Content = ResponseHeadersView;
            });
            RequestTextChangedCommand = new ActionCommand(x =>
            {
                if (x is Frame frame)
                {
                    if (string.IsNullOrWhiteSpace(UrlText))
                    {
                        frame.IsEnabled = false;
                        return;
                    }
                    frame.IsEnabled = true;
                }
            });
            ParamsBtnClickCommand = new ActionCommand(x =>
            {
                if (x is Frame frame)
                {
                    frame.Content = ParamsView;
                }
            });
            HeadersBtnClickCommand = new ActionCommand(x =>
            {
                if (x is Frame frame)
                {
                    frame.Content = HeadersView;
                }
            });

            RightClickOnResponse = new ActionCommand(x =>
             {
                 Visible = Visibility.Collapsed;
             });

            SendBtnCommand = new ActionCommand(async x =>
            {
                var lbitem = (HeadersView.DataContext as KeyValuePageViewModel)?.LbItems;

                foreach (var item in lbitem)
                    Client.DefaultRequestHeaders.Add(item.KeyTxt.Text, item.ValueTxt.Text);
                if (queryString.EndsWith('&'))
                    queryString = queryString.Remove(queryString.Length - 1);
                var Url = UrlText + "?" + queryString;
                if (Url.EndsWith('?'))
                    Url = Url.Remove(Url.Length - 1);

                MessageBox.Show(Url + " Url Request Sended");
                var request = new HttpRequestMessage(CbSelected, Url);

                switch (CbSelected.Method)
                {
                    case "GET":
                        await GetMethodAsync(Url);
                        break;
                    case "DELETE":
                        await DeleteMethodAsync(Url);
                        break;
                    case "POST":
                        await PostMethodAsync(Url);
                        break;
                    case "PUT":
                        await PutMethodAsync(Url);
                        break;
                }

                Visible = Visibility.Visible;
            });



        }
        private async Task PutMethodAsync(string url)
        {
            var t = await Client.PutAsync(url, new StringContent(Body.TextEditor.Text));
            MessageBox.Show("Updated");
            ResponseBody.TextEditor.Text = await t.Content.ReadAsStringAsync();
        }

        private async Task PostMethodAsync(string url)
        {
            var t = await Client.PostAsync(url, new StringContent(Body.TextEditor.Text));
            var text = await t.Content.ReadAsStringAsync();

            if (text == "1")
            {
                MessageBox.Show("Added");
            }
            else if (text == "0")
                MessageBox.Show("User Already Have In DB");

            ResponseBody.TextEditor.Text = text;
        }
        private async Task GetMethodAsync(string url)
        {
            var t = await Client.GetAsync(url);
            var Json = await t.Content.ReadAsStringAsync();
            // take Json And Write Responce Body
            ResponseBody.TextEditor.Text = Json;

        }
        private async Task DeleteMethodAsync(string url)
        {
            var t = await Client.DeleteAsync(url);
            if (t.StatusCode == System.Net.HttpStatusCode.OK)
                MessageBox.Show("User Deleted In Database");
            else if (t.StatusCode == System.Net.HttpStatusCode.NotFound)
                MessageBox.Show("User Not Found");

            var Json = await t.Content.ReadAsStringAsync();
            ResponseBody.TextEditor.Text = Json;
        }

        public string queryString { get; set; } = "";
        private void PostManViewModel_ValueChangedEvent(object? sender, EventArgs e)
        {
            queryString = "";
            foreach (var item in (ParamsView.DataContext as KeyValuePageViewModel).LbItems)
            {
                var Temp = item.DataContext as KeyValueUCViewModel;
                if (Temp.IsChecked)
                    queryString += $"{Temp.Key}={Temp.Value}&";

            }
            //if (queryString.EndsWith('&'))
            //    queryString = queryString.Remove(queryString.Length - 1);
            //if (queryString.EndsWith('?'))
            //    queryString = queryString.Remove(queryString.Length - 1);
        }

        private void DefaultHeadersAdd()
        {
            var KeyValueUc = new UserControls.KeyValueUC();
            KeyValueUc.Width = 700;

            (KeyValueUc.DataContext as KeyValueUCViewModel).IsChecked = true;
            (KeyValueUc.DataContext as KeyValueUCViewModel).Key = "Accept";
            (KeyValueUc.DataContext as KeyValueUCViewModel).Value = "*/*";

            (HeadersView.DataContext as KeyValuePageViewModel).LbItems.Add(KeyValueUc);

            KeyValueUc = new UserControls.KeyValueUC();
            KeyValueUc.Width = 700;

            (KeyValueUc.DataContext as KeyValueUCViewModel).IsChecked = true;
            (KeyValueUc.DataContext as KeyValueUCViewModel).Key = "Accept";
            (KeyValueUc.DataContext as KeyValueUCViewModel).Value = "application/json";


            (HeadersView.DataContext as KeyValuePageViewModel).LbItems.Add(KeyValueUc);

            KeyValueUc = new UserControls.KeyValueUC();
            KeyValueUc.Width = 700;

            (KeyValueUc.DataContext as KeyValueUCViewModel).IsChecked = false;
            (KeyValueUc.DataContext as KeyValueUCViewModel).Key = "Connection";
            (KeyValueUc.DataContext as KeyValueUCViewModel).Value = "keep-alive";

            (HeadersView.DataContext as KeyValuePageViewModel).LbItems.Add(KeyValueUc);

        }
    }
}

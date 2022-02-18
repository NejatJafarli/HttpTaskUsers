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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace sln_HttpClient.ViewModels
{
    public class PostManViewModel : Tab, INotifyPropertyChanged
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
        private HttpMethod _cbSelected;
        public HttpMethod CbSelected
        {
            get { return _cbSelected; }
            set { _cbSelected = value; OnPropertyRaised(); }
        }
        public ICommand ParamsBtnClickCommand { get; set; }
        public ICommand HeadersBtnClickCommand { get; set; }
        public ICommand SendBtnCommand { get; set; }
        public KeyValuePage ParamsView { get; set; } = new KeyValuePage();
        public KeyValuePage HeadersView { get; set; } = new KeyValuePage();
        public HttpClient Client { get; set; } = new HttpClient();
        public PostManViewModel()
        {

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

            SendBtnCommand = new ActionCommand(x =>
            {
                var lbitem = (HeadersView.DataContext as KeyValuePageViewModel)?.LbItems;

                foreach (var item in lbitem)
                    Client.DefaultRequestHeaders.Add(item.KeyTxt.Text, item.ValueTxt.Text);



            });



        }

        private void PostManViewModel_ValueChangedEvent(object? sender, EventArgs e)
        {
            var Url = URL;

            Url = Url + "?";
            foreach (var item in (ParamsView.DataContext as KeyValuePageViewModel).LbItems)
            {
                var Temp = item.DataContext as KeyValueUCViewModel;
                if (Temp.IsChecked)
                    Url += $"{Temp.Key}={Temp.Value}&";

            }
            if (Url.EndsWith('&'))
                Url = Url.Remove(Url.Length - 1);
            UrlText = Url;
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

using Microsoft.Xaml.Behaviors.Core;
using sln_HttpClient.Interfaces;
using sln_HttpClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace sln_HttpClient.ViewModels
{
    public class TestViewModel : MainWindow
    {
        public PostManView View { get; set; }
        public TestViewModel()
        {
            NewTabCommand = new ActionCommand(p => NewTab());
            Tabs = new ObservableCollection<ITab>();
            var data = new PostManView { TabName = $"PostMan Tab {Tabs.Count}" };
            data.CloseRequest += Tab_CloseRequest;
            Tabs.Add(data);
            SelectedItem = data;
            //Tabs.CollectionChanged += Tabs_CollectionChanged;
        }

        //private void Tabs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    ITab tab;
        //    switch (e.Action)
        //    {
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
        //            tab = (ITab)e.NewItems[0];
        //            tab.CloseRequest += Tab_CloseRequest;
        //            break;
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
        //            tab = (ITab)e.OldItems[0];
        //            tab.CloseRequest -= Tab_CloseRequest;
        //            break;
        //    }
        //}


        public ITab SelectedItem
        {
            get { return (ITab)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ITab), typeof(TestViewModel));




        public ObservableCollection<ITab> Tabs { get; set; }
        private void NewTab()
        {
            var data = new PostManView { TabName = $"PostMan Tab {Tabs.Count}" };
            data.CloseRequest += Tab_CloseRequest;
            Tabs.Add(data);

            //MessageBox.Show(SelectedItem.TabName);
            //data = new PostManView { TabName = $"PostMan Tab {Tabs.Count}" };
            //data.CloseRequest += Tab_CloseRequest;

            //Tabs.Add(data);
            ////Tabs.Add(new MainView { TabName = "Postman Tab 1" });
        }

        private void Tab_CloseRequest(object? sender, EventArgs e)
        {
            Tabs.Remove((ITab)sender);
        }

        public ICommand NewTabCommand { get; }
        public ICommand HyperLinkClickCommand { get; }
    }
}

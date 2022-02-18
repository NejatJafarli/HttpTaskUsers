using Microsoft.Xaml.Behaviors.Core;
using sln_HttpClient.Interfaces;
using sln_HttpClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sln_HttpClient.ViewModels
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            NewTabCommand = new ActionCommand(p => NewTab());
            Tabs = new ObservableCollection<ITab>();
            Tabs.CollectionChanged += Tabs_CollectionChanged;
        }

        private void Tabs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ITab tab;
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    tab = (ITab)e.NewItems[0];
                    tab.CloseRequest += Tab_CloseRequest;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    tab=(ITab)e.OldItems[0];
                    tab.CloseRequest -= Tab_CloseRequest;
                    break;
            }
        }

       

        public ObservableCollection<ITab> Tabs { get; set; }
        private void NewTab()
        {
            Tabs.Add(new PostManViewModel { TabName=$"PostMan Tab {Tabs.Count}"});
        }

        private void Tab_CloseRequest(object? sender, EventArgs e)
        {
            Tabs.Remove((ITab)sender);
        }

        public ICommand NewTabCommand { get; }
    }
}

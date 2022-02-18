using Microsoft.Xaml.Behaviors.Core;
using sln_HttpClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace sln_HttpClient.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl,ITab
    {
        public MainView()
        {
            InitializeComponent();
            btnDel.IsEnabled = false;
            btnPut.IsEnabled = false;
            CloseCommand = new ActionCommand(x =>
              {
                  CloseRequest?.Invoke(this, EventArgs.Empty);
              });
        }

        public string TabName { get ; set; }

        public ICommand CloseCommand { get; }

        public event EventHandler CloseRequest;

        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB.SelectedItems.Count == 1)
            {
                btnDel.IsEnabled = true;
                btnPut.IsEnabled = true;
            }
            else
            {
                btnDel.IsEnabled = false;
                btnPut.IsEnabled = false;
            }
        }
    }
}

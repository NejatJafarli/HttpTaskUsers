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
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            btnDel.IsEnabled = false;
            btnPut.IsEnabled = false;
        }

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

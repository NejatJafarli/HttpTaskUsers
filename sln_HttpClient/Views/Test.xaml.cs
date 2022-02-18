using sln_HttpClient.Interfaces;
using sln_HttpClient.ViewModels;
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
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }
        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            =>
            Application.Current.Shutdown();

        private void PostManView_CloseRequest(object sender, EventArgs e)
        {
            MessageBox.Show("HI");
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

            var temp = this.DataContext as TestViewModel;
            temp.Tabs.Remove(sender as ITab);
            MessageBox.Show("DWEQ");
        }
    }
}

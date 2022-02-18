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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sln_HttpClient.UserControls
{
    /// <summary>
    /// Interaction logic for KeyValueUserControl.xaml
    /// </summary>
    public partial class KeyValueUC : UserControl
    {
        public KeyValueUC()
        {
            InitializeComponent();
        }
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check)
            {
                if (check.IsChecked == true)
                {
                    KeyTxt.Foreground = Brushes.Black;
                    ValueTxt.Foreground = Brushes.Black;
                }
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox check)
            {
                if (check.IsChecked == false)
                {
                    KeyTxt.Foreground = Brushes.LightGray;
                    ValueTxt.Foreground = Brushes.LightGray;
                }
            }
        }
    }
}

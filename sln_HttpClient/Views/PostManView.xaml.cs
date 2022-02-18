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
    /// Interaction logic for PostManView.xaml
    /// </summary>
    public partial class PostManView : Window
    {
        public PostManView()
        {
            InitializeComponent();
        }


      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index =int.Parse((e.Source as Button).Uid);

            GridCursor.Margin=new Thickness(10+120*index,20,0,0);
        }
    }
}

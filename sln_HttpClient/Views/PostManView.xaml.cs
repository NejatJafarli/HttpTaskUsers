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
    /// Interaction logic for PostManView.xaml
    /// </summary>
    public partial class PostManView : UserControl,ITab
    {
        public PostManView()
        {

            InitializeComponent();
            CloseCommand = new ActionCommand(x =>
              {
                  CloseRequest.Invoke(this, EventArgs.Empty);
              });
        } 
        public string TabName { get ; set; }

        public ICommand CloseCommand { get; }

        public event EventHandler CloseRequest;

      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index =int.Parse((e.Source as Button).Uid);

            GridCursor.Margin=new Thickness(10+120*index,20,0,0);
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            int index = int.Parse((e.Source as Button).Uid);

            GriddCursor.Margin = new Thickness(10 + 120 * index, 20, 0, 0);
        }
    }

}

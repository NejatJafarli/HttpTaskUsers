using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for BodyUC.xaml
    /// </summary>
    public partial class BodyUC : UserControl
    {
        public string Text { get; set; }

        public BodyUC()
        {
            InitializeComponent();
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            Text= TextEditor.Text;
        }
    }
}

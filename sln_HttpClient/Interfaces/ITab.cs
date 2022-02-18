using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sln_HttpClient.Interfaces
{
    public interface ITab
    {
       public string TabName { get; set; }
       public ICommand CloseCommand { get; }
       public event EventHandler CloseRequest;
    }
    public abstract class Tab : ITab
    {
        public Tab()
        {
            CloseCommand = new ActionCommand(x => CloseRequest.Invoke(this, EventArgs.Empty)); 
        }
        public ICommand CloseCommand { get; set;}
        public string TabName { get; set; }
        public event EventHandler CloseRequest;
    }

}

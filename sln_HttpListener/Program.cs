
using sln_HttpListener;
using sln_HttpListener.Data_Model.Context;
using System.Net;
using System.Text.Json;

partial class Program
{
    static public MainController Controller { get; set; }
    static public string ServerUrl { get; set; } = "http://localhost:45678/";

    static void Main(string[] args)
    {
        StartServer();
    }
    static void StartServer()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add(ServerUrl);
        listener.Start();

        Controller = new MainController(listener);
    }
}

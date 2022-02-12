using sln_HttpListener.Data_Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace sln_HttpListener
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    class MyAtributes : Attribute
    {
        public string Name { get; set; }
    }
    public class MainController
    {
        static public GenericRepository<User> Db { get; set; } = new GenericRepository<User>();
        static public StreamWriter sw { get; set; }
        static public StreamReader sr { get; set; }
        static public List<User> Users { get; set; }
        static public HttpListenerResponse Response { get; set; }
        static public HttpListenerRequest Request { get; set; }
        public MainController(HttpListener Listener)
        {

            while (true)
            {
                Users = Db.GetAll().ToList();
                Console.WriteLine("Lisining");
                var context = Listener.GetContext();
                Request = context.Request;
                Response = context.Response;
                Console.WriteLine($" {Request.LocalEndPoint} Client Requested");
                Response.ContentType = "application/json";


                sw = new StreamWriter(Response.OutputStream);
                sr = new StreamReader(Request.InputStream);

                CallMethodForHttpMethod(Request);
            }
        }

        private static void CallMethodForHttpMethod(HttpListenerRequest request)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var methods = assembly.GetTypes()
                          .SelectMany(t => t.GetMethods())
                          .Where(m => m.GetCustomAttributes(typeof(MyAtributes), false).Length > 0)
                          .ToArray();
            var InvokeThisMethod = methods.Where(x => x.GetCustomAttributes(false).OfType<MyAtributes>()
            .First().Name == request.HttpMethod).FirstOrDefault();

            InvokeThisMethod?.Invoke(null, null);
        }

        [MyAtributes(Name = "GET")]
        static public void GetMethod()
        {
            Console.WriteLine("Get Method");
            List<PostedUser> postedUsers = new List<PostedUser>();
            foreach (var user in Users)
                postedUsers.Add(new PostedUser(user));
            sw.WriteLine(JsonSerializer.Serialize<List<PostedUser>>(postedUsers));
            sw.Flush();
            sw.Close();
        }

        [MyAtributes(Name = "POST")]
        static public void PostMethod()
        {
            Console.WriteLine("Post Method");
            var ClientSendedUser = JsonSerializer.Deserialize<PostedUser>(sr.ReadToEnd());
            User User = CreateNewUser(ClientSendedUser);
            var IsHave = Db.Get(x => x.Name == User.Name && x.Surname == User.Surname).FirstOrDefault();
            if (IsHave is null)
            {
                Db.Insert(User);
                sw.Write("1");
                Console.WriteLine("Added New User");
            }
            else
            {
                Console.WriteLine("User Already Have In DB");
                sw.Write("0");
            }
            sw.Flush();
            sw.Close();
        }
        [MyAtributes(Name = "DELETE")]
        static public void DelMethod()
        {
            Console.WriteLine("DELETE Method");
            var IsHave = Db.Get(x => x.Name == Request.QueryString["Name"] && x.Surname == Request.QueryString["Surname"]).FirstOrDefault();
            if (IsHave is null)
                Response.StatusCode = 404;
            else
            {
                Db.Delete(IsHave);
                Response.StatusCode = 200;
            }
            Response.Close();
        }
        [MyAtributes(Name = "PUT")]
        static public void PutMethod()
        {
            Console.WriteLine("PUT Method");
            var ClientSendedUser = JsonSerializer.Deserialize<PostedUser[]>(sr.ReadToEnd());
            var IsHave = Db.Get(x => x.Name == ClientSendedUser[0].Name && x.Surname == ClientSendedUser[0].Surname).FirstOrDefault();
            IsHave.Name = ClientSendedUser[1].Name;
            IsHave.Surname = ClientSendedUser[1].Surname;
            Db.SaveChanges();
            sw.Flush();
            sw.Close();
        }

        static public User CreateNewUser(PostedUser NewUser)
        {
            var CreateUser = new User();
            CreateUser.Name = NewUser.Name;
            CreateUser.Surname = NewUser.Surname;
            return CreateUser;
        }
    }
}

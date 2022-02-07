
using System.Net;
using System.Text.Json;

class Program
{
    static public int gId { get; set; } = 4;
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    class PostedUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public PostedUser()
        {

        }
        public PostedUser(User user)
        {
            Name = user.Name;
            Surname = user.Surname;
        }
    }
    static void Main(string[] args)
    {
        List<User> users = new List<User>();
        users.Add(new User() { Id = 1, Name = "John", Surname = "Doe" });
        users.Add(new User() { Id = 2, Name = "Jane", Surname = "Doe" });
        users.Add(new User() { Id = 3, Name = "Jack", Surname = "Doe" });
        //server
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:45678/");
        listener.Start();
        while (true)
        {
            Console.WriteLine("Lisining");
            var context = listener.GetContext();
            var request = context.Request;
            var response = context.Response;
            Console.WriteLine("Client Requested");

            response.AddHeader("Content-Type", "application/json");


            var sw = new StreamWriter(response.OutputStream);
            var sr = new StreamReader(request.InputStream);

            switch (request.HttpMethod)
            {
                case "POST":
                    var NewUser = JsonSerializer.Deserialize<PostedUser>(sr.ReadToEnd());

                    var CreateUser = new User();
                    CreateUser.Id = gId++;
                    CreateUser.Name = NewUser.Name;
                    CreateUser.Surname = NewUser.Surname;
                    users.Add(CreateUser);
                    sw.WriteLine("Readed");
                    sw.Flush();
                    sw.Close();
                    break;
                case "GET":
                    List<PostedUser> postedUsers = new List<PostedUser>();
                    foreach (var user in users)
                        postedUsers.Add(new PostedUser(user));
                    sw.WriteLine(JsonSerializer.Serialize<List<PostedUser>>(postedUsers));
                    sw.Flush();
                    sw.Close();
                    break;
                default:
                    break;
            }
        }



    }
}

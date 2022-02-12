
using System.Text;
using System.Text.Json;

class User
{
    public string Name { get; set; }
    public string Surname { get; set; }
}
class Program
{
    static void Main(string[] args)
    {
        var url = "http://localhost:45678/";
        //create httpClient
        var client = new HttpClient();
        while (true)
        {
            Console.WriteLine("1)Create User");
            Console.WriteLine("2.Get User");

            var userEnter = Console.ReadLine();
            if (userEnter == "1")
            {

                Console.WriteLine("Enter Name");
                var username = Console.ReadLine();
                Console.WriteLine("Enter Surname");
                var Surname = Console.ReadLine();

                User user = new User();
                user.Name = username;
                user.Surname = Surname;

                var message = new HttpRequestMessage(HttpMethod.Post, url);

                message.Content = new StringContent(JsonSerializer.Serialize<User>(user), Encoding.UTF8, "application/json");
                var response = client.SendAsync(message).Result;

            }
            else if (userEnter == "2")
            {
                Console.WriteLine("1)Get All User");
                Console.WriteLine("2)Get User By Name");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        var response = client.GetAsync(url).Result;

                        var ListUsers = JsonSerializer.Deserialize<List<User>>(response.Content.ReadAsStringAsync().Result);

                        foreach (var item in ListUsers)
                        {
                            Console.WriteLine(item.Name);
                            Console.WriteLine(item.Surname);
                            Console.WriteLine();
                        }
                        break;
                    case "2":
                        Console.WriteLine("enter User Max Name Lenght");
                        var input2 = Console.ReadLine();
                        var len=input2.Length;
                        url += $"?NameGreaterThan={len}&min=100&max=1000";

                        var response2 = client.GetAsync(url).Result;

                        var ListUsers2 = JsonSerializer.Deserialize<List<User>>(response2.Content.ReadAsStringAsync().Result);

                        foreach (var item in ListUsers2)
                        {
                            Console.WriteLine(item.Name);
                            Console.WriteLine(item.Surname);
                            Console.WriteLine();
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
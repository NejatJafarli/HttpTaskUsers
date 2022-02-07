
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
        //create httpClient
        var client = new HttpClient();
        while (true)
        {

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

                var message = new HttpRequestMessage(HttpMethod.Post, "http://localhost:45678/");

                message.Content = new StringContent(JsonSerializer.Serialize<User>(user), Encoding.UTF8, "application/json");
                var response = client.SendAsync(message).Result;
            }
            else if (userEnter == "2")
            {
                var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:45678/");
                var response = client.SendAsync(message).Result;

                var ListUsers = JsonSerializer.Deserialize<List<User>>(response.Content.ReadAsStringAsync().Result);

                foreach (var item in ListUsers)
                {
                    Console.WriteLine(item.Name);
                    Console.WriteLine(item.Surname);
                    Console.WriteLine();
                }
            }
        }




    }
}
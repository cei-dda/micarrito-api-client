using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace micarrito_api_client
{
    class User
    {
        [JsonPropertyName("id")]
        public long Id { set; get; }
        [JsonPropertyName("firstname")]
        public string Firstname { set; get; }
        [JsonPropertyName("lastname")]
        public string Lastname { set; get; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static async Task<User> GetUsers()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var task = client.GetStreamAsync("http://localhost:8080/users/1");

            var user = await JsonSerializer.DeserializeAsync<User>(await task);
            return user;
        }

        private static Task<HttpResponseMessage> SaveUser(User user)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonSerializer.Serialize(user);
            HttpContent content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var task = client.PostAsync("http://localhost:8080/users", content);
            return task;
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            User user = await GetUsers();
            Console.WriteLine("id: " + user.Id);
            Console.WriteLine("firstname: " + user.Firstname);
            Console.WriteLine("lastname: " + user.Lastname);

            User userToSave = new User();
            userToSave.Id = 20;
            userToSave.Firstname = "perdon";
            userToSave.Lastname = "me mando dotnet";

            await SaveUser(userToSave);

            Console.WriteLine("wiiiii no explote");
        }
    }
}

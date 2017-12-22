using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace TestClient
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();
        static async Task MainAsync()
        {
            Console.WriteLine("Please insert the secret");
            string pass = Console.ReadLine();//secret

            // discover endpoints from metadata

            DiscoveryClient discoClient = new DiscoveryClient("http://52.191.196.80/");
            discoClient.Policy.ValidateIssuerName = false;
            discoClient.Policy.RequireHttps = false;
            var disco = await discoClient.GetAsync();

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadLine();
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", pass);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadLine();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://52.191.193.31/api/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadLine();
        }
    }
}

using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DP.Client1
{
    class Program
    {


        private static async Task<string> GetToken()
        {
            using var client = new HttpClient();

            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = "https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0/",
                Policy =
                            {
                                ValidateEndpoints = false
                            }
            });

            if (disco.IsError)
                throw new InvalidOperationException(
                    $"No discovery document. Details: {disco.Error}");

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "fce95216-40e5-4a34-b041-f287e46532be",
                ClientSecret = "XWGsyzt9uM-Ia2SA8WE7~gvUJ~4og-U_1p",
                Scope = "api://fce95216-40e5-4a34-b041-f287e46532be/.default"
            };

            var token = await client.RequestClientCredentialsTokenAsync(tokenRequest);

            if (token.IsError)
                throw new InvalidOperationException($"Couldn't gather token. Details: {token.Error}");

            return token.AccessToken;
        }


        static async Task Main(string[] args)
        {

            Console.ReadKey();
            HttpClient client = new HttpClient();

            string token = await GetToken();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);

            Patient p = new Patient();
            p.Name = "Jan";
            p.Surname = "Kowalski";
            p.Age = 40;
            p.Email = "s9827bs@ms.wwsi.edu.pl";
            p.TestDate = DateTime.Now;

            string userJson = JsonSerializer.Serialize(p);

            await client.PutAsync("https://localhost:5001/api/Patients",
                new StringContent(userJson, Encoding.UTF8, "application/json"));

        }
    }

    public class Patient
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public DateTime TestDate { get; set; }

        public string Email { get; set; }

    }
}

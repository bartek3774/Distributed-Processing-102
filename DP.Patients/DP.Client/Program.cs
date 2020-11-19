using IdentityModel.Client;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DP.Client
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

        public static object JsonConvert { get; private set; }

        static async Task Main(string[] args)
        {
                   
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Add Patient");
            Console.WriteLine("2) List all registered");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddPatient();
                    return true;
                case "2":
                    PatientsPrint();
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }

        private static async void AddPatient()
        {
            Console.Clear();
            Console.WriteLine("###Patient registration###\n");
            Patient p = new Patient();
            Console.WriteLine("Name:");
            p.Name = Console.ReadLine();

            Console.WriteLine("Surname:");
            p.Surname = Console.ReadLine();

            Console.WriteLine("Age:");
            p.Age = Int16.Parse(Console.ReadLine());

            Console.WriteLine("Email:");
            p.Email = Console.ReadLine();

            p.TestDate = DateTime.Now;

            Console.WriteLine("\n\nPlease press any key to register");
            Console.ReadKey();
            HttpClient client = new HttpClient();

            //string token = await GetToken();


            var app = PublicClientApplicationBuilder.Create("fce95216-40e5-4a34-b041-f287e46532be")
                .WithAuthority("https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0/")
                .WithDefaultRedirectUri()
                .Build();

            var result = await app.AcquireTokenInteractive(new[] { "api://fce95216-40e5-4a34-b041-f287e46532be/.default" }).ExecuteAsync();

            string token = result.AccessToken;


            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);


            string userJson = System.Text.Json.JsonSerializer.Serialize(p);


            await client.PostAsync("https://localhost:5001/api/Patients",
                new StringContent(userJson, Encoding.UTF8, "application/json"));

        }


        public static void PatientsPrint()
        {
            Console.Clear();

            WebClient wc = new WebClient();

            var jsonString = wc.DownloadString("https://localhost:5001/api/Patients");

            JToken parsedJson = JToken.Parse(jsonString);

            Console.WriteLine(parsedJson);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

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

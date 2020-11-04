using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DP.Client
{
    class Program
    {
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

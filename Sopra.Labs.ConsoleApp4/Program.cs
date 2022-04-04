using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace Sopra.Labs.ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ZipInfo();
        }

        static void ZipInfo()
        {
            // GET http://api.zippopotam.us/es/28013
            // GET http://api.zippopotam.us/es/34815    //array en la propiedad "places"
            var http = new HttpClient();
            http.BaseAddress = new Uri("http://api.zippopotam.us/es/");

            Console.Write("Codigo postal: ");
            string postal = Console.ReadLine();

            HttpResponseMessage response = http.GetAsync(postal).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                Console.WriteLine($"Codigo postal: {data["post code"]}");
                Console.WriteLine($"Pais: {data["country"]} ({data["country abbreviation"]})");
                //Console.WriteLine($"places: {data["places"]}");
                Console.WriteLine();
                //Console.WriteLine($"Nº lugares: {data["places"].GetType()}");
                Console.WriteLine($"Nº lugares: {data["places"].Count}");
                foreach (var place in data["places"])
                {
                    Console.WriteLine($"Lugar: {place["place name"]}");
                    Console.WriteLine($"Posicion: {place["latitude"]}, {place["longitude"]}");
                    Console.WriteLine($"Provincia: {place["state"]} ({place["state abbreviation"]})");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }
}

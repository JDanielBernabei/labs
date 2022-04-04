using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Json;

namespace Sopra.Labs.ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ZipInfo();
        }

        static void Estudiante()
        {
            // Obtener los datos del estudiante 11. Dos metodos, extenso y abreviado
            //http://school.labs.com.es/api/students/11

            Console.Write("Estudiante: ");

            HttpResponseMessage response = http.GetAsync("http://school.labs.com.es/api/students/11").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine($"ID: {data["id"]}");
                Console.WriteLine($"Nombre: {data["firstName"]} {data["lastName"]}");
                Console.WriteLine($"Fecha de nacimiento: {data["dateOfBirth"]}");
                Console.WriteLine($"Clase ID: {data["classId"]}, Clase: {data["class"]}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }

            Console.WriteLine();

            // Metodo 2
            var data2 = http.GetFromJsonAsync<Student>("http://school.labs.com.es/api/students/11").Result;
            Console.WriteLine($"ID: {data2.Id}");
            Console.WriteLine($"Nombre: {data2.Firstname} {data2.Lastname}");
            Console.WriteLine($"Fecha de nacimiento: {data2.DateOfBirth}");
            Console.WriteLine($"Clase ID: {data2.ClassId}");
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

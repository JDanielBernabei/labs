using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Json;

using System.Collections;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Sopra.Labs.ConsoleApp4
{
    internal class Program
    {
        private static HttpClient http = new HttpClient();
        
        static void Main(string[] args)
        {
            http.BaseAddress = new Uri("https://openapi.emtmadrid.es/");
            parkings(EmtMadridGetToken());

        }
        public class ParkingResponse
        {
            [JsonProperty("data")]
            public List<ParkingData> Parkings { get; set; }
        }

        public class ParkingData
        {
            public string Name { get; set; }
            public string Address { get; set; }
            
            [JsonProperty("freeParking")]
            public int? PlazasLibres { get; set; }
        }

        /*
            public class ParkingResponse
            {
                public string Code { get; set; }
                public string Description { get; set; }

                [JsonProperty("datetime")]
                public DateTime DateTimeData { get; set; }
                public List<ParkingData> Data { get; set; }
            }
            public class ParkingData
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int? FreeParking { get; set; }
            }

         */

        static void parkings(string accessToken)
        {
            //Pedir un parking y dar el numero total de plazas. Hacer la suma usando Linq
            http.DefaultRequestHeaders.Clear();            
            http.DefaultRequestHeaders.Add("accessToken", accessToken);

            var response = http.GetAsync("v2/citymad/places/parkings/availability/").Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<ParkingResponse>(response.Content.ReadAsStringAsync().Result);
                // Comprobar que se ha recibido la respuesta correcta
                var parkingData = data.Parkings
                                    .Sum(r => r.PlazasLibres);
                Console.WriteLine($"Plazas: {parkingData}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        static void TimeArrivalBus(string accessToken)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("accessToken", accessToken);
            request.RequestUri = new Uri("https://openapi.emtmadrid.es/v2/transport/busemtmad/stops/888/arrives/");
            //string body = "{\"cultureInfo\":\"ES\",\"Text_StopRequired_YN\":\"Y\",\"Text_EstimationsRequired_YN\":\"Y\",\"Text_IncidencesRequired_YN\":\"N\",\"DateTime_Referenced_Incidencies_YYYYMMDD\":\"20220405\"}";
            //request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var body = new { cultureInfo = "ES", Text_StopRequired_YN = "Y", Text_EstimationsRequired_YN = "Y", Text_IncidencesRequired_YN = "Y", DateTime_Referenced_Incidencies_YYYYMMDD = "20220405" };
            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            
            request.Method = HttpMethod.Post;

            var response = http.Send(request);

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response2 = http.PostAsync($"transport/busemtmad/stops/{{numParada}}/arrives/", content);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                //Console.WriteLine($"Informacion de parada: {data}");
                var infoParada = data["data"][0]["Arrive"];
                foreach(var bus in infoParada)
                {
                    Console.WriteLine($"Linea: {bus["line"]}");
                    Console.WriteLine($"Llegada: {bus["estimateArrive"]}s");
                }                
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        static string EmtMadridGetToken()
        {
            http.DefaultRequestHeaders.Add("X-ClientId", "66f93756-02d8-4fa7-945f-d72a1e34816c");
            http.DefaultRequestHeaders.Add("passKey", "21FCE554F2F2975497079C6ECF1A3B253EC654193F275ED61862D3B4E2D634B4911B8C6F39CFEB28FF5869EF7CFA7057DC34D1269A7B0D80E5749062E18ECE60");

            var response = http.GetAsync("v2/mobilitylabs/user/login/").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine($"Access Token: {data["data"][0]["accessToken"]}");
                return data["data"][0]["accessToken"];
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
            return null;
        }

        static void DeleteStudent()
        {
            // Borrar
            // http.BaseAddress = new Uri("http://school.labs.com.es/api/students/");

            Student student = new Student();

            Console.Write("Id del estudiante a borar: ");
            Int32.TryParse(Console.ReadLine(), out int i);
            student.Id = i;

            var check = http.GetAsync(student.Id.ToString()).Result;
            string checkContent = check.Content.ReadAsStringAsync().Result;
            Console.Write($"Estudiante a borar: {checkContent}");

            Console.WriteLine("Confirmar? Y/N");
            if (Console.ReadLine() == "Y")
            {
                http.DeleteAsync(student.Id.ToString());
                var checkDeleted = http.GetAsync(student.Id.ToString()).Result;
                if (!checkDeleted.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Estudiante borrado:");
                    student = JsonConvert.DeserializeObject<Student>(checkContent);
                    Console.WriteLine($"Id: {student.Id}");
                    Console.WriteLine($"Nombre: {student.Firstname}");
                    Console.WriteLine($"Apellido: {student.Lastname}");
                    Console.WriteLine($"Fecha de nacimiento: {student.DateOfBirth}");
                    Console.WriteLine($"Clase: {student.ClassId}");

                }
                else
                {
                    Console.WriteLine($"Error: Registro no borrado");
                }
            }
            else
            {
                Console.WriteLine("Borrado abortado");
            }
        }

        static void PutStudent()
        {
            // Actualizar
            // http.BaseAddress = new Uri("http://school.labs.com.es/api/students/");

            Student student = new Student();

            Console.Write("Id del estudiante a modificar: ");
            Int32.TryParse(Console.ReadLine(), out int i);
            student.Id = i;

            var check = http.GetAsync(student.Id.ToString()).Result;
            string checkContent = check.Content.ReadAsStringAsync().Result;
            Console.WriteLine($"Estudiante a modificar: {checkContent}");
            Console.WriteLine("Confirmar? Y/N");
            if (Console.ReadLine() == "Y")
            {
                Console.Write("Nombre: ");
                student.Firstname = Console.ReadLine();

                Console.Write("Apellido: ");
                student.Lastname = Console.ReadLine();

                Console.Write("Fecha de nacimiento:");
                student.DateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.Write("Clase: ");
                Int32.TryParse(Console.ReadLine(), out int j);
                student.ClassId = j;

                var content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");

                var response = http.PutAsync(student.Id.ToString(), content).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Estudiante modificado");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Modificacion abortada");
            }
        }

        static void PostStudent()
        {
            // Crear una instancia de student
            // Rellenamo con datos cogidos por consola, menos ID
            // Conexion Post -> 201 Created -> Student
            // Mostramos el ID asignado
            http.BaseAddress = new Uri("http://school.labs.com.es/api/students/");

            Student student = new Student();

            Console.Write("Nombre: ");
            student.Firstname = Console.ReadLine();

            Console.Write("Apellido: ");
            student.Lastname = Console.ReadLine();

            Console.Write("Fecha de nacimiento:");
            student.DateOfBirth = DateTime.Parse(Console.ReadLine());

            Console.Write("Clase: ");
            Int32.TryParse(Console.ReadLine(), out int j);
            student.ClassId = j;

            var content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
            //Console.WriteLine(JsonConvert.SerializeObject(student));

            var response = http.PostAsync("", content).Result;
            // var response2 = http.PostAsJsonAsync<Student>("", student).Result;  // En try catch para errores

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var studenResponse = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);
                Console.Write($"ID del estudiante creado: {studenResponse.Id}");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
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

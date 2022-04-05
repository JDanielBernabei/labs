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
        private static HttpClient http = new HttpClient();
        static void Main(string[] args)
        {
            PostStudent();
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

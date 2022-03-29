using System;

namespace Sopra.Labs.ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalcularValores();
        }

        static void CalcularLetraDni()
        {
            int numeroDni = 0;
            char[] letras = { 'T', 'R', 'W', 'A', 'G', 'M', 'Y', 'F', 'P', 'D', 'X', 'B', 'N', 'J', 'Z', 'S', 'Q', 'V', 'H', 'L', 'C', 'K', 'E' };

            Console.WriteLine("Introduzca DNI (sin letra)");
            bool result = int.TryParse(Console.ReadLine(), out numeroDni);

            if (result)
            {
                Console.WriteLine($"La letra del DNI es: {letras[numeroDni % 23]}");
            } else
            {
                Console.WriteLine("El número introducido no es correcto");
            }
    }

        static void MostrarTablaMultiplicar()
        {
            int num = 0;
            Console.WriteLine("Introduzca un numero");
            bool result = int.TryParse(Console.ReadLine(), out num);

            //Utilizando un For
            Console.WriteLine($"(Bucle for) - Tabla de multiplicar del {num}:");
            for (int i= 0; i < 11; i++)
            {
                Console.WriteLine($"{num} x {i} = {num * i}");
            }

            //Utilizando un While
            int j = 0;              
            Console.WriteLine($"(Bucle while) - Tabla de multiplicar del {num}:");
            while (j < 11)
            {
                Console.WriteLine($"{num} x {j} = {num * j++}");
            }
        }

        static void MostrarValores()
        {
            // desde el valor de inicio al valor final
            // en diferentes saltos
            int inicio, final, salto;

            Console.WriteLine("Introduzca valor de inicio");
            bool result = int.TryParse(Console.ReadLine(), out inicio);

            Console.WriteLine("Introduzca valor final");
            result = int.TryParse(Console.ReadLine(), out final);

            Console.WriteLine("Introduzca el salto");
            result = int.TryParse(Console.ReadLine(), out salto);

            for (int i = inicio; i < final; i += salto) //No me deja usar inicio como contador
            {
                Console.WriteLine($"{i}");
            }
            Console.WriteLine($"{final}");
        }

        static void CalcularValores()
        {
            // numero de valores?
            // los almacenamos en un array
            // calculos: max, min, media, suma

            int size = 0;

            Console.WriteLine("Introduzca numero de valores");
            bool result = int.TryParse(Console.ReadLine(), out size);

            if (size < 0)
            {
                Console.WriteLine("Tamaño incorrecto");
                return;
            }

            int[] array = new int[size];
            int max = Int32.MaxValue, min = Int32.MinValue, mean, summ = 0;

            Console.WriteLine("Introduzca valores");
            for (int i = 0; i < size; i++)
            {
                result = int.TryParse(Console.ReadLine(), out array[i]);
                if (i == 0)
                {
                    max = array[i];
                    min = array[i];
                }
                if (max < array[i]) max = array[i];
                if (min > array[i]) min = array[i];
                summ += array[i];
            }
            mean = summ / size;

            Console.WriteLine("Calculos:");
            Console.WriteLine($"Maximo = {max}, Minimo = {min}, Media = {mean}, Suma ={summ}");
        }        
    }
}

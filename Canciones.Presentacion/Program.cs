using System;
using System.Threading.Tasks;
using Canciones.Entidades;
using Canciones.Logica;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;

namespace Canciones.Presentacion
{
    class Program
    {
        private static CancionService _service;
        private static string connectionString;

        static async Task Main(string[] args)
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");
            _service = new CancionService(connectionString);

            Console.WriteLine("* SONICA *\n");

            bool exit = false;
            while (!exit)
            {
                MostrarMenu();
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ListarCanciones();
                        break;
                    case "2":
                        await AgregarCancion();
                        break;
                    case "3":
                        await BuscarCanciones();
                        break;
                    case "4":
                        await EliminarCancion();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Gracias por usar SONICA");
                        break;
                    default:
                        Console.WriteLine("Opcion no valida. Intente de nuevo");
                        break;
                }
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("* MENÚ PRINCIPAL *");
            Console.WriteLine("1. Listar todas las canciones");
            Console.WriteLine("2. Agregar nueva cancion");
            Console.WriteLine("3. Buscar canciones");
            Console.WriteLine("4. Eliminar cancion");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        static async Task ListarCanciones()
        {
            Console.WriteLine("\n--- LISTADO DE CANCIONES ---");
            var songs = await _service.ObtenerTodasAsync();
            if (songs.Count == 0)
            {
                Console.WriteLine("No hay canciones registradas");
                return;
            }

            foreach (var cancion in songs)
            {
                
                cancion.MostrarInfo();
            }
        }

        static async Task AgregarCancion()
        {
            Console.WriteLine("\n--- AGREGAR NUEVA CANCION ---");
            Console.Write("Titulo: ");
            string tittle = Console.ReadLine();
            Console.Write("Artista: ");
            string artist = Console.ReadLine();
            Console.Write("Genero: ");
            string genre = Console.ReadLine();
            Console.Write("Fecha: ");
            string yearInput = Console.ReadLine();
            int? year = string.IsNullOrWhiteSpace(yearInput) ? (int?)null : int.Parse(yearInput);


            var newSong = new Cancion(tittle, artist, genre, year);

            try
            {
                await _service.AgregarCancionAsync(newSong);
                Console.WriteLine("Cancion agregada exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task BuscarCanciones()
        {
            Console.WriteLine("\n--- BUSQUEDA DE CANCIONES ---");
            Console.WriteLine("1. Buscar por titulo");
            Console.WriteLine("2. Buscar por titulo y artista");
            Console.Write("Opcion: ");
            string opt = Console.ReadLine();

            List<Cancion> results = new List<Cancion>();

            if (opt == "1")
            {
                Console.Write("Ingrese titulo (o parte): ");
                string tittle = Console.ReadLine();
                results = await _service.BuscarCancionesAsync(tittle); 
            }
            else if (opt == "2")
            {
                Console.Write("Titulo: ");
                string tittle = Console.ReadLine();
                Console.Write("Artista: ");
                string artist = Console.ReadLine();
                results = await _service.BuscarCancionesAsync(tittle, artist); // Sobrecarga con 2 parámetros
            }
            else
            {
                Console.WriteLine("Opcion invalida");
                return;
            }

            if (results.Count == 0)
                Console.WriteLine("No se encontraron coincidencias");
            else
            {
                Console.WriteLine($"\nResultados ({results.Count}):");
                foreach (var c in results)
                    c.MostrarInfo(); 
            }
        }

        static async Task EliminarCancion()
        {
            Console.WriteLine("\n--- ELIMINAR CANCION ---");
            await ListarCanciones();
            Console.Write("Ingrese el ID de la cancion a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    bool delete = await _service.EliminarCancionAsync(id);
                    if (delete)
                        Console.WriteLine("Cancion eliminada correctamente");
                    else
                        Console.WriteLine("No se encontro una cancion con ese ID");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID invalido");
            }
        }
    }
}
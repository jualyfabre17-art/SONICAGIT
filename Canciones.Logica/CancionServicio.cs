using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Canciones.Datos;
using Canciones.Entidades;

namespace Canciones.Logica
{
    public class CancionService
    {
        private readonly ICancionRepository _repository;

        public CancionService(string connectionString)
        {
            _repository = new CancionRepository(connectionString);
        }

        
        public async Task<List<Cancion>> ObtenerTodasAsync()
        {
            return await _repository.ObtenerTodasAsync();
        }

        
        public async Task<bool> AgregarCancionAsync(Cancion cancion)
        {
            if (string.IsNullOrWhiteSpace(cancion.Tittle))
                throw new ArgumentException("El titulo es obligatorio");
            if (string.IsNullOrWhiteSpace(cancion.Artist))
                throw new ArgumentException("El artista es obligatorio");
            if (string.IsNullOrWhiteSpace(cancion.Genre))
                throw new ArgumentException("El genero es obligatorio");

            await _repository.AgregarAsync(cancion);
            return true;
        }

        
        public async Task<bool> EliminarCancionAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID invalido");
            return await _repository.EliminarAsync(id);
        }

        
        public async Task<List<Cancion>> BuscarCancionesAsync(string titulo)
        {
            return await _repository.BuscarPorTituloAsync(titulo);
        }

        
        public async Task<List<Cancion>> BuscarCancionesAsync(string titulo, string artista)
        {
            return await _repository.BuscarPorTituloYArtistaAsync(titulo, artista);
        }

        
        public async Task<Cancion> ObtenerPorIdAsync(int id)
        {
            return await _repository.ObtenerPorIdAsync(id);
        }

        public async Task<bool> ActualizarAsync(Cancion cancion)
        {
            if (cancion.Id <= 0) throw new ArgumentException("ID invalido");
            if (string.IsNullOrWhiteSpace(cancion.Tittle)) throw new ArgumentException("El titulo es obligatorio");
            if (string.IsNullOrWhiteSpace(cancion.Artist)) throw new ArgumentException("El artista es obligatorio");
            if (string.IsNullOrWhiteSpace(cancion.Genre)) throw new ArgumentException("El genero es obligatorio");
            await _repository.ActualizarAsync(cancion);
            return true;
        }
       
    }
}

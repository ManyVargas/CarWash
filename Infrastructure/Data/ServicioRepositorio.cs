using Core.Entities;
using Core.Interfaces;
using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ServicioRepositorio : IServicioRepositorio
    {
        private readonly AppDbContext _context;

        public ServicioRepositorio(AppDbContext context)
        {
            _context = context;
        }
        public async Task ActualizarServicioAsync(Servicio servicio)
        {
            try
            {
                var servicioExistente = await _context.Servicios.FirstOrDefaultAsync(s => s.Nombre == servicio.Nombre);

                if (servicioExistente == null)
                {
                    throw new Exception("No se encontro un servicio con ese nombre.");
                }

                servicioExistente.Nombre = servicio.Nombre ?? servicioExistente.Nombre;
                servicioExistente.Descripcion = servicio.Descripcion ?? servicioExistente.Descripcion;
                servicioExistente.Precio = servicio?.Precio ?? servicioExistente.Precio;
                servicioExistente.DuracionMinutos = servicio?.DuracionMinutos ?? servicioExistente.DuracionMinutos;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado al actualizar el servicio: " + ex.Message);
            }
        }

        public async Task EliminarServicioAsync(string nombre)
        {
            try
            {
                var servicioExistente = await _context.Servicios.FirstOrDefaultAsync(s => s.Nombre == nombre);

                if (servicioExistente == null)
                {
                    throw new Exception("No se encontro un servicio con ese nombre.");
                }

                _context.Servicios.Remove(servicioExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al intentar eliminar el servicio: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Servicio>> ObtenerServiciosAsync()
        {
            try
            {
                return await _context.Servicios.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error: {ex.Message}");
            }
        }

        public async Task RegistrarServicioAsync(Servicio servicio)
        {
            try
            {
                var existe = await _context.Servicios.FirstOrDefaultAsync(s => s.Nombre == servicio.Nombre);

                if (existe != null)
                {
                    throw new Exception("Ya existe un servicio con este nombre.");
                }

                await _context.Servicios.AddAsync(servicio);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar el servicio.", ex);
            }
        }

        public async Task<Servicio> ObtenerServicioAsync(string nombre)
        {
            try
            {
                var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Nombre == nombre);

                if (servicio == null)
                {
                    throw new Exception("No se encontro ningun servicio con el dato proporcionado.");
                }

                return servicio;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al consultar el servicio.", ex);
            }
        }
    }
}

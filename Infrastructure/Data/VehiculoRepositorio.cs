using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class VehiculoRepositorio : IVehiculoRepositorio
    {
        private readonly AppDbContext _context;

        public VehiculoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                var existe = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == vehiculo.Placa);

                if (existe == null)
                {
                    throw new Exception("No se encontro un vehiculo con el dato proporcionado.");
                }

                existe.Anio = vehiculo?.Anio ?? existe.Anio;
                existe.Color = vehiculo?.Color ?? existe.Color;
                existe.Marca = vehiculo?.Marca ?? existe.Marca;
                existe.Modelo = vehiculo?.Modelo ?? existe.Modelo;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado al actualizar el cliente, Repositorio: " + ex.Message);
            }
        }

        public async Task RegistrarVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                var existe = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == vehiculo.Placa);
                if (existe != null)
                {
                    throw new Exception("Ya existe un vehiculo con estos datos.");
                }

                await _context.Vehiculos.AddAsync(vehiculo);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar el vehiculo.", ex);
            }
        }

        public async Task<IEnumerable<Vehiculo>> ObtenerVehiculosAsync()
        {
            try
            {
                var vehiculos = await _context.Vehiculos.ToListAsync();

                return vehiculos.Select(v => new Vehiculo
                {
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Anio = v.Anio,
                    ClienteId = v.ClienteId,
                    Color = v.Color,
                    Placa = v.Placa
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error desde el repositorio{ex.Message}");
            }
        }

        public async Task<Vehiculo> ObtenerVehiculoAsync(string placa)
        {
            try
            {
                var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa);

                if(vehiculo == null)
                {
                    throw new Exception("No se encontro ningun cliente con el dato proporcionado.");
                }

                return vehiculo;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al consultar el cliente.", ex);
            }
        }
    }
}

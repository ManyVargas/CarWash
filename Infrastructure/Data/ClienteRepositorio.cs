using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml;

namespace Infrastructure.Data
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly AppDbContext _context;
        

        public ClienteRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarClienteAsync(Cliente cliente)
        {
            
            try
            {
                var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.Telefono == cliente.Telefono || c.Email == cliente.Email);

                if (clienteExistente == null)
                {
                    throw new Exception("No se encontro un cliente con el dato proporcionado.");
                }

                clienteExistente.Nombre = cliente.Nombre ?? clienteExistente.Nombre;
                clienteExistente.Apellido = cliente.Apellido ?? clienteExistente.Apellido;
                clienteExistente.Telefono = cliente.Telefono ?? clienteExistente.Telefono;
                clienteExistente.Email = cliente.Email ?? clienteExistente.Email;
                clienteExistente.Direccion = cliente.Direccion ?? clienteExistente.Direccion;

                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado al actualizar el cliente: " + ex.Message);
            }
        }

        public async Task AgregarClienteAsync(Cliente cliente)
        {
            try
            {
                var existe = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == cliente.Email);
                if (existe != null)
                {
                    throw new Exception("Ya existe un cliente con este correo electronico.");
                }
                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar el cliente.", ex);
            }
        }

        public async Task<Cliente> ObtenerClienteAsync(string? email = null, string? telefono = null)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email || c.Telefono == telefono);

                if(cliente == null)
                {
                    throw new Exception("No se encontro ningun cliente con el dato proporcionado.");
                }

                return cliente;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al consultar el cliente.", ex);
            }
        }

        public async Task EliminarClienteAsync(string? email, string? telefono)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email || c.Telefono == telefono);

                if (cliente == null)
                {
                    throw new Exception("No se encontro un cliente con el dato proporcionado.");
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al intentar eliminar el cliente: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosClientesAsync()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error: {ex.Message}");
            }
        }
    }
}

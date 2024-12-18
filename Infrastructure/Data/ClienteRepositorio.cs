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

        public async Task<bool> ActualizarCliente(Cliente cliente)
        {
            
            try
            {
                var clienteExiste = await _context.Cliente.FirstOrDefaultAsync(c => c.Telefono == cliente.Telefono);

                if (clienteExiste == null)
                {
                    return false;
                }

                clienteExiste.Nombre = cliente.Nombre;
                clienteExiste.Apellido = cliente.Apellido;
                clienteExiste.Telefono = cliente.Telefono;
                clienteExiste.Email = cliente.Email;
                clienteExiste.Direccion = cliente.Direccion;

                _context.Cliente.Update(clienteExiste);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado: " + ex.Message);
            }
        }

        public async Task AgregarClienteAsync(Cliente cliente)
        {
            try
            {
                await _context.Cliente.AddAsync(cliente);
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
                //Verificar que haya uno de los dos datos presentes
                if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(telefono))
                    throw new ArgumentException("Debe proporcionar un email o teléfono para buscar el cliente.");

                //hacer la query dinamica
                var query = _context.Cliente.AsQueryable();

                if (!string.IsNullOrWhiteSpace(email))
                {
                    query.Where(u => u.Email == email);
                }

                if (!string.IsNullOrWhiteSpace(telefono))
                {
                    query.Where(u => u.Telefono == telefono);

                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al consultar el cliente.", ex);
            }
        }
    }
}

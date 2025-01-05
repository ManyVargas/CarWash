using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IClienteRepositorio
    {
        Task AgregarClienteAsync(Cliente cliente);
        Task<Cliente> ObtenerClienteAsync(string email = null, string telefono = null);
        Task ActualizarClienteAsync(Cliente cliente);
        Task EliminarClienteAsync(string email = null, string telefono = null);
        Task<IEnumerable<Cliente>> ObtenerTodosClientesAsync(); 
    }
}

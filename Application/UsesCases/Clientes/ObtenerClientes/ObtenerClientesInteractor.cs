using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Clientes.ObtenerClientes
{
    public class ObtenerClientesInteractor
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ObtenerClientesInteractor(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<IEnumerable<ObtenerClientesResponse>> Handle()
        {
            try
            {
                var clientes = await _clienteRepositorio.ObtenerTodosClientesAsync();

                if (clientes == null || !clientes.Any())
                {
                    return Enumerable.Empty<ObtenerClientesResponse>();
                }

                
                return clientes.Select(c => new ObtenerClientesResponse
                {
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Email = c.Email,
                    Telefono = c.Telefono,
                    Direccion = c.Direccion
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error al obtener los clientes.", ex);
            }
        }
    }
}

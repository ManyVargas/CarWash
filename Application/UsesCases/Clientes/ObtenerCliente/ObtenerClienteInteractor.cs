using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Clientes.ObtenerCliente
{
    public class ObtenerClienteInteractor
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ObtenerClienteInteractor(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<ObtenerClienteResponse> Handle(ObtenerClienteRequest obtenerClienteRequest)
        {
            if (string.IsNullOrEmpty(obtenerClienteRequest.Email) && string.IsNullOrEmpty(obtenerClienteRequest.Telefono))
            {
                return new ObtenerClienteResponse {Exito = false, Mensaje = "Debe introducir un Email o Telefono valido." };
            }

            var clienteTelefono = await _clienteRepositorio.ObtenerClienteAsync(telefono: obtenerClienteRequest.Telefono);
            if (clienteTelefono != null)
            {
                return new ObtenerClienteResponse
                {
                    Exito = true,
                    Nombre = clienteTelefono.Nombre,
                    Apellido = clienteTelefono.Apellido,
                    Telefono = clienteTelefono.Telefono,
                    Email = clienteTelefono.Email,
                    Direccion = clienteTelefono.Direccion
                };
            }

            var clienteEmail = await _clienteRepositorio.ObtenerClienteAsync(email: obtenerClienteRequest.Email);
            if (clienteEmail != null)
            {
                return new ObtenerClienteResponse 
                { 
                    Exito = true,
                    Nombre = clienteEmail.Nombre,
                    Apellido = clienteEmail.Apellido,
                    Telefono = clienteEmail.Telefono,
                    Email = clienteEmail.Email,
                    Direccion = clienteEmail.Direccion
                };
            }

            return new ObtenerClienteResponse { Exito = false, Mensaje = "No se encontró un cliente con el Email o Telefono proporcionado." };
        }
    }
}

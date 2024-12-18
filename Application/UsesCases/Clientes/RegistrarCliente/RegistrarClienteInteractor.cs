using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Clientes.RegistrarCliente
{
    public class RegistrarClienteInteractor
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public RegistrarClienteInteractor(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<RegistrarClienteResponse> Handle(RegistrarClienteRequest clienteRequest)
        {
            if (clienteRequest == null)
            {
                return new RegistrarClienteResponse { Exito = false, Mensaje = "Debe ingresar un cliente valido" };
            }

            if (await _clienteRepositorio.ObtenerClienteAsync(telefono: clienteRequest.Telefono) == null
                || await _clienteRepositorio.ObtenerClienteAsync(email: clienteRequest.Email) == null)
            {
                return new RegistrarClienteResponse { Exito = false, Mensaje = "No se encontro un cliente con el dato proporcionado" };
            }

            var nuevoCliente = new Cliente
            {
                Nombre = clienteRequest.Nombre,
                Apellido = clienteRequest.Apellido,
                Telefono = clienteRequest.Telefono,
                Direccion = clienteRequest.Direccion,
                Email = clienteRequest.Email
            };

            await _clienteRepositorio.AgregarClienteAsync(nuevoCliente);

            return new RegistrarClienteResponse { Exito = true, Mensaje = "Cliente agregado exitosamente.", ClienteId = nuevoCliente.ClienteId };
        }
    }
}

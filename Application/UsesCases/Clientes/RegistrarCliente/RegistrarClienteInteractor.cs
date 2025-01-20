using Core.Entities;
using Core.Interfaces;

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
            try
            {
                if (clienteRequest == null)
                {
                    return new RegistrarClienteResponse { Exito = false, Mensaje = "Debe ingresar un cliente valido" };
                }

                var cliente = new Cliente
                {
                    Nombre = clienteRequest.Nombre,
                    Apellido = clienteRequest.Apellido,
                    Telefono = clienteRequest.Telefono,
                    Direccion = clienteRequest.Direccion,
                    Email = clienteRequest.Email
                };

                await _clienteRepositorio.AgregarClienteAsync(cliente);


                return new RegistrarClienteResponse { Exito = true, Mensaje = "Cliente agregado exitosamente.", ClienteId = cliente.ClienteId };
            }
            catch (Exception ex)
            {
                return new RegistrarClienteResponse { Exito = false, Mensaje = $"Error inesperado al registrar el cliente: {ex.Message}" };
            }
            
        }
    }
}

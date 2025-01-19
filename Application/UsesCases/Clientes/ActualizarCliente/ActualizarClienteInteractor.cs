using Core.Entities;
using Core.Interfaces;


namespace Application.UsesCases.Clientes.ActualizarCliente
{
    public class ActualizarClienteInteractor
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ActualizarClienteInteractor(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<ActualizarClienteResponse> Handle(ActualizarClienteRequest actualizarClienteRequest)
        {
            if (actualizarClienteRequest == null)
            {
                return new ActualizarClienteResponse { Exito = false, Mensaje = "Debe ingresar datos validos." };
            }

            var cliente = new Cliente
            {
                Nombre = actualizarClienteRequest.Nombre,
                Apellido = actualizarClienteRequest.Apellido,
                Telefono = actualizarClienteRequest.Telefono,
                Direccion = actualizarClienteRequest.Direccion,
                Email = actualizarClienteRequest.Email
            };

            try
            {
                await _clienteRepositorio.ActualizarClienteAsync(cliente);


                return new ActualizarClienteResponse { Exito = true, Mensaje = "Cliente actualizado exitosamente" };
            }
            catch (Exception ex)
            {

                return new ActualizarClienteResponse { Exito = false, Mensaje = $"Error al actualizar el cliente: {ex.Message}" };
            }

        }
    }
}

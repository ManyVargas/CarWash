using Core.Entities;
using Core.Interfaces;


namespace Application.UsesCases.Clientes.ActualizarCliente
{
    public class ActualizarClienteInteractor
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IPublicadorMensajes _publicadorMensajes;

        public ActualizarClienteInteractor(IClienteRepositorio clienteRepositorio, IPublicadorMensajes publicadorMensajes)
        {
            _clienteRepositorio = clienteRepositorio;
            _publicadorMensajes = publicadorMensajes;
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

                var mensaje = new { TipoAccion = "ActualizarCliente", Datos = cliente };
                await _publicadorMensajes.PublicarAsync("Clientes", mensaje);


                return new ActualizarClienteResponse { Exito = true, Mensaje = "Cliente actualizado exitosamente" };
            }
            catch (Exception ex)
            {

                return new ActualizarClienteResponse { Exito = false, Mensaje = $"Error al actualizar el cliente: {ex.Message}" };
            }

        }
    }
}

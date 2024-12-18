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
            if (string.IsNullOrEmpty(actualizarClienteRequest.Telefono))
            {
                return new ActualizarClienteResponse { Exito = false, Mensaje = "Debe ingresar un telefono valido." };
            }

            //var cliente = await _clienteRepositorio.ObtenerClienteAsync(telefono: actualizarClienteRequest.Telefono);

            //if (cliente == null)
            //{
            //    return new ActualizarClienteResponse { Exito = false, Mensaje = "No se encontro un cliente con ese telefono." };
            //}
            var cliente = new Cliente
            {
                Telefono = actualizarClienteRequest.Telefono
            };

            //cliente.Nombre = actualizarClienteRequest.Nombre ?? cliente.Nombre;
            //cliente.Apellido = actualizarClienteRequest.Apellido ?? cliente.Apellido;
            //cliente.Email = actualizarClienteRequest.Email ?? cliente.Email;
            //cliente.Email = actualizarClienteRequest.Telefono ?? cliente.Telefono;
            //cliente.Direccion = actualizarClienteRequest.Direccion ?? cliente.Direccion;


            if (await _clienteRepositorio.ActualizarCliente(cliente))
            {
                return new ActualizarClienteResponse { Exito = true, Mensaje = "Cliente actualizado exitosamente" };
            }



            return new ActualizarClienteResponse { Exito = false, Mensaje = "Error al actualizar el cliente, intente de nuevo." };

        }
    }
}

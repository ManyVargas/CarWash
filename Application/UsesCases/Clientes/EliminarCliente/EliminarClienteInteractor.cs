using Core.Interfaces;


namespace Application.UsesCases.Clientes.EliminarCliente
{
    public class EliminarClienteInteractor
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public EliminarClienteInteractor(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<EliminarClienteResponse> Handle(EliminarClienteRequest eliminarClienteRequest)
        {
            if (string.IsNullOrEmpty(eliminarClienteRequest.Telefono) && string.IsNullOrEmpty(eliminarClienteRequest.Email))
            {
                return new EliminarClienteResponse { Exito = false, Mensaje = "Debe ingresar un email o un telefono valido." };
            }


            try
            {

                await _clienteRepositorio.EliminarClienteAsync(eliminarClienteRequest.Email, eliminarClienteRequest.Telefono);
                return new EliminarClienteResponse { Exito = true, Mensaje = "Cliente eliminado exitosamente." };

            }
            catch (Exception ex)
            {
                return new EliminarClienteResponse { Exito = false, Mensaje = $"Error al eliminar el cliente: {ex.Message}" };
            }
        }
    }
}

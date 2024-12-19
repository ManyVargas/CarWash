using Core.Interfaces;


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
            try
            {
                if (string.IsNullOrEmpty(obtenerClienteRequest.Email) && string.IsNullOrEmpty(obtenerClienteRequest.Telefono))
                {
                    return new ObtenerClienteResponse {Exito = false, Mensaje = "Debe proporcionar un email o telefono valido."};
                }

                var cliente = await _clienteRepositorio.ObtenerClienteAsync(obtenerClienteRequest.Email, obtenerClienteRequest.Telefono);

                if (cliente != null)
                {
                    return new ObtenerClienteResponse
                    {
                        Exito = true,
                        Nombre = cliente.Nombre,
                        Apellido = cliente.Apellido,
                        Telefono = cliente.Telefono,
                        Email = cliente.Email,
                        Direccion = cliente.Direccion
                    };
                }

                return new ObtenerClienteResponse {Exito = false, Mensaje = "Cliente no enccontrado." };

            }
            catch (Exception ex)
            {
                return new ObtenerClienteResponse
                {
                    Exito = false,
                    Mensaje = $"Ocurrió un error al obtener el cliente: {ex.Message}"
                };
            }
            
        }
    }
}

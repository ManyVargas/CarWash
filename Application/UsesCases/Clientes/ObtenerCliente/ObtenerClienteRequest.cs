

using System.Diagnostics.CodeAnalysis;

namespace Application.UsesCases.Clientes.ObtenerCliente
{
    public class ObtenerClienteRequest
    {
        [AllowNull]
        public string Email { get; set; }
        [AllowNull]
        public string Telefono { get; set; }
    }
}

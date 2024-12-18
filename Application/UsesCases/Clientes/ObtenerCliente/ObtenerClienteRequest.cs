using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Clientes.ObtenerCliente
{
    public class ObtenerClienteRequest
    {
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}

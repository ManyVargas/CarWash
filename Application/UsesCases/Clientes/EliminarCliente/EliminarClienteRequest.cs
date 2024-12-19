using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Clientes.EliminarCliente
{
    public class EliminarClienteRequest
    {
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}

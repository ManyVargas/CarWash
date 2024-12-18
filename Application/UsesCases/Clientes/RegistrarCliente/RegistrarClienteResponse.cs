using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Clientes.RegistrarCliente
{
    public class RegistrarClienteResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int ClienteId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Facturas.GenerarFactura
{
    public class GenerarFacturaDetalleRequest
    {
        public int FacturaId { get; set; }
        public string NombreServicio { get; set; }
        public int Cantidad { get; set; }
    }
}

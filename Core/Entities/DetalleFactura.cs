using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DetalleFactura
    {
        public int DetalleId { get; set; }
        public int FacturaId { get; set; }
        public int ServicioId { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}

using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Facturas.GenerarFactura
{
    public class GenerarFacturaResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public Factura Factura { get; set; }
    }
}

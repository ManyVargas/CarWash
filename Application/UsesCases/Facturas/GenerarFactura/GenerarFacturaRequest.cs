using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Facturas.GenerarFactura
{
    public class GenerarFacturaRequest
    {
        public string ClienteTelefono { get; set; }
        public string ClienteEmail { get; set; }
        public string UsuarioTelefono { get; set; }
        public string UsuarioEmail { get; set; }
        public int ClienteId { get; set; }
        //public Cliente Cliente { get; set; }
        public int UsuarioId { get; set; }
        //public Usuario Usuario { get; set; }
        public string Metodo_Pago { get; set; }
        public List<GenerarFacturaDetalleRequest> Detalles { get; set; }
    }
}

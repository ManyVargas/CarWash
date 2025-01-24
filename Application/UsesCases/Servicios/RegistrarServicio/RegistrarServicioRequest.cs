using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.RegistrarServicio
{
    public class RegistrarServicioRequest
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }
        public string DuracionMinutos { get; set; }
    }
}

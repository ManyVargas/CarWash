using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.RegistrarServicio
{
    public class RegistrarServicioResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int ServicioId { get; set; }
    }
}

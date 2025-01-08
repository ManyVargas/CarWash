using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Servicio
    {
        public int ServicioId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
    }
}

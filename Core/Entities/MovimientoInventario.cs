using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MovimientoInventario
    {
        public int MovimientoId { get; set; }
        public int ProductoId { get; set; }
        public string Tipo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public string Observaciones { get; set; }
    }
}

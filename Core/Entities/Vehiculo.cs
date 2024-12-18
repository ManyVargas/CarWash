using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Vehiculo
    {
        public  int VehiculoId { get; set; }
        public int ClienteId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public DateTime Anio { get; set; }
        public string Color { get; set; }
        public string Matricula { get; set; }
    }
}

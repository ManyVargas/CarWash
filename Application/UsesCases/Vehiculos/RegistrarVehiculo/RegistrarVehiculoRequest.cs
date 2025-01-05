using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Vehiculos.RegistrarVehiculo
{
    public class RegistrarVehiculoRequest
    {
        public int ClienteId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Color { get; set; }
        public string Placa { get; set; }
    }
}

using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IVehiculoRepositorio
    {
        Task RegistrarVehiculoAsync(Vehiculo vehiculo);
        Task ActualizarVehiculoAsync(Vehiculo vehiculo);
        Task<IEnumerable<Vehiculo>> ObtenerVehiculosAsync();
        Task<Vehiculo> ObtenerVehiculoAsync(string placa);

    }
}

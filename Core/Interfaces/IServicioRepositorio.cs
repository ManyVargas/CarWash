using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IServicioRepositorio
    {
        Task RegistrarServicioAsync(Servicio servicio);
        Task ActualizarServicioAsync(Servicio servicio);
        Task EliminarServicioAsync(string nombre);
        Task<IEnumerable<Servicio>> ObtenerServiciosAsync();
        Task<Servicio> ObtenerServicioAsync(string nombre);
    }
}

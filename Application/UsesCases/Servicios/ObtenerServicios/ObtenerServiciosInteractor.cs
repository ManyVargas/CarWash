using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.ObtenerServicios
{
    public class ObtenerServiciosInteractor
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public ObtenerServiciosInteractor(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

        public async Task<IEnumerable<ObtenerServiciosResponse>> Handle()
        {
            try
            {
                var servicios = await _servicioRepositorio.ObtenerServiciosAsync();

                if (servicios == null || !servicios.Any())
                {
                    return Enumerable.Empty<ObtenerServiciosResponse>();
                }

                return servicios.Select(s => new ObtenerServiciosResponse
                {
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    Precio = s.Precio,
                    DuracionMinutos = s.DuracionMinutos,
                });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error al obtener los servicios.", ex);
            }
        }
    }
}

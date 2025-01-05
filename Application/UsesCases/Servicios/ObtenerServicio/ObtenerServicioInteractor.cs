using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.ObtenerServicio
{
    public class ObtenerServicioInteractor
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public ObtenerServicioInteractor(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

        public async Task<ObtenerServicioResponse> Handle(ObtenerServicioRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Nombre))
                {
                    return new ObtenerServicioResponse
                    {
                        Exito = false,
                        Mensaje = "Debe ingresar un nombre valido."
                    };
                }

                var servicio = await _servicioRepositorio.ObtenerServicioAsync(request.Nombre);

                if (servicio == null)
                {
                    return new ObtenerServicioResponse
                    {
                        Exito = false,
                        Mensaje = "No se encontro un servicio con este nombre"
                    };
                }

                return new ObtenerServicioResponse
                {
                    Exito = true,
                    Nombre = servicio.Nombre,
                    Descripcion = servicio.Descripcion,
                    Precio = servicio.Precio,
                    DuracionMinutos = servicio.DuracionMinutos
                };
            }
            catch (Exception ex)
            {
                return new ObtenerServicioResponse
                {
                    Exito = false,
                    Mensaje = ex.Message
                };
            }
       }
    }
}

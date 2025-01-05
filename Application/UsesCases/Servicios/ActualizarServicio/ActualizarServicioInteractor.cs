using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.ActualizarServicio
{
    public class ActualizarServicioInteractor
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public ActualizarServicioInteractor(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

        public async Task<ActualizarServicioResponse> Handle(ActualizarServicioRequest actualizarServicioRequest)
        {
            try
            {
                if (actualizarServicioRequest == null)
                {
                    return new ActualizarServicioResponse
                    {
                        Exito = false,
                        Mensaje = "Debe ingresar al menos un nuevo dato."
                    };
                }

                var servicio = new Servicio
                {
                    Nombre = actualizarServicioRequest.Nombre,
                    Descripcion = actualizarServicioRequest.Descripcion,
                    Precio = actualizarServicioRequest.Precio,
                    DuracionMinutos = actualizarServicioRequest.DuracionMinutos
                };

                await _servicioRepositorio.ActualizarServicioAsync(servicio);
                return new ActualizarServicioResponse
                {
                    Exito = true,
                    Mensaje = "Servicio actualizado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ActualizarServicioResponse
                {
                    Exito = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
}

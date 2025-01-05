using Core.Entities;
using Core.Interfaces;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.RegistrarServicio
{
    public class RegistrarServicioInteractor
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public RegistrarServicioInteractor(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }

        public async Task<RegistrarServicioResponse> Handle(RegistrarServicioRequest registrarServicioRequest)
        {
            try
            {
                if(registrarServicioRequest == null ||
                    string.IsNullOrEmpty(registrarServicioRequest.Precio.ToString()) ||
                    string.IsNullOrEmpty(registrarServicioRequest.Nombre) ||
                    string.IsNullOrEmpty(registrarServicioRequest.DuracionMinutos.ToString()))
                {
                    return new RegistrarServicioResponse
                    {
                        Exito = false,
                        Mensaje = "Todos los campos son obligatorios."
                    };
                }

                if(registrarServicioRequest.Precio < 0)
                {
                    return new RegistrarServicioResponse
                    {
                        Exito = false,
                        Mensaje = "El precio debe ser mayor a 0."
                    };
                }

                if (registrarServicioRequest.DuracionMinutos < 0)
                {
                    return new RegistrarServicioResponse
                    {
                        Exito = false,
                        Mensaje = "La duracion debe ser mayor a 0."
                    };
                }

                var servicio = new Servicio
                {
                    Nombre = registrarServicioRequest.Nombre,
                    Descripcion = registrarServicioRequest.Descripcion,
                    DuracionMinutos = registrarServicioRequest.DuracionMinutos,
                    Precio = registrarServicioRequest.Precio
                };

                await _servicioRepositorio.RegistrarServicioAsync(servicio);
                return new RegistrarServicioResponse
                {
                    Exito = true,
                    Mensaje = "Servicio registrado exitosamente.",
                    ServicioId = servicio.ServicioId
                };

            }
            catch (Exception ex)
            {
                return new RegistrarServicioResponse
                {
                    Exito = false,
                    Mensaje = $"Error desde el Handle: {ex.Message}"
                };
            }
        }
    }
}

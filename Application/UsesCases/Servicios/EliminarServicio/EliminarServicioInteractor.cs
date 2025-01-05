using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Servicios.EliminarServicio
{
    public class EliminarServicioInteractor
    {
        private readonly IServicioRepositorio _servicioRepositorio;

        public EliminarServicioInteractor(IServicioRepositorio servicioRepositorio)
        {
            _servicioRepositorio = servicioRepositorio;
        }


        public async Task<EliminarServicioResponse> Handle(EliminarServicioRequest request)
        {
            try
            {
                if(string.IsNullOrEmpty(request.Nombre))
                {
                    return new EliminarServicioResponse
                    {
                        Exito = false,
                        Mensaje = "Debe ingresar el nombre del servicio que desea eliminar."
                    };
                }

                await _servicioRepositorio.EliminarServicioAsync(request.Nombre);
                return new EliminarServicioResponse
                {
                    Exito = true,
                    Mensaje = "Servicio eliminado exitosamente."
                };
            }
            catch (Exception ex)
            {
                return new EliminarServicioResponse
                {
                    Exito = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
}

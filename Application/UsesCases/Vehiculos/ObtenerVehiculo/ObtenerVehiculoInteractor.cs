using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Vehiculos.ObtenerVehiculo
{
    public class ObtenerVehiculoInteractor
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;

        public ObtenerVehiculoInteractor(IVehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        public async Task<ObtenerVehiculoResponse> Handle(ObtenerVehiculoRequest obtenerVehiculoRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(obtenerVehiculoRequest.Placa))
                {
                    return new ObtenerVehiculoResponse { Exito = false, Mensaje = "Debe proporcionar una placa valida."};
                }

                var vehiculo = await _vehiculoRepositorio.ObtenerVehiculoAsync(obtenerVehiculoRequest.Placa);

                if (vehiculo == null)
                {
                    return new ObtenerVehiculoResponse { Exito = false, Mensaje = "No se encontro un vehiculo con esa placa." };
                }

                return new ObtenerVehiculoResponse
                {
                    Exito = true,
                    ClienteId = vehiculo.ClienteId,
                    Marca = vehiculo.Marca,
                    Modelo = vehiculo.Modelo,
                    Anio = vehiculo.Anio,
                    Color = vehiculo.Color,
                    Placa = vehiculo.Placa
                };


            }
            catch (Exception e)
            {
                return new ObtenerVehiculoResponse
                {
                    Exito = false,
                    Mensaje = $"Ocurrio un error desde el interactor: {e.Message}"
                };
            }
        } 
    }
}

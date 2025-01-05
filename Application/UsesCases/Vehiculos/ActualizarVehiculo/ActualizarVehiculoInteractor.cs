using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Vehiculos.ActualizarVehiculo
{
    public class ActualizarVehiculoInteractor
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;

        public ActualizarVehiculoInteractor(IVehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        public async Task<ActualizarVehiculoResponse> Handle(ActualizarVehiculoRequest actualizarVehiculoRequest)
        {
            try
            {
                if(actualizarVehiculoRequest == null)
                {
                    return new ActualizarVehiculoResponse { Exito = false, Mensaje = "Debe ingresar datos validos" };
                }

                var vehiculo = new Vehiculo
                {
                    Anio = actualizarVehiculoRequest.Anio,
                    Color = actualizarVehiculoRequest.Color,
                    Marca = actualizarVehiculoRequest.Marca,
                    Placa = actualizarVehiculoRequest.Placa,
                    Modelo = actualizarVehiculoRequest.Modelo
                };

                await _vehiculoRepositorio.ActualizarVehiculoAsync(vehiculo);
                return new ActualizarVehiculoResponse {Exito = true, Mensaje = "Vehiculo actualizado exitosamente." };
            }
            catch (Exception ex)
            {
                return new ActualizarVehiculoResponse {Exito = false, Mensaje = $"Error al actualizar el vehiculo desde el HANDLE: {ex.Message}" };
            }
            }
    }
}

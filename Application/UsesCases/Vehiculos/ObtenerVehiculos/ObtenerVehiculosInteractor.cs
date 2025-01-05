using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Vehiculos.ObtenerVehiculos
{
    public class ObtenerVehiculosInteractor
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;

        public ObtenerVehiculosInteractor(IVehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        public async Task<IEnumerable<ObtenerVehiculosResponse>> Handle()
        {
            try
            {
                var vehiculos = await _vehiculoRepositorio.ObtenerVehiculosAsync();

                if (!vehiculos.Any() || vehiculos == null)
                {
                    return Enumerable.Empty<ObtenerVehiculosResponse>();
                }
                var response = vehiculos.Select(r => new ObtenerVehiculosResponse
                {
                    ClienteId = r.ClienteId,
                    Marca = r.Marca,
                    Modelo = r.Modelo,
                    Anio = r.Anio,
                    Color = r.Color,
                    Placa = r.Placa
                });
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error desde el interactor: {ex.Message}");
            }
        }
    }
}

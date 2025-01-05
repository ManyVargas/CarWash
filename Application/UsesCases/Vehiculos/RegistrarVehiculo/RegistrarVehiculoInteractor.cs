using Core.Entities;
using Core.Interfaces;


namespace Application.UsesCases.Vehiculos.RegistrarVehiculo
{
    public class RegistrarVehiculoInteractor
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;

        public RegistrarVehiculoInteractor(IVehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        public async Task<RegistrarVehiculoResponse> Handle(RegistrarVehiculoRequest registrarVehiculoRequest)
        {
            try
            {
                if (registrarVehiculoRequest == null)
                {
                    return new RegistrarVehiculoResponse { Exito = false, Mensaje = "Debe ingresar datos del vehiculo validos." };
                }

                var nuevoVehiculo = new Vehiculo
                {
                    ClienteId = registrarVehiculoRequest.ClienteId,
                    Marca = registrarVehiculoRequest.Marca,
                    Modelo = registrarVehiculoRequest.Modelo,
                    Anio = registrarVehiculoRequest.Anio,
                    Color = registrarVehiculoRequest.Color,
                    Placa = registrarVehiculoRequest.Placa
                };

                await _vehiculoRepositorio.RegistrarVehiculoAsync(nuevoVehiculo);
                return new RegistrarVehiculoResponse { Exito = true, Mensaje = "Vehiculo registrado exitosamente", VehiculoId = nuevoVehiculo.VehiculoId };
            }
            catch (Exception ex)
            {
                return new RegistrarVehiculoResponse {Exito = false, Mensaje = $"Error al agregar el vehiculo. HANDLER: {ex.Message}" };
            }
        }
    }
}

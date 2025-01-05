using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Vehiculos.ActualizarVehiculo;
using Application.UsesCases.Vehiculos.ObtenerVehiculo;
using Application.UsesCases.Vehiculos.ObtenerVehiculos;
using Application.UsesCases.Vehiculos.RegistrarVehiculo;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly RegistrarVehiculoInteractor _registrarVehiculoInteractor;
        private readonly ActualizarVehiculoInteractor _actualizarVehiculoInteractor;
        private readonly ObtenerVehiculosInteractor _obtenerVehiculosInteractor;
        private readonly ObtenerVehiculoInteractor _obtenerVehiculoInteractor;

        public VehiculoController(
            RegistrarVehiculoInteractor registrarVehiculoInteractor,
            ActualizarVehiculoInteractor actualizarVehiculoInteractor,
            ObtenerVehiculosInteractor obtenerVehiculosInteractor,
            ObtenerVehiculoInteractor obtenerVehiculoInteractor)
        {
            _registrarVehiculoInteractor = registrarVehiculoInteractor;
            _actualizarVehiculoInteractor = actualizarVehiculoInteractor;
            _obtenerVehiculosInteractor = obtenerVehiculosInteractor;
            _obtenerVehiculoInteractor = obtenerVehiculoInteractor;
        }


        [HttpGet("obtenerTodos")]
        public async Task<ActionResult<ObtenerVehiculosResponse>> ObtenerTodosVehiculos()
        {
            try
            {
                var vehiculos = await _obtenerVehiculosInteractor.Handle();
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVehiculo(RegistrarVehiculoRequest registrarVehiculoRequest)
        {
            try
            {
                if(registrarVehiculoRequest == null)
                {
                    return BadRequest("Debe ingresar datos validos.");
                }

                var registrarVehiculoResponse = await _registrarVehiculoInteractor.Handle(registrarVehiculoRequest);

                if(registrarVehiculoResponse.Exito == false)
                {
                    return BadRequest(registrarVehiculoResponse.Mensaje);
                }

                return Ok(registrarVehiculoResponse);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarVehiculo(ActualizarVehiculoRequest actualizarVehiculoRequest, string placa)
        {
            try
            {
                if (string.IsNullOrEmpty(placa))
                {
                    return BadRequest("Debe proporcionar una placa valida para identificar el vehiculo.");
                }

                if (actualizarVehiculoRequest == null)
                {
                    return BadRequest("Ingrese datos validos para actualizar.");
                }

                var obtenerVehiculoRequest = new ObtenerVehiculoRequest
                {
                    Placa = placa,
                };

                var vehiculoExistente = await _obtenerVehiculoInteractor.Handle(obtenerVehiculoRequest);

                if (vehiculoExistente.Exito == false || vehiculoExistente == null)
                {
                    return BadRequest(vehiculoExistente?.Mensaje ?? "Vehiculo no encontrado.");
                }

                var vehiculoExistenteNuevo = new ActualizarVehiculoRequest
                {
                    Marca = vehiculoExistente.Marca,
                    Modelo = vehiculoExistente.Modelo,
                    Anio = vehiculoExistente.Anio,
                    Color = vehiculoExistente.Color,
                    Placa = vehiculoExistente.Placa
                };

                vehiculoExistenteNuevo.Marca = actualizarVehiculoRequest.Marca;//?? vehiculoExistenteNuevo.Marca;
                vehiculoExistenteNuevo.Modelo = actualizarVehiculoRequest.Modelo; //??vehiculoExistenteNuevo.Modelo;
                vehiculoExistenteNuevo.Anio = actualizarVehiculoRequest.Anio; //?? vehiculoExistenteNuevo.Anio;
                vehiculoExistenteNuevo.Color = actualizarVehiculoRequest.Color;//?? vehiculoExistenteNuevo.Color;
                vehiculoExistenteNuevo.Placa = placa;//?? vehiculoExistenteNuevo.Placa;

                var response = await _actualizarVehiculoInteractor.Handle(vehiculoExistenteNuevo);

                if (response.Exito == false)
                {
                    return BadRequest(response.Mensaje);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

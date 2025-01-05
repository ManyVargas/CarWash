using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Servicios.ActualizarServicio;
using Application.UsesCases.Servicios.EliminarServicio;
using Application.UsesCases.Servicios.ObtenerServicio;
using Application.UsesCases.Servicios.ObtenerServicios;
using Application.UsesCases.Servicios.RegistrarServicio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly ObtenerServiciosInteractor _obtenerServiciosInteractor;
        private readonly ActualizarServicioInteractor _actualizarServicioInteractor;
        private readonly EliminarServicioInteractor _eliminarServicioInteractor;
        private readonly RegistrarServicioInteractor _registrarServicioInteractor;
        private readonly ObtenerServicioInteractor _obtenerServicioInteractor;

        public ServicioController(ObtenerServiciosInteractor obtenerServiciosInteractor,
            ActualizarServicioInteractor actualizarServicioInteractor,
            EliminarServicioInteractor eliminarServicioInteractor,
            RegistrarServicioInteractor registrarServicioInteractor,
            ObtenerServicioInteractor obtenerServicioInteractor)
        {
            _obtenerServiciosInteractor = obtenerServiciosInteractor;
            _actualizarServicioInteractor = actualizarServicioInteractor;
            _eliminarServicioInteractor = eliminarServicioInteractor;
            _registrarServicioInteractor = registrarServicioInteractor;
            _obtenerServicioInteractor = obtenerServicioInteractor;
        }

        [HttpGet("todosServicios")]
        public async Task<ActionResult<IEnumerable<ObtenerServiciosResponse>>> ObtenerTodosServicios()
        {
            try
            {
                var servicios = await _obtenerServiciosInteractor.Handle();

                return Ok(servicios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPost("agregar")]
        public async Task<IActionResult> RegistrarServicio(RegistrarServicioRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Ingrese datos validos.");
                }

                var registrarServicioResponse = await _registrarServicioInteractor.Handle(request);

                return Ok(registrarServicioResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarServicio(ActualizarServicioRequest request, string nombre)
        {
            try
            {
                if(request == null || string.IsNullOrEmpty(nombre))
                {
                    return BadRequest("Debe proporcionar un nombre valido.");
                }

                var obtenerServicioRequest = new ObtenerServicioRequest
                {
                    Nombre = nombre
                };

                var servicioExiste = await _obtenerServicioInteractor.Handle(obtenerServicioRequest);

                if(servicioExiste == null || servicioExiste.Exito == false)
                {
                    return BadRequest(servicioExiste?.Mensaje ?? "Servicio no encontrado");
                }

                var servicioExistenteNuevo = new ActualizarServicioRequest
                {
                    Nombre = servicioExiste.Nombre,
                    Descripcion = servicioExiste.Descripcion,
                    Precio = servicioExiste.Precio,
                    DuracionMinutos = servicioExiste.DuracionMinutos
                };

                servicioExistenteNuevo.Nombre = nombre ?? request.Nombre;
                servicioExistenteNuevo.Descripcion = request.Descripcion;
                servicioExistenteNuevo.Precio = request.Precio;
                servicioExistenteNuevo.DuracionMinutos = request.DuracionMinutos;

                var response = await _actualizarServicioInteractor.Handle(servicioExistenteNuevo);

                if(response.Exito == false)
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

        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarServicio(string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    return BadRequest("Debe proporcionar un nombre valido.");
                }

                var eliminarServicioRequest = new EliminarServicioRequest
                {
                    Nombre = nombre
                };

                var response = await _eliminarServicioInteractor.Handle(eliminarServicioRequest);

                if (response == null || response.Exito == false)
                {
                    return BadRequest(response?.Mensaje ?? "Servicio no encontrado");
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

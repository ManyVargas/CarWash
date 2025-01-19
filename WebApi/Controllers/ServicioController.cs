using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Logs.RegistrarLog;
using Application.UsesCases.Servicios.ActualizarServicio;
using Application.UsesCases.Servicios.EliminarServicio;
using Application.UsesCases.Servicios.ObtenerServicio;
using Application.UsesCases.Servicios.ObtenerServicios;
using Application.UsesCases.Servicios.RegistrarServicio;
using Microsoft.AspNetCore.Authorization;
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
        private readonly RegistrarLogInteractor _registrarLog;

        public ServicioController(ObtenerServiciosInteractor obtenerServiciosInteractor,
            ActualizarServicioInteractor actualizarServicioInteractor,
            EliminarServicioInteractor eliminarServicioInteractor,
            RegistrarServicioInteractor registrarServicioInteractor,
            ObtenerServicioInteractor obtenerServicioInteractor,
            RegistrarLogInteractor registrarLog)
        {
            _obtenerServiciosInteractor = obtenerServiciosInteractor;
            _actualizarServicioInteractor = actualizarServicioInteractor;
            _eliminarServicioInteractor = eliminarServicioInteractor;
            _registrarServicioInteractor = registrarServicioInteractor;
            _obtenerServicioInteractor = obtenerServicioInteractor;
            _registrarLog = registrarLog;
        }

        [HttpGet("todosServicios")]
        public async Task<ActionResult<IEnumerable<ObtenerServiciosResponse>>> ObtenerTodosServicios()
        {
            await _registrarLog.Handle("Information", nameof(ServicioController), "Se comenzaron a obtener todos los servicios.");
            try
            {
                var servicios = await _obtenerServiciosInteractor.Handle();

                await _registrarLog.Handle("Information", nameof(ServicioController), "Se obtuvieron todos los servicios correctamente.");
                return Ok(servicios);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ServicioController), "Error al obtener todos los servicios.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpPost("agregar")]
        [Authorize]
        public async Task<IActionResult> RegistrarServicio(RegistrarServicioRequest request)
        {
            await _registrarLog.Handle("Information", nameof(ServicioController), "Se comenzo con el registro de un servicio.");
            try
            {
                if (request == null)
                {
                    await _registrarLog.Handle("Warning", nameof(ServicioController), "Se ingresaron datos invalidos para registrar un servicio.");
                    return BadRequest("Ingrese datos validos.");
                }

                var registrarServicioResponse = await _registrarServicioInteractor.Handle(request);

                await _registrarLog.Handle("Information", nameof(ServicioController), "Se registro un servicio correctamente.");
                return Ok(registrarServicioResponse);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ServicioController), "Error al intentar registrar un servicio.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("actualizar")]
        [Authorize]
        public async Task<IActionResult> ActualizarServicio(ActualizarServicioRequest request, string nombre)
        {
            await _registrarLog.Handle("Information", nameof(ServicioController), "Se comenzo con la actualizacion de un servicio.");
            try
            {
                if(request == null || string.IsNullOrEmpty(nombre))
                {
                    await _registrarLog.Handle("Warning", nameof(ServicioController), "Se ingreso un nombre no valido para la actualizacion del servicio.");
                    return BadRequest("Debe proporcionar un nombre valido.");
                }

                var obtenerServicioRequest = new ObtenerServicioRequest
                {
                    Nombre = nombre
                };

                var servicioExiste = await _obtenerServicioInteractor.Handle(obtenerServicioRequest);

                if(servicioExiste == null || servicioExiste.Exito == false)
                {
                    await _registrarLog.Handle("Warning", nameof(ServicioController), $"No se encontro un servicio con el nombre: {nombre}.");
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

                await _registrarLog.Handle("Information", nameof(ServicioController), "Se actualizo un servicio correctamente.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ServicioController), "Error al actualizar un servicio.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("eliminar")]
        [Authorize]
        public async Task<IActionResult> EliminarServicio(string nombre)
        {
            await _registrarLog.Handle("Information", nameof(ServicioController), "Se comenzo a eliminar un servicio.");
            try
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    await _registrarLog.Handle("Warning", nameof(ServicioController), "Se introdujo un nombre no valido para eliminar un servicio.");
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

                await _registrarLog.Handle("Information", nameof(ServicioController), "Servicio eliminado correctamente.");
                return Ok(response);
            }
            catch(Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ServicioController), "Error al intentar eliminar un servicio.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

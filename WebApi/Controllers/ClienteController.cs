using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Logs.RegistrarLog;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly RegistrarClienteInteractor _registrarClienteInteractor;
        private readonly ActualizarClienteInteractor _actualizarClienteInteractor;
        private readonly EliminarClienteInteractor _eliminarClienteInteractor;
        private readonly ObtenerClientesInteractor _obtenerClientesInteractor;
        private readonly ObtenerClienteInteractor _obtenerClienteInteractor;
        private readonly RegistrarLogInteractor _registrarLog;

        public ClienteController(
            RegistrarClienteInteractor registrarClienteInteractor,
            ActualizarClienteInteractor actualizarClienteInteractor,
            EliminarClienteInteractor eliminarClienteInteractor,
            ObtenerClientesInteractor obtenerClientesInteractor,
            ObtenerClienteInteractor obtenerClienteInteractor,
            RegistrarLogInteractor registrarLog)
        {
            _registrarClienteInteractor = registrarClienteInteractor;
            _actualizarClienteInteractor = actualizarClienteInteractor;
            _eliminarClienteInteractor = eliminarClienteInteractor;
            _obtenerClientesInteractor = obtenerClientesInteractor;
            _obtenerClienteInteractor = obtenerClienteInteractor;
            _registrarLog = registrarLog;
        }


        [HttpGet("todosClientes")]
        public async Task<ActionResult<IEnumerable<ObtenerClientesResponse>>> ObtenerTodosClientes()
        {
            await _registrarLog.Handle("Information", nameof(ClienteController), "Iniciando la busqueda de todos los clientes.");
            try
            {
                var clientes = await _obtenerClientesInteractor.Handle();

                await _registrarLog.Handle("Information", nameof(ClienteController), "Clientes encontrados correctamente.");
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ClienteController), "Error al buscar los clientes.",ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<ObtenerClienteResponse>> ObtenerCliente(string? telefono, string? email)
        {
            if (!string.IsNullOrEmpty(telefono))
            {
                await _registrarLog.Handle("Information", nameof(ClienteController), $"Iniciando la búsqueda del cliente con número: {telefono}");
            }
            else if (!string.IsNullOrEmpty(email))
            {
                await _registrarLog.Handle("Information", nameof(ClienteController), $"Iniciando la búsqueda del cliente con correo: {email}");
            }
            else
            {
                await _registrarLog.Handle("Error", nameof(ClienteController), "No se proporcionó ni número ni correo para la búsqueda del cliente.");
            }

            try
            {
                if (string.IsNullOrEmpty(telefono) && string.IsNullOrEmpty(email))
                {
                    return BadRequest("Debe proporcionar un teléfono o un email para buscar el cliente.");
                }

                var obtenerUsuarioRequest = new ObtenerClienteRequest
                {
                    Telefono = telefono,
                    Email = email
                };


                var cliente = await _obtenerClienteInteractor.Handle(obtenerUsuarioRequest);

                if (cliente.Exito == false)
                {
                    return NotFound(cliente.Mensaje);
                }

                await _registrarLog.Handle("Information", nameof(ClienteController), "Cliente enccontrado correctamente.");
                return Ok(cliente);

            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ClienteController), "Error al buscar el cliente.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("agregar")]
        [Authorize]
        public async Task<IActionResult> AgregarCliente(RegistrarClienteRequest registrarClienteRequest)
        {
            //_logger.LogInformation("Comenzando a agregar un cliente.");
            await _registrarLog.Handle("Information", nameof(ClienteController), "Comenzando a agregar un cliente.");
            try
            {
                if (registrarClienteRequest == null)
                {
                    return BadRequest("Ingrese datos validos.");
                }

                var registrarClienteResponse = await _registrarClienteInteractor.Handle(registrarClienteRequest);

                if (registrarClienteResponse.Exito == false)
                {
                    return BadRequest(registrarClienteResponse.Mensaje);
                }

                await _registrarLog.Handle("Information", nameof(ClienteController), "Cliente agregado correctamente.");
                return Ok(registrarClienteResponse);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ClienteController), "Error al agregar el cliente.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("actualizar")]
        [Authorize]
        public async Task<IActionResult> ActualizarCliente(ActualizarClienteRequest actualizarClienteRequest, string? telefono, string? email)
        {
            await _registrarLog.Handle("Information", nameof(ClienteController), "Comenzando a actualizar un cliente.");
            try
            {

                if (string.IsNullOrEmpty(telefono) && string.IsNullOrEmpty(email))
                {
                    return BadRequest("Debe proporcionar un teléfono o email para identificar al cliente.");
                }

                if (actualizarClienteRequest == null)
                {
                    return BadRequest("Ingrese datos validos para actualizar.");
                }

                var obtenerClienteRequest = new ObtenerClienteRequest
                {
                    Telefono = telefono,
                    Email = email
                };


                var clienteExistente = await _obtenerClienteInteractor.Handle(obtenerClienteRequest);

                if (clienteExistente.Exito == false || clienteExistente == null)
                {
                    return BadRequest(clienteExistente?.Mensaje ?? "Usuario no encontrado.");
                }

                var clienteExistenteNuevo = new ActualizarClienteRequest
                {
                    Nombre = clienteExistente.Nombre,
                    Apellido = clienteExistente.Apellido,
                    Telefono = telefono ?? clienteExistente.Telefono,
                    Email = email ?? clienteExistente.Email,
                    Direccion = clienteExistente.Direccion
                };

                clienteExistenteNuevo.Nombre = actualizarClienteRequest.Nombre ?? clienteExistenteNuevo.Nombre;
                clienteExistenteNuevo.Apellido = actualizarClienteRequest.Apellido ?? clienteExistenteNuevo.Apellido;
                clienteExistenteNuevo.Telefono = actualizarClienteRequest.Telefono ?? clienteExistenteNuevo.Telefono;
                clienteExistenteNuevo.Email = actualizarClienteRequest.Email ?? clienteExistenteNuevo.Email;
                clienteExistenteNuevo.Direccion = actualizarClienteRequest.Direccion ?? clienteExistenteNuevo.Direccion;


                var response = await _actualizarClienteInteractor.Handle(clienteExistenteNuevo);

                if (response.Exito == false)
                {
                    return BadRequest(response.Mensaje);
                }

                await _registrarLog.Handle("Information", nameof(ClienteController), "Cliente actualizado correctamente.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ClienteController), "Error al actualizar el cliente.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("eliminar")]
        [Authorize]
        public async Task<IActionResult> EliminarCliente(string? telefono, string? email)
        {
            await _registrarLog.Handle("Information", nameof(ClienteController), "Comenzando a eliminar un cliente.");
            try
            {
                if (string.IsNullOrWhiteSpace(telefono) && string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Debe proporcionar un teléfono o email para identificar al cliente.");
                }

                
                var eliminarClienteRequest = new EliminarClienteRequest
                {
                    Email = email,
                    Telefono = telefono
                };

                var response = await _eliminarClienteInteractor.Handle(eliminarClienteRequest);

                if (response.Exito == false)
                {
                    return BadRequest($"{response.Mensaje}");
                }

                await _registrarLog.Handle("Information", nameof(ClienteController), "Cliente eliminado correctamente.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(ClienteController), "Error al eliminar el cliente.", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Core.Entities;
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

        public ClienteController(
            RegistrarClienteInteractor registrarClienteInteractor,
            ActualizarClienteInteractor actualizarClienteInteractor,
            EliminarClienteInteractor eliminarClienteInteractor,
            ObtenerClientesInteractor obtenerClientesInteractor,
            ObtenerClienteInteractor obtenerClienteInteractor)
        {
            _registrarClienteInteractor = registrarClienteInteractor;
            _actualizarClienteInteractor = actualizarClienteInteractor;
            _eliminarClienteInteractor = eliminarClienteInteractor;
            _obtenerClientesInteractor = obtenerClientesInteractor;
            _obtenerClienteInteractor = obtenerClienteInteractor;
        }


        [HttpGet("todosClientes")]
        public async Task<ActionResult<IEnumerable<ObtenerClientesResponse>>> ObtenerTodosClientes()
        {
            try
            {
                var clientes = await _obtenerClientesInteractor.Handle();


                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<ObtenerClienteResponse>> ObtenerCliente(string? telefono, string? email)
        {
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

                return Ok(cliente);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarCliente(RegistrarClienteRequest registrarClienteRequest)
        {
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

                return Ok(registrarClienteResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }


        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCliente(ActualizarClienteRequest actualizarClienteRequest, string? telefono, string? email)
        {
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarCliente(string? telefono, string? email)
        {
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

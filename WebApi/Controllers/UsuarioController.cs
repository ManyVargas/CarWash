
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly ObtenerUsuariosInteractor _obtenerUsuariosInteractor;
        private readonly ObtenerUsuarioInteractor _obtenerUsuarioInteractor;
        private readonly RegistrarUsuarioInteractor _registrarUsuarioInteractor;
        private readonly ActualizarUsuarioInteractor _actualizarUsuarioInteractor;
        private readonly EliminarUsuarioInteractor _eliminarUsuarioInteractor;

        public UsuarioController(
            ObtenerUsuariosInteractor obtenerUsuariosInteractor, 
            ObtenerUsuarioInteractor obtenerUsuarioInteractor,
            RegistrarUsuarioInteractor registrarUsuarioInteractor,
            ActualizarUsuarioInteractor actualizarUsuarioInteractor,
            EliminarUsuarioInteractor eliminarUsuarioInteractor)
        {
            _obtenerUsuariosInteractor = obtenerUsuariosInteractor;
            _obtenerUsuarioInteractor = obtenerUsuarioInteractor;
            _registrarUsuarioInteractor = registrarUsuarioInteractor;
            _actualizarUsuarioInteractor = actualizarUsuarioInteractor;
            _eliminarUsuarioInteractor = eliminarUsuarioInteractor;
        }

        [HttpGet("todosUsuarios")]
        public async Task<ActionResult<IEnumerable<ObtenerUsuariosResponse>>> ObtenerTodosUsuarios()
        {
            try
            {
                var usuarios = await _obtenerUsuariosInteractor.Handle();

                if (usuarios == null)
                {
                    return NotFound("No hay usuarios para mostrar.");
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<ObtenerUsuarioResponse>> ObtenerUsuario(string? telefono, string? email)
        {
            try
            {
                if (string.IsNullOrEmpty(telefono) && string.IsNullOrEmpty(email))
                {
                    return BadRequest("Debe proporcionar un teléfono o un email para buscar el usuario.");
                }

                var obtenerUsuarioRequest = new ObtenerUsuarioRequest
                {
                    Telefono = telefono,
                    Email = email
                };


                var usuario = await _obtenerUsuarioInteractor.Handle(obtenerUsuarioRequest);

                if (usuario.Exito == false)
                {
                    return NotFound(usuario.Mensaje);
                }

                return Ok(usuario);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarUsuario(RegistrarUsuarioRequest registrarUsuarioRequest)
        {
            try
            {
                if (registrarUsuarioRequest == null)
                {
                    return BadRequest("Ingrese datos validos.");
                }

                var registrarUsuarioResponse = await _registrarUsuarioInteractor.Handle(registrarUsuarioRequest);

                if(registrarUsuarioResponse.Exito == false)
                {
                    return BadRequest(registrarUsuarioResponse.Mensaje);
                }

                return Ok(registrarUsuarioResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        
        }

        [HttpPut("actualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario(ActualizarUsuarioRequest actualizarUsuarioRequest, string? telefono, string? email)
        {
            try
            {

                if (string.IsNullOrEmpty(telefono) && string.IsNullOrEmpty(email))
                {
                    return BadRequest("Debe proporcionar un teléfono o email para identificar al usuario.");
                }

                if (actualizarUsuarioRequest == null)
                {
                    return BadRequest("Ingrese datos validos para actualizar.");
                }

                var obtenerUsuarioRequest = new ObtenerUsuarioRequest
                {
                    Telefono = telefono,
                    Email = email
                };


                var usuarioExistente = await _obtenerUsuarioInteractor.Handle(obtenerUsuarioRequest);

                if(usuarioExistente.Exito == false || usuarioExistente == null)
                {
                    return BadRequest(usuarioExistente?.Mensaje ?? "Usuario no encontrado.");
                }

                var usuarioExistenteNuevo = new ActualizarUsuarioRequest
                {
                    Nombre = usuarioExistente.Nombre,
                    Apellido = usuarioExistente.Apellido,
                    Telefono = telefono ?? usuarioExistente.Telefono,
                    Email = email ?? usuarioExistente.Email,
                    Direccion = usuarioExistente.Direccion,
                    Rol = usuarioExistente.Rol
                };

                usuarioExistenteNuevo.Nombre = actualizarUsuarioRequest.Nombre ?? usuarioExistenteNuevo.Nombre;
                usuarioExistenteNuevo.Apellido = actualizarUsuarioRequest.Apellido ?? usuarioExistenteNuevo.Apellido;
                usuarioExistenteNuevo.Telefono = actualizarUsuarioRequest.Telefono ?? usuarioExistenteNuevo.Telefono;
                usuarioExistenteNuevo.Email = actualizarUsuarioRequest.Email ?? usuarioExistenteNuevo.Email;
                usuarioExistenteNuevo.Direccion = actualizarUsuarioRequest.Direccion ?? usuarioExistenteNuevo.Direccion;
                usuarioExistenteNuevo.Rol = actualizarUsuarioRequest.Rol ?? usuarioExistenteNuevo.Rol;


                var response = await _actualizarUsuarioInteractor.Handle(usuarioExistenteNuevo);

                if (response.Exito == false)
                {
                    return BadRequest(response.Mensaje);
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarUsuario(string? telefono, string? email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(telefono) && string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Debe proporcionar un teléfono o email para identificar al usuario.");
                }

                var obtenerUsuarioRequest = new ObtenerUsuarioRequest
                {
                    Telefono = telefono,
                    Email = email
                };


                var usuarioExistente = await _obtenerUsuarioInteractor.Handle(obtenerUsuarioRequest);

                if (usuarioExistente.Exito == false || usuarioExistente == null)
                {
                    return BadRequest(usuarioExistente?.Mensaje ?? "Usuario no encontrado.");
                }

                var eliminarUsuarioRequest = new EliminarUsuarioRequest
                {
                    Email = email,
                    Telefono = telefono
                };

                var response = await _eliminarUsuarioInteractor.Handle(eliminarUsuarioRequest);

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

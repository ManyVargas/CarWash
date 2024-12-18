
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

        public UsuarioController(
            ObtenerUsuariosInteractor obtenerUsuariosInteractor, 
            ObtenerUsuarioInteractor obtenerUsuarioInteractor,
            RegistrarUsuarioInteractor registrarUsuarioInteractor)
        {
            _obtenerUsuariosInteractor = obtenerUsuariosInteractor;
            _obtenerUsuarioInteractor = obtenerUsuarioInteractor;
            _registrarUsuarioInteractor = registrarUsuarioInteractor;
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

        [HttpGet("usuarios/buscar")]
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
    }
}

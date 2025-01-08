using Application.UsesCases.Usuarios.LoginUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginUsuarioController : ControllerBase
    {
        private readonly LoginUsuarioInteractor _loginUsuarioInteractor;

        public LoginUsuarioController(LoginUsuarioInteractor loginUsuarioInteractor)
        {
            _loginUsuarioInteractor = loginUsuarioInteractor;
        }

        [HttpPost("Usuario/login")]
        public async Task<IActionResult> LoginUsuario([FromBody]LoginUsuarioRequest loginUsuarioRequest)
        {
            try
            {
                var usuario = await _loginUsuarioInteractor.Handle(loginUsuarioRequest);

                return Ok(new { Message = "Login exitoso.", Usuario = usuario });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}

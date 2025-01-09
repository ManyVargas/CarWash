using Application.UsesCases.Logs.RegistrarLog;
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
        private readonly RegistrarLogInteractor _registrarLog;

        public LoginUsuarioController(LoginUsuarioInteractor loginUsuarioInteractor, RegistrarLogInteractor registrarLog)
        {
            _loginUsuarioInteractor = loginUsuarioInteractor;
            _registrarLog = registrarLog;
        }

        [HttpPost("Usuario/login")]
        public async Task<IActionResult> LoginUsuario([FromBody]LoginUsuarioRequest loginUsuarioRequest)
        {
            await _registrarLog.Handle("Information", nameof(LoginUsuarioController), $"Un usuario comenzo a iniciar sesion con el correo: {loginUsuarioRequest.Email}.");
            try
            {
                var usuario = await _loginUsuarioInteractor.Handle(loginUsuarioRequest);

                await _registrarLog.Handle("Information", nameof(LoginUsuarioController), $"Inicio de sesion exitoso con el correo: {loginUsuarioRequest.Email}.");
                return Ok(new { Message = "Login exitoso.", Usuario = usuario });
            }
            catch (UnauthorizedAccessException ex)
            {
                await _registrarLog.Handle("Error", nameof(LoginUsuarioController), $"Error al iniciar sesion con el correo: {loginUsuarioRequest.Email}", ex.Message);
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}
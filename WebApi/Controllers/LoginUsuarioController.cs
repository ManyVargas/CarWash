using Application.UsesCases.Logs.RegistrarLog;
using Application.UsesCases.Usuarios.LoginUsuario;
using MediatR;
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
        private readonly IMediator _mediator;

        public LoginUsuarioController(LoginUsuarioInteractor loginUsuarioInteractor, RegistrarLogInteractor registrarLog, IMediator mediator)
        {
            _loginUsuarioInteractor = loginUsuarioInteractor;
            _registrarLog = registrarLog;
            _mediator = mediator;
        }

        [HttpPost("Usuario/login")]
        public async Task<IActionResult> LoginUsuario([FromBody]LoginUsuarioRequest loginUsuarioRequest, CancellationToken cancellationToken)
        {
            await _registrarLog.Handle("Information", nameof(LoginUsuarioController), $"Un usuario comenzo a iniciar sesion con el correo: {loginUsuarioRequest.Email}.");
            try
            {
                var usuario = await _loginUsuarioInteractor.Handle(loginUsuarioRequest, cancellationToken);

                await _registrarLog.Handle("Information", nameof(LoginUsuarioController), $"Inicio de sesion exitoso con el correo: {loginUsuarioRequest.Email}.");
                var result = await _mediator.Send(loginUsuarioRequest);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                await _registrarLog.Handle("Error", nameof(LoginUsuarioController), $"Error al iniciar sesion con el correo: {loginUsuarioRequest.Email}", ex.Message);
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}
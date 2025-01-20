using Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.LoginUsuario
{
    public class LoginUsuarioInteractor : IRequestHandler<LoginUsuarioRequest, LoginUsuarioResponse>
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtToken _jwtToken;

        public LoginUsuarioInteractor(IUsuarioRepositorio usuarioRepositorio, IPasswordHasher passwordHasher, IJwtToken jwtToken)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _passwordHasher = passwordHasher;
            _jwtToken = jwtToken;
        }


        public async Task<LoginUsuarioResponse> Handle(LoginUsuarioRequest loginUsuarioRequest, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepositorio.ObtenerUsuarioAsync(email:loginUsuarioRequest.Email);
            
            if (usuario == null || !_passwordHasher.VerifyPassword(loginUsuarioRequest.Contraseña, usuario.Contraseña))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            var token = _jwtToken.GenerarToken(usuario.UsuarioId.ToString(),usuario.Nombre,usuario.Rol);

            return new LoginUsuarioResponse
            {
                Nombre = usuario.Nombre,
                UsuarioID = usuario.UsuarioId,
                Rol = usuario.Rol,
                Token = token
            };
        }
    }
}

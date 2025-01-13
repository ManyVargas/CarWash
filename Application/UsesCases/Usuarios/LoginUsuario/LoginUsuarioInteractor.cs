using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.LoginUsuario
{
    public class LoginUsuarioInteractor
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUsuarioInteractor(IUsuarioRepositorio usuarioRepositorio, IPasswordHasher passwordHasher)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _passwordHasher = passwordHasher;
        }


        public async Task<LoginUsuarioResponse> Handle(LoginUsuarioRequest loginUsuarioRequest)
        {
            var usuario = await _usuarioRepositorio.ObtenerUsuarioAsync(email:loginUsuarioRequest.Email);
            
            if (usuario == null || !_passwordHasher.VerifyPassword(loginUsuarioRequest.Contraseña, usuario.Contraseña))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            return new LoginUsuarioResponse
            {
                Nombre = usuario.Nombre,
                UsuarioID = usuario.UsuarioId,
                Rol = usuario.Rol
            };
        }
    }
}

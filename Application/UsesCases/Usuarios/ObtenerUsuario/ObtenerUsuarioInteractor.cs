using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.ObtenerUsuario
{
    public class ObtenerUsuarioInteractor
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public ObtenerUsuarioInteractor(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<ObtenerUsuarioResponse> Handle(ObtenerUsuarioRequest obtenerUsuario)
        {
            if (string.IsNullOrWhiteSpace(obtenerUsuario.Email) && string.IsNullOrWhiteSpace(obtenerUsuario.Telefono))
            {
                return new ObtenerUsuarioResponse { Exito = false, Mensaje = "Ingrese el Email o el Telefono." };

            }

            var usuario = await _usuarioRepositorio.ObtenerUsuarioAsync(obtenerUsuario.Email, obtenerUsuario.Telefono);

            if (usuario == null)
            {
                return new ObtenerUsuarioResponse { Exito = false, Mensaje = "No se encontro un usuario con el dato proporcionado." };
            }

            return new ObtenerUsuarioResponse 
            {
                Exito = true, 
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                Email = usuario.Email,
                Rol = usuario.Rol
                
            };
        }
    }
}

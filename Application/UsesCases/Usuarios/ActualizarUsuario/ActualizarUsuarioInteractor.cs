using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.ActualizarUsuario
{
    public class ActualizarUsuarioInteractor
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public ActualizarUsuarioInteractor(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }


        public async Task<ActualizarUsuarioResponse> Handle(ActualizarUsuarioRequest usuarioRequest)
        {
            if (usuarioRequest == null)
            {
                return new ActualizarUsuarioResponse {Exito = false, Mensaje = "No se pudo actualizar la informacion del usuario"};
            }

            var viejoUsuario = await _usuarioRepositorio.ObtenerUsuarioAsync(usuarioRequest.Email);

            if(viejoUsuario == null)
            {
                return new ActualizarUsuarioResponse
                {
                    Exito = false,
                    Mensaje = "No se encontro un usuario con el email proporcionado."
                };
            }


            await _usuarioRepositorio.ActualizarUsuarioAsync(viejoUsuario);

            return new ActualizarUsuarioResponse
            {
                Exito = true,
                Mensaje = "Usuario actualizado exitosamente."
            };
        }
    }
}

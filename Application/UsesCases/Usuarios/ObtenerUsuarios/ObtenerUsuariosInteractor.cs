using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.ObtenerUsuario
{
    public class ObtenerUsuariosInteractor
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public ObtenerUsuariosInteractor(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IEnumerable<ObtenerUsuariosResponse>> Handle()
        {
            try
            {
                var usuarios = await _usuarioRepositorio.ObtenerTodosUsuariosAsync();

                if (usuarios == null || !usuarios.Any())
                {
                    return Enumerable.Empty<ObtenerUsuariosResponse>();
                }

                //Como devuelve un IEnumerable debemos utilizar funcion lambda para poder seleccionar
                //cada registro y mapearlo a las propiedades del response
                var response = usuarios.Select(u => new ObtenerUsuariosResponse
                {
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    Direccion = u.Direccion,
                    Rol = u.Rol,
                    Contraseña = u.Contraseña
                });

                return response;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ocurrió un error al obtener los usuarios.", ex);
            }

        }
    }
}

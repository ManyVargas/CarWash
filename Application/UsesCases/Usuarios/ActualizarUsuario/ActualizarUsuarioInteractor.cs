using Core.Entities;
using Core.Interfaces;


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
                return new ActualizarUsuarioResponse { Exito = false, Mensaje = "Debe ingresar datos validos." };
            }

            var usuario = new Usuario
            {
                Nombre = usuarioRequest.Nombre,
                Apellido = usuarioRequest.Apellido,
                Telefono = usuarioRequest.Telefono,
                Email = usuarioRequest.Email,
                Direccion = usuarioRequest.Direccion,
                Rol = usuarioRequest.Rol
            };

            try
            {
                await _usuarioRepositorio.ActualizarUsuarioAsync(usuario);

                return new ActualizarUsuarioResponse { Exito = true, Mensaje = "Usuario Actualizado exitosamente." };

            }
            catch (Exception ex)
            {
                return new ActualizarUsuarioResponse
                {
                    Exito = false,
                    Mensaje = $"Error al actualizar el usuario: {ex.Message}"
                };
            }

        }
    }
}

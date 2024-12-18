using Core.Interfaces;


namespace Application.UsesCases.Usuarios.EliminarUsuario
{
    public class EliminarUsuarioInteractor
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public EliminarUsuarioInteractor(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<EliminarUsuarioResponse> Handle(EliminarUsuarioRequest eliminarUsuarioRequest)
        {
            if(string.IsNullOrWhiteSpace(eliminarUsuarioRequest.Email))
            {
                return new EliminarUsuarioResponse {Exito =false, Mensaje = "Debe ingresar un usuario valido." };
            }

            var usuario = await _usuarioRepositorio.ObtenerUsuarioAsync(eliminarUsuarioRequest.Email);

            if(usuario == null)
            {
                return new EliminarUsuarioResponse { Exito = false, Mensaje = "El usuario ingresado no existe." };
            }

            await _usuarioRepositorio.EliminarUsuarioAsync(usuario.Email);

            return new EliminarUsuarioResponse {Exito = true, Mensaje = "Usuario eliminado exitosamente." };
        }

    }
}

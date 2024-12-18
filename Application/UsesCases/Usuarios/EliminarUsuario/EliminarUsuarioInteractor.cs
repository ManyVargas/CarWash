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
            if (string.IsNullOrEmpty(eliminarUsuarioRequest.Email) && string.IsNullOrEmpty(eliminarUsuarioRequest.Telefono))
            {
                return new EliminarUsuarioResponse { Exito = false, Mensaje = "Debe ingresar un dato valido." };
            }

            try
            {

                await _usuarioRepositorio.EliminarUsuarioAsync(eliminarUsuarioRequest.Email, eliminarUsuarioRequest.Telefono);
                return new EliminarUsuarioResponse { Exito = true, Mensaje = "Usuario eliminado exitosamente." };

            }
            catch (Exception ex)
            {
                return new EliminarUsuarioResponse { Exito = false, Mensaje = $"Error al eliminar el usuario: {ex.Message}" };
            }

        }

    }
}

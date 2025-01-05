using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext _context;

        public UsuarioRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarUsuarioAsync(Usuario usuario)
        {

            try
            {
                var existe = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

                if (existe != null)
                {
                    throw new Exception("Ya existe un usuario con este correo electronico.");
                }

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar el usuario.", ex);
            }
        }

        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            try
            {
                var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email || u.Telefono == usuario.Telefono);

                if (usuarioExistente == null)
                {
                    throw new Exception("No se encontro un usuario con el dato proporcionado.");
                }

                usuarioExistente.Nombre = usuario.Nombre ?? usuarioExistente.Nombre;
                usuarioExistente.Apellido = usuario.Apellido ?? usuarioExistente.Apellido;
                usuarioExistente.Telefono = usuario.Telefono ?? usuarioExistente.Telefono;
                usuarioExistente.Direccion = usuario.Direccion ?? usuarioExistente.Direccion;
                usuarioExistente.Email = usuario.Email ?? usuarioExistente.Email;
                usuarioExistente.Rol = usuario.Rol ?? usuarioExistente.Rol;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado al actualizar el usuario: " + ex.Message);
            }
        }

        public async Task EliminarUsuarioAsync(string? email, string? telefono)
        {
            try
            {

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email || u.Telefono == telefono); ;

                if (usuario == null)
                {
                    throw new Exception("No se encontro un usuario con el dato proporcionado.");
                }

                _context.Usuarios.Remove(usuario);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al intentar eliminar el usuario: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosUsuariosAsync()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error: {ex.Message}");
            }
        }

        public async Task<Usuario> ObtenerUsuarioAsync(string? email, string? telefono)
        {
            //Verificar que haya uno de los dos datos presentes
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(telefono))
            {
                throw new ArgumentException("Debe proporcionar un email o teléfono para buscar el usuario.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email || u.Telefono == telefono);

            if (usuario == null)
            {
                throw new("No se encontro un usuario con el dato proporcionado.");

            }
            return usuario;

        }
    }
}

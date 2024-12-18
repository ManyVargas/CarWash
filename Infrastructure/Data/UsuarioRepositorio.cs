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

        public async Task AgregarUsuarioAsync(Usuario usuario)
        {

            try
            {
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
                var usuarioViejo = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
                if (usuarioViejo != null)
                {
                    usuarioViejo.Nombre = usuario.Nombre ?? usuarioViejo.Nombre;
                    usuarioViejo.Apellido = usuario.Apellido ?? usuarioViejo.Apellido;
                    usuarioViejo.Telefono = usuario.Telefono ?? usuarioViejo.Telefono;
                    usuarioViejo.Direccion = usuario.Direccion ?? usuarioViejo.Direccion;
                    usuarioViejo.Email = usuario.Email ?? usuarioViejo.Email;
                    usuarioViejo.Rol = usuario.Rol ?? usuarioViejo.Rol;

                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado: " + ex.Message);
            }
        }

        public async Task EliminarUsuarioAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentNullException(nameof(email), "El email es obligatorio.");
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email); ;

                if (usuario == null)
                {
                    throw new InvalidOperationException("No se encontró un usuario con el email especificado.");
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

        public async Task<Usuario> ObtenerUsuarioAsync(string? email = null, string? telefono = null)
        {
            //Verificar que haya uno de los dos datos presentes
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("Debe proporcionar un email o teléfono para buscar el usuario.");

            //hacer la query dinamica
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(telefono))
            {
                query = query.Where(u => u.Telefono == telefono);

            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email == email);
            }



            return await query.FirstOrDefaultAsync();


        }
    }
}

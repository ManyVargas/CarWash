﻿using Core.Entities;

namespace Core.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task AgregarUsuarioAsync(Usuario usuario);
        Task<Usuario> ObtenerUsuarioAsync(string email = null, string telefono = null);
        Task ActualizarUsuarioAsync(Usuario usuario);
        Task EliminarUsuarioAsync(string email = null, string telefono = null);
        Task<IEnumerable<Usuario>> ObtenerTodosUsuariosAsync(); 
        
    }
}

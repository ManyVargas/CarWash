using Core.Entities;
using Core.Interfaces;
using Core.UsesCases.Usuarios.RegistrarUsuario;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

var opciones = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer("Server=Manuel;Database=CarWash;Integrated Security=True;TrustServerCertificate=True").Options;

using var context = new AppDbContext(opciones);

IPasswordHasher hasher = new PasswordHasher();

var usuarioRepositorio = new UsuarioRepositorio(context);

RegistrarUsuarioInteractor registrarUsuarioInteractor = new RegistrarUsuarioInteractor(usuarioRepositorio, hasher);


var nuevoUsuario = new RegistrarUsuarioRequest
{
    Nombre = "Juan",
    Apellido = "Vargas",
    Telefono = "8091111111",
    Direccion = "Calle segunda",
    Email = "many@gmail.commm",
    Contraseña = "abcpass",
    Rol = "Administrador"
};

var respuesta = await registrarUsuarioInteractor.Handle(nuevoUsuario);

//foreach (var item in usuario)
//{
//    Console.WriteLine($"Nombre: {item.Nombre}\nRol: {item.Rol}\n");
//}

var usuario = await usuarioRepositorio.ObtenerUsuarioAsync(nuevoUsuario.Email);
Console.WriteLine($"Usuario encontrado: {usuario.Nombre} {usuario.Apellido}\nRol: {usuario.Rol}");

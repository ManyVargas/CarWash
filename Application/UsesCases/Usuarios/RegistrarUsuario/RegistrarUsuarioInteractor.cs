﻿using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioInteractor
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPasswordHasher _passwordHasher;

        public RegistrarUsuarioInteractor(IUsuarioRepositorio usuarioRepositorio, IPasswordHasher passwordHasher)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegistrarUsuarioResponse> Handle(RegistrarUsuarioRequest request)
        {
            if(await _usuarioRepositorio.ObtenerUsuarioAsync(request.Email) != null)
            {
                return new RegistrarUsuarioResponse { Exito = false, Mensaje = "El correo electónico ya existe." };
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Contraseña);

            var nuevoUsuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Email = request.Email,
                Contraseña = hashedPassword,
                Rol = request.Rol
            };

            await _usuarioRepositorio.AgregarUsuarioAsync(nuevoUsuario);

            return new RegistrarUsuarioResponse { Exito = true, Mensaje = "Usuario registrado exitosamente.",UsuarioId = nuevoUsuario.UsuarioId };
        }
    }
}
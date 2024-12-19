using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ColeccionExtencionesServicios
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            //Servicios de Usuarios
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddTransient<ObtenerUsuariosInteractor>();
            services.AddTransient<ObtenerUsuarioInteractor>();
            services.AddTransient<RegistrarUsuarioInteractor>();
            services.AddTransient<EliminarUsuarioInteractor>();
            services.AddTransient<ActualizarUsuarioInteractor>();

            //Servicios de Clientes
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddTransient<ObtenerClienteInteractor>();
            services.AddTransient<RegistrarClienteInteractor>();
            services.AddTransient<EliminarClienteInteractor>();
            services.AddTransient<ActualizarClienteInteractor>();
            services.AddTransient<ObtenerClientesInteractor>();



            return services;
        }
    }
}

using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Facturas.GenerarFactura;
using Application.UsesCases.Servicios.ActualizarServicio;
using Application.UsesCases.Servicios.EliminarServicio;
using Application.UsesCases.Servicios.ObtenerServicio;
using Application.UsesCases.Servicios.ObtenerServicios;
using Application.UsesCases.Servicios.RegistrarServicio;
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Application.UsesCases.Vehiculos.ActualizarVehiculo;
using Application.UsesCases.Vehiculos.ObtenerVehiculo;
using Application.UsesCases.Vehiculos.ObtenerVehiculos;
using Application.UsesCases.Vehiculos.RegistrarVehiculo;
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
            
            //Servicios de Vehiculos
            services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();
            services.AddTransient<ObtenerVehiculosInteractor>();
            services.AddTransient<RegistrarVehiculoInteractor>();
            services.AddTransient<ActualizarVehiculoInteractor>();
            services.AddTransient<ObtenerVehiculoInteractor>();

            //Servicios de Servicios
            services.AddScoped<IServicioRepositorio, ServicioRepositorio>();
            services.AddTransient<ObtenerServiciosInteractor>();
            services.AddTransient<ActualizarServicioInteractor>();
            services.AddTransient<EliminarServicioInteractor>();
            services.AddTransient<RegistrarServicioInteractor>();
            services.AddTransient<ObtenerServicioInteractor>();

            //Servicios de Facturas
            services.AddScoped<IFacturaRepositorio, FacturaRepositorio>();
            services.AddTransient<GenerarFacturaInteractor>();
            services.AddTransient<GenerarFacturaRequest>();
            services.AddTransient<GenerarFacturaResponse>();
            return services;
        }
    }
}

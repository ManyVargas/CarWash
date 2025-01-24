using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Facturas.GenerarFactura;
using Application.UsesCases.Logs.RegistrarLog;
using Application.UsesCases.Servicios.ActualizarServicio;
using Application.UsesCases.Servicios.EliminarServicio;
using Application.UsesCases.Servicios.ObtenerServicio;
using Application.UsesCases.Servicios.ObtenerServicios;
using Application.UsesCases.Servicios.RegistrarServicio;
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.LoginUsuario;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Application.UsesCases.Vehiculos.ActualizarVehiculo;
using Application.UsesCases.Vehiculos.ObtenerVehiculo;
using Application.UsesCases.Vehiculos.ObtenerVehiculos;
using Application.UsesCases.Vehiculos.RegistrarVehiculo;
using Core.Interfaces;
using FluentAssertions.Common;
using Infrastructure.Data;
using Infrastructure.Frameworks.Authentication;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
namespace Infrastructure.Extensions
{
    public static class ColeccionExtencionesServicios
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // configuración de serilog
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn("origen", SqlDbType.NVarChar) { DataLength = 100 },
                    new SqlColumn("excepcion", SqlDbType.NVarChar) { DataLength = -1 }
                }
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.MSSqlServer(
                    connectionString: configuration.GetConnectionString("BackupConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = false
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .CreateLogger();

            // Registrar Serilog como el sistema de logging principal
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            // Configuración de base de datos
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BackupConnection")));

            // Configuración de base de datos backup
            //services.AddDbContext<BackUpDbContext>(options =>
                //options.UseSqlServer(configuration.GetConnectionString("BackupConnection")));


            //Servicio de Hashear contraseñas
            services.AddScoped<IPasswordHasher, PasswordHasher>();


            // Servicios de Usuarios
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddTransient<ObtenerUsuariosInteractor>();
            services.AddTransient<ObtenerUsuarioInteractor>();
            services.AddTransient<RegistrarUsuarioInteractor>();
            services.AddTransient<EliminarUsuarioInteractor>();
            services.AddTransient<ActualizarUsuarioInteractor>();

            // Servicios de Clientes
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddTransient<ObtenerClienteInteractor>();
            services.AddTransient<RegistrarClienteInteractor>();
            services.AddTransient<EliminarClienteInteractor>();
            services.AddTransient<ActualizarClienteInteractor>();
            services.AddTransient<ObtenerClientesInteractor>();

            // Servicios de Vehiculos
            services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();
            services.AddTransient<ObtenerVehiculosInteractor>();
            services.AddTransient<RegistrarVehiculoInteractor>();
            services.AddTransient<ActualizarVehiculoInteractor>();
            services.AddTransient<ObtenerVehiculoInteractor>();

            // Servicios de Servicios
            services.AddScoped<IServicioRepositorio, ServicioRepositorio>();
            services.AddTransient<ObtenerServiciosInteractor>();
            services.AddTransient<ActualizarServicioInteractor>();
            services.AddTransient<EliminarServicioInteractor>();
            services.AddTransient<RegistrarServicioInteractor>();
            services.AddTransient<ObtenerServicioInteractor>();

            // Servicios de Facturas
            services.AddScoped<IFacturaRepositorio, FacturaRepositorio>();
            services.AddTransient<GenerarFacturaInteractor>();
            services.AddTransient<GenerarFacturaRequest>();
            services.AddTransient<GenerarFacturaResponse>();

            //Servicios de Login de Usuarios
            services.AddTransient<LoginUsuarioRequest>();
            services.AddTransient<LoginUsuarioInteractor>();
            services.AddTransient<LoginUsuarioResponse>();

            //Servicios de Logs
            services.AddScoped<ILogRepositorio, LogRepositorio>();
            services.AddTransient<RegistrarLogInteractor>();


            //Servicio de JwtToken
            // 1) Mapeo de JwtSettings a la sección "Jwt"
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            // 2) Registrar la clase que implementa IJwtTokenGenerator
            services.AddScoped<IJwtToken, JwtTokenGenerator>();
            services.AddMediatR(config =>
            {
                // Registra uno o varios assemblies
                config.RegisterServicesFromAssemblies(typeof(LoginUsuarioRequest).Assembly);
            });



            return services;
        }
    }
}

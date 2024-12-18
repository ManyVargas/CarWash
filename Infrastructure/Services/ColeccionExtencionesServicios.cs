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
            Console.WriteLine("REGISTRANDO SERVICIOS de Infrastructure");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddTransient<ObtenerUsuariosInteractor>();
            services.AddTransient<ObtenerUsuarioInteractor>();
            services.AddTransient<RegistrarUsuarioInteractor>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();



            return services;
        }
    }
}

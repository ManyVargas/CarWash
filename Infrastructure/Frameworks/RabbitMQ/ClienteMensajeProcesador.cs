using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Frameworks.RabbitMQ
{
    public class ClienteMensajeProcesador : BackgroundService
    {
        private readonly ISuscriptorMensajes _subscriptor;

        public ClienteMensajeProcesador(ISuscriptorMensajes subscriber)
        {
            _subscriptor = subscriber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscriptor.Subscribir<dynamic>("Clientes", async (mensaje) =>
            {
                var tipoAccion = mensaje.TipoAccion;
                var datos = mensaje.Datos;

                switch (tipoAccion)
                {
                    case "ActualizarCliente":
                        var cliente = JsonConvert.DeserializeObject<Cliente>(datos.ToString());
                        Console.WriteLine($"Procesando actualización del cliente: {cliente.Nombre}");
                        // Si necesitas realizar alguna operación asincrónica:
                        await Task.Delay(100); // Ejemplo de operación asincrónica
                        break;

                    default:
                        Console.WriteLine($"Tipo de acción desconocido: {tipoAccion}");
                        break;
                }
            });

            await Task.CompletedTask; // Para cumplir con el contrato de async
        }
    }
}

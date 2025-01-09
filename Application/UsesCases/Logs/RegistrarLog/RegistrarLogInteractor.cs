using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Logs.RegistrarLog
{
    public class RegistrarLogInteractor
    {
        private readonly ILogRepositorio _logRepositorio;

        public RegistrarLogInteractor(ILogRepositorio logRepositorio)
        {
            _logRepositorio = logRepositorio;
        }

        public async Task Handle(string nivel, string origen, string mensaje, string excepcion = null)
        {
            var log = new Log
            {
                Fecha = DateTime.UtcNow,
                Nivel = nivel,
                Origen = origen,
                Mensaje = mensaje,
                Excepcion = excepcion
            };


            await _logRepositorio.RegistrarLogAsync(log);
        }
    }
}

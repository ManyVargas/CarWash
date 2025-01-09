using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class LogRepositorio : ILogRepositorio
    {
        private readonly AppDbContext _context;

        public LogRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarLogAsync(Log log)
        {
            _context.Set<Log>().Add(log);
            await _context.SaveChangesAsync();
        }
    }
}

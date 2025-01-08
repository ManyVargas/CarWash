using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FacturaRepositorio : IFacturaRepositorio
    {
        private readonly AppDbContext _context;

        public FacturaRepositorio(AppDbContext context)
        {
            _context = context;
        }


        public async Task GenerarFactura(Factura factura)
        {
            try
            {
                await _context.Facturas.AddAsync(factura);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el repositorio: {ex.Message}");
            }
        }

        public async Task GenerarDetalleFactura(DetalleFactura detalleFactura)
        {
            try
            {
                await _context.Detalles_Factura.AddAsync(detalleFactura);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el detalle de la factura: {ex.Message}");
            }
        }

    }
}

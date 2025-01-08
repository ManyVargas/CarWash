using Core.Entities;
using Core.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Facturas.GenerarFactura
{
    public class GenerarFacturaInteractor
    {
        private readonly IFacturaRepositorio _facturaRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IServicioRepositorio _servicioRepositorio;

        public GenerarFacturaInteractor(
            IFacturaRepositorio facturaRepositorio, 
            IClienteRepositorio clienteRepositorio, 
            IUsuarioRepositorio usuarioRepositorio,
            IServicioRepositorio servicioRepositorio)
        {
            _facturaRepositorio = facturaRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _servicioRepositorio = servicioRepositorio;
        }

        public async Task<GenerarFacturaResponse> Handle(GenerarFacturaRequest generarFacturaRequest)
        {
            try
            {
                var cliente = await _clienteRepositorio.ObtenerClienteAsync(generarFacturaRequest.ClienteEmail, generarFacturaRequest.ClienteTelefono);

                if (cliente == null)
                {
                    return new GenerarFacturaResponse
                    {
                        Exito = false,
                        Mensaje = "Cliente no encontrado"
                    };
                }

                generarFacturaRequest.ClienteId = cliente.ClienteId;

                var usuario = await _usuarioRepositorio.ObtenerUsuarioAsync(generarFacturaRequest.UsuarioEmail, generarFacturaRequest.UsuarioTelefono);

                if (usuario == null)
                {
                    return new GenerarFacturaResponse
                    {
                        Exito = false,
                        Mensaje = "Usuario no encontrado"
                    };
                }

                generarFacturaRequest.UsuarioId = usuario.UsuarioId;

                if (!new[] { "Efectivo", "Tarjeta", "Transferencia" }.Contains(generarFacturaRequest.Metodo_Pago))
                {
                    return new GenerarFacturaResponse
                    {
                        Exito = false,
                        Mensaje = "Método de pago no válido"
                    };
                }

                

                decimal subtotal = 0;

                foreach( var servicio in generarFacturaRequest.Detalles )
                {
                    if (servicio.Cantidad <= 0)
                    {
                        return new GenerarFacturaResponse
                        {
                            Exito = false,
                            Mensaje = "Cantidad no válida en los detalles"
                        };
                    }
                    var nombreServ = await _servicioRepositorio.ObtenerServicioAsync(servicio.NombreServicio);
                    subtotal += nombreServ.Precio * servicio.Cantidad;
                }

                decimal impuesto = subtotal * 0.18m;
                

                var factura = new Factura
                {
                    UsuarioId = generarFacturaRequest.UsuarioId,
                    ClienteId = generarFacturaRequest.ClienteId,
                    Fecha = DateTime.Now,
                    Subtotal = subtotal,
                    Total = subtotal + impuesto,
                    Metodo_Pago = generarFacturaRequest.Metodo_Pago
                };

                await _facturaRepositorio.GenerarFactura(factura);

                foreach(var detalleRequest in generarFacturaRequest.Detalles)
                {
                    
                    var detalleFactura = new DetalleFactura
                    {
                        FacturaId = factura.FacturaId,
                        NombreServicio = detalleRequest.NombreServicio,
                        Cantidad = detalleRequest.Cantidad
                    };

                    await _facturaRepositorio.GenerarDetalleFactura(detalleFactura);
                }

                return new GenerarFacturaResponse
                {
                    Exito = true,
                    Mensaje = "Factura generada exitosamente",
                    Factura = factura
                };
            }
            catch (Exception ex)
            {
                return new GenerarFacturaResponse
                {
                    Exito = false,
                    Mensaje = ex.Message,
                };
            }
        }
    }
}

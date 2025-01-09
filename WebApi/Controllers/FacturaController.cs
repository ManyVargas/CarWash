using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Facturas.GenerarFactura;
using Application.UsesCases.Logs.RegistrarLog;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly GenerarFacturaInteractor _generarFacturaInteractor;
        private readonly ObtenerClienteInteractor _obtenerClienteInteractor;
        private readonly ObtenerUsuarioInteractor _obtenerUsuarioInteractor;
        private readonly RegistrarLogInteractor _registrarLog;

        public FacturaController(
            GenerarFacturaInteractor generarFacturaInteractor,
            ObtenerClienteInteractor obtenerClienteInteractor,
            ObtenerUsuarioInteractor obtenerUsuarioInteractor,
            RegistrarLogInteractor registrarLog)
        {
            _generarFacturaInteractor = generarFacturaInteractor;
            _obtenerClienteInteractor = obtenerClienteInteractor;
            _obtenerUsuarioInteractor = obtenerUsuarioInteractor;
            _registrarLog = registrarLog;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> GenerarFactura([FromBody] GenerarFacturaRequest generarFacturaRequest)
        {
            await _registrarLog.Handle("Information", nameof(FacturaController), "Se comenzo a generar una factura.");
            try
            {
                if(generarFacturaRequest == null)
                {
                    await _registrarLog.Handle("Warning", nameof(FacturaController), "No se ingresaron datos validos para generar una factura.");
                    return BadRequest("Ingrese datos validos.");
                }

                if(string.IsNullOrEmpty(generarFacturaRequest.UsuarioTelefono) && string.IsNullOrEmpty(generarFacturaRequest.UsuarioEmail))
                {
                    await _registrarLog.Handle("Warning", nameof(FacturaController), "No se ingreso un dato valido del usuario para generar una factura.");
                    return BadRequest("Debe ingresar un dato para el usuario.");
                }

                if (string.IsNullOrEmpty(generarFacturaRequest.ClienteTelefono) && string.IsNullOrEmpty(generarFacturaRequest.ClienteEmail))
                {
                    await _registrarLog.Handle("Warning", nameof(FacturaController), "No se ingreso un dato valido del cliente para generar una factura.");
                    return BadRequest("Debe ingresar un dato para el cliente.");
                }

                if (string.IsNullOrEmpty(generarFacturaRequest.Metodo_Pago) )
                {
                    await _registrarLog.Handle("Warning", nameof(FacturaController), "No se ingresaron datos validos para generar una factura.");
                    return BadRequest("Debe ingresar un metodo de pago.");
                }

                var response = await _generarFacturaInteractor.Handle(generarFacturaRequest);

                if(response.Exito == false)
                {
                    await _registrarLog.Handle("Warning", nameof(FacturaController), "No se pudo generar la factura");
                    return BadRequest(response.Mensaje);
                }

                await _registrarLog.Handle("Information", nameof(FacturaController), "Factura generada correctamente.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _registrarLog.Handle("Error", nameof(FacturaController), "Error al generar la factura", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

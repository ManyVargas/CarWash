using Application.UsesCases.Clientes.ObtenerCliente;
using Application.UsesCases.Facturas.GenerarFactura;
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

        public FacturaController(
            GenerarFacturaInteractor generarFacturaInteractor,
            ObtenerClienteInteractor obtenerClienteInteractor,
            ObtenerUsuarioInteractor obtenerUsuarioInteractor)
        {
            _generarFacturaInteractor = generarFacturaInteractor;
            _obtenerClienteInteractor = obtenerClienteInteractor;
            _obtenerUsuarioInteractor = obtenerUsuarioInteractor;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> GenerarFactura([FromBody] GenerarFacturaRequest generarFacturaRequest)
        {
            try
            {
                if(generarFacturaRequest == null)
                {
                    return BadRequest("Ingrese datos validos.");
                }

                if(string.IsNullOrEmpty(generarFacturaRequest.UsuarioTelefono) && string.IsNullOrEmpty(generarFacturaRequest.UsuarioEmail))
                {
                    return BadRequest("Debe ingresar un dato para el usuario.");
                }

                if (string.IsNullOrEmpty(generarFacturaRequest.ClienteTelefono) && string.IsNullOrEmpty(generarFacturaRequest.ClienteEmail))
                {
                    return BadRequest("Debe ingresar un dato para el cliente.");
                }

                if (string.IsNullOrEmpty(generarFacturaRequest.Metodo_Pago) )
                {
                    return BadRequest("Debe ingresar un metodo de pago.");
                }

                var response = await _generarFacturaInteractor.Handle(generarFacturaRequest);

                if(response.Exito == false)
                {
                    return BadRequest(response.Mensaje);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

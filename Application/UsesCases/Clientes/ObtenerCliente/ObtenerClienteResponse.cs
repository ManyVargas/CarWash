﻿

namespace Application.UsesCases.Clientes.ObtenerCliente
{
    public class ObtenerClienteResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
    }
}

using Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.ObtenerUsuario
{
    public class ObtenerUsuarioResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }
}

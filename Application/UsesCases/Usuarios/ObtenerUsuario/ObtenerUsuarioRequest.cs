using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.ObtenerUsuario
{
    public class ObtenerUsuarioRequest
    {
        [AllowNull]
        public string Email { get; set; }
        [AllowNull]
        public string Telefono { get; set; }
    }
}

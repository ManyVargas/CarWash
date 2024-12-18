using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.EliminarUsuario
{
    public class EliminarUsuarioRequest
    {
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}

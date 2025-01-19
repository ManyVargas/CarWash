using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.LoginUsuario
{
    public class LoginUsuarioResponse
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
    }
}

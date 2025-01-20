using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.LoginUsuario
{
    public class LoginUsuarioRequest : IRequest<LoginUsuarioResponse>
    {
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsesCases.Usuarios.EliminarUsuario
{
    public class EliminarUsuarioResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}

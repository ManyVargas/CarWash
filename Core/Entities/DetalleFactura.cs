﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DetalleFactura
    {
        [Key]
        public int DetalleId { get; set; }
        public int FacturaId { get; set; }
        public string NombreServicio { get; set; }
        public int Cantidad { get; set; }
    }
}

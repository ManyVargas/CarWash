﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Factura
    {
        public int FacturaId { get; set; }
        public string ClienteNombre { get; set; }
        public string UsuarioNombre { get; set; }
        public DateTime Fecha { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public string Metodo_Pago { get; set; }
    }
}

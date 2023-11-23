using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ProductoVenta
    {
        public string NombreProducto { get; set; } = null!;
        public int UnidadesVendidas { get; set; }
        public decimal TotalFacturado { get; set; } 

        public decimal TotalFacturadoImpuestos { get; set; }
    }
}
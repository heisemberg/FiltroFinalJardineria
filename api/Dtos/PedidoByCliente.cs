using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class PedidoByCliente
    {
        public int CodigoCliente { get; set; }
        public DateOnly? FechaEsperada { get; set; }
        public DateOnly? FechaEntrega { get; set; }
        public string Demorados { get; set; } = null!;

    }
}
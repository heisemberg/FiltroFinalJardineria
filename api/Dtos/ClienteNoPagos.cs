using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ClienteNoPagos
    {
        public string NombreCliente { get; set; } = null!;
        public string NombreRepVentas { get; set; } = null!;
        public string CiudadOficina { get; set; } = null!;
    }
}
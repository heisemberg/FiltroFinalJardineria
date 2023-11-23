using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ClientePedidos
    {
        public string NombreCliente { get; set; } = null!;
        public int CantidadPedidos { get; set; }

    }
}
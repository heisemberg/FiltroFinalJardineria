using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class GamaProductoDto
    {
        public string Gama { get; set; } = null!;

        public string DescriptionTexto { get; set; }

        public string DescriptionHtml { get; set; }

        public string Imagen { get; set; }
    }
}
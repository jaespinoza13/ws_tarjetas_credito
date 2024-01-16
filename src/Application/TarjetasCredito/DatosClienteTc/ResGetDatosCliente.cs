using Application.Common.ISO20022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.DatosClienteTc
{
    public class ResGetDatosCliente : ResComun
    {
        public object cuerpo { get; set; } = new object();
        public string? str_mensaje { get; set; }
        public string? str_codigo { get; set; }
    }
}

using Application.Common.ISO20022.Models;
using Domain.Entities.DatosCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.DatosClienteTc
{
    public class ResGetDatosCliente : ResComun
    {
        public List<DatosCliente> datos_cliente { get; set; } = new List<DatosCliente>();
        public List<DireccionDomicilio> lst_dir_domicilio { get; set; } = new List<DireccionDomicilio>();
        public List<DireccionTrabajo> lst_dir_trabajo { get; set; } = new List<DireccionTrabajo>();
    }
}

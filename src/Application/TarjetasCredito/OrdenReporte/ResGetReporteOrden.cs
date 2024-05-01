using Application.Common.ISO20022.Models;
using Domain.Entities.Orden;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.OrdenReporte
{
    public class ResGetReporteOrden : ResComun
    {
        public byte[] byt_reporte { get; set; }
        //public Orden reporte { get; set; }

    }
}

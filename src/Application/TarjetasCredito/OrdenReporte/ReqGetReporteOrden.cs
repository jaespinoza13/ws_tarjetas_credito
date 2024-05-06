using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.OrdenReporte
{
    public class ReqGetReporteOrden : Header, IRequest<ResGetReporteOrden>
    {
        public string str_numero_orden { get; set; }

    }
}

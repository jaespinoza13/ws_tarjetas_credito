using Application.Common.Models;
using Application.TarjetasCredito.OrdenReporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IOrdenDat
    {
        Task<RespuestaTransaccion> get_reporte_orden(ReqGetReporteOrden request);
    }
}

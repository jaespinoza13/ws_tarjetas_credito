using Application.Common.Models;
using Application.TarjetasCredito.AnalistasCredito.AddSolicitud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IAnalistaSolicitudDat
    {
        Task<RespuestaTransaccion> addAnalistaSolicitud(ReqAddAnalistaSolicitud addAnalistaSolicitud);
    }
}

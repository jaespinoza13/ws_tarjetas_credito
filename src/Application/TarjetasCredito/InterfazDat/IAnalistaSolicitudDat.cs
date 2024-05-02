using Application.Common.Models;
using Application.TarjetasCredito.AnalistasCredito.AddAnalistaSolicitud;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IAnalistaSolicitudDat
    {
        Task<RespuestaTransaccion> addAnalistaSolicitud(ReqAddAnalistaSolicitud addAnalistaSolicitud);
    }
}

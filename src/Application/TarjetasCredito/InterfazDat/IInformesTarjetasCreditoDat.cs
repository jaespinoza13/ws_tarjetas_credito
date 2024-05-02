using Application.Common.Models;
using Application.TarjetasCredito.ComentariosAsesor;

namespace Application.TarjetasCredito.InterfazDat;

public interface IInformesTarjetasCreditoDat
{
    Task<RespuestaTransaccion> AddInforme(ReqAddInforme request);
    Task<RespuestaTransaccion> GetInforme(ReqGetInforme request);

}

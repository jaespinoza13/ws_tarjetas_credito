using Application.Common.Models;
using Application.TarjetasCredito.SituacionFinanciera;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface ISitFinDat
    {
        Task<RespuestaTransaccion> get_situacion_financiera(ReqGetSitFin request);
    }
}

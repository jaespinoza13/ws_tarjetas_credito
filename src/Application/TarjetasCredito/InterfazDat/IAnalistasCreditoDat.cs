using Application.Common.Models;
using Application.TarjetasCredito.AnalistasCredito.GetAnalistas;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IAnalistasCreditoDat
    {
        Task<RespuestaTransaccion> getAnalistasCredito(ReqGetAnalistasCredito reqGetAnalistasCredito);
    }
}

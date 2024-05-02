using Application.Common.Models;
using Application.TarjetasCredito.InformacionEconomica;

namespace Application.TarjetasCredito.InterfazDat;

public interface IInfoFinDat
{
    Task<RespuestaTransaccion> get_informacion_economica(ReqGetInfEco request);
}

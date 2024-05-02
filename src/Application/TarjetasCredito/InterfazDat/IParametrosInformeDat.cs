using Application.Common.Models;
using Application.TarjetasCredito.ComentariosAsesor;

namespace Application.TarjetasCredito.InterfazDat;

public interface IParametrosInformeDat
{
    Task<RespuestaTransaccion> get_parametros_informe(ReqGetInforme request);

}

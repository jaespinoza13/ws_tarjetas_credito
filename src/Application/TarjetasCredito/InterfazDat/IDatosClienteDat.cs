using Application.Common.Models;
using Application.TarjetasCredito.DatosClienteTc;

namespace Application.TarjetasCredito.InterfazDat;
public interface IDatosClienteDat
{
    Task<RespuestaTransaccion> get_datos_cliente(ReqGetDatosCliente request);
}


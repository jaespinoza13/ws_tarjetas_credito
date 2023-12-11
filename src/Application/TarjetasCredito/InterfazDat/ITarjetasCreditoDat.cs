using Application.Common.Models;
using Application.TarjetasCredito.AgregarSolicitudTc;


namespace Application.TarjetasCredito.InterfazDat;

public interface ITarjetasCreditoDat
{
    Task<RespuestaTransaccion> add_cliente(ReqAgregarSolicitudTc request);

}

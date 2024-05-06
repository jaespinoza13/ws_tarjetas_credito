using Application.Common.Models;

namespace Application.TarjetasCredito.InterfazDat;

public interface IGarantiasConstituidasDat
{
    Task<RespuestaTransaccion> get_gar_cns_soc(string str_num_ente);
}

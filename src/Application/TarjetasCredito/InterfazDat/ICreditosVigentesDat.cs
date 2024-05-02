using Application.Common.Models;

namespace Application.TarjetasCredito.InterfazDat;

public interface ICreditosVigentesDat
{
    Task<RespuestaTransaccion> get_creditos_vigentes(string str_num_ente);

}

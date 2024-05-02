using Application.Common.Models;

namespace Infrastructure.MemoryCache;

public interface IActivosPasivosDat
{
    Task<RespuestaTransaccion> get_activos_pasivos_socio(string str_num_ente);
}

using Application.Common.Models;

namespace Infrastructure.MemoryCache
{
    public interface IParametrosDat
    {
        Task<RespuestaTransaccion> getParametros(string str_nombre);
    }
}

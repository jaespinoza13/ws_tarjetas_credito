using Application.Common.Models;

namespace Infrastructure.MemoryCache
{
    public interface IFuncionalidadesDat
    {
        Task<RespuestaTransaccion> getFuncionalidades(int int_id_sistema, int int_tipo_funcionalidad = -1, int int_id_perfil = -1);
    }
}

using Application.Common.Models;
using Application.TarjetasCredito.CatalogoAgencias;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface ICatalogoAgenciasDat
    {
        Task<RespuestaTransaccion> get_catalogo_agencias(ReqGetCatalogoAgencias request);
    }
}

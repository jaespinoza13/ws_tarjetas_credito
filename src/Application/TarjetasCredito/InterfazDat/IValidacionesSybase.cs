using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IValidacionesSybase
    {
        Task<RespuestaTransaccion> validarPermisoPerfil(Header header);
    }
}

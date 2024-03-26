using Application.Common.Models;
using Domain.Entities.Axentria;

namespace Application.Common.Interfaces.Apis
{
    public interface IValidacionesBuro
    {
        RespuestaTransaccion addDocumento(string str_id_documento, ReqLoadDocumento reqAddAutorizacion, string str_id_transaccion);
        RespuestaTransaccion updateDocumento(string str_cod_documento, ReqLoadDocumento reqAddAutorizacion, string str_id_transaccion);
    }
}

using Domain.Entities.Axentria;

namespace Application.Common.Interfaces.Apis
{
    public interface IWsGestorDocumental
    {
        string addDocumento(ReqLoadDocumento reqLoadDocumento, string str_id_transaccion);
    }
}

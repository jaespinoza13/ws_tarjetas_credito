using Application.Common.Models;

namespace Application.Common.Interfaces.Dat;

public interface IParametros
{
    Task<RespuestaTransaccion> getParametros(string str_nombre);

}

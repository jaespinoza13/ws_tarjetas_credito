
using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Infrastructure.Common.Interfaces;

public interface IHttpService
{
    Task<string> solicitar_servicio(SolicitarServicio solicitarServicio, Header header);

    object solicitar_servicio_async(SolicitarServicio solicitarServicio);

}

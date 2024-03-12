using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.ObtenerSolicitudes
{
    public class ReqGetSolicitudes : Header, IRequest<ResGetSolicitudes>
    {
        public string str_estado {  get; set; } = string.Empty;
    }
}

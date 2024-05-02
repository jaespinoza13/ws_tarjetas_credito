using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.AnalistasCredito.GetAnalistas
{
    public class ReqGetAnalistasCredito : Header, IRequest<ResGetAnalistasCredito>
    {
    }
}

using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.InformacionEconomica
{
    public class ReqGetInfEco : ResComun, IRequest<ResGetInfEco>
    {
    }
}

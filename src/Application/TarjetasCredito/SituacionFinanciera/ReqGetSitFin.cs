using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.SituacionFinanciera
{
    public class ReqGetSitFin : ResComun, IRequest<ResGetSitFin>
    {
    }
}

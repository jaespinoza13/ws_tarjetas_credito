using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.InformacionEconomica;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.SituacionFinanciera
{
    public class ReqGetSitFin : ResComun, IRequest<ResGetSitFin>
    {
    }
}

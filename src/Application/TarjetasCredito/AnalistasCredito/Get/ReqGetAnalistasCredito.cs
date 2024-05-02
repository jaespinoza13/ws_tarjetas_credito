using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.AnalistasCredito.Get
{
    public class ReqGetAnalistasCredito : Header, IRequest<ResGetAnalistasCredito>
    {
    }
}

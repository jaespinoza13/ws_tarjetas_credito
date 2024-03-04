using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.DatosClienteTc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.CatalogoAgencias
{
    public class ReqGetCatalogoAgencias : ResComun, IRequest<ResGetCatalogoAgencias>
    {
        
    }
}

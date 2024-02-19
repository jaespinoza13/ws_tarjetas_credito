using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.DatosClienteTc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InformacionFinanciera
{
    public class ReqGetInfoFin: ResComun, IRequest<ResGetInfoFin>
    {
        public int int_num_ente {  get; set; } 
    }
}

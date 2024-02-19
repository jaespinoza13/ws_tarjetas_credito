using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.DatosClienteTc
{
    public class ReqGetDatosCliente : ResComun, IRequest<ResGetDatosCliente>
    {
        public string? str_identificacion { get; set; } = string.Empty;
        public string? str_login_usuario { get; set; } = string.Empty;
    }
}

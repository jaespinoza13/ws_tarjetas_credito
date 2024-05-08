using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.TarjetaCreditoEnProceso;

public class ReqGetTCEnProceso : ResComun, IRequest<ResGetTCEnProceso>
{
    [Required]
    public string str_identificacion { get; set; } = string.Empty;
}

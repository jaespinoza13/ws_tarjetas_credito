using Application.Common.ISO20022.Models;
using Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntregaRecepcionTarjCred.GetTarjetasCredito;

public class ReqGetTarjetaCredito : Header, IRequest<ResGetTarjetaCredito>
{
    public string str_nem_prod { get; set; } = string.Empty;
    public string str_tipo_prod { get; set; } = string.Empty;
    public string str_estado_tc { get; set; } = string.Empty;
}

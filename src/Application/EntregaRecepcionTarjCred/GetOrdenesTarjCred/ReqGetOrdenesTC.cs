using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.ObtenerSolicitudes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;

public class ReqGetOrdenesTC : Header, IRequest<ResGetOrdenesTC>
{
    public string str_orden_tipo { get; set; } = string.Empty;
}

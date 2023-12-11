using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.Transacciones.ObtenerTransacciones;

public class ReqObtenerTransaccion: Header, IRequest<ResObtenerTransaccion>
{

    public DateTime dtt_fecha_desde { get; set; }
    public DateTime dtt_fecha_hasta { get; set; }
    public int int_servicio { get; set; }
    public int int_pago { get; set; }
    public int int_pagina { get; set; }
    public int int_estado { get; set; }
    public string str_nemonico { get; set; }


}
using Application.Common.ISO20022.Models;
using Domain.Entities.Transaccion;
using MediatR;

namespace Application.Transacciones.ActualizarTransacciones;

public class ReqActualizarTransaccion : Header, IRequest<ResActualizarTransaccion>
{
    public string str_pagos_id { get; set; } = string.Empty;
    public int int_estado_id { get; set; }
    public string str_justificacion { get; set; } = string.Empty;
}
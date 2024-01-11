using Application.Common.ISO20022.Models;
using Domain.Entities.Transaccion;

namespace Application.TarjetasCredito.AgregarSolicitudTc;
public class ResAgregarSolicitudTc : ResComun
{

    public string? str_mensaje { get; set; }
    public string? str_codigo { get; set; }
    //public List<PagosError> lst_errores_pagos { get; set; } = new();

}
using Application.Common.ISO20022.Models;
using Domain.Entities.Transaccion;

namespace Application.Transacciones.ObtenerTransacciones;

public class ResObtenerTransaccion: ResComun
{
    public List<Transaccion> lst_transacciones { get; set; } = new();
    public int int_total_registros { get; set; } = 0;

}
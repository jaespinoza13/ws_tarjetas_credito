using Application.Common.ISO20022.Models;
using Domain.Entities.Transaccion;

namespace Application.Transacciones.ObtenerEstadosMonitoreo;

public class ResObtenerEstado : ResComun
{
    public List<EstadoPago> lst_estados { get; set; } = new();

}
using Application.Common.ISO20022.Models;
using Application.Transacciones.ObtenerTransacciones;
using MediatR;

namespace Application.Transacciones.ObtenerEstadosMonitoreo;

public class ReqObtenerEstado : Header, IRequest<ResObtenerEstado>
{



}
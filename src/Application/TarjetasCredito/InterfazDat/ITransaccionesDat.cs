using Application.Transacciones.ObtenerTransacciones;
using Application.Transacciones.ActualizarTransacciones;
using Application.Transacciones.ObtenerEstadosMonitoreo;
using Application.Common.Models;
namespace Application.Transacciones.InterfazDat;


public interface ITransaccionesDat
{
    Task<RespuestaTransaccion> obtener_transaccion(ReqObtenerTransaccion request);
    Task<RespuestaTransaccion> actualizar_transaccion(ReqActualizarTransaccion request);
    Task<RespuestaTransaccion> obtener_estados_monitoreo(ReqObtenerEstado request);


}


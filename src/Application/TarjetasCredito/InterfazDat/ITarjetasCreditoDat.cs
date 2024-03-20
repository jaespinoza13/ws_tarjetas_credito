using Application.Common.Models;
using Application.TarjetasCredito.ActualizarSolicitudTC;
using Application.TarjetasCredito.AgregarComentario;
using Application.TarjetasCredito.AgregarProspectoTC;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.ObtenerFlujoSolicitud;
using Application.TarjetasCredito.ObtenerSolicitudes;


namespace Application.TarjetasCredito.InterfazDat;

public interface ITarjetasCreditoDat
{
    Task<RespuestaTransaccion> addSolicitudTc(ReqAddSolicitudTc request);
    Task<RespuestaTransaccion> getSolititudesTc(ReqGetSolicitudes reqGetSolicitudes);
    Task<RespuestaTransaccion> addProcesoSolicitud(ReqAddProcesoSolicitud reqAddProcesoSolicitud);
    Task<RespuestaTransaccion> getFlujoSolicitud(ReqGetFlujoSolicitud reqGetFlujoSolicitud);
    Task<RespuestaTransaccion> updSolicitudTc(ReqActualizarSolicitudTC reqActualizarSolicitudTC);
    Task<RespuestaTransaccion> addProspectoTc(ReqAddProspectoTc reqAddProspectoTc);
}

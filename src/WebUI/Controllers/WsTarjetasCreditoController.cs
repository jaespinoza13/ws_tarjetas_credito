using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.ActualizarSolicitudTC;
using Application.TarjetasCredito.AgregarComentario;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.DatosClienteTc;
using Application.TarjetasCredito.ObtenerFlujoSolicitud;
using Application.TarjetasCredito.ObtenerSolicitudes;
using Domain.Types;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebUI.Filters;
using wsMegomovil.Filters;
namespace WebUI.Controllers;

[Route( "api/wsTarjetasCredito" )]
[Produces( "application/json" )]
[ApiController]
// Se comenta para no solicitar token
//[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.InvitadoInterno )]
//[ServiceFilter( typeof( ClaimControlFilter ) )]
[ServiceFilter( typeof( RequestFilter ) )]
[ProducesResponseType( typeof( ResBadRequestException ), (int)HttpStatusCode.BadRequest )]
[ProducesResponseType( typeof( ResException ), (int)HttpStatusCode.Unauthorized )]
[ProducesResponseType( typeof( ResException ), (int)HttpStatusCode.InternalServerError )]

public class WsTarjetasCreditoController : ControllerBase
{
    private readonly IMediator _mediator;

    public WsTarjetasCreditoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost( "GET_DATOS_CLIENTE" )]
    public async Task<ActionResult<ResGetDatosCliente>> get_informacion_cliente(ReqGetDatosCliente request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }


    [HttpPost( "ADD_SOLICITUD_TC" )]
    public async Task<ActionResult<ResAddSolicitudTc>> add_solicitud_tarjeta_credito(ReqAddSolicitudTc request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_SOLICITUDES_TC" )]
    public async Task<ActionResult<ResGetSolicitudes>> getSolicitudesTc(ReqGetSolicitudes reqGetSolicitudes)
    {
        var result = await _mediator.Send( reqGetSolicitudes );
        return Ok( result );
    }

    [HttpPost( "ADD_COMENTARIO_SOLICITUD" )]
    public async Task<ActionResult<ResAddComentario>> addComentarioSolicitud(ReqAddProcesoSolicitud reqAgregarComentario)
    {
        var result = await _mediator.Send( reqAgregarComentario );
        return Ok( result );
    }

    [HttpPost( "GET_FLUJO_SOLICITUD" )]
    public async Task<ActionResult<ResGetFlujoSolicitud>> getFlujoSolicitud(ReqGetFlujoSolicitud reqGetFlujoSolicitud)
    {
        var result = await _mediator.Send( reqGetFlujoSolicitud );
        return Ok( result );
    }

    [HttpPost( "UPD_SOLICITUD_TC" )]
    public async Task<ActionResult<ResActualizarSolicutdTC>> upd_solicitud_tarjeta_credito(ReqActualizarSolicitudTC request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }
}
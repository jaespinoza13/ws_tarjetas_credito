﻿using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.ActualizarSolicitudTC;
using Application.TarjetasCredito.AgregarComentario;
using Application.TarjetasCredito.AgregarProspectoTC;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.AnalistasCredito.AddAnalistaSolicitud;
using Application.TarjetasCredito.AnalistasCredito.GetAnalistas;
using Application.TarjetasCredito.CatalogoAgencias;
using Application.TarjetasCredito.ComentariosAsesor;
using Application.TarjetasCredito.ComentariosGestion;
using Application.TarjetasCredito.DatosClienteTc;
using Application.TarjetasCredito.InformacionEconomica;
using Application.TarjetasCredito.ObtenerFlujoSolicitud;
using Application.TarjetasCredito.ObtenerSolicitudes;
using Application.TarjetasCredito.Resoluciones;
using Application.TarjetasCredito.SituacionFinanciera;
using Application.TarjetasCredito.TarjetaCreditoEnProceso;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wsMegomovil.Filters;
using Application.TarjetasCredito.AnularSolicitud;
using Application.Parametros;
using Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;
using Application.EntregaRecepcionTarjCred.GetTarjetasCredito;
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
    public async Task<ActionResult<ResAddProcesoSolicitud>> addComentarioSolicitud(ReqAddProcesoSolicitud reqAgregarComentario)
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

    [HttpPost( "GET_INFORMACION_ECONOMICA" )]
    public async Task<ActionResult<ResGetInfEco>> get_informacion_economica(ReqGetInfEco request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }


    [HttpPost( "GET_SITUACION_FINANCIERA" )]
    public async Task<ActionResult<ResGetSitFin>> get_situacion_financiera(ReqGetSitFin request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_CATALOGO_AGENCIAS" )]
    public async Task<ActionResult<ResGetCatalogoAgencias>> get_catalogo_agencias(ReqGetCatalogoAgencias request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "ADD_PROSPECTO_TC" )]
    public async Task<ActionResult<ResAddProspectoTc>> addProspectoTc(ReqAddProspectoTc request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_COMENTARIOS_GESTION" )]
    public async Task<ActionResult<ResGetComentGestion>> get_comentarios_gestion(ReqGetComentGestion request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "ADD_INFORME_TC" )]
    public async Task<ActionResult<ResAddInforme>> add_informe_tc(ReqAddInforme request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_INFORME_TC" )]
    public async Task<ActionResult<ResGetInforme>> get_informe_tc(ReqGetInforme request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_RESOLUCION" )]
    public async Task<ActionResult<ResGetResoluciones>> get_resolucion_tc(ReqGetResoluciones request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "ADD_RESOLUCION" )]
    public async Task<ActionResult<ResAddResoluciones>> add_resolucion_tc(ReqAddResoluciones request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }
    [HttpPost( "UPD_RESOLUCION" )]
    public async Task<ActionResult<ResUpdResolucion>> upd_resolucion_tc(ReqUpdResoluciones request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }
    [HttpPost( "GET_SOL_TC_EN_PROCESO" )]
    public async Task<ActionResult<ResGetTCEnProceso>> get_solicitud_en_proceso(ReqGetTCEnProceso request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_ANALISTAS" )]
    public async Task<ActionResult<ResGetAnalistasCredito>> get_analistas(ReqGetAnalistasCredito request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "ADD_PROCESO_ESPECIFICO" )]
    public async Task<ActionResult<ResAddProcesoEspecifico>> add_proceso_especifico(ReqAddProcesoEspecifico request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }
    [HttpPost( "GET_PARAMETROS" )]
    public async Task<ActionResult<ResGetParametros>> get_parametros_tc(ReqGetParametros request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_ORDENES_TC" )]
    public async Task<ActionResult<ResGetOrdenesTC>> get_ordenes_tc(ReqGetOrdenesTC request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }

    [HttpPost( "GET_TARJETAS_CREDITO" )]
    public async Task<ActionResult<ResGetTarjetaCredito>> get_tarjetas_credito(ReqGetTarjetaCredito request)
    {
        var result = await _mediator.Send( request );
        return Ok( result );
    }
}
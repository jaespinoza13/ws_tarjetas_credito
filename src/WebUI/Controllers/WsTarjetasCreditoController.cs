﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using MediatR;

using Application.TarjetasCredito.AgregarSolicitudTc;

using Application.Common.ISO20022.Models;
using Domain.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebUI.Filters;
using Application.TarjetasCredito.DatosClienteTc;
namespace WebUI.Controllers;

[Route( "api/wsTarjetasCredito" )]
[Produces( "application/json" )]
[ApiController]
// Se comenta para no solicitar token
//[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Rol.SocioInvitadoInterno )]
//[ServiceFilter( typeof( CryptographyAesFilter ) )]
//[ServiceFilter( typeof( ClaimControlFilter ) )]
//[ServiceFilter( typeof( SessionControlFilter ) )]
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
    public async Task<ActionResult<ResAgregarSolicitudTc>> add_solicitud_tarjeta_credito(ReqAgregarSolicitudTc request)
    {
        var result = await _mediator.Send( request );

        return Ok( result );
    }
}
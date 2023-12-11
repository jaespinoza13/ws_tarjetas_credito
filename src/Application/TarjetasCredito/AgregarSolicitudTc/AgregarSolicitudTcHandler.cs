using System.Reflection;
using Application.Common.Interfaces;
using Application.TarjetasCredito.AgregarSolicitudTc;

using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.Transaccion;
using MediatR;
using Application.Common.Converting;
using Application.Common.Models;
using System;
using System.Collections.Generic;
using Application.Transacciones.InterfazDat;


namespace Application.TarjetasCredito.AgregarSolicitudTc;
public class AgregarSolicitudTcHandler : IRequestHandler<ReqAgregarSolicitudTc, ResAgregarSolicitudTc>
{
    private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
    private readonly ILogs _logs;
    private readonly string str_clase;

    public AgregarSolicitudTcHandler(ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs)
    {
        _tarjetasCreditoDat = tarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;

    }
    public async Task<ResAgregarSolicitudTc> Handle(ReqAgregarSolicitudTc request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_SOLICITUD_TC";
        var respuesta = new ResAgregarSolicitudTc();
        respuesta.LlenarResHeader( request );


        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            var result_transacction = await _tarjetasCreditoDat.add_cliente( request );

            if (result_transacction.str_codigo.Equals( "000" ))
            {
                respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];
            }

            respuesta.str_res_codigo = result_transacction.str_codigo;
            respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];

            await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
        }
        catch ( Exception e )
        {
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );

        }

        return respuesta;

    }

}

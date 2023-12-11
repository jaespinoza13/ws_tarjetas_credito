using System.Reflection;
using Application.Transacciones.InterfazDat;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Transaccion;
using MediatR;
using Application.Transacciones.ObtenerTransacciones;
using System.IO.Compression;


namespace Application.Transacciones.ActualizarTransacciones;
public class ActualizarTransaccionHandler : IRequestHandler<ReqActualizarTransaccion, ResActualizarTransaccion>
{
    private readonly ITransaccionesDat _transaccionesDat;
    private readonly ILogs _logs;
    private readonly string str_clase;

    public ActualizarTransaccionHandler(ITransaccionesDat transaccionesDat, ILogs logs)
    {
        _transaccionesDat = transaccionesDat;
        _logs = logs;
        str_clase = GetType().Name;

    }

    public async Task<ResActualizarTransaccion> Handle(ReqActualizarTransaccion request, CancellationToken cancellationToken)
    {
        const string str_operacion = "UPDATE_TRANSACCIONES";
        var respuesta = new ResActualizarTransaccion();
        respuesta.LlenarResHeader( request );

        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

            var result_transacction = await _transaccionesDat.actualizar_transaccion( request );
            

            if (result_transacction.str_codigo.Equals( "000" ))
            {
                respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];
            }


            if (result_transacction.str_codigo.Equals( "002" ))
            {
                var result = (ConjuntoDatos)result_transacction.obj_cuerpo;
                var iErroresPagos = Conversions.ConvertToListClassDynamic<PagosError>( result, 0 );
                respuesta.lst_errores_pagos = (List<PagosError>)iErroresPagos;

                respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];
            }
            

            respuesta.str_res_codigo = result_transacction.str_codigo;
            respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];

            
            await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

        }
        catch (Exception e)
        {
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }

}
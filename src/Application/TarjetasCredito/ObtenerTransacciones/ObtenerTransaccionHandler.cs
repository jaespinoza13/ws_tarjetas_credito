using System.Reflection;
using Application.Transacciones.InterfazDat;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Transaccion;
using MediatR;

namespace Application.Transacciones.ObtenerTransacciones;

public class ObtenerTransaccionHandler : IRequestHandler<ReqObtenerTransaccion, ResObtenerTransaccion>
{
    private readonly ITransaccionesDat _transaccionesDat;
    private readonly ILogs _logs;
    private readonly string str_clase;

    public ObtenerTransaccionHandler(ITransaccionesDat transaccionesDat, ILogs logs)
    {
        _transaccionesDat = transaccionesDat;
        _logs = logs;
        str_clase = GetType().Name;
    }

    public async Task<ResObtenerTransaccion> Handle(ReqObtenerTransaccion request, CancellationToken cancellationToken)
    {
        const string str_operacion = "GET_TRANSACCIONES";
        var respuesta = new ResObtenerTransaccion();
        respuesta.LlenarResHeader( request );

        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

            var result_transacction = await _transaccionesDat.obtener_transaccion( request );
           
            if (result_transacction.str_codigo.Equals( "000" ))
            {
                var result = (ConjuntoDatos)result_transacction.obj_cuerpo;
                var iTransacciones = Conversions.ConvertToListClassDynamic<Transaccion>( result, 0 );
                respuesta.lst_transacciones = (List<Transaccion>)iTransacciones;
                respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];
                respuesta.int_total_registros = Convert.ToInt32( result_transacction.diccionario["int_total_registros"]);
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
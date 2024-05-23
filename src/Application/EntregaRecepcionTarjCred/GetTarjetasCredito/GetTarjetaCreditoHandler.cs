using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;
using Application.TarjetasCredito.InterfazDat;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred.ResGetOrdenesTC;
using static Application.EntregaRecepcionTarjCred.GetTarjetasCredito.ResGetTarjetaCredito;

namespace Application.EntregaRecepcionTarjCred.GetTarjetasCredito;

public class GetTarjetaCreditoHandler : IRequestHandler<ReqGetTarjetaCredito, ResGetTarjetaCredito>
{
    private readonly ApiSettings _settings;
    private readonly string str_operacion;
    private readonly IOrdenesTarjCredDat _ordenesTarjCredDat;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogs _logs;
    private readonly string str_clase;
    public GetTarjetaCreditoHandler(IOptionsMonitor<ApiSettings> options, IOrdenesTarjCredDat ordenesTarjCredDat, ILogs logs, IMemoryCache memoryCache)
    {
        this._settings = options.CurrentValue;
        this._ordenesTarjCredDat = ordenesTarjCredDat;
        this._logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_TARJETAS_CREDITO";
        _memoryCache = memoryCache;
    }
    public async Task<ResGetTarjetaCredito> Handle(ReqGetTarjetaCredito request, CancellationToken cancellationToken)
    {
        ResGetTarjetaCredito respuesta = new ResGetTarjetaCredito();
        List<ResTarjetaCredito> lst_tarj_cred = new List<ResTarjetaCredito> ();
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();
            // Se recupera la informacion de la memoria cache 
            var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );
            //Se emplea LINQ para la consulta
            request.str_tipo_prod = (from par in lst_parametros
                                       where par.str_nemonico == request.str_nem_prod.ToString()
                                       select par.str_valor_ini + par.str_valor_fin).FirstOrDefault()! ;
            res_tran = await _ordenesTarjCredDat.get_tarjetas_credito( request );
            lst_tarj_cred = Conversions.ConvertConjuntoDatosTableToListClass<ResTarjetaCredito>( (ConjuntoDatos)res_tran.cuerpo, 0 );
            respuesta.lst_tarj_cred = lst_tarj_cred;
            respuesta.str_res_codigo = res_tran.codigo;
            respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
        }
        catch (Exception e)
        {

            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }
        return respuesta;

    }
}

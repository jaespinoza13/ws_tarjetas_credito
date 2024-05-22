using Application.Common.Interfaces.Dat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities;
using iText.Layout.Element;
using System.Reflection;
using Application.Common.Converting;
using static Domain.Entities.ComentariosAsesorCredito.Informes;
using static Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred.ResGetOrdenesTC;

namespace Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;

public class GetOrdenesTCHandler : IRequestHandler<ReqGetOrdenesTC, ResGetOrdenesTC>
{
    private readonly ApiSettings _settings;
    //private readonly IParametersInMemory _parametersInMemory;
    //private readonly IFuncionalidadesInMemory _funcionalidadesMemory;
    private readonly string str_operacion;
    private readonly IOrdenesTarjCredDat _ordenesTarjCredDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    public GetOrdenesTCHandler(IOptionsMonitor<ApiSettings> options, IOrdenesTarjCredDat ordenesTarjCredDat, ILogs logs)
    {
        this._settings = options.CurrentValue;
        this._ordenesTarjCredDat = ordenesTarjCredDat;
        this._logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_ORDENES_TC";
    }
    public async Task<ResGetOrdenesTC> Handle(ReqGetOrdenesTC request, CancellationToken cancellationToken)
    {
        ResGetOrdenesTC respuesta = new();
        List<OrdenesTC> lst_ordenes = new List<OrdenesTC>();
        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();
            res_tran = await _ordenesTarjCredDat.get_ordenes_tarj_cred( request );
            lst_ordenes = Conversions.ConvertConjuntoDatosTableToListClass<OrdenesTC>( (ConjuntoDatos)res_tran.cuerpo, 0 );
            respuesta.lst_ordenes = lst_ordenes;
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

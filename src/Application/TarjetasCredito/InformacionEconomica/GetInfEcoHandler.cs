using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.DatosClienteTc;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.DatosCliente;
using Domain.Entities.Informacion_Financiera;
using Domain.Entities.SituacionFinanciera;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.TarjetasCredito.InformacionEconomica;

public class GetInfEcoHandler : IRequestHandler<ReqGetInfEco, ResGetInfEco>
{
    private readonly IInfoFinDat _infoFinDat;

    private readonly ILogs _logs;

    private readonly string str_clase;

    private readonly string str_operacion;
    private readonly IMemoryCache _memoryCache;


    public GetInfEcoHandler(IInfoFinDat infoFinDat, ILogs logs, IMemoryCache memoryCache)
    {
        _infoFinDat = infoFinDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_INF_FIN";
        _memoryCache = memoryCache;
    }
    public async Task<ResGetInfEco> Handle(ReqGetInfEco request, CancellationToken cancellationToken)
    {
        ResGetInfEco respuesta = new();
        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();
            res_tran = await _infoFinDat.get_informacion_economica(request);
            List<Ingresos> data_list_ing = new List<Ingresos>();
            List<Egresos> data_list_egr = new List<Egresos>();
            respuesta.lst_ingresos_socio = Conversions.ConvertConjuntoDatosTableToListClass<Ingresos>( (ConjuntoDatos)res_tran.cuerpo,0 )!;
            respuesta.lst_egresos_socio = Conversions.ConvertConjuntoDatosTableToListClass<Egresos>( (ConjuntoDatos)res_tran.cuerpo,1 )!;
            foreach (Ingresos ingresos in respuesta.lst_ingresos_socio)
            {
                Ingresos obj_ingresos = new Ingresos
                {
                    int_codigo = ingresos.int_codigo,
                    str_descripcion = ingresos.str_descripcion,
                    dcm_valor = ingresos.dcm_valor,

                };
                data_list_ing.Add( obj_ingresos );
            }
            respuesta.lst_ingresos_socio = data_list_ing;

            foreach (Egresos egresos in respuesta.lst_egresos_socio)
            {
                Egresos obj_egresos = new Egresos
                {
                    int_codigo = egresos.int_codigo,
                    str_descripcion = egresos.str_descripcion,
                    dcm_valor = egresos.dcm_valor,

                };
                data_list_egr.Add( obj_egresos);
            }
            //Se almacena en memoria cache 
            if (data_list_ing.Any())
            {
                _memoryCache.Set( $"Informacion_ing_{request.str_ente}_ente", data_list_ing );
            }
            if (data_list_egr.Any())
            {
                _memoryCache.Set( $"Informacion_egr_{request.str_ente}_ente", data_list_egr );
            }
            respuesta.lst_egresos_socio = data_list_egr;
            respuesta.str_res_codigo = res_tran.codigo;
            respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
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


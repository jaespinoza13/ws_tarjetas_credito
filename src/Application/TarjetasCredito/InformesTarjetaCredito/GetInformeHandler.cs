using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosAsesorCredito;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Reflection;
using static Domain.Entities.ComentariosAsesorCredito.Informes;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class GetInformeHandler : IRequestHandler<ReqGetInforme, ResGetInforme>
{
    private readonly IParametrosInformeDat _iParametrosInformeDat;
    private readonly IInformesTarjetasCreditoDat _iInformesDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;
    private readonly IMemoryCache _memoryCache;

    public GetInformeHandler(IParametrosInformeDat IParametrosInformeDat, ILogs logs, IInformesTarjetasCreditoDat iComentariosAsesorDat, IMemoryCache memoryCache)
    {
        _iParametrosInformeDat = IParametrosInformeDat;
        _iInformesDat = iComentariosAsesorDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_INFORMES";
        _memoryCache = memoryCache;
    }
    public async Task<ResGetInforme> Handle(ReqGetInforme request, CancellationToken cancellationToken)
    {
        ResGetInforme respuesta = new();
        List<ResInformes> lst_informe = new List<ResInformes>();
        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();

            // Se recupera la informacion de la memoria cache 

            var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );


            //Se emplea LINQ para la consulta
            request.str_nem_par_inf = (from par in lst_parametros
                                       where par.str_valor_fin == request.int_id_est_sol.ToString()
                                       select par.str_valor_ini).FirstOrDefault()!;


            //Se obtiene los parametros del informe (POSTGRES)
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _iInformesDat.GetInforme( request );
            lst_informe = Conversions.ConvertConjuntoDatosTableToListClass<ResInformes>( (ConjuntoDatos)res_tran.cuerpo, 0 );
            bool bool_ver_res = lst_informe.All( x => x.json_res_inf == " " );
            if (lst_informe.Count > 0 & res_tran.codigo == "000" & bool_ver_res == false)
            {
                string str_informe = lst_informe[0].json_res_inf;
                List<Informes> lst_informes_des = JsonConvert.DeserializeObject<List<Informes>>( str_informe )!;
                respuesta.lst_informe = lst_informes_des;
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            }
            else if (res_tran.codigo == "001")
            {
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            }
            else
            {
                //Caso contrario se obtiene de (SYBASE)
                respuesta.lst_informe = new List<Informes>();
                res_tran = await _iParametrosInformeDat.get_parametros_informe( request );
                respuesta.lst_informe = Conversions.ConvertConjuntoDatosTableToListClass<Informes>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            }

        }
        catch (Exception e)
        {

            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }
        return respuesta;
    }
}

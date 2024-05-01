using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.InformacionAdicional;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosAsesorCredito;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class AddInformeHandler : IRequestHandler<ReqAddInforme, ResAddInforme>
{
    private readonly IInformesTarjetasCreditoDat _addComentarioAsesorDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly ApiSettings _settings;
    private readonly IMemoryCache _memoryCache;
    public AddInformeHandler(IOptionsMonitor<ApiSettings> options, IInformesTarjetasCreditoDat addComentarioAsesorDat, ILogs logs, IMemoryCache memoryCache)
    {
        _addComentarioAsesorDat = addComentarioAsesorDat;
        _logs = logs;
        str_clase = GetType().Name;
        _settings = options.CurrentValue;
        _memoryCache = memoryCache;
    }
    public async Task<ResAddInforme> Handle(ReqAddInforme request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_INFORME";
        ResAddInforme respuesta = new ResAddInforme();
        RespuestaTransaccion res_tran = new();
        List<Informes> data_list_informes = new List<Informes>();
        respuesta.LlenarResHeader( request );

        foreach (Informes obj_informes in request.lst_informe)
        {
            Informes obj_informes_nuevo = new Informes{
                int_id_parametro = obj_informes.int_id_parametro,
                str_tipo = obj_informes.str_tipo,
                str_descripcion = obj_informes.str_descripcion,
                str_detalle = obj_informes.str_detalle

            };
            data_list_informes.Add( obj_informes_nuevo );
        }
        request.str_informes_json = JsonConvert.SerializeObject( data_list_informes );

        try
        {
            // Se recupera la informacion de la memoria cache 

            var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );


            //Se emplea LINQ para la consulta
            request.str_nem_par_inf = (from par in lst_parametros
                                       where par.str_valor_fin == request.int_id_est_sol.ToString()
                                       select par.str_valor_ini).FirstOrDefault()!;


            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _addComentarioAsesorDat.AddInforme( request );
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

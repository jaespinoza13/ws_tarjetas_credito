using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Application.TarjetasCredito.Resoluciones.ResGetResoluciones;
using static Domain.Entities.ComentariosAsesorCredito.Informes;

namespace Application.TarjetasCredito.Resoluciones;

public class GetResolucionesHandler : IRequestHandler<ReqGetResoluciones, ResGetResoluciones>
{
    private readonly ITarjetasCreditoDat _iTarjetasCreditoDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;
    //private readonly IMemoryCache _memoryCache;
    public GetResolucionesHandler(ITarjetasCreditoDat iTarjetasCreditoDat, ILogs logs )
    {
        _iTarjetasCreditoDat = iTarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_RESOLUCIONES";
    }
    public async Task<ResGetResoluciones> Handle(ReqGetResoluciones request, CancellationToken cancellationToken)
    {
        ResGetResoluciones respuesta = new ResGetResoluciones();
        List<Resolucion> lst_resolucion = new List<Resolucion>();
        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();
            res_tran = await _iTarjetasCreditoDat.GetResoluciones(request);
            lst_resolucion = Conversions.ConvertConjuntoDatosTableToListClass<Resolucion>( (ConjuntoDatos)res_tran.cuerpo, 0 );
            respuesta.lst_resoluciones = lst_resolucion;
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

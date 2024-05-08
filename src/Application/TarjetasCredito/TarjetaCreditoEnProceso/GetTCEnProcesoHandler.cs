using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Application.TarjetasCredito.TarjetaCreditoEnProceso.ResGetTCEnProceso;
using static Domain.Entities.ComentariosAsesorCredito.Informes;
//using wsGestorDocumentalSoap;

namespace Application.TarjetasCredito.TarjetaCreditoEnProceso;

public class GetTCEnProcesoHandler : IRequestHandler<ReqGetTCEnProceso,ResGetTCEnProceso>
{
    private readonly ITarjetasCreditoDat _iTarjetasCreditoDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;
    public GetTCEnProcesoHandler(ITarjetasCreditoDat iTarjetasCreditoDat, ILogs logs)
    {
        _iTarjetasCreditoDat = iTarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_SOLCITUDES_TC_EN_PROCESO";
    }
    public async Task<ResGetTCEnProceso> Handle(ReqGetTCEnProceso request, CancellationToken cancellationToken)
    {
        ResGetTCEnProceso respuesta = new ResGetTCEnProceso();
        //List<ResProceso> lst_get_sol_tc_procerso = new List<ResProceso>();
        respuesta.LlenarResHeader( request );
        try
            {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();
            res_tran = await _iTarjetasCreditoDat.GetSolicituTCEnProceso( request );
            respuesta.str_res_codigo = res_tran.codigo;
            respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            if (res_tran.codigo == "000")
            {
                respuesta.str_res_codigo_solicitud = "005";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = "Ya existe una solicitud en proceso.";
            }
            else
            {
                respuesta.str_res_codigo_solicitud = "004";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = "No existen solicitudes en proceso...";
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

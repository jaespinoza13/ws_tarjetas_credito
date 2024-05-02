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

namespace Application.TarjetasCredito.Resoluciones;

public class UpdResolucionesHandler : IRequestHandler<ReqUpdResoluciones, ResUpdResolucion>
{
    private readonly ITarjetasCreditoDat _iTarjetasCreditoDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;
    public UpdResolucionesHandler(ITarjetasCreditoDat iTarjetasCreditoDat, ILogs logs)
    {
        _iTarjetasCreditoDat = iTarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "UPDATE_RESOLUCIONES";
    }
    public async Task<ResUpdResolucion> Handle(ReqUpdResoluciones request, CancellationToken cancellationToken)
    {
        const string str_operacion = "UPDATE_RESOLUCION";
        ResUpdResolucion respuesta = new ResUpdResolucion();
        RespuestaTransaccion res_tran = new();

        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _iTarjetasCreditoDat.UpdateResoluciones( request );
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

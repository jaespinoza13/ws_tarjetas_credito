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

public class AddResolucionesHandler : IRequestHandler<ReqAddResoluciones, ResAddResoluciones>
{
    private readonly ITarjetasCreditoDat _iTarjetasCreditoDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;
    //private readonly IMemoryCache _memoryCache;

    public AddResolucionesHandler(ITarjetasCreditoDat iTarjetasCreditoDat, ILogs logs) {
       
        _iTarjetasCreditoDat = iTarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_RESOLUCIONES";

    }
    public async Task<ResAddResoluciones> Handle(ReqAddResoluciones request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_RESOLUCION";
        ResAddResoluciones respuesta = new ResAddResoluciones();
        RespuestaTransaccion res_tran = new();

        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _iTarjetasCreditoDat.AddResoluciones( request );
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

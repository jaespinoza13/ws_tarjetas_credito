using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosAsesorCredito;
using Domain.Entities.InformeUAC;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InformeUAC;

public class AddInformeUACHandler : IRequestHandler<ReqAddInformeUAC,ResAddInformeUAC>
{
    private readonly IInformeUAC _addInformeUACDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly ApiSettings _settings;

    public AddInformeUACHandler(IOptionsMonitor<ApiSettings> options, IInformeUAC addInformeUACDat, ILogs logs)
    {
        _addInformeUACDat = addInformeUACDat;
        _logs = logs;
        str_clase = GetType().Name;
        _settings = options.CurrentValue;
    }

    public async Task<ResAddInformeUAC> Handle(ReqAddInformeUAC request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_INFORME_UAC";
        ResAddInformeUAC respuesta = new ResAddInformeUAC();
        RespuestaTransaccion res_tran = new RespuestaTransaccion();
        List<InformeAnalisisUAC> data_list_inf_uac = new List<InformeAnalisisUAC>();
        respuesta.LlenarResHeader( request );

        foreach (InformeAnalisisUAC informe_uac in request.lst_inf_anl_uac)
        {
            InformeAnalisisUAC obj_inf_uac = new InformeAnalisisUAC
            {
                int_id_parametro = informe_uac.int_id_parametro,
                str_tipo = informe_uac.str_tipo,
                str_descripcion = informe_uac.str_descripcion,
                str_detalle = informe_uac.str_detalle

            };
            data_list_inf_uac.Add( obj_inf_uac );
        }
        request.str_obj_anl_uac_json = JsonConvert.SerializeObject( data_list_inf_uac );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _addInformeUACDat.AddInformeUAC(request);
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

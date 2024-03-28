using Application.Common.Interfaces;
using Application.Common.Interfaces.Apis;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.Axentria;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;


namespace Application.TarjetasCredito.AgregarSolicitudTc;
public class AddSolicitudTcHandler : IRequestHandler<ReqAddSolicitudTc, ResAddSolicitudTc>
{
    private readonly IParametersInMemory _parametersInMemory;
    private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
    private readonly IWsGestorDocumental _wsGestorDocumental;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly ApiSettings _settings;

    public AddSolicitudTcHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory, IWsGestorDocumental wsGestorDocumental)
    {
        _tarjetasCreditoDat = tarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        _parametersInMemory = parametersInMemory;
        _settings = options.CurrentValue;
        _wsGestorDocumental = wsGestorDocumental;
    }
    public async Task<ResAddSolicitudTc> Handle(ReqAddSolicitudTc request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_SOLICITUD_TC";
        var respuesta = new ResAddSolicitudTc();
        var res_tran = new RespuestaTransaccion();
        respuesta.LlenarResHeader( request );


        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

            request.int_estado = _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro;
            request.int_estado_entregado = _parametersInMemory.FindParametroNemonico( _settings.estado_entregado ).int_id_parametro;

            var rango_clasica = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_clasica ).str_valor_ini;
            var rango_black = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_black ).str_valor_ini;
            var rango_gold = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_gold ).str_valor_ini;

            string rangoEncontrado = "";

            if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_clasica )) != "N")
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_clasica ).int_id_parametro;
            }
            else if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_black )) != "N")
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_black ).int_id_parametro;
            }
            else if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_gold )) != "N")
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_gold ).int_id_parametro;
            }
            else
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_clasica ).int_id_parametro;
            }

            res_tran = await _tarjetasCreditoDat.addSolicitudTc( request );

            if (res_tran.codigo == "000")
            {
                var req_load_doc = new ReqLoadDocumento();

                req_load_doc.int_canal = Convert.ToInt32( request.str_id_sistema );
                req_load_doc.int_oficina = Convert.ToInt32( request.str_id_oficina );
                req_load_doc.int_tipo_ide = Convert.ToInt32( request.str_num_documento );
                req_load_doc.str_nombre_canal = request.str_nemonico_canal;

                var a = _wsGestorDocumental.addDocumento( req_load_doc, request.str_id_transaccion );
            }

            respuesta.str_res_codigo = res_tran.codigo;
            respuesta.str_res_estado_transaccion = res_tran.codigo == "000" ? "OK" : "ERR";
            respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];

        }
        catch (Exception e)
        {
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
        return respuesta;

    }

    public string validaRango(decimal valor, string rango)
    {
        string[] parts = rango.Split( '|' );

        int minValue, maxValue;
        if (!int.TryParse( parts[0], out minValue ) || !int.TryParse( parts[1], out maxValue ))
        {
            return "N";
        }
        if (valor >= minValue && valor <= maxValue)
        {
            return rango;
        }
        else
        {
            return "N";
        }
    }


}

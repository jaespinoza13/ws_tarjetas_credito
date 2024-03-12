using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;


namespace Application.TarjetasCredito.AgregarSolicitudTc;
public class AddSolicitudTcHandler : IRequestHandler<ReqAddSolicitudTc, ResAddSolicitudTc>
{
    private readonly IParametersInMemory _parametersInMemory;
    private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly ApiSettings _settings;

    public AddSolicitudTcHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory)
    {
        _tarjetasCreditoDat = tarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        _parametersInMemory = parametersInMemory;
        _settings = options.CurrentValue;

    }
    public async Task<ResAddSolicitudTc> Handle(ReqAddSolicitudTc request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_SOLICITUD_TC";
        var respuesta = new ResAddSolicitudTc();
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

            if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_clasica )) != "No está en este rango")
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_clasica ).int_id_parametro;
            }
            else if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_black )) != "No está en este rango")
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_black ).int_id_parametro;
            }
            else if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_gold )) != "No está en este rango")
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_gold ).int_id_parametro;
            }
            else
            {
                request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_clasica ).int_id_parametro;
            }

            var result_transacction = await _tarjetasCreditoDat.addSolicitudTc( request );

            respuesta.str_res_codigo = result_transacction.codigo;
            respuesta.str_res_info_adicional = result_transacction.diccionario["str_o_error"];

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
            return "No está en este rango";
        }

        if (valor >= minValue && valor <= maxValue)
        {
            return rango;
        }
        else
        {
            return "No está en este rango";
        }
    }


}

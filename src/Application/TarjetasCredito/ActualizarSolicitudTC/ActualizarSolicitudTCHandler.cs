using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application.TarjetasCredito.ActualizarSolicitudTC
{
    public class ActualizarSolicitudTCHandler : IRequestHandler<ReqActualizarSolicitudTC, ResActualizarSolicutdTC>
    {
        private readonly IParametersInMemory _parametersInMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;
        private readonly ApiSettings _settings;

        public ActualizarSolicitudTCHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
        }

        public async Task<ResActualizarSolicutdTC> Handle(ReqActualizarSolicitudTC reqActualizarSolicitudTC, CancellationToken cancellationToken)
        {
            ResActualizarSolicutdTC respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "UPD_SOLICITUD_TC";
            respuesta.LlenarResHeader( reqActualizarSolicitudTC );

            try
            {
                await _logs.SaveHeaderLogs( reqActualizarSolicitudTC, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                if (reqActualizarSolicitudTC.int_estado == _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro)
                {
                    res_tran = await _tarjetasCreditoDat.updSolicitudTc( reqActualizarSolicitudTC );

                    if (res_tran.codigo == "000")
                    {

                    }

                    respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"].ToString();
                }
                else
                {
                    res_tran.codigo = "001";
                    respuesta.str_res_info_adicional = "Esta solicitud no se encuentra en estado creado, por lo tanto no se puede actualizar";
                }
                respuesta.str_res_estado_transaccion = res_tran.codigo == "000" ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;

            }
            catch (Exception ex)
            {

            }

            return respuesta;
        }
    }
}

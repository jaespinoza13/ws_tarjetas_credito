using Application.Common.Interfaces.Dat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Application.TarjetasCredito.ObtenerFlujoSolicitud;
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

                if(reqActualizarSolicitudTC.int_estado == _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro)
                {
                    res_tran = await _tarjetasCreditoDat.updSolicitudTc( reqActualizarSolicitudTC );

                    if (res_tran.codigo == "000")
                    {

                    }
                }

            }
            catch (Exception ex)
            {

            }

            return respuesta;
        }
    }
}

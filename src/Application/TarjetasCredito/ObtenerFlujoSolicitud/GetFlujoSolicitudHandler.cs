using Application.Common.Interfaces.Dat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;
using Application.Common.Utilidades;
using static Application.TarjetasCredito.ObtenerSolicitudes.ResGetSolicitudes;
using static Application.TarjetasCredito.ObtenerFlujoSolicitud.ResGetFlujoSolicitud;

namespace Application.TarjetasCredito.ObtenerFlujoSolicitud
{
    public class GetFlujoSolicitudHandler : IRequestHandler<ReqGetFlujoSolicitud, ResGetFlujoSolicitud>
    {
        private readonly IParametersInMemory _parametersInMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;
        private readonly ApiSettings _settings;

        public GetFlujoSolicitudHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
        }

        public async Task<ResGetFlujoSolicitud> Handle(ReqGetFlujoSolicitud reqGetFlujoSolicitud, CancellationToken cancellationToken)
        {
            ResGetFlujoSolicitud respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "GET_FLUJO_SOLICITUD";
            respuesta.LlenarResHeader( reqGetFlujoSolicitud );
            try
            {
                await _logs.SaveHeaderLogs( reqGetFlujoSolicitud, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                res_tran = await _tarjetasCreditoDat.getFlujoSolicitud(reqGetFlujoSolicitud);

                if (res_tran.codigo == "000")
                {
                    List<FlujoSolicitudes> lista_solicitudes = Mapper.ConvertConjuntoDatosToListClass<FlujoSolicitudes>( res_tran.cuerpo);

                    foreach (FlujoSolicitudes solicitudes in lista_solicitudes)
                    {
                        solicitudes.str_estado = _parametersInMemory.FindParametroId( solicitudes.int_estado ).str_valor_ini;
                        respuesta.flujo_solicitudes.Add( solicitudes );
                    }
                }
                else
                {
                    respuesta.str_res_codigo = "001";
                    respuesta.str_res_info_adicional = "No existe un flujo para esta solicitud";
                }
            }
            catch (Exception ex) { }
            return respuesta;
        }
    }
}
    
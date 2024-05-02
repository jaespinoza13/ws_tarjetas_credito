using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.AnalistasCredito.AddAnalistaSolicitud;
using Application.TarjetasCredito.InterfazDat;
using Application.TarjetasCredito.ObtenerSolicitudes;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application.TarjetasCredito.AnalistasCredito.GetAnalistas
{
    public class GetAnalistasCreditoHandler : IRequestHandler<ReqGetAnalistasCredito, ResGetAnalistasCredito>
    {
        private readonly ApiSettings _settings;
        private readonly IAnalistasCreditoDat _analistasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;

        public GetAnalistasCreditoHandler(IOptionsMonitor<ApiSettings> options, IAnalistasCreditoDat analistasCreditoDat, ILogs logs)
        {
            _settings = options.CurrentValue;
            _analistasCreditoDat = analistasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
        }

        public async Task<ResGetAnalistasCredito> Handle(ReqGetAnalistasCredito reqGetAnalistasCredito, CancellationToken cancellationToken)
        {
            ResGetAnalistasCredito respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "GET_SOLICITUDES_TC";
            respuesta.LlenarResHeader( reqGetAnalistasCredito );
            var funcionalidad = new Domain.Funcionalidades.Funcionalidad();

            try
            {
                await _logs.SaveHeaderLogs( reqGetAnalistasCredito, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                res_tran = await _analistasCreditoDat.getAnalistasCredito( reqGetAnalistasCredito );
                respuesta.lst_analistas = Mapper.ConvertConjuntoDatosToListClass<ResGetAnalistasCredito.Analistas>( res_tran.cuerpo );

                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_estado_transaccion = res_tran.codigo == "000" ? "OK" : "ERR";
            }
            catch (Exception ex)
            {
                await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, ex );
                throw new ArgumentException( respuesta.str_id_transaccion );
            }
            await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            return respuesta;
        }
    }
}

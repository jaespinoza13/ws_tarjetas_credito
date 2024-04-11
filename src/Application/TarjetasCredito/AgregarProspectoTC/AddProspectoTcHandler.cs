
using Application.Common.Interfaces.Dat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using Application.TarjetasCredito.AnularSolicitud;
using System.Reflection;

namespace Application.TarjetasCredito.AgregarProspectoTC
{
    public class AddProspectoTcHandler : IRequestHandler<ReqAddProspectoTc,ResAddProspectoTc>
    {
        private readonly IParametersInMemory _parametersInMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;
        private readonly ApiSettings _settings;

        public AddProspectoTcHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
        }

        public async Task<ResAddProspectoTc> Handle(ReqAddProspectoTc reqAddProspectoTc, CancellationToken cancellationToken)
        {
            const string str_operacion = "AGREGAR_PROSPECTO";
            var respuesta = new ResAddProspectoTc();
            var res_tran = new RespuestaTransaccion();
            respuesta.LlenarResHeader( reqAddProspectoTc );

            try
            {
                await _logs.SaveHeaderLogs( reqAddProspectoTc, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
                res_tran = await _tarjetasCreditoDat.addProspectoTc( reqAddProspectoTc );
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_estado_transaccion = res_tran.codigo =="000" ? "OK" : "ERR";

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

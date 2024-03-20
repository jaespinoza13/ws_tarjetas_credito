using Application.Common.Interfaces.Dat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.TarjetasCredito.AgregarSolicitudTc;
using System.Reflection;
using Application.TarjetasCredito.AgregarComentario;

namespace Application.TarjetasCredito.AnularSolicitud
{
    public class AddAnularSolicitudHandler : IRequestHandler<ReqAddAnularSolicitud, ResAddAnularSolicitud>
    {
        private readonly IParametersInMemory _parametersInMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;
        private readonly ApiSettings _settings;

        public AddAnularSolicitudHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
        }

        public async Task<ResAddAnularSolicitud> Handle(ReqAddAnularSolicitud reqAddAnularSolicitud, CancellationToken cancellationToken)
        {
            const string str_operacion = "ANULAR_SOLICITUD_HANDLER";
            var respuesta = new ResAddAnularSolicitud();
            var res_tran = new RespuestaTransaccion();
            respuesta.LlenarResHeader( reqAddAnularSolicitud );

            try
            {
                await _logs.SaveHeaderLogs( reqAddAnularSolicitud, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                int estado = _parametersInMemory.FindParametroNemonico( _settings.estado_anulado ).int_id_parametro;

                if(estado != 0)
                {
                    ReqAddProcesoSolicitud reqAddProceso = new();

                    reqAddProceso.int_estado = estado;
                    reqAddProceso.int_id_solicitud = reqAddAnularSolicitud.int_id_solicitud;
                    reqAddProceso.str_comentario = reqAddAnularSolicitud.str_comentario;
                    res_tran = await _tarjetasCreditoDat.addProcesoSolicitud(reqAddProceso);
    
                    respuesta.str_res_codigo = res_tran.codigo;
                    respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
                }

            }
            catch ( Exception ex ) { }

            return respuesta;
        }
     }
}

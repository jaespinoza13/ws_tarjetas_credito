using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.AgregarComentario;
using Application.TarjetasCredito.InterfazDat;
using iText.Kernel.Pdf.Canvas.Wmf;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application.TarjetasCredito.AnularSolicitud
{
    public class AddProcesoEspecificoHandler : IRequestHandler<ReqAddProcesoEspecifico, ResAddProcesoEspecifico>
    {
        private readonly IParametersInMemory _parametersInMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly IFuncionalidadesInMemory _funcionalidadesInMemory;
        private readonly ILogs _logs;
        private readonly string str_clase;
        private readonly ApiSettings _settings;

        public AddProcesoEspecificoHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory, IFuncionalidadesInMemory funcionalidadesInMemory)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
            _funcionalidadesInMemory = funcionalidadesInMemory;
        }

        public async Task<ResAddProcesoEspecifico> Handle(ReqAddProcesoEspecifico reqAddProcesoEspecifico, CancellationToken cancellationToken)
        {
            const string str_operacion = "PROCESO_ESPECIFICO_HANDLER";
            var respuesta = new ResAddProcesoEspecifico();
            var res_tran = new RespuestaTransaccion();
            respuesta.LlenarResHeader( reqAddProcesoEspecifico );

            try
            {
                await _logs.SaveHeaderLogs( reqAddProcesoEspecifico, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                bool permiso = Validaciones.ValidarEstado( reqAddProcesoEspecifico.str_estado, _settings, _funcionalidadesInMemory, reqAddProcesoEspecifico.str_id_perfil );

                if (permiso)
                {
                    int estado = _parametersInMemory.FindParametroNemonico( reqAddProcesoEspecifico.str_estado ).int_id_parametro;
                    if (estado != 0)
                    {
                        ReqAddProcesoSolicitud reqAddProceso = new();

                        reqAddProceso.int_estado = estado;
                        reqAddProceso.int_id_solicitud = reqAddProcesoEspecifico.int_id_solicitud;
                        reqAddProceso.str_comentario = reqAddProcesoEspecifico.str_comentario;
                        reqAddProceso.str_login = reqAddProcesoEspecifico.str_login;
                        reqAddProceso.str_id_oficina = reqAddProcesoEspecifico.str_id_oficina;
                        res_tran = await _tarjetasCreditoDat.addProcesoSolicitud( reqAddProceso );

                        respuesta.str_res_codigo = res_tran.codigo;
                        respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
                    }
                }
                else
                {
                    res_tran.diccionario.Add( "str_error", "No tiene permiso para realizar la acción que está intentando" );
                    res_tran.codigo = "001";
                }
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_estado_transaccion = respuesta.str_res_codigo == "000" ? "OK" : "ERR";
                respuesta.str_res_info_adicional = respuesta.str_res_codigo == "000" ? "" : res_tran.diccionario["str_error"].ToString();
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

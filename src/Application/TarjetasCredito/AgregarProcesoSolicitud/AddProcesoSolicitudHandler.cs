using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.InterfazDat;
using Domain.Funcionalidades;
using iText.Kernel.Pdf.Canvas.Wmf;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application.TarjetasCredito.AgregarComentario
{
    public class AddProcesoSolicitudHandler : IRequestHandler<ReqAddProcesoSolicitud, ResAddProcesoSolicitud>
    {
        public readonly ApiSettings _settings;
        private readonly IParametersInMemory _parametersInMemory;
        private readonly IFuncionalidadesMemory _funcionalidadesMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;

        public AddProcesoSolicitudHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory, IFuncionalidadesMemory funcionalidadesMemory)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
            _funcionalidadesMemory = funcionalidadesMemory;
        }

        public async Task<ResAddProcesoSolicitud> Handle(ReqAddProcesoSolicitud reqAgregarComentario, CancellationToken cancellationToken)
        {
            ResAddProcesoSolicitud respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "ADD_PROCESO_SOLICITUD";
            respuesta.LlenarResHeader( reqAgregarComentario );
            string estado, func_nombre = "";
            int int_funcionalidad;

            try
            {
                await _logs.SaveHeaderLogs( reqAgregarComentario, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                (reqAgregarComentario.int_estado, estado)= Validaciones.obtener_nuevo_estado_proceso( reqAgregarComentario.int_estado, _parametersInMemory, reqAgregarComentario.bl_regresa_estado);

                if (reqAgregarComentario.int_estado > 0)
                {
                    for (int i = 0; i < _settings.permisosAccion.Count; i++)
                    {
                        if (estado == _settings.estadosSolTC[i]) func_nombre = _settings.permisosAccion[i];

                        if (func_nombre != "")
                        {
                            int_funcionalidad = _funcionalidadesMemory.FindFuncionalidadNombre( func_nombre ).fun_id;

                            if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( reqAgregarComentario.str_id_perfil ), int_funcionalidad ))
                            {
                                res_tran = await _tarjetasCreditoDat.addProcesoSolicitud( reqAgregarComentario );

                                if(_parametersInMemory.FindParametroId(reqAgregarComentario.int_estado).str_nemonico == _settings.estado_analisis_gestor)
                                {

                                }

                                res_tran.codigo = "000";
                                func_nombre = "";
                            }
                            else
                            {
                                res_tran.diccionario.Add( "str_error", "No tiene permiso para realizar la acción que está intentando" );
                                res_tran.codigo = "001";
                            }
                        }
                    }
                }
                else
                {
                    res_tran.diccionario.Add( "str_error", "El proceso que intenta realizar no se encuentra disponible por el momento" );
                    res_tran.codigo = "001";
                }

                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_estado_transaccion = respuesta.str_res_codigo == "000" ? "OK" : "ERR";
                respuesta.str_res_info_adicional = respuesta.str_res_codigo == "000" ? "" : res_tran.diccionario["str_error"].ToString() ;

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

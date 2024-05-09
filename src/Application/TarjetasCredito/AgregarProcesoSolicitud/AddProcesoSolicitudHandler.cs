using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.AnalistasCredito.AddAnalistaSolicitud;
using Application.TarjetasCredito.AnalistasCredito.GetAnalistas;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application.TarjetasCredito.AgregarComentario
{
    public class AddProcesoSolicitudHandler : IRequestHandler<ReqAddProcesoSolicitud, ResAddProcesoSolicitud>
    {
        public readonly ApiSettings _settings;
        private readonly IParametersInMemory _parametersInMemory;
        private readonly IFuncionalidadesInMemory _funcionalidadesMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly IAnalistasCreditoDat _analistasCreditoDat;
        private readonly IAnalistaSolicitudDat _analistaSolicitudDat;
        private readonly ILogs _logs;
        private readonly string str_clase;

        public AddProcesoSolicitudHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory, IFuncionalidadesInMemory funcionalidadesMemory, IAnalistasCreditoDat analistasCreditoDat, IAnalistaSolicitudDat analistaSolicitudDat)
        {
            _tarjetasCreditoDat = tarjetasCreditoDat;
            _logs = logs;
            str_clase = GetType().Name;
            _parametersInMemory = parametersInMemory;
            _settings = options.CurrentValue;
            _funcionalidadesMemory = funcionalidadesMemory;
            _analistasCreditoDat = analistasCreditoDat;
            _analistaSolicitudDat = analistaSolicitudDat;
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

                (reqAgregarComentario.int_estado, estado) = Validaciones.obtener_nuevo_estado_proceso( reqAgregarComentario.int_estado, _parametersInMemory, reqAgregarComentario.bl_regresa_estado );

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

                                if (res_tran.codigo == "000" && _parametersInMemory.FindParametroId( reqAgregarComentario.int_estado ).str_nemonico == _settings.estado_analisis_uac)
                                {
                                    ReqGetAnalistasCredito getAnalistasCredito = new ReqGetAnalistasCredito();
                                    getAnalistasCredito.str_id_oficina = reqAgregarComentario.str_id_oficina;
                                    res_tran = await _analistasCreditoDat.getAnalistasCredito( getAnalistasCredito );
                                    var lst_analistas = Mapper.ConvertConjuntoDatosToListClass<ResGetAnalistasCredito.Analistas>( res_tran.cuerpo );
                                    string id_analista = null!, login_analista= null!;
                                    for (int j = 0; j < lst_analistas.Count; j++) 
                                    {
                                        id_analista = id_analista + lst_analistas[j].int_id_usuario.ToString() + "|";
                                        login_analista = login_analista + lst_analistas[j].str_login.ToString() + "|";
                                    }
                                    id_analista = id_analista.TrimEnd( '|' );
                                    login_analista = login_analista.TrimEnd( '|' );
                                    ReqAddAnalistaSolicitud addAnalistaSolicitud = new ReqAddAnalistaSolicitud();
                                    addAnalistaSolicitud.int_id_solicitud = reqAgregarComentario.int_id_solicitud;
                                    addAnalistaSolicitud.str_id_analista = id_analista;
                                    addAnalistaSolicitud.str_analista = login_analista;
                                    res_tran = await _analistaSolicitudDat.addAnalistaSolicitud( addAnalistaSolicitud );
                                }

                                res_tran.codigo = "000";
                                func_nombre = "";
                            }
                            else
                            {
                                res_tran.diccionario.Add( "str_error", "No tiene permiso para realizar la acción que está intentando" );
                                res_tran.codigo = "001";
                            }
                            break;
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
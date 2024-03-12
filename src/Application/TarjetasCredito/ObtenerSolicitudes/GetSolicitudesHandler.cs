using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.InterfazDat;
using iText.Kernel.Pdf.Canvas.Wmf;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;
using static Application.TarjetasCredito.ObtenerSolicitudes.ResGetSolicitudes;

namespace Application.TarjetasCredito.ObtenerSolicitudes
{
    public class GetSolicitudesHandler : IRequestHandler<ReqGetSolicitudes, ResGetSolicitudes>
    {
        private readonly ApiSettings _settings;
        private readonly IParametersInMemory _parametersInMemory;
        private readonly IFuncionalidadesMemory _funcionalidadesMemory;
        private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
        private readonly ILogs _logs;
        private readonly string str_clase;

        public GetSolicitudesHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory, IFuncionalidadesMemory funcionalidadesMemory)
        {
            this._settings = options.CurrentValue;
            this._tarjetasCreditoDat = tarjetasCreditoDat;
            this._logs = logs;
            str_clase = GetType().Name;
            this._parametersInMemory = parametersInMemory;
            _funcionalidadesMemory = funcionalidadesMemory;
        }

        public async Task<ResGetSolicitudes> Handle(ReqGetSolicitudes reqGetSolicitudes, CancellationToken cancellationToken)
        {
            ResGetSolicitudes respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "GET_SOLICITUDES_TC";
            respuesta.LlenarResHeader( reqGetSolicitudes );

            try
            {
                await _logs.SaveHeaderLogs( reqGetSolicitudes, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                for (int i = 0; i < _settings.permisosVisualizacion.Count; i++)
                {
                    if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( reqGetSolicitudes.str_id_perfil ),
                    _funcionalidadesMemory.FindFuncionalidadNombre( _settings.permisosVisualizacion[i] ).fun_id ))
                        reqGetSolicitudes.str_estado = reqGetSolicitudes.str_estado + _parametersInMemory.FindParametroNemonico( _settings.estadosSolTC[i] ).int_id_parametro.ToString() + "|";
                }
                reqGetSolicitudes.str_estado = reqGetSolicitudes.str_estado.TrimEnd( '|' );

                res_tran = await _tarjetasCreditoDat.getSolititudesTc( reqGetSolicitudes );

                if (res_tran.codigo == "000")
                {
                    List<SolicitudTc> lista_solicitudes = Mapper.ConvertConjuntoDatosToListClass<SolicitudTc>( res_tran.cuerpo );

                    foreach (SolicitudTc solicitudes in lista_solicitudes)
                    {
                        solicitudes.str_tipo_tarjeta = _parametersInMemory.FindParametroId( solicitudes.int_tipo_tarjeta ).str_valor_ini;
                        solicitudes.int_estado = Convert.ToInt32( solicitudes.str_estado );
                        solicitudes.str_estado = _parametersInMemory.FindParametroId( solicitudes.int_estado ).str_valor_ini;

                        respuesta.solicitudes.Add( solicitudes );

                    }
                }
                else
                {
                    respuesta.str_res_codigo = "001";
                    respuesta.str_res_info_adicional = "No existen solicitudes";
                }
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

        public string validar_estados_visualizar(string perfil)
        {
            string estados = "";

            for (int i = 0; i<_settings.permisosVisualizacion.Count; i++)
            {
                if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ),
                _funcionalidadesMemory.FindFuncionalidadNombre( _settings.permisosVisualizacion[i] ).fun_id ))
                    estados = estados  + _parametersInMemory.FindParametroNemonico( _settings.estadosSolTC[i] ).int_id_parametro.ToString() + "|";
                    estados = estados.TrimEnd( '|' );
            }

            //if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ),
            //    _funcionalidadesMemory.FindFuncionalidadNombre( _settings.verSolEstCreado ).int_id_permiso ))
            //    estados = _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro.ToString();

            //if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ),
            //    _funcionalidadesMemory.FindFuncionalidadNombre( _settings.verSolEstCreado ).int_id_permiso ))
            //    estados = estados + "|" + _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro.ToString();

            //if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ),
            //    _funcionalidadesMemory.FindFuncionalidadNombre( _settings.verSolEstCreado ).int_id_permiso ))
            //    estados = estados + "|" + _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro.ToString();

            //if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ),
            //    _funcionalidadesMemory.FindFuncionalidadNombre( _settings.verSolEstCreado ).int_id_permiso ))
            //    estados = estados + "|" + _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro.ToString();

            //if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ),
            //    _funcionalidadesMemory.FindFuncionalidadNombre( _settings.verSolEstCreado ).int_id_permiso ))
            //    estados = estados + _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro.ToString();

            return estados;
        }
    }
}

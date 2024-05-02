using Application.Common.Interfaces.Dat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.InterfazDat;
using Application.TarjetasCredito.ObtenerSolicitudes;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Application.TarjetasCredito.ObtenerSolicitudes.ResGetSolicitudes;

namespace Application.TarjetasCredito.AnalistasCredito.Get
{
    public class GetAnalistasCreditoHandler
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

        public async Task<ResGetSolicitudes> Handle(ReqGetSolicitudes reqGetSolicitudes, CancellationToken cancellationToken)
        {
            ResGetSolicitudes respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "GET_SOLICITUDES_TC";
            respuesta.LlenarResHeader( reqGetSolicitudes );
            var funcionalidad = new Domain.Funcionalidades.Funcionalidad();

            try
            {
                await _logs.SaveHeaderLogs( reqGetSolicitudes, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                /*for (int i = 0; i < _settings.permisosVisualizacion.Count; i++)
                {
                    funcionalidad = _funcionalidadesMemory.FindFuncionalidadNombre( _settings.permisosVisualizacion[i] );
                    if (funcionalidad != null)
                    {
                        if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( reqGetSolicitudes.str_id_perfil ), funcionalidad.fun_id ))
                            reqGetSolicitudes.str_estado = reqGetSolicitudes.str_estado + _parametersInMemory.FindParametroNemonico( _settings.estadosSolTC[i] ).int_id_parametro.ToString() + "|";
                    }
                }
                reqGetSolicitudes.str_estado = reqGetSolicitudes.str_estado.TrimEnd( '|' );
                */

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

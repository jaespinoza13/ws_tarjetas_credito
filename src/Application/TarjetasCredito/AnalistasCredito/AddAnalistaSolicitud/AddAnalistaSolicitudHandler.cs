using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.AnalistasCredito.GetAnalistas;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application.TarjetasCredito.AnalistasCredito.AddAnalistaSolicitud
{
    public class AddAnalistaSolicitudHandler : IRequestHandler<ReqAddAnalistaSolicitud, ResAddAnalistaSolicitud>
    {
        private readonly ApiSettings _settings;
        private readonly IParametersInMemory _parametersInMemory;
        private readonly IAnalistasCreditoDat _analistasCreditoDat;
        private readonly IAnalistaSolicitudDat _analistaSolicitudDat;
        private readonly ILogs _logs;
        private readonly string str_clase;

        public AddAnalistaSolicitudHandler(IOptionsMonitor<ApiSettings> options, ILogs logs, IParametersInMemory parametersInMemory, IAnalistasCreditoDat analistasCreditoDat, IAnalistaSolicitudDat analistaSolicitudDat)
        {
            this._settings = options.CurrentValue;
            this._logs = logs;
            str_clase = GetType().Name;
            this._parametersInMemory = parametersInMemory;
            this._analistasCreditoDat = analistasCreditoDat;
            this._analistaSolicitudDat = analistaSolicitudDat;
        }

        public async Task<ResAddAnalistaSolicitud> Handle(ReqAddAnalistaSolicitud reqAddAnalistaSolicitud, CancellationToken cancellationToken)
        {
            ResAddAnalistaSolicitud respuesta = new();
            RespuestaTransaccion res_tran = new();
            const string str_operacion = "ADD_ANALISTA_SOLICITUD";
            respuesta.LlenarResHeader( reqAddAnalistaSolicitud );
            var funcionalidad = new Domain.Funcionalidades.Funcionalidad();
            string a = null!;

            try
            {
                await _logs.SaveHeaderLogs( reqAddAnalistaSolicitud, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

                if (_parametersInMemory.FindParametroId( reqAddAnalistaSolicitud.int_estado ).str_nemonico == _settings.estado_analisis_uac)
                {
                    if (String.IsNullOrEmpty( reqAddAnalistaSolicitud.str_id_analista ))
                    {
                        ReqGetAnalistasCredito getAnalistasCredito = new ReqGetAnalistasCredito();
                        getAnalistasCredito.str_id_oficina = reqAddAnalistaSolicitud.str_id_oficina;
                        res_tran = await _analistasCreditoDat.getAnalistasCredito( getAnalistasCredito );
                        var lst_analistas = Mapper.ConvertConjuntoDatosToListClass<ResGetAnalistasCredito.Analistas>( res_tran.cuerpo );
                        for (int j = 0; j < lst_analistas.Count; j++)
                        {
                            reqAddAnalistaSolicitud.str_id_analista = reqAddAnalistaSolicitud.str_id_analista + lst_analistas[j].int_id_usuario.ToString() + "|";
                            reqAddAnalistaSolicitud.str_analista = reqAddAnalistaSolicitud.str_id_analista + lst_analistas[j].str_login + "|";
                        }
                        reqAddAnalistaSolicitud.str_id_analista = reqAddAnalistaSolicitud.str_id_analista.TrimEnd( '|' );
                    }

                    res_tran = await _analistaSolicitudDat.addAnalistaSolicitud( reqAddAnalistaSolicitud );

                    respuesta.str_res_codigo = "001";
                    respuesta.str_res_info_adicional = "No existen prospecciones";

                    respuesta.str_res_estado_transaccion = res_tran.codigo == "000" ? "OK" : "ERR";
                }
                else
                {
                    respuesta.str_res_codigo = "001";
                    respuesta.str_res_estado_transaccion = "ERR";
                    respuesta.str_res_info_adicional = "No se puede realizar este proceso para esta solicitud de tarjeta";
                }
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

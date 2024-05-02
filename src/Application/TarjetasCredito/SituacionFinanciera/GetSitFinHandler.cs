using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.SituacionFinanciera;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace Application.TarjetasCredito.SituacionFinanciera
{
    public class GetSitFinHandler : IRequestHandler<ReqGetSitFin, ResGetSitFin>
    {
        private readonly ISitFinDat _infoSitDat;
        private readonly ILogs _logs;
        public readonly IMemoryCache _memoryCache;
        private readonly string str_clase;
        private readonly string str_operacion;
        public DateTime dt_fecha_codigos;
        public GetSitFinHandler(ISitFinDat infoSitFinDat, ILogs logs, IMemoryCache memoryCache)
        {
            _infoSitDat = infoSitFinDat;
            _logs = logs;
            str_clase = GetType().Name;
            str_operacion = "GET_SITUACION_FINANCIERA";
            this._memoryCache = memoryCache;
        }

        public async Task<ResGetSitFin> Handle(ReqGetSitFin request, CancellationToken cancellationToken)
        {
            ResGetSitFin respuesta = new();
            respuesta.LlenarResHeader( request );
            try
            {
                await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
                RespuestaTransaccion res_tran = new();
                res_tran = await _infoSitDat.get_situacion_financiera( request );
                List<DepositosPlazoFijo> data_lst_dep = new List<DepositosPlazoFijo>();
                List<CreditosHistoricos> data_lst_cred = new List<CreditosHistoricos>();
                respuesta.lst_dep_plazo_fijo = Conversions.ConvertConjuntoDatosTableToListClass<DepositosPlazoFijo>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
                respuesta.lst_creditos_historicos = Conversions.ConvertConjuntoDatosTableToListClass<CreditosHistoricos>( (ConjuntoDatos)res_tran.cuerpo, 1 )!;
                foreach (DepositosPlazoFijo dpf in respuesta.lst_dep_plazo_fijo)
                {
                    DepositosPlazoFijo obj_dpf = new DepositosPlazoFijo
                    {
                        int_id_cuenta = dpf.int_id_cuenta,
                        str_num_cuenta = dpf.str_num_cuenta,
                        dcm_ahorro = dpf.dcm_ahorro,
                        dtt_fecha_movimiento = dpf.dtt_fecha_movimiento,
                        dcm_promedio = dpf.dcm_promedio,
                        str_tipo_cta = dpf.str_tipo_cta,
                        str_estado = dpf.str_estado,
                        int_orden = dpf.int_orden,
                    };
                    data_lst_dep.Add( obj_dpf );
                }
                respuesta.lst_dep_plazo_fijo = data_lst_dep;

                foreach (CreditosHistoricos cred_hist in respuesta.lst_creditos_historicos)
                {
                    CreditosHistoricos obj_cred_hist = new CreditosHistoricos
                    {
                        int_operacion = cred_hist.int_operacion,
                        str_tipo = cred_hist.str_tipo,
                        str_operacion_cred = cred_hist.str_operacion_cred,
                        dcm_monto_aprobado = cred_hist.dcm_monto_aprobado,
                        dtt_fecha_concesion = cred_hist.dtt_fecha_concesion,
                        dtt_fecha_vencimiento = cred_hist.dtt_fecha_vencimiento,
                        int_cuotas_vencidas = cred_hist.int_cuotas_vencidas,
                        int_dias_mora = cred_hist.int_dias_mora,
                        int_orden = cred_hist.int_orden,
                    };
                    data_lst_cred.Add( obj_cred_hist );
                }
                respuesta.lst_creditos_historicos = data_lst_cred;
                respuesta.str_res_codigo = res_tran.codigo;
                //Analizar si se deja esta sección
                if (data_lst_dep.Any())
                {
                    _memoryCache.Set( $"Informacion_dpfs_{request.str_ente}_ente", data_lst_dep );
                }
                if (data_lst_cred.Any())
                {
                    _memoryCache.Set( $"Informacion_cred_hist_{request.str_ente}_ente", data_lst_cred );
                }
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
                await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            }
            catch (Exception e)
            {

                await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
                throw new ArgumentException( respuesta.str_id_transaccion );
            }
            return respuesta;

        }
    }
}

using Application.Common.Converting;
using Application.Common.Functions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.Agencias;
using Domain.Entities.Orden;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace Application.TarjetasCredito.OrdenReporte
{
    public class GetReporteOrdenHandler : IRequestHandler<ReqGetReporteOrden, ResGetReporteOrden>
    {

        private readonly ILogs _logs;
        public readonly IMemoryCache _memoryCache;
        private readonly string str_clase;
        private readonly string str_operacion;
        public DateTime dt_fecha_codigos;
        private readonly IOrdenDat _iordenDat;

        public GetReporteOrdenHandler(ILogs logs, IMemoryCache memoryCache, IOrdenDat iordenDat)
        {
            _logs = logs;
            str_clase = GetType().Name;
            str_operacion = "GET_REPORTE_ORDEN";
            this._memoryCache = memoryCache;
            _iordenDat = iordenDat;

        }

        public async Task<ResGetReporteOrden> Handle(ReqGetReporteOrden request, CancellationToken cancellationToken)
        {
            ResGetReporteOrden respuesta = new();

            /*
             var orden  = @" 
                {
                    'orden': 164,
                    'prefijo_tarjeta': '53',
                    'cost_emision': 'cobro_emision',
                    'descripcion': 'TARJETAS SOLICITADAS PARA MES DE ABRIL',
                    'agencia_solicita': 'MATRIZ',
                    'tarjetas_solicitadas':
                        [
                        { 'cuenta': '410010064540', tipo_identificacion: 'C', identificacion: '1150214375', ente: '189610', nombre: 'DANNY VASQUEZ', nombre_impreso: 'DANNY VASQUEZ', tipo: 'BLACK', cupo: '8000', key: 23, Agencia: { nombre: 'MATRIZ', id: '1' } },
                        { 'cuenta': '410010061199', tipo_identificacion: 'R', identificacion: '1105970712001', ente: '515146', nombre: 'JUAN TORRES', nombre_impreso: 'JUAN TORRES', tipo: 'GOLDEN', cupo: '15000', key: 38, Agencia: { nombre: 'MATRIZ', id: '1' } },
                        { 'cuenta': '410010061514', tipo_identificacion: 'R', identificacion: '1105970714001', ente: '515148', nombre: 'ROBERTH TORRES', nombre_impreso: 'ROBERTH TORRES', tipo: 'ESTÁNDAR', cupo: '15000', key: 58, Agencia: { nombre: 'MATRIZ', id: '1' } },
                        { 'cuenta': '410010064000', tipo_identificacion: 'P', identificacion: 'PZ970715', ente: '515149', nombre: 'ROBERTH TORRES', nombre_impreso: 'ROBERTH TORRES', tipo: 'GOLDEN', cupo: '15000', key: 68, Agencia: { nombre: 'MATRIZ', id: '1' } }
                        ]

                }";
             */

            try
            {
                //LLAMAR A INTERFAZ QUE OBTIENE EL REPORTE
                RespuestaTransaccion res_tran = new();

                //Console.WriteLine( "RESULT REQGEST" );
                //Console.WriteLine( request.str_numero_orden );
                res_tran = await _iordenDat.get_reporte_orden( request );


                //TODO: REVIsAR 
                //respuesta.lst_agencias = Conversions.ConvertConjuntoDatosTableToListClass<Agencias>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
                //var conver = Conversions.ConvertConjuntoDatosTableToListClass<Orden>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
                //List<ProspectosTc> lista_prospectos = Mapper.ConvertConjuntoDatosToListClass<ProspectosTc>( res_tran.cuerpo );


                //DATOS QUEMADOS PARA CREAR REPORTE
                Orden orden_buscada = new();

                orden_buscada.str_estado_orden = "CREADA";
                orden_buscada.str_numero_orden = "164";
                orden_buscada.str_prefijo = "2500 53";
                orden_buscada.str_costo_emision = "2500 53";
                orden_buscada.str_descripcion = "TARJETAS SOLICITADAS PARA MES DE ABRIL";
                orden_buscada.str_agencia_solicita = "MATRIZ";

                Tarjeta_Solicitada tar_sol_n1 = new() { str_cuenta = "410010064540", str_tipo_identificacion= "C", str_identificacion = "1150214375", str_ente = "189610", str_nombre= "DANNY VASQUEZ", str_nombre_impreso = "DANNY VASQUEZ", str_tipo = "BLACK", str_cupo= "8000" };
                Tarjeta_Solicitada tar_sol_n2 = new () { str_cuenta = "410010061199", str_tipo_identificacion = "R", str_identificacion = "1105970712001", str_ente = "515146", str_nombre = "JUAN TORRES", str_nombre_impreso = "JUAN TORRES", str_tipo = "GOLDEN", str_cupo = "15000" };
                Tarjeta_Solicitada tar_sol_n3 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "2000" };
                Tarjeta_Solicitada tar_sol_n4 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "3000" };
                Tarjeta_Solicitada tar_sol_n5 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "4000" };
                Tarjeta_Solicitada tar_sol_n6 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "5000" };
                Tarjeta_Solicitada tar_sol_n7 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "6000" };
                Tarjeta_Solicitada tar_sol_n8 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "7000" };
                Tarjeta_Solicitada tar_sol_n9 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "8000" };
                Tarjeta_Solicitada tar_sol_n10 = new() { str_cuenta = "410010061514", str_tipo_identificacion = "R", str_identificacion = "1105970714001", str_ente = "515148", str_nombre = "ROBERTH TORRES", str_nombre_impreso = "ROBERTH TORRES", str_tipo = "ESTÁNDAR", str_cupo = "9000" };
                
                List<object> tarjetas = new List<object>() { tar_sol_n1, tar_sol_n2 , tar_sol_n3, tar_sol_n4, tar_sol_n5, tar_sol_n6, tar_sol_n7, tar_sol_n8, tar_sol_n9, tar_sol_n10 };
                orden_buscada.lst_tarjetas_solicitadas = tarjetas;
                orden_buscada.str_cantidad = tarjetas.Count()+"";


                //PDF GENERAR
                byte[] doc_pdf =  GenerarReporte.ReportePDF(orden_buscada );

                if(doc_pdf.Length == 0)
                {
                    _ = _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, new Exception( "ERROR EN CLASE ReportePDF AL GENERAR PDF ORDEN" ) );
                    throw new ArgumentException( respuesta.str_id_transaccion );
                }

                respuesta.byt_reporte = doc_pdf;
                respuesta.str_res_codigo = res_tran.codigo;

            }
            catch (Exception ex)
            {
                _ = _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, ex );
                throw new ArgumentException( respuesta.str_id_transaccion );
            }
            _ = _logs.SaveResponseLogs( "PDF GENERADO EXITOSAMENTE", str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            return respuesta;

        }
    }
}

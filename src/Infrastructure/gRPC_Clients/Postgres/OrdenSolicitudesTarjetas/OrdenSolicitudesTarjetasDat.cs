using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Application.TarjetasCredito.OrdenReporte;
using Domain.Entities.Agencias;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;

namespace Infrastructure.gRPC_Clients.Postgres.OrdenSolicitudesTarjetas
{
    public class OrdenSolicitudesTarjetasDat : IOrdenDat
    {
        private readonly ILogs _logService;
        private readonly DALPostgreSqlClient _objClienteDal;
        private readonly string str_clase;
        private readonly ApiSettings _settings;
        public OrdenSolicitudesTarjetasDat(IOptionsMonitor<ApiSettings> options, ILogs logService, DALPostgreSqlClient objClienteDal)
        {
            _logService = logService;
            _settings = options.CurrentValue;
            str_clase = GetType().FullName!;

            var handler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds( _settings.delayOutGrpcSybase ),
                KeepAlivePingTimeout = TimeSpan.FromSeconds( _settings.timeoutGrpcSybase ),
                EnableMultipleHttp2Connections = true
            };
            var canal = GrpcChannel.ForAddress( _settings.client_grpc_postgres!,
                new GrpcChannelOptions { HttpHandler = handler } );
            _objClienteDal = new DALPostgreSqlClient( canal );
        }

        //MODIFICAR
        public async Task<RespuestaTransaccion> get_reporte_orden(ReqGetReporteOrden request)
        {   

            //TODO: REVISAR PARA TRAER MEDIANTE FUNCIONES LAS CONSULTAS
            var respuesta = new RespuestaTransaccion();
            respuesta.codigo = "000";
            try
            {
               // Console.WriteLine(request.str_numero_orden);

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

                //JObject objetoJson = JObject.Parse( orden );
                dynamic objetoOrden = JsonConvert.DeserializeObject( orden );


                //var orden = "(164,53,cobro_emision,descripcion,TARJETAS SOLICITADAS PARA MES DE ABRIL,MATRIZ)";

                /*if (resultado.ListaTablas.Count > 0)
               {
                   respuesta.cuerpo = Funciones.ObtenerDataBasePg( resultado );
                   respuesta.codigo = "000";
                   respuesta.diccionario.Add( "str_error", "" );
               }*/
                respuesta.cuerpo = objetoOrden;
                respuesta.codigo = "000";
                //respuesta.cuerpo = Funciones.ObtenerDataBasePg( orden ); 
                respuesta.diccionario.Add( "str_error", "" );

                //Console.WriteLine( respuesta.cuerpo );


            }
            catch
            (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            }
            return respuesta;
        }
    }
}

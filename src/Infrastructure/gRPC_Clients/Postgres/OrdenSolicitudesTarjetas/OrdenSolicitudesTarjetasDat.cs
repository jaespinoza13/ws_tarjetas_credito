using AccesoDatosPostgresql.Neg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.Agencias;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;
using System.Reflection;

namespace Infrastructure.gRPC_Clients.Postgres.OrdenSolicitudesTarjetas
{
    public class OrdenSolicitudesTarjetasDat : IOrdenesTarjCredDat
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

        public async Task<RespuestaTransaccion> get_ordenes_tarj_cred(ReqGetOrdenesTC request)
        {

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                var ds = new DatosSolicitud();
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_orden_tipo", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_orden_tipo } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );
                ds.NombreSP = NameSps.getOrdenesTC;
                ds.NombreBD = _settings.DB_meg_tarjetas_credito;
                var resultado = _objClienteDal.ExecuteReader( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones
                var lst_valores = resultado.ListaPSalidaValores.ToList();
                respuesta.cuerpo = Funciones.ObtenerDataBasePg( resultado );
                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
                var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();
                respuesta.codigo = str_codigo.Trim().PadLeft( 3, '0' );
                respuesta.diccionario.Add( "str_o_error", str_error );


            }
            catch
            (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
                await _logService.SaveExceptionLogs( request, MethodBase.GetCurrentMethod()!.Name, "addSolicitudTC", str_clase, ex );
                throw new ArgumentException( request.str_id_transaccion );
            }
            return respuesta;
        }
    }
}

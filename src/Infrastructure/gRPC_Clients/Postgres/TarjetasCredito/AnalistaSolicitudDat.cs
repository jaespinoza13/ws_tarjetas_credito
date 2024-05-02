using AccesoDatosPostgresql.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.AnalistasCredito.AddSolicitud;
using Application.TarjetasCredito.AnalistasCredito.Get;
using Application.TarjetasCredito.InterfazDat;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AccesoDatosGrpcAse.Neg.DAL;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;

namespace Infrastructure.gRPC_Clients.Postgres.TarjetasCredito
{
    public class AnalistaSolicitudDat : IAnalistaSolicitudDat
    {
        private readonly ILogs _logService;
        private readonly DALPostgreSqlClient _objClienteDal;
        private readonly string str_clase;
        private readonly ApiSettings _settings;
        public AnalistaSolicitudDat(IOptionsMonitor<ApiSettings> options, ILogs logService, DALPostgreSqlClient objClienteDal)
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



        public async Task<RespuestaTransaccion> addAnalistaSolicitud(ReqAddAnalistaSolicitud reqAddAnalistaSolicitud)
        {
            RespuestaTransaccion respuesta = new RespuestaTransaccion();

            try
            {
                var ds = new DatosSolicitud();
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_analista", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddAnalistaSolicitud.str_id_analista } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_analista", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddAnalistaSolicitud.str_analista } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_solicitud", TipoDato = TipoDato.Integer, ObjValue = reqAddAnalistaSolicitud.int_id_solicitud.ToString() } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
                ds.NombreSP = NameSps.addAnalistaSolicitud;
                ds.NombreBD = _settings.DB_meg_tarjetas_credito;

                var resultado = _objClienteDal.ExecuteNonQuery( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );

                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
                var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();
                respuesta.codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
                respuesta.diccionario.Add( "str_o_error", str_error.ToString() );
            }
            catch (Exception exception)
            {
                respuesta.codigo = "001";
                respuesta.diccionario.Add( "str_o_error", exception.ToString() );
                await _logService.SaveErroresDb( reqAddAnalistaSolicitud, "", MethodBase.GetCurrentMethod()!.Name, str_clase, exception );
                throw new ArgumentException( reqAddAnalistaSolicitud.str_id_transaccion )!;
            }
            return respuesta;
        }
    }
}

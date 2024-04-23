using AccesoDatosPostgresql.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.ComentariosAsesor;
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
using static AccesoDatosPostgresql.Neg.DALPostgreSql;

namespace Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;

public class ComentariosAsesorTcDat : IComentarioAsesorDat
{
    private readonly ILogs _logService;
    private readonly DALPostgreSqlClient _objClienteDal;
    private readonly string str_clase;
    private readonly ApiSettings _settings;
    public ComentariosAsesorTcDat(IOptionsMonitor<ApiSettings> options, ILogs logService, DALPostgreSqlClient objClienteDal)
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
    public async Task<RespuestaTransaccion> AddComentario(ReqAddComentariosAsesor request)
    {
        RespuestaTransaccion respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_solicitud", TipoDato = TipoDato.Integer, ObjValue = request.int_id_sol.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_cmnt_ase_json", TipoDato = TipoDato.Json, ObjValue = request.str_cmnt_ase_json } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = NameSps.updComentariosAsesorTc;
            ds.NombreBD = _settings.DB_meg_tarjetas_credito;

            var resultado = _objClienteDal.ExecuteNonQuery( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones
            var lst_valores = resultado.ListaPSalidaValores.ToList();
            var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.codigo = str_codigo.Trim().PadLeft( 3, '0' );
            respuesta.diccionario.Add( "str_o_error", str_error );

        }
        catch (Exception ex)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            await _logService.SaveExceptionLogs( request, MethodBase.GetCurrentMethod()!.Name, "addSolicitudTC", str_clase, ex );
            throw new ArgumentException( request.str_id_transaccion );

        }

        return respuesta;
    }

    public async Task<RespuestaTransaccion> GetComentarios(ReqGetComentariosAsesor request)
    {
        RespuestaTransaccion respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_sol", TipoDato = TipoDato.Integer, ObjValue = request.int_id_sol.ToString() } );
            //ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@json_comentarios", TipoDato = TipoDato.Json } );
            //ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            //ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = NameSps.getComentariosAsesorTc;
            ds.NombreBD = _settings.DB_meg_tarjetas_credito;

            var resultado = _objClienteDal.ExecuteReader( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones

            if (resultado.ListaTablas.Count > 0)
            {
                respuesta.cuerpo = Funciones.ObtenerDataBasePg( resultado );
                respuesta.codigo = "000";
                respuesta.diccionario.Add( "str_o_error", "" );
            }

        }
        catch (Exception ex)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            await _logService.SaveExceptionLogs( request, MethodBase.GetCurrentMethod()!.Name, "addSolicitudTC", str_clase, ex );
            throw new ArgumentException( request.str_id_transaccion );

        }

        return respuesta;
    }


}

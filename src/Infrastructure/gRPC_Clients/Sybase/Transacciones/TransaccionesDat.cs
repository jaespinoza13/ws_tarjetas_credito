using System.Reflection;
using AccesoDatosPostgresql.Neg;
using Application.Transacciones.ObtenerTransacciones;
using Application.Transacciones.ActualizarTransacciones;
using Application.Transacciones.ObtenerEstadosMonitoreo;
using Application.Transacciones.InterfazDat;
using Application.Common.Interfaces;
using Application.Common.Models;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using Infrastructure.Services;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Application.Common.ISO20022.Models;


namespace Infrastructure.gRPC_Clients.Sybase.Transacciones;

public class TransaccionesDat : ITransaccionesDat
{

    private readonly ILogs _logService;
    private readonly DALPostgreSqlClient _objClienteDal;
    private readonly string str_clase;
    private readonly ApiSettings _settings;

    public TransaccionesDat(IOptionsMonitor<ApiSettings> options, ILogs logService)
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
        var canal = GrpcChannel.ForAddress( _settings.client_grpc_sybase!,
            new GrpcChannelOptions { HttpHandler = handler } );
        _objClienteDal = new DALPostgreSqlClient( canal );

    }



    public async Task<RespuestaTransaccion> obtener_transaccion(ReqObtenerTransaccion request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nemonico", TipoDato = TipoDato.VarChar, ObjValue = request.str_nemonico } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_desde", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_desde.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_hasta", TipoDato = TipoDato.DateTime, ObjValue = request.dtt_fecha_hasta.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_servicio", TipoDato = TipoDato.Integer, ObjValue = request.int_servicio.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_pago", TipoDato = TipoDato.Integer, ObjValue = request.int_pago.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_pagina", TipoDato = TipoDato.Integer, ObjValue = request.int_pagina.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer, ObjValue = request.int_estado.ToString() } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_total_filas", TipoDato = TipoDato.Integer } );
            
            // Parametros de salida
           // ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );

            ds.NombreSP = "get_pagos_monitoreo";
            ds.NombreBD = _settings.DB_meg_convenios;

            //var resultado = _objClienteDal.ExecuteDataSet( ds );
            //var lst_valores = resultado.ListaPSalidaValores.ToList();

            //var str_total_filas = lst_valores.Find( x => x.StrNameParameter == "@int_total_filas" )!.ObjValue;
            //var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            //var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();


          //  respuesta.str_codigo = str_codigo.Trim().PadLeft( 3, '0' );

            //respuesta.diccionario.Add( "str_o_error", str_error );
            //respuesta.diccionario.Add( "int_total_registros", str_total_filas );

            if (respuesta.str_codigo == "000")
            {
               // respuesta.obj_cuerpo = Funciones.ObtenerDatos( resultado );
            }



        }
        catch (Exception ex)
        {
            respuesta.str_codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            await _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, ex, str_clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }


    public async Task<RespuestaTransaccion> actualizar_transaccion(ReqActualizarTransaccion request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();
            Funciones.llenar_datos_auditoria_salida( ds, request );
           // ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_pagos_id", TipoDato = TipoDato.VarChar, ObjValue = request.str_pagos_id.ToString() } ); 
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado_pago", TipoDato = TipoDato.Integer, ObjValue = request.int_estado_id.ToString() } );
           // ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_justificacion", TipoDato = TipoDato.VarChar, ObjValue = request.str_justificacion.ToString() } );

            ds.NombreSP = "set_estado_pagos";
            ds.NombreBD = _settings.DB_meg_convenios;

            //var resultado = _objClienteDal.ExecuteDataSet( ds );
            //var lst_valores = resultado.ListaPSalidaValores.ToList();

//            var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
  //          var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

    //        respuesta.str_codigo = str_codigo.Trim().PadLeft( 3, '0' );

      //      respuesta.diccionario.Add( "str_o_error", str_error );

            if (respuesta.str_codigo == "000" || respuesta.str_codigo == "002")
            {
        //        respuesta.obj_cuerpo = Funciones.ObtenerDatos( resultado );
            }


        }
        catch (Exception ex)
        {
            respuesta.str_codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            await _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, ex, str_clase );
            throw new ArgumentException( request.str_id_transaccion );
        }
        return respuesta;
    }


    public async Task<RespuestaTransaccion> obtener_estados_monitoreo(ReqObtenerEstado request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            // Parametros de salida
            //ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );

            ds.NombreSP = "get_estados_monitoreo";
            ds.NombreBD = _settings.DB_meg_convenios;

            //var resultado = _objClienteDal.ExecuteDataSet( ds );
            //var lst_valores = resultado.ListaPSalidaValores.ToList();

            //var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            //var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();


            //respuesta.str_codigo = str_codigo.Trim().PadLeft( 3, '0' );

//            respuesta.diccionario.Add( "str_o_error", str_error );

            if (respuesta.str_codigo == "000")
            {
  //              respuesta.obj_cuerpo = Funciones.ObtenerDatos( resultado );
            }



        }
        catch (Exception ex)
        {
            respuesta.str_codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            await _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, ex, str_clase );
            throw new ArgumentException( request.str_id_transaccion );
        }

        return respuesta;
    }

}


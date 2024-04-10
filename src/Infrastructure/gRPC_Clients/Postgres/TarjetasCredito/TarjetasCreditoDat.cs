using AccesoDatosPostgresql.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.ActualizarSolicitudTC;
using Application.TarjetasCredito.AgregarComentario;
using Application.TarjetasCredito.AgregarProspectoTC;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.InterfazDat;
using Application.TarjetasCredito.ObtenerFlujoSolicitud;
using Application.TarjetasCredito.ObtenerSolicitudes;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using System.Reflection;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;

namespace Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;

public class TarjetasCreditoDat : ITarjetasCreditoDat
{
    private readonly ILogs _logService;
    private readonly DALPostgreSqlClient _objClienteDal;
    private readonly string str_clase;
    private readonly ApiSettings _settings;

    public TarjetasCreditoDat(IOptionsMonitor<ApiSettings> options, ILogs logService, DALPostgreSqlClient objClienteDal)
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


        //_logService = logService;
        //_settings = options.CurrentValue;
        //str_clase = GetType().Name;
        //_objClienteDal = objClienteDal;

    }

    public async Task<RespuestaTransaccion> addSolicitudTc(ReqAddSolicitudTc request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            //Datos socio
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_documento", TipoDato = TipoDato.Character, ObjValue = request.str_tipo_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_num_documento", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_num_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = request.str_ente.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = request.int_ente.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nombres", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_nombres } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_primer_apellido", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_primer_apellido } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_segundo_apellido", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_segundo_apellido } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_nacimiento", TipoDato = TipoDato.Date, ObjValue = request.dtt_fecha_nacimiento.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_sexo", TipoDato = TipoDato.Character, ObjValue = request.str_sexo } );

            // tcr_solicitudes
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_tipo_tarjeta", TipoDato = TipoDato.Integer, ObjValue = request.int_tipo_tarjeta.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer, ObjValue = request.int_estado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado_entregado", TipoDato = TipoDato.Integer, ObjValue = request.int_estado_entregado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_solicitado", TipoDato = TipoDato.Money, ObjValue = request.dec_cupo_solicitado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_sugerido", TipoDato = TipoDato.Money, ObjValue = request.dec_cupo_sugerido.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_aprobado", TipoDato = TipoDato.Money, ObjValue = request.dec_cupo_aprobado.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_tipo_registro", TipoDato = TipoDato.Integer, ObjValue = request.int_tipo_registro.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario_proc", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_usuario_proc } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_oficina_proc", TipoDato = TipoDato.Integer, ObjValue = request.int_oficina_proc.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_num_promotor", TipoDato = TipoDato.Integer, ObjValue = request.int_num_promotor.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_habilitada_compra", TipoDato = TipoDato.Character, ObjValue = request.str_habilitada_compra } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_max_compra", TipoDato = TipoDato.Numeric, ObjValue = request.dec_max_compra.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_denominacion_socio", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_denominacion_socio } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_denominacion_tarjeta", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_denominacion_tarjeta } );

            //Parametros
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_producto", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_codigo_producto } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_sucursal", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_sucursal.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_modelo_tratamiento", TipoDato = TipoDato.Integer, ObjValue = request.int_modelo_tratamiento.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_afinidad", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_codigo_afinidad } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_marca_graba", TipoDato = TipoDato.Character, ObjValue = request.str_marca_graba } );

            //Campos agregados 
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_calle_num_puerta", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_calle_num_puerta } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_barrio", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_barrio } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_provincia", TipoDato = TipoDato.Character, ObjValue = request.str_codigo_provincia } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_postal", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_codigo_postal } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_zona_geografica", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_zona_geografica } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_cod_lim_compra", TipoDato = TipoDato.Character, ObjValue = request.str_cod_lim_compra } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_numero_cuenta", TipoDato = TipoDato.Numeric, ObjValue = request.int_numero_cuenta.ToString() } );
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_cuenta_banc", TipoDato = TipoDato.Character, ObjValue = request.str_tipo_cuenta_banc } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comentario", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_comentario_proceso } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comentario_adicional", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_comentario_adicional } );
            //Hay que analizar si se mantiene los 3 sigueintes campos tomando en cuenta el que se encuentra comentado 
            //ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_doc_aut_cons_buro", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_id_doc_adicional } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_doc_tratamiento_datos_per", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_id_doc_tratamiento_datos_per } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_doc_adicional", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_id_doc_adicional } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_act_soc", TipoDato = TipoDato.Json, ObjValue = request.str_act_soc_json } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_pas_soc", TipoDato = TipoDato.Json, ObjValue = request.str_pas_soc_json } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_dpfs_soc", TipoDato = TipoDato.Json, ObjValue = request.str_dpfs_json} );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_crd_his_soc", TipoDato = TipoDato.Json, ObjValue = request.str_cred_hist_json } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_ing_soc", TipoDato = TipoDato.Json, ObjValue = request.str_ingr_soc_json } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_egr_soc", TipoDato = TipoDato.Json, ObjValue = request.str_egr_soc_json } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_cred_vig_soc", TipoDato = TipoDato.Json, ObjValue = request.str_cred_vig_json } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@json_gar_cns_soc", TipoDato = TipoDato.Json, ObjValue = request.str_gar_cns_json } );


            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = NameSps.addSolicitudTC;
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

    public async Task<RespuestaTransaccion> getSolititudesTc(ReqGetSolicitudes reqGetSolicitudes)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_perfil", TipoDato = TipoDato.CharacterVarying, ObjValue = reqGetSolicitudes.str_id_perfil.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario", TipoDato = TipoDato.CharacterVarying, ObjValue = reqGetSolicitudes.str_login.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_estados", TipoDato = TipoDato.CharacterVarying, ObjValue = reqGetSolicitudes.str_estado.ToString() } );

            ds.NombreSP = NameSps.getSolicitudesTc;
            ds.NombreBD = _settings.DB_meg_tarjetas_credito;


            var resultado = _objClienteDal.ExecuteReader( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones
            var lst_valores = resultado.ListaPSalidaValores.ToList();

            if (resultado.ListaTablas.Count > 0)
            {
                respuesta.cuerpo = Funciones.ObtenerDataBasePg( resultado );
                respuesta.codigo = "000";
                respuesta.diccionario.Add( "str_error", "" );
            }


        }
        catch (Exception ex)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            //await _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, ex, str_clase );

            await _logService.SaveExceptionLogs( reqGetSolicitudes, MethodBase.GetCurrentMethod()!.Name, "getSolicitudesTc", str_clase, ex );
            throw new ArgumentException( reqGetSolicitudes.str_id_transaccion );

        }
        return respuesta;
    }

    public async Task<RespuestaTransaccion> getProspectosTc(ReqGetSolicitudes reqGetSolicitudes)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario", TipoDato = TipoDato.CharacterVarying, ObjValue = reqGetSolicitudes.str_login.ToString() } );

            ds.NombreSP = NameSps.getProspectosTc;
            ds.NombreBD = _settings.DB_meg_tarjetas_credito;


            var resultado = _objClienteDal.ExecuteReader( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones
            var lst_valores = resultado.ListaPSalidaValores.ToList();

            if (resultado.ListaTablas.Count > 0)
            {
                respuesta.cuerpo = Funciones.ObtenerDataBasePg( resultado );
                respuesta.codigo = "000";
                respuesta.diccionario.Add( "str_error", "" );
            }


        }
        catch (Exception ex)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            //await _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, ex, str_clase );

            await _logService.SaveExceptionLogs( reqGetSolicitudes, MethodBase.GetCurrentMethod()!.Name, "getSolicitudesTc", str_clase, ex );
            throw new ArgumentException( reqGetSolicitudes.str_id_transaccion );

        }
        return respuesta;
    }

    public async Task<RespuestaTransaccion> addProcesoSolicitud(ReqAddProcesoSolicitud reqAgregarComentario)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_sol_id", TipoDato = TipoDato.Integer, ObjValue = reqAgregarComentario.int_id_solicitud.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comentario", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAgregarComentario.str_comentario.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAgregarComentario.str_login.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_oficina", TipoDato = TipoDato.Integer, ObjValue = reqAgregarComentario.str_id_oficina } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer, ObjValue = reqAgregarComentario.int_estado.ToString() } );

            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = NameSps.addComentarioProceso;
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
            await _logService.SaveExceptionLogs( reqAgregarComentario, MethodBase.GetCurrentMethod()!.Name, "addComentarioProceso", str_clase, ex );
            throw new ArgumentException( reqAgregarComentario.str_id_transaccion );

        }
        return respuesta;
    }

    public async Task<RespuestaTransaccion> getFlujoSolicitud(ReqGetFlujoSolicitud reqGetFlujoSolicitud)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_solicitud", TipoDato = TipoDato.Integer, ObjValue = reqGetFlujoSolicitud.int_id_solicitud.ToString() } );

            ds.NombreSP = NameSps.getFlujoSolicitud;
            ds.NombreBD = _settings.DB_meg_tarjetas_credito;

            var resultado = _objClienteDal.ExecuteReader( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones
            var lst_valores = resultado.ListaPSalidaValores.ToList();

            if (resultado.ListaTablas.Count > 0)
            {
                respuesta.cuerpo = Funciones.ObtenerDataBasePg( resultado );
                respuesta.codigo = "000";
                respuesta.diccionario.Add( "str_error", "" );
            }
        }
        catch (Exception ex)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            await _logService.SaveExceptionLogs( reqGetFlujoSolicitud, MethodBase.GetCurrentMethod()!.Name, "getFlujoSolititud", str_clase, ex );
            throw new ArgumentException( reqGetFlujoSolicitud.str_id_transaccion );

        }
        return respuesta;
    }

    public async Task<RespuestaTransaccion> updSolicitudTc(ReqActualizarSolicitudTC request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_solicitud", TipoDato = TipoDato.Integer, ObjValue = request.int_id_solicitud.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_flujo_sol", TipoDato = TipoDato.Integer, ObjValue = request.int_id_flujo_sol.ToString() } );

            //Datos socio
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nombres", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_nombres } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_primer_apellido", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_primer_apellido } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_segundo_apellido", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_segundo_apellido } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_nacimiento", TipoDato = TipoDato.Date, ObjValue = request.dtt_fecha_nacimiento.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_sexo", TipoDato = TipoDato.Character, ObjValue = request.str_sexo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = request.int_ente.ToString()} );

            // tcr_solicitudes
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer, ObjValue = request.int_estado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado_entregado", TipoDato = TipoDato.Integer, ObjValue = request.int_estado_entregado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_solicitado", TipoDato = TipoDato.Money, ObjValue = request.dec_cupo_solicitado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario_proc", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_usuario_proc } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_habilitada_compra", TipoDato = TipoDato.Character, ObjValue = request.str_habilitada_compra } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_max_compra", TipoDato = TipoDato.Numeric, ObjValue = request.dec_max_compra.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_denominacion_socio", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_denominacion_socio } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_denominacion_tarjeta", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_denominacion_tarjeta } );

            //Campos agregados 
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_calle_num_puerta", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_calle_num_puerta } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_barrio", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_barrio } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_provincia", TipoDato = TipoDato.Character, ObjValue = request.str_codigo_provincia } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_postal", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_codigo_postal } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_zona_geografica", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_zona_geografica } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_cod_lim_compra", TipoDato = TipoDato.Character, ObjValue = request.str_cod_lim_compra } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_numero_cuenta", TipoDato = TipoDato.Numeric, ObjValue = request.int_numero_cuenta.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_cuenta_banc", TipoDato = TipoDato.Character, ObjValue = request.str_tipo_cuenta_banc } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comentario", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_comentario_proceso } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_doc_aut_cons_buro", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_id_doc_adicional } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_doc_tratamiento_datos_per", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_id_doc_tratamiento_datos_per } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_doc_adicional", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_id_doc_adicional } );

            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = NameSps.updSolicitudTC;
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

    public async Task<RespuestaTransaccion> addProspectoTc(ReqAddProspectoTc reqAddProspectoTc)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_documento", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_tipo_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_num_documento", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_num_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = reqAddProspectoTc.int_ente.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nombres", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_nombres } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_apellidos", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_apellidos } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_celular", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_celular } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_correo", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_correo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_solicitado", TipoDato = TipoDato.Money, ObjValue = reqAddProspectoTc.dec_cupo_solicitado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario_crea", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_login} );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_oficina_crea", TipoDato = TipoDato.Integer, ObjValue = reqAddProspectoTc.str_id_oficina} );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_autorizacion_cons_buro", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_id_autoriza_cons_buro } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_autorizacion_datos_pers", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_id_autoriza_datos_per} );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comentario", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_comentario } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_comentario_adicional", TipoDato = TipoDato.CharacterVarying, ObjValue = reqAddProspectoTc.str_comentario_adicional } );

            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = NameSps.addProspeccionTC;
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
            await _logService.SaveExceptionLogs( reqAddProspectoTc, MethodBase.GetCurrentMethod()!.Name, "addProspectoTc", str_clase, ex );
            throw new ArgumentException( reqAddProspectoTc.str_id_transaccion );
        }
        return respuesta;
    }
}
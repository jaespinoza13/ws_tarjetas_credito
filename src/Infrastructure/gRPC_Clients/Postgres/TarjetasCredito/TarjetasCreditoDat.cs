using System.Reflection;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;

using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Infrastructure.Common.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;
using AccesoDatosPostgresql.Neg;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Google.Protobuf.WellKnownTypes;

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

    public async Task<RespuestaTransaccion> add_cliente(ReqAgregarSolicitudTc request)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {

            var ds = new DatosSolicitud();
            //Funciones.LlenarDatosAuditoria( ds, request );

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_tipo_documento", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_tipo_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_num_documento", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_num_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente", TipoDato = TipoDato.Integer, ObjValue = request.int_ente.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nombres", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_nombres } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_primer_apellido", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_primer_apellido } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_segundo_apellido", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_segundo_apellido } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_nacimiento", TipoDato = TipoDato.Date, ObjValue = request.dtt_fecha_nacimiento.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_sexo", TipoDato = TipoDato.Character, ObjValue = request.str_sexo } );

            // tcr_solicitudes
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_tipo_tarjeta", TipoDato = TipoDato.Integer, ObjValue = request.int_tipo_tarjeta.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_solicitado", TipoDato = TipoDato.Numeric, ObjValue = request.dec_cupo_solicitado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_cupo_aprobado", TipoDato = TipoDato.Numeric, ObjValue = request.dec_cupo_aprobado.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_celular", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_celular } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_correo", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_correo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_solicitud", TipoDato = TipoDato.TimestampWithoutTimeZone, ObjValue = request.dtt_fecha_solicitud.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dtt_fecha_actualizacion", TipoDato = TipoDato.TimestampWithoutTimeZone, ObjValue = request.dtt_fecha_actualizacion.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario_crea", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_usuario_crea } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_oficina_crea", TipoDato = TipoDato.Integer, ObjValue = request.int_oficina_crea.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_oficina_entrega", TipoDato = TipoDato.Integer, ObjValue = request.int_oficina_entrega.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_ente_aprobador", TipoDato = TipoDato.Integer, ObjValue = request.int_ente_aprobador.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_producto", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_codigo_producto } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_sucursal", TipoDato = TipoDato.Integer, ObjValue = request.int_codigo_sucursal.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_modelo_tratamiento", TipoDato = TipoDato.Integer, ObjValue = request.int_modelo_tratamiento.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo_afinidad", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_codigo_afinidad.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_num_promotor", TipoDato = TipoDato.Integer, ObjValue = request.int_num_promotor.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_habilitada_compra", TipoDato = TipoDato.Character, ObjValue = request.str_habilitada_compra } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_max_compra", TipoDato = TipoDato.Numeric, ObjValue = request.dec_max_compra.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_denominacion_tarjeta", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_denominacion_tarjeta } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_marca_graba", TipoDato = TipoDato.Character, ObjValue = request.str_marca_graba } );
            //Campos agregados 
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_calle_num_puerta", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_calle_num_puerta.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_localidad", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_localidad.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_barrio", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_barrio.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_provincia", TipoDato = TipoDato.Character, ObjValue = request.str_codigo_provincia } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_postal", TipoDato = TipoDato.Character, ObjValue = request.str_codigo_postal } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_zona_geografica", TipoDato = TipoDato.Character, ObjValue = request.str_zona_geografica } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_grupo_liquidacion", TipoDato = TipoDato.Character, ObjValue = request.str_grupo_liquidacion } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@dec_imp_lim_compras", TipoDato = TipoDato.Numeric, ObjValue = request.dec_imp_lim_compras.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_telefono_2", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_telefono_2.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_datos_adicionales", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_datos_adicionales.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_ocupacion", TipoDato = TipoDato.Character, ObjValue = request.str_codigo_ocupacion.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_duracion", TipoDato = TipoDato.Character, ObjValue = request.str_duracion } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_marca_emision", TipoDato = TipoDato.Character, ObjValue = request.str_marca_emision } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_rfc", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_rfc.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_marca_tpp", TipoDato = TipoDato.Character, ObjValue = request.str_marca_tpp } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_rsrv_uso_credencial_1", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_rsrv_uso_credencial_1.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_rsrv_uso_credencial_2", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_rsrv_uso_credencial_2.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_cuarta_linea", TipoDato = TipoDato.CharacterVarying, ObjValue = request.str_cuarta_linea.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_numero_cuenta", TipoDato = TipoDato.Numeric, ObjValue = request.int_numero_cuenta.ToString() } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );

            ds.NombreSP = "add_solicitud_tc"; //add_clint2
            ds.NombreBD = _settings.DB_meg_tarjetas_credito;


            var resultado = _objClienteDal.ExecuteNonQuery( ds );//ExecuteNonQuery para sps - ExecuteReader para funciones
            var lst_valores = resultado.ListaPSalidaValores.ToList();
            var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.str_codigo = str_codigo.Trim().PadLeft( 3, '0' );
            respuesta.diccionario.Add( "str_o_error", str_error );

            if (respuesta.str_codigo == "000")
            {
                //respuesta.obj_cuerpo = Funciones.ObtenerDatos( resultado );
                respuesta.obj_cuerpo = resultado;
            }

        }
        catch (Exception ex)
        {
            respuesta.str_codigo = "003";
            respuesta.diccionario.Add( "str_error", ex.InnerException != null ? ex.InnerException.Message : ex.Message );
            //await _logService.SaveExcepcionDataBaseSybase( request, MethodBase.GetCurrentMethod()!.Name, ex, str_clase );

            await _logService.SaveExceptionLogs( request, MethodBase.GetCurrentMethod()!.Name, "add", str_clase, ex );
            throw new ArgumentException( request.str_id_transaccion );

        }

        return respuesta;

    }

}

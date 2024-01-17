﻿using System.Text.Json;
using AccesoDatosGrpcMongo.Neg;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using static AccesoDatosGrpcMongo.Neg.DALMongo;

namespace Infrastructure.gRPC_Clients.Mongo;

public class LogsMongoDat : IMongoDat
{
    private readonly ApiSettings _settings;
    private readonly DALMongoClient _objClienteMongo;

    public LogsMongoDat(IOptionsMonitor<ApiSettings> settings, DALMongoClient objClienteMongo)
    {
        _settings = settings.CurrentValue;
        _objClienteMongo = objClienteMongo;
    }

    public async Task GuardarCabeceraMongo(dynamic request)
    {
        var ds = new DatosSolicitud();
        try
        {
            string solRequest = JsonSerializer.Serialize( request );
            ds.StrNameBD = _settings.nombre_base_mongo;
            ds.NombreColeccion = _settings.coll_peticiones;
            ds.Filter = string.Empty;
            ds.SolTran = solRequest;

            await _objClienteMongo.insertar_documentoAsync( ds );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }


    public async Task GuardarRespuestaMongo(dynamic request)
    {
        var ds = new DatosSolicitud();
        try
        {
            string solRequest = JsonSerializer.Serialize( request );
            ds.StrNameBD = _settings.nombre_base_mongo;
            ds.NombreColeccion = _settings.coll_respuesta;
            ds.Filter = string.Empty;
            ds.SolTran = solRequest;
            await _objClienteMongo.insertar_documentoAsync( ds );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public async Task GuardarExcepcionesMongo(dynamic result, object excepcion)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var body = new
            {
                idHeader = result.str_id_transaccion,
                result.str_id_servicio,
                result.str_nemonico_canal,
                result.dt_fecha_operacion,
                result.str_ip_dispositivo,
                result.str_login,
                result.str_id_oficina,
                rsc_res_info_adicional = result.str_res_info_adicional,
                error = excepcion.ToString()
            };

            var solCabecera = JsonSerializer.Serialize( body );
            datosSolicitud.StrNameBD = _settings.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _settings.coll_errores;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = solCabecera;

            await _objClienteMongo.insertar_documentoAsync( datosSolicitud );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public async Task GuardarExcepcionesDataBase(dynamic result, object excepcion)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var body = new
            {
                idHeader = result.str_id_transaccion,
                result.str_id_servicio,
                result.str_nemonico_canal,
                result.dt_fecha_operacion,
                result.str_ip_dispositivo,
                result.str_login,
                result.str_id_oficina,
                error = excepcion.ToString()
            };

            var solCabecera = JsonSerializer.Serialize( body );
            datosSolicitud.StrNameBD = _settings.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _settings.coll_errores_db;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = solCabecera;

            await _objClienteMongo.insertar_documentoAsync( datosSolicitud );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public async Task GuardaErroresHttp(object request, object excepcion, string strIdTransaccion)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var bjson = new
            {
                str_id_transaccion = strIdTransaccion,
                fecha = DateTime.Now,
                objeto = request,
                error = excepcion.ToString(),
            };

            var solCabecera = JsonSerializer.Serialize( bjson );
            datosSolicitud.StrNameBD = _settings.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _settings.coll_errores_http;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = solCabecera;

            await _objClienteMongo.insertar_documentoAsync( datosSolicitud );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public async Task GuardarAmenazasMongo(ValidacionInyeccion request)
    {
        try
        {
            var datosSolicitud = new DatosSolicitud();
            var serCabecera = JsonSerializer.Serialize( request );
            datosSolicitud.StrNameBD = _settings.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _settings.coll_amenazas;
            datosSolicitud.Filter = string.Empty;
            datosSolicitud.SolTran = serCabecera;

            await _objClienteMongo.insertar_documentoAsync( datosSolicitud );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public RespuestaTransaccion buscar_peticiones_diarias(string filtro)
    {
        var respuesta = new RespuestaTransaccion();
        var ds = new DatosSolicitud();
        try
        {
            ds.StrNameBD = _settings.nombre_base_mongo;
            ds.NombreColeccion = _settings.coll_peticiones_diarias;
            ds.Filter = filtro;
            ds.SolTran = string.Empty;

            var res = _objClienteMongo.buscar_documentos( ds );

            respuesta.codigo = "000";
            respuesta.cuerpo = res.Mensaje;
        }
        catch (Exception ex)

        {
            respuesta.codigo = "001";
            respuesta.diccionario.Add( "str_error", ex.ToString() );
        }

        return respuesta;
    }

    public void actualizar_peticion_diaria(string filtro, string peticion)
    {
        var ds = new DatosSolicitud();

        try
        {
            ds.StrNameBD = _settings.nombre_base_mongo;
            ds.NombreColeccion = _settings.coll_peticiones_diarias;
            ds.Filter = filtro;
            ds.SolTran = peticion;
            _objClienteMongo.actualizar_documento_avanzado( ds );
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public void guardar_promedio_peticion_diaria(string strOperacion, string strFecha)
    {
        var datosSolicitud = new DatosSolicitud();
        try
        {
            var strFiltro = "{'str_operacion':'" + strOperacion + "'}";
            datosSolicitud.StrNameBD = _settings.nombre_base_mongo;
            datosSolicitud.NombreColeccion = _settings.coll_promedio_peticiones_diarias;
            datosSolicitud.Filter = strFiltro;
            datosSolicitud.SolTran = string.Empty;
            var res = _objClienteMongo.buscar_documentos( datosSolicitud );
            var resultMongo = res.Mensaje;
            var promedio = calcular_promedio( strOperacion );
            if (resultMongo != null && resultMongo != "[]")
            {
                var strDatosUpdate = "{$set:{'dbl_promedio_peticion':" + promedio +
                                       ",'str_fecha_actualizacion':'" + strFecha + "'}}";

                datosSolicitud.Filter = strFiltro;
                datosSolicitud.SolTran = strDatosUpdate;

                _objClienteMongo.actualizar_documento( datosSolicitud );
            }
            else
            {
                object solicitud = new
                    { dbl_promedio_peticion = promedio,
                        str_operacion = strOperacion, str_fecha_actualizacion = strFecha };
                datosSolicitud.Filter = string.Empty;
                datosSolicitud.SolTran = JsonSerializer.Serialize( solicitud );
                _objClienteMongo.insertar_documento( datosSolicitud );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine( "Error en mongo: " + ex.Message );
        }
    }

    public int calcular_promedio(string strOperacion)
    {
        var strFiltro = "[{ $match: { str_operacion:'" + strOperacion + "'} }," +
                         "{$group:{_id: '$str_operacion',dbl_promedio_peticion: { $avg: '$int_num_peticion'}}}]";
        var intRespuesta = 0;
        var ds = new DatosSolicitud();
        try
        {
            ds.StrNameBD = _settings.nombre_base_mongo;
            ds.NombreColeccion = _settings.coll_peticiones_diarias;
            ds.Filter = strFiltro;
            ds.SolTran = string.Empty;

            var res = _objClienteMongo.buscar_documentos_avanzado( ds );

            var resDatosMongo = res.Mensaje;
            if (resDatosMongo != null && resDatosMongo != "[]")
            {
                resDatosMongo = resDatosMongo.Replace( "[", "" ).Replace( "]", "" );
                var peticionDiaria = JsonSerializer.Deserialize<PromedioPeticionDiaria>( resDatosMongo );
                intRespuesta = Convert.ToInt32( peticionDiaria!.dbl_promedio_peticion );
            }
        }
        catch (Exception)

        {
            intRespuesta = 0;
        }

        return intRespuesta;
    }

    public int obtener_promedio(string strOperacion)
    {
        var strFiltro = "{'str_operacion':'" + strOperacion + "'}";
        var intRespuesta = 0;
        var ds = new DatosSolicitud();
        try
        {
            ds.StrNameBD = _settings.nombre_base_mongo;
            ds.NombreColeccion = _settings.coll_promedio_peticiones_diarias;
            ds.Filter = strFiltro;
            ds.SolTran = string.Empty;

            var res = _objClienteMongo.buscar_documentos( ds );

            var resDatosMongo = res.Mensaje;
            if (resDatosMongo != null && resDatosMongo != "[]")
            {
                resDatosMongo = resDatosMongo.Replace( "ObjectId(", " " ).Replace( ")", " " );

                resDatosMongo = resDatosMongo.Replace( "[", "" ).Replace( "]", "" );
                var peticionDiaria = JsonSerializer.Deserialize<PromedioPeticionDiaria>( resDatosMongo );
                intRespuesta = Convert.ToInt32( peticionDiaria!.dbl_promedio_peticion );
            }
        }
        catch (Exception)

        {
            intRespuesta = 0;
        }

        return intRespuesta;
    }
}
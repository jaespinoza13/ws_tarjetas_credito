using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class HttpService : IHttpService
{
    private readonly ApiSettings _config;
    public HttpService(IOptionsMonitor<ApiSettings> options)
    {
        this._config = options.CurrentValue;
    }
    public object solicitar_servicio_async(SolicitarServicio solicitarServicio)
    {
        var respuesta = new object { };

        try
        {
            var client = new HttpClient();
            var request = createRequest( solicitarServicio );

            addHeaders( solicitarServicio, client );

            client.SendAsync( request );
            //Task.Delay(100);
            //client.Dispose();
        }
        catch
        {
            // No se requiere ningun proceso aqui
        }
        return respuesta;
    }

    public async Task<string> solicitar_servicio(SolicitarServicio solicitarServicio, Header header)
    {
        string str_cod_http = "000";
        try
        {
            HttpClient client = new HttpClient();
            var request = createRequest( solicitarServicio );

            addHeaders( solicitarServicio, client );

            var response = await client.SendAsync( request );

            client.Dispose();

            str_cod_http = Convert.ToInt32( response.StatusCode ).ToString();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new ArgumentException( response.ToString() );
            }

        }
        catch (Exception ex)
        {
            str_cod_http = str_cod_http == "401" ? "165" : "001";
            string str_mensaje = str_cod_http == "165" ? "Tu sesión ha caducado, por favor ingresa nuevamente." : "Error en solicitud del servicio.";
            var respuesta = new
            {
                codigo = str_cod_http,
                str_res_codigo = str_cod_http,
                str_res_info_adicional = str_mensaje,
                diccionario = new { ERROR = str_mensaje },
                cuerpo = new { }
            };
            guardarErrorHttp( ex, header );
            return JsonSerializer.Serialize( respuesta );
        }
    }
    private static void addHeaders(SolicitarServicio solicitarServicio, HttpClient httpClient)
    {
        if (solicitarServicio.valueAuth != null)
        {
            httpClient.DefaultRequestHeaders.Add( solicitarServicio.tipoAuth, solicitarServicio.valueAuth );
        }

        foreach (var header in solicitarServicio.dcyHeadersAdicionales)
        {
            httpClient.DefaultRequestHeaders.Add( header.Key, header.Value.ToString() );
        }
    }

    /// <summary>
    /// Crear solicitud http
    /// </summary>
    /// <param name="solicitarServicio"></param>
    /// <returns></returns>
    private static HttpRequestMessage createRequest(SolicitarServicio solicitarServicio)
    {
        string str_solicitud = JsonSerializer.Serialize( solicitarServicio.objSolicitud );

        var request = new HttpRequestMessage();
        if (solicitarServicio.contentType == "application/x-www-form-urlencoded")
        {
            var parametros = JsonSerializer.Deserialize<Dictionary<string, string>>( str_solicitud )!;
            request.Content = new FormUrlEncodedContent( parametros );
        }
        else
            request.Content = new StringContent( str_solicitud, Encoding.UTF8, solicitarServicio.contentType );

        request.Method = new HttpMethod( solicitarServicio.tipoMetodo );
        request.RequestUri = new Uri( solicitarServicio.urlServicio, System.UriKind.RelativeOrAbsolute );
        request.Content.Headers.Add( "No-Paging", "true" );
        return request;
    }

    private void guardarErrorHttp(Exception ex, Header header)
    {
        try
        {
            header.dt_fecha_operacion = DateTime.Now;

            var resComun = new ResComun();
            resComun.LlenarResHeader( header );
            resComun.str_res_codigo = "001";
            resComun.str_res_estado_transaccion = "ERR";

            Dictionary<string, object> dic_obj = JsonSerializer.Deserialize<Dictionary<string, object>>( JsonSerializer.Serialize( resComun ) )!;

            dic_obj.Add( "str_tipo_error", "HTTP" );
            dic_obj.Add( "str_error", ex.ToString() );

            var solicitud = new
            {
                dtt_fecha = DateTime.Now.ToString( "yyyy-MM-ddTHH:mm:ss" ),
                str_id_transaccion = header.str_id_transaccion,
                str_tipo = "RESPUESTA",
                str_webservice = _config.nombre_base_mongo,
                str_clase = "HttpService",
                str_metodo = "solicitar_servicio",
                str_operacion = header.str_id_servicio!.Replace( "REQ_", "" ),
                obj_objeto = dic_obj,
                bln_save_log_txt = true,
                str_nombre_coleccion = "",
                str_nombre_bd = ""
            };

            var solServicio = new SolicitarServicio();

            solServicio.objSolicitud = solicitud;
            solServicio.urlServicio = _config.url_logs + "ADD_LOG";
            solServicio.tipoAuth = "Authorization-Mego";
            solServicio.valueAuth = _config.auth_logs;

            solicitar_servicio_async( solServicio );
        }
        catch (Exception exception)
        {
            Console.WriteLine( exception.ToString() );
        }
    }
}
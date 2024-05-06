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

    public async Task<string> solicitar_servicio(SolicitarServicio solicitarServicio)
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
            throw new ArgumentException( ex.ToString() );
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
}
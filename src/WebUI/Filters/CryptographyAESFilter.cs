using Application.Common.Converting;
using Application.Common.Cryptography;
using Application.Common.Interfaces.Dat;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace WebUI.Filters;

public class CryptographyAesFilter : IActionFilter
{
    private readonly IKeysDat _keysDat;
    private readonly ApiSettings _settings;

    public CryptographyAesFilter(IKeysDat keysDat, IOptionsMonitor<ApiSettings> options)
    {
        _keysDat = keysDat;
        _settings = options.CurrentValue;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //Desencripta datos de entrada
        var modelRequest = context.ActionArguments.First();
        var reqGetKeys = JsonSerializer.Deserialize<ReqGetKeys>( JsonSerializer.Serialize( modelRequest.Value ) )!;

        if (!_settings.lst_canales_encriptar.Contains( reqGetKeys.str_nemonico_canal )) return;
        var resTran = _keysDat.GetKeys( reqGetKeys );
        var key = Conversions.ConvertConjuntoDatosToClass<ResGetKeys>( (ConjuntoDatos)resTran.cuerpo, 0 );

        try
        {
            if (modelRequest.Value!.GetType().GetMethod( "DecryptAES" ) != null)
                modelRequest.Value!.GetType().GetMethod( "DecryptAES" )!.Invoke( modelRequest.Value,
                    new object[] { key } );


            modelRequest.Value.GetType().GetMethod( "DecryptAESHeader" )!.Invoke( modelRequest.Value,
                new object[] { key } );
        }
        catch (Exception)
        {
            GenerarExcepcion( context );
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result == null) return;
        var response = ((ObjectResult)context.Result!).Value;


        if (response == null) return;
        var reqGetKeys = JsonSerializer.Deserialize<ReqGetKeys>( JsonSerializer.Serialize( response ) )!;
        if (!_settings.lst_canales_encriptar.Contains( reqGetKeys.str_nemonico_canal )) return;
        var resTran = _keysDat.GetKeys( reqGetKeys );
        var key = Conversions.ConvertConjuntoDatosToClass<ResGetKeys>( (ConjuntoDatos)resTran.cuerpo, 0 );

        try
        {
            if (response.GetType().GetMethod( "EncryptAES" ) != null)

                response.GetType().GetMethod( "EncryptAES" )!.Invoke( response, new object[] { key } );


            response.GetType().GetMethod( "EncryptAESHeader" )!.Invoke( response,
                new object[] { key } );
        }
        catch (Exception)
        {
            throw new ArgumentException( "Error: Credenciales inválidas 002" );
        }
    }

    private static void GenerarExcepcion(ActionExecutingContext context)
    {
        var resException = new ResException
        {
            str_res_codigo = Convert.ToInt32( HttpStatusCode.Unauthorized ).ToString(),
            str_res_id_servidor = "Error: Credenciales inválidas",
            str_res_estado_transaccion = "ERR",
            dt_res_fecha_msj_crea = DateTime.Now,
            str_res_info_adicional = "Tu sesión ha caducado, por favor ingresa nuevamente."
        };

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Result = new ObjectResult( resException );
    }
}
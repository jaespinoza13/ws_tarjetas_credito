using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Domain.Parameters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.MemoryCache;

internal class ParametersInMemory : IParametersInMemory
{
    public readonly ApiSettings _settings;
    public readonly IParametrosDat _parametros;
    public readonly IMemoryCache _memoryCache;
    public DateTime dt_fecha_codigos;

    public ParametersInMemory(IOptionsMonitor<ApiSettings> options, IParametrosDat parametros, IMemoryCache memoryCache)
    {
        this._settings = options.CurrentValue;
        this._parametros = parametros;
        this._memoryCache = memoryCache;
    }

    public void LoadConfiguration()
    {
        try
        {

            Dictionary<string, object> CodigosError = new();

            RespuestaTransaccion resTran = _parametros.getParametros( "PARAMETROS_MEMORIA" ).Result;

            if (resTran.codigo == "000")
            {
                var lst_parametros_back = Mapper.ConvertConjuntoDatosToListClass<Parametro>( resTran.cuerpo );

                dt_fecha_codigos = DateTime.Now.Date;
                _memoryCache.Set( "Parametros_back", lst_parametros_back );
            }
            else
                throw new ArgumentException( "Sin parametros" );


            dt_fecha_codigos = DateTime.Now.Date;
        }
        catch (Exception ex)
        {
            throw new ArgumentException( ex.Message );
        }
    }

    public void ValidaParametros()
    {
        if (DateTime.Compare( DateTime.Now, dt_fecha_codigos.AddDays( 1 ) ) > 0)
        {
            LoadConfiguration();
        }
    }

    public List<Parametro> FindParametros(string str_tipo_parametro)
    {
        return _memoryCache.Get<List<Parametro>>( str_tipo_parametro );
    }
    public Parametro FindParametroNemonico(string str_nemonico)
    {
        var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );
        Parametro parametro = new Parametro();
        return lst_parametros.Find( x => x.str_nemonico == str_nemonico )!; 
    }
    public Parametro FindParametroValorFin(string str_valor_fin)
    {
        var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );
        return lst_parametros.Find( x => x.str_valor_fin == str_valor_fin )!;
    }
    public Parametro FindParametroId(int int_id_param)
    {
        var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );
        return lst_parametros.Find( x => x.int_id_parametro == int_id_param )!; 
    }
    public string getMensajeProceso(string str_codigo, string str_mensaje = "")
    {
        var dcc_codigos = _memoryCache.Get<Dictionary<string, object>>( "CodigosError" );
        if (dcc_codigos.ContainsKey( str_codigo ))
        {
            return dcc_codigos[str_codigo].ToString()!;
        }
        return str_mensaje;


    }
}

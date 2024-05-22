using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.ComentariosGestion;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosGestion;
using Domain.Parameters;
//using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parametros;

public class GetParametrosHandler : IRequestHandler<ReqGetParametros, ResGetParametros>
{
    private readonly ApiSettings _settings;
    private readonly ILogs _logsService;
    private readonly string str_clase;
    private readonly string str_operacion;
    public readonly IMemoryCache _memoryCache;
    public GetParametrosHandler(IOptionsMonitor<ApiSettings> options, ILogs logsService, IMemoryCache memoryCache)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;
        this.str_clase = GetType().FullName!;
        str_operacion = "GET_PARAMETROS_MEMORIA";
        _memoryCache = memoryCache;
    }
    public async Task<ResGetParametros> Handle(ReqGetParametros request, CancellationToken cancellationToken)
    {
        ResGetParametros respuesta = new();
        respuesta.LlenarResHeader( request );
        try
        {
            var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" )!;
            respuesta.lst_parametros = lst_parametros;
            var str_codigo = '0';
            var str_error = "";
            respuesta.str_res_codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
           
        }
        catch (Exception e)
        {

            Console.WriteLine( e.ToString() );
            await _logsService.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }
}

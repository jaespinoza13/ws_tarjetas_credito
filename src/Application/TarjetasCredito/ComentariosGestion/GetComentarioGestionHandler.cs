using Application.Common.Interfaces;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace Application.TarjetasCredito.ComentariosGestion;

public class GetComentarioGestionHandler : IRequestHandler<ReqGetComentGestion, ResGetComentGestion>
{
    private readonly IComentariosGestionDat _comentarioGestionDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;
    public readonly IMemoryCache _memoryCache;

    public GetComentarioGestionHandler(IComentariosGestionDat comentarioGestionDat, ILogs logs, IMemoryCache memoryCache)
    {
        _comentarioGestionDat = comentarioGestionDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_COMENTARIOS_GESTION";
        _memoryCache = memoryCache;
    }

    public async Task<ResGetComentGestion> Handle(ReqGetComentGestion request, CancellationToken cancellationToken)
    {
        ResGetComentGestion respuesta = new();
        try
        {
            respuesta = await _comentarioGestionDat.get_coment_gest_sol( request );
        }
        catch (Exception e)
        {

            Console.WriteLine( e.ToString() );
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        return respuesta;
    }

}


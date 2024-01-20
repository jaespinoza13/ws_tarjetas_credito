using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.DatosCliente;
using MediatR;
using System.Reflection;


namespace Application.TarjetasCredito.DatosClienteTc;

    public class GetDatosClienteHandler : IRequestHandler<ReqGetDatosCliente, ResGetDatosCliente>
    {
    private readonly IDatosClienteDat _datosClienteDat;

    private readonly ILogs _logs;

    private readonly string str_clase;

    private readonly string str_operacion;

    public GetDatosClienteHandler(IDatosClienteDat datosCleinteDat, ILogs logs) {

            _datosClienteDat = datosCleinteDat;
            _logs = logs;
            str_clase = GetType().Name;
            str_operacion = "GET_DATOS_CLIENTE";

    }
        public async Task<ResGetDatosCliente> Handle(ReqGetDatosCliente request, CancellationToken cancellationToken)
        {
        ResGetDatosCliente respuesta = new();
        respuesta.LlenarResHeader( request );
        try
        {
        await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
        RespuestaTransaccion res_tran = new();
        res_tran = await _datosClienteDat.get_datos_cliente( request );
        respuesta.datos_cliente = Conversions.ConvertConjuntoDatosToListClass<DatosCliente>( (ConjuntoDatos)res_tran.cuerpo )!;
        await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
        }
        catch (Exception e)
        {
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );

        }

        return respuesta;

    }


    }


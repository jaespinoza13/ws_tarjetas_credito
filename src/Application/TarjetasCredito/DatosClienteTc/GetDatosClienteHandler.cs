using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.DatosCliente;
using iText.Layout.Element;
using MediatR;
using System.Data;
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
            List<DireccionDomicilio> data_list_dom = new List<DireccionDomicilio>();
            List<DireccionTrabajo> data_list_trab = new List<DireccionTrabajo>();
            respuesta.datos_cliente = Conversions.ConvertConjuntoDatosToListClassPos0<DatosCliente>( (ConjuntoDatos)res_tran.cuerpo )!;
            respuesta.dir_domicilio = Conversions.ConvertConjuntoDatosToListClassPos1<DireccionDomicilio>( (ConjuntoDatos)res_tran.cuerpo )!;
            respuesta.dir_trabajo = Conversions.ConvertConjuntoDatosToListClassPos2<DireccionTrabajo>( (ConjuntoDatos)res_tran.cuerpo )!;
            foreach (DireccionTrabajo dir_trabajo in respuesta.dir_trabajo)
            {
                DireccionTrabajo obj_dir_trabajo = new DireccionTrabajo
                {
                    str_dir_ciudad = dir_trabajo.str_dir_ciudad,
                    str_dir_sector = dir_trabajo.str_dir_sector,
                    str_dir_barrio = dir_trabajo.str_dir_barrio,
                    str_dir_descripcion_emp = dir_trabajo.str_dir_descripcion_emp,
                    str_dir_num_casa = dir_trabajo.str_dir_num_casa


                };
                data_list_trab.Add( obj_dir_trabajo );
            }
            respuesta.dir_trabajo = data_list_trab;
            foreach (DireccionDomicilio dir_domicilio in respuesta.dir_domicilio)
            {
                DireccionDomicilio obj_dir_domicilio = new DireccionDomicilio
                {
                    str_dir_ciudad = dir_domicilio.str_dir_ciudad,
                    str_dir_sector = dir_domicilio.str_dir_sector,
                    str_dir_barrio = dir_domicilio.str_dir_barrio,
                    str_dir_descripcion_dom = dir_domicilio.str_dir_descripcion_dom,
                    str_dir_num_casa = dir_domicilio.str_dir_num_casa


                };
                data_list_dom.Add( obj_dir_domicilio );
            }
            respuesta.dir_domicilio = data_list_dom;
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


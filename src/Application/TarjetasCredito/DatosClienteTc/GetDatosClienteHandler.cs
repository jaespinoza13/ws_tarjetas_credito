using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.InterfazDat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application.TarjetasCredito.DatosClienteTc;

    public class GetDatosClienteHandler : IRequestHandler<ReqGetDatosCliente, ResGetDatosCliente>
    {
    private readonly IDatosClienteDat _datosClienteDat;

    private readonly ILogs _logs;

    private readonly string str_clase;

        public GetDatosClienteHandler(IDatosClienteDat datosCleinteDat, ILogs logs) {

            _datosClienteDat = datosCleinteDat;
            _logs = logs;
            str_clase = GetType().Name;

        }
        public async Task<ResGetDatosCliente> Handle(ReqGetDatosCliente request, CancellationToken cancellationToken)
        {
            const string str_operacion = "GET_DATOS_CLIENTE";
            var respuesta = new ResGetDatosCliente();
            //respuesta.LlenarResHeader( request );
            try
            {
                await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
                //var result_transacction = await _datosClienteDat.get_datos_cliente( request );
                ResGetDatosCliente res_tran = await _datosClienteDat.get_datos_cliente( request );
            respuesta.cuerpo = res_tran.cuerpo;
            respuesta.LlenarResHeader( request );

            await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            //respuesta.cuerpo= res_tran;
             //return result_transacction;
                }
            catch (Exception e)
            {
                await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
                throw new ArgumentException( respuesta.str_id_transaccion );

            }

            return respuesta;

        }


    }


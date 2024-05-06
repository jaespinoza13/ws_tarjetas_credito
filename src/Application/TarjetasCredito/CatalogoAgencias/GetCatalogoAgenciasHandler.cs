using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.Agencias;
using MediatR;
using System.Reflection;

namespace Application.TarjetasCredito.CatalogoAgencias
{
    public class GetCatalogoAgenciasHandler : IRequestHandler<ReqGetCatalogoAgencias, ResGetCatalogoAgencias>
    {
        private readonly ICatalogoAgenciasDat _catalogoAgenciasDat;
        private readonly ILogs _logs;
        private readonly string str_clase;
        private readonly string str_operacion;

        public GetCatalogoAgenciasHandler(ICatalogoAgenciasDat catalogoAgenciasDat, ILogs logs)
        {
            _catalogoAgenciasDat = catalogoAgenciasDat;
            _logs = logs;
            str_clase = GetType().Name;
            str_operacion = "GET_CATALOGO_AGENCIAS";
        }
        public async Task<ResGetCatalogoAgencias> Handle(ReqGetCatalogoAgencias request, CancellationToken cancellationToken)
        {
            ResGetCatalogoAgencias respuesta = new();
            respuesta.LlenarResHeader( request );
            try
            {
                await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
                RespuestaTransaccion res_tran = new();
                res_tran = await _catalogoAgenciasDat.get_catalogo_agencias( request );
                List<Agencias> data_list_agencias = new List<Agencias>();
                respuesta.lst_agencias = Conversions.ConvertConjuntoDatosTableToListClass<Agencias>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;

                foreach (Agencias agencias in respuesta.lst_agencias)
                {
                    Agencias obj_agencias = new Agencias
                    {
                        str_cod_marca = agencias.str_cod_marca,
                        str_cod_numero = agencias.str_cod_numero,
                        str_cod_denominacion = agencias.str_cod_denominacion,
                        str_cod_domicilio = agencias.str_cod_domicilio

                    };
                    data_list_agencias.Add( obj_agencias );
                }
                respuesta.lst_agencias = data_list_agencias;
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
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
}

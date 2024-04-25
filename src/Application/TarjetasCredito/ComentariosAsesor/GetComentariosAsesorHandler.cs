using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utilidades;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosAsesorCredito;
using Domain.Entities.ComentariosGestion;
using Domain.Entities.SituacionFinanciera;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Application.TarjetasCredito.ObtenerSolicitudes.ResGetSolicitudes;
using static Domain.Entities.ComentariosAsesorCredito.ComentarioAsesor;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class GetComentariosAsesorHandler : IRequestHandler<ReqGetComentariosAsesor, ResGetComentariosAsesor>
{
    private readonly IParametrosInformeDat _iParametrosInformeDat;
    private readonly IComentarioAsesorDat _iComentariosAsesorDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly string str_operacion;

    public GetComentariosAsesorHandler(IParametrosInformeDat IParametrosInformeDat, ILogs logs, IComentarioAsesorDat iComentariosAsesorDat)
    {
        _iParametrosInformeDat = IParametrosInformeDat;
        _iComentariosAsesorDat = iComentariosAsesorDat;
        _logs = logs;
        str_clase = GetType().Name;
        str_operacion = "GET_COMENTARIOS_ASESOR_CREDITO";
        _iComentariosAsesorDat = iComentariosAsesorDat;
    }
    public async Task<ResGetComentariosAsesor> Handle(ReqGetComentariosAsesor request, CancellationToken cancellationToken)
    {
        ResGetComentariosAsesor respuesta = new();
        List<ComentarioAsesorRes> obj_cmnt_ase_res = new List<ComentarioAsesorRes>();
        respuesta.LlenarResHeader( request );
        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase ); //Logs ws_logs
            RespuestaTransaccion res_tran = new();

            //Se obtiene los parametros del informe (POSTGRES)
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _iComentariosAsesorDat.GetComentarios( request );
            obj_cmnt_ase_res = Conversions.ConvertConjuntoDatosTableToListClass<ComentarioAsesorRes>( (ConjuntoDatos)res_tran.cuerpo, 0 );
            bool bool_ver_res = obj_cmnt_ase_res.All( obj_cmnt_ase_res => obj_cmnt_ase_res.json_comentarios == " " );
            if (obj_cmnt_ase_res.Count > 0 & res_tran.codigo == "000" & bool_ver_res == false)
            {
                string jsonString = obj_cmnt_ase_res[0].json_comentarios;
                List<ComentarioAsesor> comentarios = JsonConvert.DeserializeObject<List<ComentarioAsesor>>( jsonString )!;
                respuesta.lst_comn_ase_cre = comentarios;
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            }
            else if (res_tran.codigo == "001")
            {
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            }
            else
            {
                //Caso contrario se obtiene de (SYBASE)
                respuesta.lst_comn_ase_cre = new List<ComentarioAsesor>();
                res_tran = await _iParametrosInformeDat.get_parametros_informe( request );
                respuesta.lst_comn_ase_cre = Conversions.ConvertConjuntoDatosTableToListClass<ComentarioAsesor>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
            }

        }
        catch (Exception e)
        {

            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }
        return respuesta;
    }
}

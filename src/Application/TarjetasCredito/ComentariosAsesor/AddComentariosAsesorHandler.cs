using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.InformacionAdicional;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosAsesorCredito;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class AddComentariosAsesorHandler : IRequestHandler<ReqAddComentariosAsesor, ResAddComentariosAsesor>
{
    private readonly IComentarioAsesorDat _addComentarioAsesorDat;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly ApiSettings _settings;
    public AddComentariosAsesorHandler(IOptionsMonitor<ApiSettings> options, IComentarioAsesorDat addComentarioAsesorDat, ILogs logs)
    {
        _addComentarioAsesorDat = addComentarioAsesorDat;
        _logs = logs;
        str_clase = GetType().Name;
        _settings = options.CurrentValue;
    }
    public async Task<ResAddComentariosAsesor> Handle(ReqAddComentariosAsesor request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_COMENTARIO_ASESOR";
        ResAddComentariosAsesor respuesta = new ResAddComentariosAsesor();
        RespuestaTransaccion res_tran = new();
        List<ComentarioAsesor> data_list_cmnt_ase = new List<ComentarioAsesor>();
        respuesta.LlenarResHeader( request );

        foreach (ComentarioAsesor comentario_asesor in request.lst_cmnt_ase_cre)
        {
            ComentarioAsesor obj_cmnt_ase = new ComentarioAsesor{
                int_id_parametro = comentario_asesor.int_id_parametro,
                str_tipo = comentario_asesor.str_tipo,
                str_descripcion = comentario_asesor.str_descripcion,
                str_detalle = comentario_asesor.str_detalle

            };
            data_list_cmnt_ase.Add(obj_cmnt_ase);
        }
        request.str_cmnt_ase_json = JsonConvert.SerializeObject( data_list_cmnt_ase );

        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            res_tran = await _addComentarioAsesorDat.AddComentario( request );
            respuesta.str_res_codigo = res_tran.codigo;
            respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];
        }
        catch (Exception e)
        {
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }
        return respuesta;
    }
}

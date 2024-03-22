using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InformacionEconomica;
using Application.TarjetasCredito.InterfazDat;
using Application.TarjetasCredito.SituacionFinanciera;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccesoDatosGrpcAse.Neg.DAL;
using AccesoDatosGrpcAse.Neg;
using Infrastructure.Common.Funciones;
using Infrastructure.gRPC_Clients.Postgres;

namespace Infrastructure.gRPC_Clients.Sybase;

public class SituacionFinancieraDat : ISitFinDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient _objClienteDal;
    private readonly ILogs _logsService;
    private readonly string str_clase;

    public SituacionFinancieraDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;
        this.str_clase = GetType().FullName!;
        _objClienteDal = objClienteDal;

    }
    public async Task<RespuestaTransaccion> get_situacion_financiera(ReqGetSitFin request)
    {
        RespuestaTransaccion respuesta = new RespuestaTransaccion();
        try
        {
            DatosSolicitud ds = new();
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_num_ente", TipoDato = TipoDato.Integer, ObjValue = request.str_ente.ToString() } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
            ds.NombreSP = NameSps.getSitFinSoc;
            ds.NombreBD = _settings.DB_meg_atms;


            var resultado = await _objClienteDal.ExecuteDataSetAsync( ds );

            var lst_valores = new List<ParametroSalidaValores>();


            foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );

            var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();
            respuesta.codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_o_error", str_error.ToString() );

        }
        catch (Exception exception)
        {

            respuesta.codigo = "001";
            respuesta.diccionario.Add( "str_o_error", exception.ToString() );
            //_logsService.SaveExcepcionDataBaseSybase( req_get_parametros, MethodBase.GetCurrentMethod()!.Name, exception, str_clase );
            throw new ArgumentException( request.str_id_transaccion )!;
        }
        return respuesta;


    }
}


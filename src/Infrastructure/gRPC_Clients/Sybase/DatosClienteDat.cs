using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using static AccesoDatosGrpcAse.Neg.DAL;
using AccesoDatosGrpcAse.Neg;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Application.TarjetasCredito.DatosClienteTc;

namespace Infrastructure.gRPC_Clients.Sybase;

public class DatosClienteDat : IDatosClienteDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient _objClienteDal;
    private readonly ILogs _logsService;
    private readonly string str_clase;
    public DatosClienteDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;

        this.str_clase = GetType().FullName!;

        _objClienteDal = objClienteDal;


    }
    public async Task<ResGetDatosCliente> get_datos_cliente(ReqGetDatosCliente request)
    {
        var respuesta = new ResGetDatosCliente();

        try
        {
            DatosSolicitud ds = new();
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@prm_dcto", TipoDato = TipoDato.VarChar, ObjValue = request.str_num_documento } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@prm_oficial", TipoDato = TipoDato.VarChar, ObjValue = request.str_login_usuario } );

            ds.NombreSP = "get_inf_cliente_tc";
            ds.NombreBD = "meg_buro";

            var resultado = _objClienteDal.ExecuteDataSet( ds );
            var lst_valores = new List<ParametroSalidaValores>();

            foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
            //var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            //var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            //respuesta.codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            //respuesta.diccionario.Add( "str_error", str_error.ToString() );
        }
        catch (Exception exception)
        {
            //respuesta.codigo = "001";
            //respuesta.diccionario.Add( "str_error", exception.ToString() );
            //_logsService.SaveExcepcionDataBaseSybase( req_get_parametros, MethodBase.GetCurrentMethod()!.Name, exception, str_clase );
            //throw new ArgumentException( req_get_parametros.str_id_transaccion )!;
        }
        return respuesta;
    }
}

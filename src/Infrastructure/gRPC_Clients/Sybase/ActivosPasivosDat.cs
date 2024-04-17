using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccesoDatosGrpcAse.Neg.DAL;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using Application.Common.ISO20022.Models;
using Infrastructure.gRPC_Clients.Postgres;
using Infrastructure.Common.Funciones;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.MemoryCache;

namespace Infrastructure.gRPC_Clients.Sybase;

public class ActivosPasivosDat : IActivosPasivosDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient _objClienteDal;
    private readonly ILogs _logsService;
    private readonly string str_clase;

    public ActivosPasivosDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objDalClient) {
        _settings = options.CurrentValue;
        _logsService = logsService;
        this.str_clase = GetType().FullName!;
        _objClienteDal = objDalClient;
    }
    public async Task<RespuestaTransaccion> get_activos_pasivos_socio(string str_num_ente)
    {
        RespuestaTransaccion respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();


            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_num_ente", TipoDato = TipoDato.Integer, ObjValue = str_num_ente.ToString() } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );

            ds.NombreSP = NameSps.getActPasSoc;
            ds.NombreBD = _settings.DB_meg_atms;

            var resultado = await _objClienteDal.ExecuteDataSetAsync( ds );

             var lst_valores = new List<ParametroSalidaValores>();

            foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
            var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();
            respuesta.codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_o_error", str_error.ToString() );


            //if (respuesta.codigo == "000")
            //{
            //    respuesta.codigo = Funciones.ObtenerDatos( resultado ).lst_tablas.Count > 1 ? respuesta.codigo = "002" : respuesta.codigo = "000";
            //    respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            //}
        }
        catch (Exception ex)
        {
            throw new ArgumentException( ex.ToString() );
        }
        return respuesta;
    }
}

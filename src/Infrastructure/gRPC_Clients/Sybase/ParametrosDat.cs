using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Infrastructure.Common.Funciones;
using Infrastructure.gRPC_Clients.Postgres;
using Infrastructure.MemoryCache;
using Microsoft.Extensions.Options;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.gRPC_Clients.Sybase;
internal class ParametrosDat : IParametros, IParametrosDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient objClienteDal;
    private readonly int int_padleft;

    public ParametrosDat(IOptionsMonitor<ApiSettings> options, DALClient dalClient)
    {
        _settings = options.CurrentValue;
        int_padleft = 3;
        objClienteDal = dalClient;
    }

    public async Task<RespuestaTransaccion> getParametros(string str_nombre)
    {
        RespuestaTransaccion respuesta = new RespuestaTransaccion();
        try
        {
            var ds = new DatosSolicitud();

            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error", TipoDato = TipoDato.Integer } );

            ds.NombreSP = NameSps.getParametros;
            ds.NombreBD = _settings.DB_meg_atms;

            var resultado = await objClienteDal.ExecuteDataSetAsync( ds );

            var lst_valores = new List<ParametroSalidaValores>();

            foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );

            respuesta.codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error" )!.ObjValue.ToString().PadLeft( int_padleft, '0' );
            respuesta.diccionario.Add( "str_error", lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim() );

            if (respuesta.codigo == "000")
            {
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
                
            }

        }
        catch (Exception ex)
        {
            throw new ArgumentException( ex.ToString() );
        }
        return respuesta;
    }

}


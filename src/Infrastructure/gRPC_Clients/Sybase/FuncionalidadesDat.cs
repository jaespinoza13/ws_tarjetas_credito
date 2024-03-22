using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Application.TarjetasCredito.DatosClienteTc;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.Common.Funciones;
using Infrastructure.gRPC_Clients.Postgres;
using Infrastructure.MemoryCache;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.gRPC_Clients.Sybase
{
    public class FuncionalidadesDat : IFuncionalidades, IFuncionalidadesDat
    {
        private readonly ApiSettings _settings;
        private readonly DALClient _objClienteDal;
        private readonly ILogs _logsService;
        private readonly string str_clase; 
        private readonly int int_padleft;

        public FuncionalidadesDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
        {
            _settings = options.CurrentValue;
            _logsService = logsService;
            int_padleft = 3;
            this.str_clase = GetType().FullName!;
            _objClienteDal = objClienteDal;
        }

        public async Task<RespuestaTransaccion> getFuncionalidades(int int_id_sistema, int int_tipo_funcionalidad = -1, int int_id_perfil = -1)
        {
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                var ds = new DatosSolicitud();

                string tipo_funcionalidad = int_tipo_funcionalidad > 0 ? int_tipo_funcionalidad.ToString() : "-1";
                string perfil = int_id_perfil > 0 ? int_id_perfil.ToString() : "-1";

                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_sistema", TipoDato = TipoDato.Integer, ObjValue = int_id_sistema.ToString() } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );

                ds.NombreSP = NameSps.getFuncPermisos;
                ds.NombreBD = _settings.DB_meg_atms;

                var resultado = await _objClienteDal.ExecuteDataSetAsync( ds );

                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );

                respuesta.codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue.ToString().PadLeft( int_padleft, '0' );
                respuesta.diccionario.Add( "str_error", lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim() );

                if (respuesta.codigo == "000")
                {
                    respuesta.codigo = Funciones.ObtenerDatos( resultado ).lst_tablas.Count > 1 ? respuesta.codigo = "002" : respuesta.codigo = "000";
                    respuesta.cuerpo = Funciones.ObtenerDatos( resultado ); 
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return respuesta;
        }

        public async Task<RespuestaTransaccion> validarPermisoPerfil(Header request)
        {
            RespuestaTransaccion respuesta = new RespuestaTransaccion();

            try
            {
                DatosSolicitud ds = new();
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_num_documento", TipoDato = TipoDato.VarChar, ObjValue = "" } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_oficial", TipoDato = TipoDato.VarChar, ObjValue = "" } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
                ds.NombreSP = NameSps.getInfCliente;
                ds.NombreBD = _settings.DB_meg_buro;

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
}

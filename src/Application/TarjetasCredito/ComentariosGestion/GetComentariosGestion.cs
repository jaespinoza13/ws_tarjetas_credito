using Application.Common.Functions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.ComentariosGestion;
using Domain.Parameters;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application.TarjetasCredito.ComentariosGestion
{
    public class GetComentariosGestion : IComentariosGestionDat
    {
        private readonly ApiSettings _settings;
        private readonly ILogs _logsService;
        private readonly string str_clase;
        public readonly IMemoryCache _memoryCache;
        public GetComentariosGestion(IOptionsMonitor<ApiSettings> options, ILogs logsService, IMemoryCache memoryCache)
        {
            _settings = options.CurrentValue;
            _logsService = logsService;
            this.str_clase = GetType().FullName!;
            _memoryCache = memoryCache;
        }
        public Task<ResGetComentGestion> get_coment_gest_sol(ReqGetComentGestion req)
        {
            string str_operacion = "GET_COMENTARIOS_GESTION_SOLICITUD";
            ResGetComentGestion res_tran = new();
            try
            {
                var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros_back" );
                res_tran.LlenarResHeader( req );

                res_tran.lst_cmnt_sol_acep = (from p in lst_parametros
                                              where p.str_nemonico.Contains( _settings.parametro_busqueda_comt )
                                              && p.str_valor_fin == _settings.par_bus_est_comt_acp
                                              select new Comentarios
                                              { int_id_parametro = p.int_id_parametro, str_comentario = p.str_valor_ini }).ToList();
                res_tran.lst_cmnt_sol_rech = (from p in lst_parametros
                                              where p.str_nemonico.Contains( _settings.parametro_busqueda_comt )
                                              && p.str_valor_fin == _settings.par_bus_est_comt_rec
                                              select new Comentarios
                                              { int_id_parametro = p.int_id_parametro, str_comentario = p.str_valor_ini }).ToList();


                var str_codigo = '0';
                var str_error = "";
                res_tran.str_res_codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
                res_tran.diccionario.Add( "str_error", str_error );
            }
            catch (Exception e)
            {
                res_tran.str_res_codigo = "001";
                res_tran.diccionario.Add( "str_o_error", e.ToString() );
                Console.WriteLine( e.ToString() );
                _logsService.SaveExceptionLogs( res_tran, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
                throw new ArgumentException( res_tran.str_id_transaccion );
            }
            return Task.FromResult( res_tran );
        }
    }

}

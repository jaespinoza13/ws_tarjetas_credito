using Application.Common.Interfaces;
using Application.Common.Interfaces.Apis;
using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.TarjetasCredito.InterfazDat;
using Domain.Entities.Axentria;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Reflection;
using Infrastructure.MemoryCache;
using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.InformacionAdicional;
using Newtonsoft.Json;
using Domain.Entities.SituacionFinanciera;
using Domain.Entities.Informacion_Financiera;
using Domain.Parameters;
using Domain.Entities.ComentariosGestion;
using System.Linq;
using static iText.IO.Image.Jpeg2000ImageData;
using Application.TarjetasCredito.ComentariosGestion;
using Domain.Entities.Memoria_Cache;
using Domain.Entities.InformacionAdicional;

namespace Application.TarjetasCredito.AgregarSolicitudTc;
public class AddSolicitudTcHandler : IRequestHandler<ReqAddSolicitudTc, ResAddSolicitudTc>
{
    private readonly IParametersInMemory _parametersInMemory;
    private readonly ITarjetasCreditoDat _tarjetasCreditoDat;
    private readonly IWsGestorDocumental _wsGestorDocumental;
    private readonly ILogs _logs;
    private readonly string str_clase;
    private readonly ApiSettings _settings;
    //private readonly IFuncionalidadesMemory _funcionalidades;
    private readonly GetInformacionAdicional _getInformacionAdicional;
    private readonly IMemoryCache _memoryCache;
    public AddSolicitudTcHandler(IOptionsMonitor<ApiSettings> options, ITarjetasCreditoDat tarjetasCreditoDat, ILogs logs, IParametersInMemory parametersInMemory, IWsGestorDocumental wsGestorDocumental, GetInformacionAdicional getInformacionAdicional, IMemoryCache memoryCache)
    {
        _tarjetasCreditoDat = tarjetasCreditoDat;
        _logs = logs;
        str_clase = GetType().Name;
        _parametersInMemory = parametersInMemory;
        _settings = options.CurrentValue;
        _wsGestorDocumental = wsGestorDocumental;
        _getInformacionAdicional = getInformacionAdicional;
        _memoryCache = memoryCache;
    }
    public async Task<ResAddSolicitudTc> Handle(ReqAddSolicitudTc request, CancellationToken cancellationToken)
    {
        const string str_operacion = "ADD_SOLICITUD_TC";
        var respuesta = new ResAddSolicitudTc();
        var res_tran = new RespuestaTransaccion();
        InformacionMemoriaSol obj_inf_mem_sol = new ();
        InformacionAdicionalSocio obj_inf_adc_soc = new ();


        respuesta.LlenarResHeader( request );

        try
        {
            await _logs.SaveHeaderLogs( request, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            request.int_estado = _parametersInMemory.FindParametroNemonico( _settings.estado_creado ).int_id_parametro;
            request.int_estado_entregado = _parametersInMemory.FindParametroNemonico( _settings.estado_entregado ).int_id_parametro;
            var rango_standard = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_standard ).str_valor_ini;
            var rango_gold = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_gold ).str_valor_ini;
            var rango_black = _parametersInMemory.FindParametroNemonico( _settings.rango_tc_black ).str_valor_ini;


            //Se consulta los comentarios que agregara el asesor 

            //No va, Definir un rango de monto solicitado
            //string rangoEncontrado = "";
            //if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_standard )) != "N")
            //{
            //    determinar_tipo_tarjeta( _settings.tarjeta_standard );
            //}
            //else if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_gold )) != "N")
            //{
            //    determinar_tipo_tarjeta( _settings.tarjeta_gold );
            //}
            //else if ((rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_black )) != "N")
            //{
            //    determinar_tipo_tarjeta( _settings.tarjeta_black );
            //}

            //string rangoEncontrado = validaRango( request.dec_cupo_solicitado, rango_standard );
            //switch (rangoEncontrado)
            //{
            //    case "rango_standard":
            //        request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_standard ).int_id_parametro;
            //        break;
            //    case "rango_gold":
            //        request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_gold ).int_id_parametro;
            //        break;
            //    case "rango_black":
            //        request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_black ).int_id_parametro;
            //        break;
            //    default:
            //        request.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_standard ).int_id_parametro;
            //        break;
            //}

           

            //Se recupera la informacion del socio (SYBASE)
            if (request.str_ente != null)
            {
                //Se recupera la información de la memoria cache 
                obj_inf_mem_sol = get_informacion_memoria( request.str_ente );

                request.str_dpfs_json = obj_inf_mem_sol.str_dpfs_json;
                request.str_cred_hist_json = obj_inf_mem_sol.str_cred_hist_json;
                request.str_egr_soc_json = obj_inf_mem_sol.str_egr_soc_json;
                request.str_ingr_soc_json = obj_inf_mem_sol.str_ingr_soc_json;

                obj_inf_adc_soc = await get_inf_adic_socio( request.str_ente );

                request.str_gar_cns_json = obj_inf_adc_soc.str_gar_cns_json;
                request.str_cred_vig_json = obj_inf_adc_soc.str_cred_vig_json;
                request.str_act_soc_json = obj_inf_adc_soc.str_act_soc_json;
                request.str_pas_soc_json = obj_inf_adc_soc.str_pas_soc_json;

            }
            //Agregar Ecxcepción

            //Se almacena la solicitud de TC
            res_tran = await _tarjetasCreditoDat.addSolicitudTc( request );
            if (res_tran.codigo == "000")
            {
                var req_load_doc = new ReqLoadDocumento();
                req_load_doc.int_canal = Convert.ToInt32( request.str_id_sistema );
                req_load_doc.int_oficina = Convert.ToInt32( request.str_id_oficina );
                req_load_doc.int_tipo_ide = Convert.ToInt32( request.str_num_documento );
                req_load_doc.str_nombre_canal = request.str_nemonico_canal;

                var a = _wsGestorDocumental.addDocumento( req_load_doc, request.str_id_transaccion );

                // Elimina los datos de la memoria caché--> Analizar si se aplica el Principio SOLID
                limpiar_cache( request.str_ente! );
                
            }

            respuesta.str_res_codigo = res_tran.codigo;
            respuesta.str_res_estado_transaccion = res_tran.codigo == "000" ? "OK" : "ERR";
            respuesta.str_res_info_adicional = res_tran.diccionario["str_o_error"];

        }
        catch (Exception e)
        {
            //Console.WriteLine( e.ToString() );
            await _logs.SaveExceptionLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase, e );
            throw new ArgumentException( respuesta.str_id_transaccion );
        }

        await _logs.SaveResponseLogs( respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
        return respuesta;

    }

    public string validaRango(decimal valor, string rango)
    {
        string[] parts = rango.Split( '|' );

        int minValue, maxValue;
        if (!int.TryParse( parts[0], out minValue ) || !int.TryParse( parts[1], out maxValue ))
        {
            return "N";
        }
        if (valor >= minValue && valor <= maxValue)
        {
            return rango;
        }
        else
        {
            return "N";
        }
    }

    public  ReqAddSolicitudTc determinar_tipo_tarjeta(string tipo_tarjeta)
    {
        ReqAddSolicitudTc req_add_sol_tip_tc = new ReqAddSolicitudTc();
        string str_inicial_producto = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_gold ).str_valor_ini;
        string str_codigo_entidad = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_gold ).str_valor_fin;
        req_add_sol_tip_tc.int_tipo_tarjeta = _parametersInMemory.FindParametroNemonico( _settings.tarjeta_gold ).int_id_parametro;
        req_add_sol_tip_tc.str_codigo_producto = str_inicial_producto + str_codigo_entidad;
        return req_add_sol_tip_tc;
    }

    public InformacionMemoriaSol get_informacion_memoria(string str_ente)
    {

        InformacionMemoriaSol req_inf_mem_sol = new InformacionMemoriaSol();

        req_inf_mem_sol.str_dpfs_json = JsonConvert.SerializeObject( _memoryCache.Get<List<DepositosPlazoFijo>>( $"Informacion_dpfs_{str_ente}_ente" ) );
        req_inf_mem_sol.str_cred_hist_json = JsonConvert.SerializeObject( _memoryCache.Get<List<CreditosHistoricos>>( $"Informacion_cred_hist_{str_ente}_ente" ) );
        req_inf_mem_sol.str_ingr_soc_json = JsonConvert.SerializeObject( _memoryCache.Get<List<Ingresos>>( $"Informacion_ing_{str_ente}_ente" ) );
        req_inf_mem_sol.str_egr_soc_json = JsonConvert.SerializeObject( _memoryCache.Get<List<Egresos>>( $"Informacion_egr_{str_ente}_ente" ) );

        return req_inf_mem_sol;
    }

    public async Task<InformacionAdicionalSocio> get_inf_adic_socio(string str_num_ente)
    {
        InformacionAdicionalSocio req_inf_adc_soc = new InformacionAdicionalSocio();
        ResActivosPasivos resActivosPasivos = new();
        ResCreditosVigentes resCreditosVigentes = new();
        ResGarantiasConstituidas resGarantiasConstituidas = new();

        //Se agrega la información de las Garantias Constituidas (SYBASE)
        resGarantiasConstituidas = await _getInformacionAdicional.LoadGarantiasConstitudas( str_num_ente );
        req_inf_adc_soc.str_gar_cns_json = JsonConvert.SerializeObject( resGarantiasConstituidas.lst_gar_cns_soc );

        //Se agrega los creditos vigentes que posee (SYBASE)
        resCreditosVigentes = await _getInformacionAdicional.LoadCreditosVigentes( str_num_ente );
        req_inf_adc_soc.str_cred_vig_json = JsonConvert.SerializeObject( resCreditosVigentes.lst_creditos_vigentes );

        //Se agregan los activos y los pasivos del socio (SYBASE)
        resActivosPasivos = await _getInformacionAdicional.LoadActivosPasivos( str_num_ente );
        req_inf_adc_soc.str_act_soc_json = JsonConvert.SerializeObject( resActivosPasivos.lst_activos_socio );
        req_inf_adc_soc.str_pas_soc_json = JsonConvert.SerializeObject( resActivosPasivos.lst_pasivos_socio );
        return req_inf_adc_soc;
    }

    public void limpiar_cache(string str_num_ente)
    {
        _memoryCache.Remove( $"Informacion_dpfs_{str_num_ente}_ente" );
        _memoryCache.Remove( $"Informacion_cred_hist_{str_num_ente}_ente" );
        _memoryCache.Remove( $"Informacion_ing_{str_num_ente}_ente" );
        _memoryCache.Remove( $"Informacion_egr_{str_num_ente}_ente" );
    }
}

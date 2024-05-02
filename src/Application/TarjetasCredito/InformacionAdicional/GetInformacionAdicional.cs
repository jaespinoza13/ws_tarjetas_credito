using Application.Common.Converting;
using Application.Common.Models;
using Application.TarjetasCredito.InformacionAdicional;
using Application.TarjetasCredito.InterfazDat;
using Domain.InformacionAdicional;
using Microsoft.Extensions.Options;

namespace Infrastructure.MemoryCache;


public class GetInformacionAdicional : IInformacionAdicional
{
    public readonly ApiSettings _settings;
    public readonly IActivosPasivosDat _activosPasivos;
    public readonly ICreditosVigentesDat _creditosVigentes;
    public readonly IGarantiasConstituidasDat _garantiasConstitudasDat;

    public GetInformacionAdicional(IOptionsMonitor<ApiSettings> options, IActivosPasivosDat activosPasivosDat, ICreditosVigentesDat creditosVigentesDat, IGarantiasConstituidasDat garantiasConstitudasDat)
    {
        this._settings = options.CurrentValue;
        this._activosPasivos = activosPasivosDat;
        this._creditosVigentes = creditosVigentesDat;
        this._garantiasConstitudasDat = garantiasConstitudasDat;
    }
    public async Task<ResActivosPasivos> LoadActivosPasivos(string str_num_ente)
    {
        RespuestaTransaccion res_tran = new();
        ResActivosPasivos res_act_pas_soc = new();
        try
        {
            var lista_activos_pasivos = new List<ActivosPasivos>();
            res_tran = await _activosPasivos.get_activos_pasivos_socio( str_num_ente );

            List<ActivosPasivos> lst_act_socio = Conversions.ConvertConjuntoDatosTableToListClass<ActivosPasivos>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
            List<ActivosPasivos> lst_pas_socio = Conversions.ConvertConjuntoDatosTableToListClass<ActivosPasivos>( (ConjuntoDatos)res_tran.cuerpo, 1 )!;
            res_act_pas_soc.lst_activos_socio = lst_act_socio;
            res_act_pas_soc.lst_pasivos_socio = lst_pas_socio;

        }
        catch (Exception ex)
        {
            throw new ArgumentException( ex.Message );
        }
        return res_act_pas_soc;
    }
    public async Task<ResCreditosVigentes> LoadCreditosVigentes(string str_num_ente)
    {
        RespuestaTransaccion res_tran = new();
        ResCreditosVigentes res_cred_vig_soc = new();
        try
        {
            var lista_creditos_vigentes = new List<CreditosVigentes>();
            res_tran = await _creditosVigentes.get_creditos_vigentes( str_num_ente );

            List<CreditosVigentes> lst_cred_vig_socio = Conversions.ConvertConjuntoDatosTableToListClass<CreditosVigentes>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
            res_cred_vig_soc.lst_creditos_vigentes = lst_cred_vig_socio;

        }
        catch (Exception ex)
        {
            throw new ArgumentException( ex.Message );
        }
        return res_cred_vig_soc;
    }

    public async Task<ResGarantiasConstituidas> LoadGarantiasConstitudas(string str_num_ente)
    {
        RespuestaTransaccion res_tran = new();
        ResGarantiasConstituidas res_agr_cns_soc = new();
        try
        {
            var lista_garantias_constituidas = new List<GarantiasConstituidas>();
            res_tran = await _garantiasConstitudasDat.get_gar_cns_soc( str_num_ente );

            List<GarantiasConstituidas> lst_gar_cns_socio = Conversions.ConvertConjuntoDatosTableToListClass<GarantiasConstituidas>( (ConjuntoDatos)res_tran.cuerpo, 0 )!;
            res_agr_cns_soc.lst_gar_cns_soc = lst_gar_cns_socio;

        }
        catch (Exception ex)
        {
            throw new ArgumentException( ex.Message );
        }
        return res_agr_cns_soc;
    }

}

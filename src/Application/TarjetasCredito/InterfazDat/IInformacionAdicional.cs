using Application.TarjetasCredito.InformacionAdicional;

namespace Infrastructure.MemoryCache;

public interface IInformacionAdicional
{
    Task<ResActivosPasivos> LoadActivosPasivos(string str_num_ente);
    Task<ResCreditosVigentes> LoadCreditosVigentes(string str_num_ente);
    Task<ResGarantiasConstituidas> LoadGarantiasConstitudas(string str_num_ente);
}

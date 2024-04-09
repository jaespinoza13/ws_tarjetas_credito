
using Application.Common.Interfaces.Dat;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Application.TarjetasCredito.InformacionAdicional;
using Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MemoryCache;

public interface IInformacionAdicional
{
    Task<ResActivosPasivos> LoadActivosPasivos(string str_num_ente);
    Task<ResCreditosVigentes> LoadCreditosVigentes(string str_num_ente);
    Task<ResGarantiasConstituidas> LoadGarantiasConstitudas(string str_num_ente);
}

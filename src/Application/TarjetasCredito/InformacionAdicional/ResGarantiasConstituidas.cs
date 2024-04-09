using Application.Common.ISO20022.Models;
using Domain.InformacionAdicional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InformacionAdicional;

public class ResGarantiasConstituidas : ResComun
{
    public List<GarantiasConstituidas> lst_gar_cns_soc { get; set; } = new List<GarantiasConstituidas>();
}

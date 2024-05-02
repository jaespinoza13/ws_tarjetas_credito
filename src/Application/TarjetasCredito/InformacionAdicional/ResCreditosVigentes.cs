using Application.Common.ISO20022.Models;
using Domain.InformacionAdicional;

namespace Application.TarjetasCredito.InformacionAdicional;

public class ResCreditosVigentes : ResComun
{
    public List<CreditosVigentes> lst_creditos_vigentes { get; set; } = new List<CreditosVigentes>();
}

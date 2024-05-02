using Application.Common.ISO20022.Models;
using Domain.Entities.SituacionFinanciera;

namespace Application.TarjetasCredito.SituacionFinanciera
{
    public class ResGetSitFin : ResComun
    {
        public List<DepositosPlazoFijo> lst_dep_plazo_fijo { get; set; } = new List<DepositosPlazoFijo>();
        public List<CreditosHistoricos> lst_creditos_historicos { get; set; } = new List<CreditosHistoricos>();
    }
}

using Application.Common.ISO20022.Models;
using Domain.Entities.Informacion_Financiera;
using Domain.Entities.SituacionFinanciera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.SituacionFinanciera
{
    public class ResGetSitFin : ResComun
    {
        public List<DepositosPlazoFijo> lst_dep_plazo_fijo { get; set; } = new List<DepositosPlazoFijo>();
        public List<CreditosHistoricos> lst_creditos_historicos { get; set; } = new List<CreditosHistoricos>();
    }
}

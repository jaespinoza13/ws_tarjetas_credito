using Application.Common.ISO20022.Models;
using Domain.InformacionAdicional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InformacionAdicional;

public class ResCreditosVigentes: ResComun
{
    public List<CreditosVigentes> lst_creditos_vigentes {  get; set; } = new List<CreditosVigentes>();
}

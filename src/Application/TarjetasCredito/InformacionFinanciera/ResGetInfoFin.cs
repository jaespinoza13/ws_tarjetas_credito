using Application.Common.ISO20022.Models;
using Domain.Entities.DatosCliente;
using Domain.Entities.Informacion_Financiera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InformacionFinanciera
{
    public class ResGetInfoFin : ResComun
    {
        public List<Ingresos> lst_ingresos_socio { get; set; } = new List<Ingresos>();
        public List<Egresos> lst_egresos_socio{ get; set; } = new List<Egresos>();
    }
}


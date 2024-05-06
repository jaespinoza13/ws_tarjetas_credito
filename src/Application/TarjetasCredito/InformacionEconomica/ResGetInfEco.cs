using Application.Common.ISO20022.Models;
using Domain.Entities.Informacion_Financiera;

namespace Application.TarjetasCredito.InformacionEconomica
{
    public class ResGetInfEco : ResComun
    {
        public List<Ingresos> lst_ingresos_socio { get; set; } = new List<Ingresos>();
        public List<Egresos> lst_egresos_socio { get; set; } = new List<Egresos>();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.SituacionFinanciera
{
    public class CreditosHistoricos
    {
        public int int_operacion { get; set; }
        public string str_tipo { get; set; } = string.Empty;
        public string str_operacion_cred { get; set; } = string.Empty;
        public Decimal dcm_monto_aprobado { get; set; }
        public DateTime dtt_fecha_vencimiento { get; set; }
        public DateTime dtt_fecha_concesion { get; set; }
        public int int_cuotas_vencidas { get; set; }
        public int int_dias_mora { get; set; }
        public int int_orden { get; set; }

    }
}

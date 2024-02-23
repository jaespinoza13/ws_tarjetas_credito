using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.SituacionFinanciera
{
    public class DepositosPlazoFijo
    {

        public int int_id_cuenta { get; set; }
        public string? str_num_cuenta { get; set; } = string.Empty;
        public Decimal dcm_ahorro { get; set; }
        public DateTime dtt_fecha_movimiento { get;set; }
        public Decimal dcm_promedio { get; set; }
        public string? str_tipo_cta { get; set; } = string.Empty;
        public string? str_estado { get; set; } = string.Empty;
        public int int_orden { get; set; }
    }
}

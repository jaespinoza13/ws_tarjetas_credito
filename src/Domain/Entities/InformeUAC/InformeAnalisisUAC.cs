using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.InformeUAC;

public class InformeAnalisisUAC
{
    public int int_id_parametro { get; set; }
    public string str_tipo { get; set; } = string.Empty;
    public string str_descripcion { get; set; } = string.Empty;
    public string str_detalle { get; set; } = string.Empty;

    public class InformeAnalisis
    {
        public string json_inf_anl_ase_cre { get; set; } = String.Empty;
    }
}

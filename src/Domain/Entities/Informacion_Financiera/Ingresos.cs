using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Informacion_Financiera
{
    public class Ingresos
    {
        public int int_codigo { get; set; } 
        public string? str_descripcion { get; set; } = string.Empty;
        public Decimal dec_valor { get; set; }  
    }
}

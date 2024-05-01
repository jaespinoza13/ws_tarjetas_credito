using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Orden
{
    public class Orden
    {
        public string str_numero_orden { get; set; }
        public string str_estado_orden { get; set; }
        public string str_agencia_solicita { get; set; }
        public string str_prefijo { get; set; }
        public string str_usuario_crea { get; set; }
        public string str_costo_emision { get; set; }
        public string str_descripcion { get; set; }
        public DateTime dtt_fecha_creacion { get; set; }
        public DateTime dtt_fecha_solicita { get; set; }
        public DateTime dtt_fecha_recepta { get; set; }
        public DateTime dtt_fecha_distribucion { get; set; }
        public DateTime dtt_fecha_cierre { get; set; }

        public string str_cantidad { get; set; }
        public List<object> lst_tarjetas_solicitadas { get; set; }

        



    }
}

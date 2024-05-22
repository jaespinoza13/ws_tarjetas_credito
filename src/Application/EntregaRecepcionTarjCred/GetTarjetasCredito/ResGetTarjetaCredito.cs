using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntregaRecepcionTarjCred.GetTarjetasCredito;

public class ResGetTarjetaCredito
{
    public List<ResTarjetaCredito> lst_tarj_cred { get; set; } = new List<ResTarjetaCredito>();
    public class ResTarjetaCredito
    {
        public int int_num_orden { get; set; }
        public string str_tipo_requerido { get; set; } = string.Empty;
        public string str_tipo_producto { get; set; } = string.Empty;
        public string str_estado { get; set; } = string.Empty;
        public string str_usuario_crea { get; set; } = string.Empty;
        public string str_usuario_recibe { get; set; } = string.Empty;
        public DateTime dtt_fecha_creacion { get; set; }
        public DateTime dtt_fecha_envia_oficina { get; set; }
        public DateTime dtt_fecha_recepcion { get; set; }
        public int int_cantidad { get; set; }
        public string str_oficina_solicita { get; set; } = string.Empty;
        public string str_oficina_destino { get; set; } = string.Empty;
        public string str_descripcion_recepta { get; set; } = string.Empty;
    }
}

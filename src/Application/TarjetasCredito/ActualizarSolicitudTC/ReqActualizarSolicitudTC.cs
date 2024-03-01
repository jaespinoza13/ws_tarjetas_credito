using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ActualizarSolicitudTC
{
    public class ReqActualizarSolicitudTC : Header, IRequest<ResActualizarSolicutdTC>
    {
        public string str_nombres { get; set; } = string.Empty;
        public string str_primer_apellido { get; set; } = string.Empty;
        public string str_segundo_apellido { get; set; } = string.Empty;
        public DateTime dtt_fecha_nacimiento { get; set; }
        public string str_sexo { get; set; } = string.Empty;

        // tcr_solicitudes
        public int int_estado { get; set; }
        public Decimal dec_cupo_solicitado { get; set; }
        public string str_celular { get; set; } = string.Empty;
        public string str_correo { get; set; } = string.Empty;
        public DateTime dtt_fecha_actualizacion { get; set; }
        public string str_usuario_proc { get; set; } = string.Empty;
        public int int_oficina_entrega { get; set; }
        public string str_habilitada_compra { get; set; } = string.Empty;
        public Decimal dec_max_compra { get; set; }
        public string str_denominacion_tarjeta { get; set; } = string.Empty;
        public string str_calle_num_puerta { get; set; } = string.Empty;
        public string str_localidad { get; set; } = string.Empty;
        public string str_barrio { get; set; } = string.Empty;
        public string str_codigo_provincia { get; set; } = string.Empty;
        public string str_codigo_postal { get; set; } = string.Empty;
        public string str_zona_geografica { get; set; } = string.Empty;
        public string str_grupo_liquidacion { get; set; } = string.Empty;
        public Decimal dec_imp_lim_compras { get; set; }
        public string str_telefono_2 { get; set; } = string.Empty;
        public string str_datos_adicionales { get; set; } = string.Empty;
        public string str_codigo_ocupacion { get; set; } = string.Empty;
        public string str_duracion { get; set; } = string.Empty;
        public string str_comentario_proceso { get; set; } = string.Empty;
        public long int_numero_cuenta { get; set; }
        public int int_estado_entregado { get; set; }
    }
}

using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.ActualizarSolicitudTC
{
    public class ReqActualizarSolicitudTC : Header, IRequest<ResActualizarSolicutdTC>
    {
        public int int_id_solicitud { get; set; }
        public int int_id_flujo_sol { get; set; }

        public string str_nombres { get; set; } = string.Empty;
        public string str_primer_apellido { get; set; } = string.Empty;
        public string str_segundo_apellido { get; set; } = string.Empty;
        public DateTime dtt_fecha_nacimiento { get; set; }
        public string str_sexo { get; set; } = string.Empty;
        public int int_ente { get; set; }

        // tcr_solicitudes
        public int int_estado { get; set; }
        public Decimal dec_cupo_solicitado { get; set; }
        public string str_celular { get; set; } = string.Empty;
        public string str_correo { get; set; } = string.Empty;
        public DateTime? dtt_fecha_actualizacion { get; set; }
        public string str_usuario_proc { get; set; } = string.Empty;
        public int int_oficina_entrega { get; set; }
        public string str_habilitada_compra { get; set; } = string.Empty;
        public Decimal dec_max_compra { get; set; }
        public string str_denominacion_tarjeta { get; set; } = string.Empty;
        public string str_calle_num_puerta { get; set; } = string.Empty;
        public string str_barrio { get; set; } = string.Empty;
        public string str_codigo_provincia { get; set; } = string.Empty;
        public string str_codigo_postal { get; set; } = string.Empty;
        public string str_zona_geografica { get; set; } = string.Empty;
        public int int_estado_entregado { get; set; }
        public Decimal dec_cupo_sugerido { get; set; }
        public Decimal dec_cupo_aprobado { get; set; }
        public int int_ente_aprobador { get; set; }
        public string str_codigo_producto { get; set; } = string.Empty;
        public int int_codigo_sucursal { get; set; }
        public int int_modelo_tratamiento { get; set; }
        public string str_codigo_afinidad { get; set; } = string.Empty;
        public int int_num_promotor { get; set; }
        public string str_denominacion_socio { get; set; } = string.Empty;
        public string str_marca_graba { get; set; } = string.Empty;
        public string str_comentario_proceso { get; set; } = string.Empty;
        public string str_tipo_cuenta_banc { get; set; } = string.Empty;
        public string str_cod_lim_compra { get; set; } = string.Empty;
        public string str_id_doc_adicional { get; set; } = string.Empty;
        public string str_id_doc_tratamiento_datos_per { get; set; } = string.Empty;
        public string str_id_doc_aut_cons_buro { get; set; } = string.Empty;
        public long int_numero_cuenta { get; set; }
    }
}

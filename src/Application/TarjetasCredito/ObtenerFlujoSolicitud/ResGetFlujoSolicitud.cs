using Application.Common.ISO20022.Models;
using System.Text.Json.Serialization;

namespace Application.TarjetasCredito.ObtenerFlujoSolicitud
{
    public class ResGetFlujoSolicitud : ResComun
    {
        public List<FlujoSolicitudes> flujo_solicitudes { get; set; } = new List<FlujoSolicitudes> { };

        public class FlujoSolicitudes
        {
            public int slw_estado { get; set; }
            public int slw_id { get; set; }
            public string str_estado { get; set; } = string.Empty;
            public string slw_cupo_solicitado { get; set; } = string.Empty;
            public string slw_cupo_aprobado { get; set; } = string.Empty;
            public string slw_celular { get; set; } = string.Empty;
            public string slw_correo { get; set; } = string.Empty;
            public DateTime slw_fecha_solicitud { get; set; }
            public DateTime slw_fecha_actualizacion { get; set; }
            public DateTime slw_fecha_aprobacion { get; set; }
            public DateTime slw_fecha_rechazo { get; set; }
            public string slw_usuario_proc { get; set; } = string.Empty;
            public int slw_oficina_proc { get; set; }
            public int slw_oficina_entrega { get; set; }
            public int slw_ente_aprobador { get; set; }
            public string slw_codigo_producto { get; set; } = string.Empty;
            public decimal slw_codigo_sucursal { get; set; }
            public decimal slw_modelo_tratamiento { get; set; }
            public string slw_codigo_afinidad { get; set; } = string.Empty;
            public int slw_num_promotor { get; set; }
            public char slw_habilitada_compra { get; set; }
            public decimal slw_max_compra { get; set; }
            public string slw_denominacion_tarjeta { get; set; } = string.Empty;
            public char slw_marca_graba { get; set; }
            public string slw_calle_num_puerta { get; set; } = string.Empty;
            public string slw_localidad { get; set; } = string.Empty;
            public string slw_barrio { get; set; } = string.Empty;
            public char slw_cod_prov { get; set; }
            public string slw_cod_postal { get; set; } = string.Empty;
            public string slw_zona_geo { get; set; } = string.Empty;
            public char slw_grupo_liquidacion { get; set; }
            public decimal slw_imp_lim_compras { get; set; }
            public string slw_telf_2 { get; set; } = string.Empty;
            public string slw_datos_adicionales { get; set; } = string.Empty;
            public string slw_cod_ocupacion { get; set; } = string.Empty;
            public string slw_duracion { get; set; } = string.Empty;
            public char slw_marca_emision { get; set; }
            public string slw_rfc { get; set; } = string.Empty;
            public char slw_marca_tpp { get; set; }
            public string slw_rsrv_uso_credencial1 { get; set; } = string.Empty;
            public string slw_rsrv_uso_credencial2 { get; set; } = string.Empty;
            public string slw_cuarta_linea { get; set; } = string.Empty;
            public string slw_numero_cuenta { get; set; } = string.Empty;
            public string slw_comentario_proceso { get; set; } = string.Empty;  
        }
    }
}

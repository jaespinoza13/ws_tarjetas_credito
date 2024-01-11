using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.AgregarSolicitudTc;

public class ReqAgregarSolicitudTc : Header, IRequest<ResAgregarSolicitudTc>
{

    public string str_tipo_documento { get; set; } = string.Empty;
    public string str_num_documento { get; set; } = string.Empty;
    public int int_ente { get; set; }
    public string str_nombres { get; set; } = string.Empty;
    public string str_primer_apellido { get; set; } = string.Empty;
    public string str_segundo_apellido { get; set; } = string.Empty;
    public DateTime dtt_fecha_nacimiento { get; set; }
    public string str_sexo { get; set; } = string.Empty;

    // tcr_solicitudes
    public int int_tipo_tarjeta { get; set; }
    public Decimal dec_cupo_solicitado { get; set; }
    public Decimal dec_cupo_aprobado { get; set; }
    public string str_celular { get; set; } = string.Empty;
    public string str_correo { get; set; } = string.Empty;
    public DateTime dtt_fecha_solicitud { get; set; }
    public DateTime dtt_fecha_actualizacion { get; set; }
    public string str_usuario_crea { get; set; } = string.Empty;
    public int int_oficina_crea { get; set; }
    public int int_oficina_entrega { get; set; }
    public int int_ente_aprobador { get; set; }
    public string str_codigo_producto { get; set; } = string.Empty;
    public int int_codigo_sucursal { get; set; }
    public int int_modelo_tratamiento { get; set; }
    public string str_codigo_afinidad { get; set; } = string.Empty;
    public int int_num_promotor { get; set; }
    public string str_habilitada_compra { get; set; } = string.Empty;
    public Decimal dec_max_compra { get; set; }
    public string str_denominacion_tarjeta { get; set; } = string.Empty;
    public string str_marca_graba { get; set; } = string.Empty;
    public string str_calle_num_puerta { get;set; } = string.Empty;
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
    public string str_marca_emision { get; set; } = string.Empty;
    public string str_rfc { get; set; } = string.Empty;
    public string str_marca_tpp { get; set; } = string.Empty;
    public string str_rsrv_uso_credencial_1 { get; set; } = string.Empty;
    public string str_rsrv_uso_credencial_2 { get; set; } = string.Empty;
    public string str_cuarta_linea { get; set; } = string.Empty;
    public long int_numero_cuenta { get; set; }  


}
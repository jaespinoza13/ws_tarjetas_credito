using Application.Common.ISO20022.Models;
using System.Text.Json.Serialization;

namespace Application.TarjetasCredito.ObtenerSolicitudes
{
    public class ResGetSolicitudes : ResComun
    {
        public List<SolicitudTc> solicitudes { get; set; } = new List<SolicitudTc> { };

        public class SolicitudTc
        {
            public int int_id { get; set; }
            public int int_ente { get; set; }
            public string str_identificacion { get; set; } = string.Empty;
            public string str_nombres { get; set; } = string.Empty;
            [JsonIgnore]
            public int int_tipo_tarjeta { get; set; }
            public string str_tipo_tarjeta { get; set; } = string.Empty;
            public string str_calificacion { get; set; } = string.Empty;
            public decimal dec_cupo_solicitado { get; set; }
            public DateTime dtt_fecha_solicitud { get; set; }
            public string str_usuario_crea { get; set; } = string.Empty;
            public int int_oficina_crea { get; set; }
            public string str_estado { get; set; } = string.Empty;
            public int int_estado { get; set; }

        }
    }
}

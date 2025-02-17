﻿using Application.Common.ISO20022.Models;
using System.Text.Json.Serialization;

namespace Application.TarjetasCredito.ObtenerSolicitudes
{
    public class ResGetSolicitudes : ResComun
    {
        public List<SolicitudTc> solicitudes { get; set; } = new List<SolicitudTc> { };
        public List<ProspectosTc> prospectos { get; set; } = new List<ProspectosTc> { };

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
            public string dec_cupo_solicitado { get; set; } = string.Empty;
            public string dtt_fecha_solicitud { get; set; } = string.Empty;
            public string str_usuario_crea { get; set; } = string.Empty;
            public int int_oficina_crea { get; set; }
            public string str_estado { get; set; } = string.Empty;
            public string str_analista { get; set; } = string.Empty;
            public int int_estado { get; set; }
        }

        public class ProspectosTc
        {
            public int pro_id { get; set; }
            public string pro_num_documento { get; set; } = string.Empty;
            public string pro_ente { get; set; } = string.Empty;
            public string pro_nombres { get; set; } = string.Empty;
            public string pro_apellidos { get; set; } = string.Empty;
            public string pro_email { get; set; } = string.Empty;
            public string pro_celular { get; set; } = string.Empty;
            public string pro_cupo_solicitado { get; set; } = string.Empty;
            public string pro_fecha_solicitud { get; set; } = string.Empty;
            public string pro_autoriza_cons_buro { get; set; } = string.Empty;
            public string pro_autoriza_datos_per { get; set; } = string.Empty;
            public string pro_usuario_crea { get; set; } = string.Empty;
            public int pro_oficina_crea { get; set; }
            public string pro_estado { get; set; } = string.Empty;

        }
    }
}

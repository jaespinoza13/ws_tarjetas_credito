using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.AgregarProspectoTC
{
    public class ReqAddProspectoTc : Header, IRequest<ResAddProspectoTc>
    {
        public int int_ente { get; set; } 
        public string str_tipo_documento { get; set; } = string.Empty;
        public string str_num_documento { get; set; } = string.Empty;
        public string str_nombres { get; set; } = string.Empty;
        public string str_apellidos { get; set; } = string.Empty;
        public string str_correo { get; set; } = string.Empty;
        public string str_celular { get; set; } = string.Empty;
        public decimal dec_cupo_solicitado { get; set; }
        public DateTime dtt_fecha_crea { get; set; }
        public string str_id_autoriza_cons_buro { get; set; } = string.Empty;
        public string str_id_autoriza_datos_per { get; set; } = string.Empty;
        public string str_estado { get; set; } = string.Empty;
        public string str_comentario { get; set; } = string.Empty;
        public string str_comentario_adicional { get; set; } = string.Empty;
    }
}

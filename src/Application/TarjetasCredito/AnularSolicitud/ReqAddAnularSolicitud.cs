using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.AnularSolicitud
{
    public class ReqAddAnularSolicitud : Header, IRequest<ResAddAnularSolicitud>
    {
        [Required]
        public int int_id_solicitud { get; set; }
        [Required]
        public string str_comentario { get; set; } = string.Empty;
        [Required]
        public int int_estado { get; set; }
    }
}

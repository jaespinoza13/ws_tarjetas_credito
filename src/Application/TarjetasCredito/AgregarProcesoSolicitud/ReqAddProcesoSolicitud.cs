using Application.Common.ISO20022.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.TarjetasCredito.AgregarComentario
{
    public class ReqAddProcesoSolicitud : Header, IRequest<ResAddProcesoSolicitud>
    {
        [Required]
        public bool bl_regresa_estado { get; set; }
        [Required]
        public int int_id_solicitud { get; set; }
        [Required]
        public string str_comentario { get; set; } = string.Empty;
        [Required]
        public int int_estado { get; set; }

        [Required]
        public string str_decision_sol { get; set; } = string.Empty;
    }
}

using Application.Common.ISO20022.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.TarjetasCredito.AgregarComentario
{
    public class ReqAddProcesoSolicitud : Header, IRequest<ResAddProcesoSolicitud>
    {
        public bool bl_regresa_estado { get; set; }
        public bool bl_ingreso_fijo { get; set; }
        public int int_id_solicitud { get; set; }
        public string str_comentario { get; set; } = string.Empty;
        public int int_estado { get; set; }
        public Decimal dcc_cupo_aprobado { get; set; }
    }
}

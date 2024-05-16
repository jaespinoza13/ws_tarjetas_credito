using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.AnularSolicitud
{
    public class ReqAddProcesoEspecifico : Header, IRequest<ResAddProcesoEspecifico>
    {
        public int int_id_solicitud { get; set; }
        public string str_comentario { get; set; } = string.Empty;
        public string str_estado { get; set; } = string.Empty;
        public Decimal dcc_cupo_aprobado { get; set; } = 0;
    }
}

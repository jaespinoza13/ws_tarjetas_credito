using Application.Common.ISO20022.Models;
using MediatR;

namespace Application.TarjetasCredito.AnalistasCredito.AddAnalistaSolicitud
{
    public class ReqAddAnalistaSolicitud : Header, IRequest<ResAddAnalistaSolicitud>
    {
        public string str_id_analista { get; set; } = string.Empty;
        public string str_analista { get; set; } = string.Empty;
        public int int_id_solicitud { get; set; }
        public int int_estado { get; set; }
    }
}

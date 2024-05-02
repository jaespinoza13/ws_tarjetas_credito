using Application.Common.ISO20022.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.TarjetasCredito.ObtenerFlujoSolicitud
{
    public class ReqGetFlujoSolicitud : Header, IRequest<ResGetFlujoSolicitud>
    {
        [Required]
        public int int_id_solicitud { get; set; }
    }
}

using Application.Common.ISO20022.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.TarjetasCredito.DatosClienteTc
{
    public class ReqGetDatosCliente : ResComun, IRequest<ResGetDatosCliente>
    {
        [Required]
        public string? str_num_documento { get; set; } = string.Empty;
        [Required]
        public string? str_login_usuario { get; set; } = string.Empty;
    }
}

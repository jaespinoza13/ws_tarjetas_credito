using Application.Common.ISO20022.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.TarjetasCredito.ComentariosAsesor;
public class ReqGetInforme : ResComun, IRequest<ResGetInforme>
{
    [Required]
    public int int_id_est_sol { get; set; }
    public string str_nem_par_inf { get; set; } = string.Empty;
    [Required]
    public int int_id_sol { get; set; }
}

using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosAsesorCredito;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class ReqAddInforme : ResComun, IRequest<ResAddInforme>
{
    [Required]
    public int int_id_sol {  get; set; }
    [Required]
    public int int_id_est_sol { get; set; }
    public string str_nem_par_inf { get; set; } = string.Empty;
    public List<Informes> lst_informe { get; set; } = new List<Informes>();
    public string str_informes_json { get; set; } = string.Empty;
    
}

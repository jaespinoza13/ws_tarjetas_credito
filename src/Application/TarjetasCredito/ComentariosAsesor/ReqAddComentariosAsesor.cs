using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosAsesorCredito;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class ReqAddComentariosAsesor : ResComun, IRequest<ResAddComentariosAsesor>
{
    public int int_id_sol {  get; set; }

    public List<ComentarioAsesor> lst_cmnt_ase_cre { get; set; } = new List<ComentarioAsesor>();

    public string str_cmnt_ase_json { get; set; } = string.Empty;
    
}

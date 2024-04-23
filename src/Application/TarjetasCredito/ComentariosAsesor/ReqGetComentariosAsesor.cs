using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ComentariosAsesor;
public class ReqGetComentariosAsesor : ResComun, IRequest<ResGetComentariosAsesor>
{
    public string str_nem_par_inf { get; set; } = string.Empty;
    public int int_id_sol { get; set; } 
}

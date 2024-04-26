using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosAsesorCredito;
using Domain.Entities.InformeUAC;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entities.InformeUAC.InformeAnalisisUAC;

namespace Application.TarjetasCredito.InformeUAC;

public class ReqAddInformeUAC : ResComun, IRequest<ResAddInformeUAC>
{
    public int int_id_sol { get; set; }
    public List<InformeAnalisisUAC> lst_inf_anl_uac { get; set; } = new List<InformeAnalisisUAC>();
    public string str_obj_anl_uac_json { get; set; } = string.Empty;

}

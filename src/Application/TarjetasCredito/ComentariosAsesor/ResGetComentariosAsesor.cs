using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosAsesorCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entities.ComentariosAsesorCredito.ComentarioAsesor;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class ResGetComentariosAsesor : ResComun
{
    public List<ComentarioAsesor> lst_comn_ase_cre { get; set; } = new List<ComentarioAsesor>();

}

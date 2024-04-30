using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosAsesorCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Entities.ComentariosAsesorCredito.Informes;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class ResGetInforme : ResComun
{
    public List<Informes> lst_informe { get; set; } = new List<Informes>();

}

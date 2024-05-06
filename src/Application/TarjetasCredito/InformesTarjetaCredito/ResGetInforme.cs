using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosAsesorCredito;

namespace Application.TarjetasCredito.ComentariosAsesor;

public class ResGetInforme : ResComun
{
    public List<Informes> lst_informe { get; set; } = new List<Informes>();

}

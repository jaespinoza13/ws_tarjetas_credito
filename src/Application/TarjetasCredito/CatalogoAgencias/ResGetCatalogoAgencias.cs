using Application.Common.ISO20022.Models;
using Domain.Entities.Agencias;

namespace Application.TarjetasCredito.CatalogoAgencias
{
    public class ResGetCatalogoAgencias : ResComun
    {
        public List<Agencias> lst_agencias { get; set; } = new List<Agencias>();
    }
}

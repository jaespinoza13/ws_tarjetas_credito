using Application.Common.ISO20022.Models;
using Domain.Entities.Agencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.CatalogoAgencias
{
    public class ResGetCatalogoAgencias : ResComun
    {

        public List<Agencias> lst_agencias { get; set; } = new List<Agencias>();
    
    }
}

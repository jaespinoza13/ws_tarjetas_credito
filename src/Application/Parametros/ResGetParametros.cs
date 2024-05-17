using Application.Common.ISO20022.Models;
using Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parametros;

public class ResGetParametros : ResComun
{
    public List<Parametro> lst_parametros { get; set; }   = new List<Parametro>();
    //public Dictionary<string, string> diccionario { get; set; } = new();
}

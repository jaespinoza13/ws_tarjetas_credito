using Application.Common.ISO20022.Models;
using Domain.Entities.ComentariosGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.ComentariosGestion;

public class ResGetComentGestion : ResComun
{
    public List<Comentarios> lst_cmnt_sol_acep {  get; set; } = new List<Comentarios>();
    public List<Comentarios> lst_cmnt_sol_rech {  get; set; } = new List<Comentarios>();
    //Analizar si se deja esta variable 
    public Dictionary<string, string> diccionario { get; set; } = new();
}

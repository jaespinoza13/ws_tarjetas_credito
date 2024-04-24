using Domain.Entities.ComentariosGestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ComentariosAsesorCredito;

public class ComentarioAsesor
{
    public int int_id_parametro {  get; set; } 
    public string str_tipo { get; set; } = string.Empty;
    public string str_descripcion { get; set; } = string.Empty;
    public string str_detalle { get; set; } = string.Empty;


    public class ComentarioAsesorRes
    {
        public string json_comentarios { get; set; } = String.Empty;
    }
}

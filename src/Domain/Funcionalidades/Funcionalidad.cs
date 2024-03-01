using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Funcionalidades
{
    public class Funcionalidad
    {
        public int int_id_permiso {  get; set; }
        public int int_tipo {  get; set; }
        public string str_tipo {  get; set; } = string.Empty;
        public string str_nombre {  get; set; } = string.Empty;
        public string str_descripcion {  get; set; } = string.Empty;
    }
}

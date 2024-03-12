using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Funcionalidades
{
    public class Funcionalidad
    {
        public int fun_id {  get; set; }
        public int fun_tipo {  get; set; }
        public string fun_tipo_nombre {  get; set; } = string.Empty;
        public string fun_nombre {  get; set; } = string.Empty;
        public string fun_descripcion {  get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DatosCliente
{
    public class DireccionDomicilio
    {
        public int int_dir_direccion {  get; set; } 
        public string str_dir_tipo { get; set; } = string.Empty;
        public string? str_dir_ciudad { get; set; } = string.Empty;
        public string? str_dir_sector { get; set; } = string.Empty;
        public string? str_dir_barrio { get; set; } = string.Empty;
        public string? str_dir_descripcion_dom { get; set; } = string.Empty;
        public string? str_dir_num_casa { get; set; } = string.Empty;

    }
}

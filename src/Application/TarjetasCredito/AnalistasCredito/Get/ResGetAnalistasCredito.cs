using Application.Common.ISO20022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.AnalistasCredito.Get
{
    public class ResGetAnalistasCredito : ResComun
    {
        public List<Analistas> lst_analistas { get; set; } = new List<Analistas>();

        public class Analistas
        {
            public int int_id_usuario { get; set; }
            public string str_login { get; set; } = string.Empty;
            public int int_oficina { get; set; }
        }
    }
}

using Application.Common.ISO20022.Models;

namespace Application.TarjetasCredito.AnalistasCredito.GetAnalistas
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

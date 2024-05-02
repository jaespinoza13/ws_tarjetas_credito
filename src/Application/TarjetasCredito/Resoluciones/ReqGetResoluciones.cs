using Application.Common.ISO20022.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.TarjetasCredito.Resoluciones;

public class ReqGetResoluciones : ResComun, IRequest<ResGetResoluciones>
{
    [Required]
    public int int_id_sol { get; set; }
    public List<Resolucion> lst_informe { get; set; } = new List<Resolucion>();
    public class Resolucion
    {
        public Decimal dec_cupo_solicitado { get; set; } = Decimal.Zero;
        public Decimal dec_cupo_sugerido { get; set; } = Decimal.Zero;
        public string str_usuario_proc { get; set; } = string.Empty;
        public DateTime dtt_fecha_actualizacion { get; set; }
        public string str_decision_solicitud { get; set; } = string.Empty;
        public string str_comentario_proceso { get; set; } = string.Empty;
    }
}

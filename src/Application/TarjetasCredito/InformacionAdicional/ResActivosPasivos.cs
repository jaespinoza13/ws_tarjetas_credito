using Application.Common.ISO20022.Models;
using Domain.InformacionAdicional;

namespace Application.TarjetasCredito.InformacionAdicional;

public class ResActivosPasivos : ResComun
{
    public List<ActivosPasivos> lst_activos_socio { get; set; } = new List<ActivosPasivos>();
    public List<ActivosPasivos> lst_pasivos_socio { get; set; } = new List<ActivosPasivos>();
}

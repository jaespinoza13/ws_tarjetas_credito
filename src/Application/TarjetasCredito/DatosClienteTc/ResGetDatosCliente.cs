using Application.Common.ISO20022.Models;
using Domain.Entities.DatosCliente;

namespace Application.TarjetasCredito.DatosClienteTc
{
    public class ResGetDatosCliente : ResComun
    {

        public List<DatosCliente> datos_cliente { get; set; } = new List<DatosCliente>();
    }
}

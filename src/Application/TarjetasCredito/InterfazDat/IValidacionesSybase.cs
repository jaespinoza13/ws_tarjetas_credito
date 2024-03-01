using Application.Common.ISO20022.Models;
using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IValidacionesSybase
    {
        Task<RespuestaTransaccion> validarPermisoPerfil(Header header);
    }
}

using Application.Common.Models;
using Application.TarjetasCredito.CatalogoAgencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface ICatalogoAgenciasDat
    {
        Task<RespuestaTransaccion> get_catalogo_agencias(ReqGetCatalogoAgencias request);
    }
}

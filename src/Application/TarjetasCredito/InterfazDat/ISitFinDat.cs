using Application.Common.Models;
using Application.TarjetasCredito.InformacionEconomica;
using Application.TarjetasCredito.SituacionFinanciera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface ISitFinDat
    {
        Task<RespuestaTransaccion> get_situacion_financiera(ReqGetSitFin request);
    }
}

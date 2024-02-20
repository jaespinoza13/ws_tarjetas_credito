using Application.Common.Models;
using Application.TarjetasCredito.DatosClienteTc;
using Application.TarjetasCredito.InformacionEconomica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IInfoFinDat
{
    Task<RespuestaTransaccion> get_informacion_economica(ReqGetInfEco request);
}

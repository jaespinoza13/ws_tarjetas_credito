using Application.Common.Models;
using Application.TarjetasCredito.ComentariosAsesor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IInformesTarjetasCreditoDat
{
    Task<RespuestaTransaccion> AddInforme(ReqAddInforme request);
    Task<RespuestaTransaccion> GetInforme(ReqGetInforme request);

}

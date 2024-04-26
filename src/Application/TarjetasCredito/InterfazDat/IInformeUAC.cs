using Application.Common.Models;
using Application.TarjetasCredito.ComentariosAsesor;
using Application.TarjetasCredito.InformeUAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IInformeUAC
{
    Task<RespuestaTransaccion> AddInformeUAC(ReqAddInformeUAC request);
    Task<RespuestaTransaccion> GetInformeUAC(ReqGetInformeUAC request);
}

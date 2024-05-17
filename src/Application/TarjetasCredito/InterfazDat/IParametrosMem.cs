using Application.Common.Models;
using Application.Parametros;
using Application.TarjetasCredito.ComentariosAsesor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IParametrosMem
{
    Task<RespuestaTransaccion> get_parametros(ReqGetParametros request);
}

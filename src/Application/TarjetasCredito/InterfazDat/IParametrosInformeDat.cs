using Application.Common.Models;
using Application.TarjetasCredito.ComentariosAsesor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IParametrosInformeDat
{
    Task<RespuestaTransaccion> get_parametros_informe(ReqGetInforme request);

}

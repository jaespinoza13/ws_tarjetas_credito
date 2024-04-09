using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IGarantiasConstituidasDat
{
    Task<RespuestaTransaccion> get_gar_cns_soc(string str_num_ente);
}

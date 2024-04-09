using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface ICreditosVigentesDat
{ 
    Task<RespuestaTransaccion> get_creditos_vigentes (string str_num_ente);

}

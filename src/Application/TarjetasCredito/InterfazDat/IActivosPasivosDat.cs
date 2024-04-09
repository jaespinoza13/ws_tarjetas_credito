using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Application.TarjetasCredito.InformacionAdicional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MemoryCache;

public interface IActivosPasivosDat
{
    Task<RespuestaTransaccion> get_activos_pasivos_socio(string str_num_ente);
}

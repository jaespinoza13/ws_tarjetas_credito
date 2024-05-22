using Application.Common.Models;
using Application.EntregaRecepcionTarjCred.GetOrdenesTarjCred;
using Application.TarjetasCredito.AgregarSolicitudTc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;

public interface IOrdenesTarjCredDat
{
    Task<RespuestaTransaccion> get_ordenes_tarj_cred(ReqGetOrdenesTC request);
}

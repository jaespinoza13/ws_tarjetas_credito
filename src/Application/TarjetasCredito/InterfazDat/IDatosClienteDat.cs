using Application.Common.Models;
using Application.TarjetasCredito.DatosClienteTc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat;
    public interface IDatosClienteDat
    {
        Task<ResGetDatosCliente> get_datos_cliente(ReqGetDatosCliente request);
    }


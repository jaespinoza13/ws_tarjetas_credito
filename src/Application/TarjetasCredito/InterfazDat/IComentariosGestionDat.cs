﻿using Application.Common.Models;
using Application.TarjetasCredito.ComentariosGestion;
using Application.TarjetasCredito.InformacionAdicional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IComentariosGestionDat
    {
        Task<ResGetComentGestion> get_coment_gest_sol(ReqGetComentGestion req);
    }
}



﻿using Application.Common.Models;

namespace Infrastructure.Common.Interfaces;

public interface IHttpService
{
    Task<string> solicitar_servicio(SolicitarServicio solicitarServicio);

    object solicitar_servicio_async(SolicitarServicio solicitarServicio);

}

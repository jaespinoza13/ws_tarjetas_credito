
using Application.Common.Interfaces;
using Application.Transacciones.InterfazDat;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.gRPC_Clients.Mongo;
using Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;
using Infrastructure.gRPC_Clients.Sybase;
using Infrastructure.gRPC_Clients.Sybase.Transacciones;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        //INTERFACES DE SERVICIOS
        services.AddSingleton<ILogs, LogsService>();
        services.AddSingleton<IMongoDat, LogsMongoDat>();
        services.AddTransient<IHttpService, HttpService>();
        services.AddTransient<ISessionControl, SessionControl.SessionControl>();
        services.AddSingleton<ISesionDat, SesionDat>();
        services.AddSingleton<IKeysDat, KeysDat>();


        //INTERFACES DE CASOS DE USO
        services.AddSingleton<ITransaccionesDat, TransaccionesDat>();

        // TARJETAS CRÉDITO
        services.AddSingleton<ITarjetasCreditoDat, TarjetasCreditoDat>();

        return services;
    }
}

using Application.Common.Interfaces;
using Application.Common.Interfaces.Dat;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.Common.Interfaces;
using Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;
using Infrastructure.gRPC_Clients.Sybase;
using Infrastructure.MemoryCache;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        //INTERFACES DE SERVICIOS
        services.AddSingleton<ILogs, LogsService>();
        //services.AddSingleton<IMongoDat, LogsMongoDat>();
        services.AddTransient<IHttpService, HttpService>();
        services.AddTransient<ISessionControl, SessionControl.SessionControl>();
        services.AddSingleton<ISesionDat, SesionDat>();
        services.AddSingleton<IKeysDat, KeysDat>();


        // TARJETAS CRÉDITO
        services.AddSingleton<ITarjetasCreditoDat, TarjetasCreditoDat>();
        services.AddSingleton<IParametros, ParametrosDat>();
        services.AddSingleton<IParametrosDat, ParametrosDat>();
        services.AddSingleton<IParametersInMemory, ParametersInMemory>();
        services.AddSingleton<ITarjetasCreditoDat, TarjetasCreditoDat>();
        services.AddSingleton<IFuncionalidades, FuncionalidadesDat>();
        services.AddSingleton<IFuncionalidadesDat, FuncionalidadesDat>();
        services.AddSingleton<IFuncionalidadesMemory, FuncionalidadesInMemory>();

        //Datos Cliente
        services.AddSingleton<IDatosClienteDat, DatosClienteDat>();

        return services;
    }
}
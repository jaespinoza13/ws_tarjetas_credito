
using Application.Common.Interfaces;
using Application.TarjetasCredito.InterfazDat;
using Infrastructure.gRPC_Clients.Mongo;
using Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;
using Infrastructure.gRPC_Clients.Sybase;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Common.Interfaces;

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

        //Datos Cliente
        services.AddSingleton<IDatosClienteDat, DatosClienteDat>();

        //Información Economica
        services.AddSingleton<IInfoFinDat, InformacionEconomicaDat>();

        //Situacion Financiera
        services.AddSingleton<ISitFinDat, SituacionFinancieraDat>();

        //Catalogo de Agencias
        services.AddSingleton<ICatalogoAgenciasDat, AgenciasDat>();

        return services;
    }
}

using Application.Common.Interfaces;
using Application.Common.Interfaces.Apis;
using Application.Common.Interfaces.Dat;
using Application.TarjetasCredito.ComentariosGestion;
using Application.TarjetasCredito.InterfazDat;
using Infraestructure.ExternalApis;
using Infraestructure.InternalApis;
using Infrastructure.Common.Interfaces;
using Infrastructure.gRPC_Clients.Postgres.TarjetasCredito;
using Infrastructure.gRPC_Clients.Sybase;
using Infrastructure.MemoryCache;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Dynamic;

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

        //Información Economica
        services.AddSingleton<IInfoFinDat, InformacionEconomicaDat>();

        //Situacion Financiera
        services.AddSingleton<ISitFinDat, SituacionFinancieraDat>();

        //Catalogo de Agencias
        services.AddSingleton<ICatalogoAgenciasDat, AgenciasDat>();

        //GestorDocumental
        services.AddSingleton<IWsGestorDocumental, wsGestorDocumental>();
        
        //Validaciones
        services.AddSingleton<IValidacionesBuro, ValidacionesBuro>();

        services.AddSingleton<IActivosPasivosDat, ActivosPasivosDat>();
        //Inyección de dependia de los Creditos Vigentes 
        services.AddSingleton<ICreditosVigentesDat, CreditosVigentesDat>();

        //Inyeccion de las garantias constitudas 
        services.AddSingleton<IGarantiasConstituidasDat,GarantiasConstitudasDat>();

        services.AddSingleton<GetInformacionAdicional>();

        services.AddSingleton<IComentariosGestionDat, GetComentariosGestion>();
        services.AddSingleton<IParametrosInformeDat, ComentariosAsesorDat>();
        services.AddSingleton<IComentarioAsesorDat, ComentariosAsesorTcDat>();
        return services;
    }
}
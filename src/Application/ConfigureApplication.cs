using Application.Common.Behaviours;
using Application.Common.ISO20022.Models;
using Application.TarjetasCredito.ComentariosGestion;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ConfigureApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //SERVICIOS
        services.AddValidatorsFromAssembly( Assembly.GetExecutingAssembly() );
        services.AddValidatorsFromAssemblyContaining<Header>();
        services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly( typeof( ConfigureApplication ).Assembly ) );
        services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( ValidationBehaviour<,> ) );
        return services;
    }
}
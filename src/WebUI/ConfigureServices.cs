using Application.Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using WebUI.Filters;
using wsMegomovil.Filters;

namespace WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // CUSTOMISE API EXCEPTIONS BEHAVIOUR
        services.AddControllersWithViews( options => options.Filters.Add<ApiExceptionFilterAttribute>() );
        services.AddValidatorsFromAssembly( Assembly.GetExecutingAssembly() );

        // CUSTOMISE DEFAULT API BEHAVIOUR
        services.Configure<ApiBehaviorOptions>( options => options.SuppressModelStateInvalidFilter = true );

        //CORS
        services.AddCors( options =>
        {
            options.AddPolicy( "CorsPolicy", policyBuilder => policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        } );

        //AUTHORIZATION
        var issuer = configuration.GetValue<string>( "issuer" );
        var publicKeyBytes = Convert.FromBase64String( configuration.GetValue<string>( "Key_token_pub" )! );
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo( publicKeyBytes, out _ );
        var keyRsa = new RsaSecurityKey( rsa );

        var securityKeyDecrypt =
            new SymmetricSecurityKey(
                Encoding.Default.GetBytes( configuration.GetValue<string>( "Key_encrypt_token" )! ) );


        services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
            .AddJwtBearer( opciones => opciones.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                IssuerSigningKey = keyRsa,
                TokenDecryptionKey = securityKeyDecrypt,
                ClockSkew = TimeSpan.Zero
            } );


        //SERVICES
        services.AddDataProtection();
        services.AddMemoryCache();
        services.AddOptions();


        //FILTERS
        services.AddTransient<CryptographyAesFilter>();
        services.AddTransient<ClaimControlFilter>();
        services.AddTransient<SessionControlFilter>();
        services.AddTransient<RequestFilter>();

        //SWAGGER
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen( c =>
        {
            c.SwaggerDoc( "v1", new OpenApiInfo
            {
                Title = "WebService Tarjetas Crédito",
                Version = "v1",
                Description = "Servicios Tarjetas Crédito",
            } );
            c.AddSecurityDefinition( "Authorization-Mego", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization-Mego",
                Type = SecuritySchemeType.ApiKey,
                Description = "Ingrese su clave de API aquí. Ejemplo: 'Auth-Mego 12345'"
            } );
            c.AddSecurityRequirement( new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization-Mego"
                        }
                    },
                    Array.Empty<string>()
                }
            } );
        } );

        //CONFIGURATIONS
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:DataBases" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:GrpcSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:ConfigMongodb" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:LogsPath" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:valida_peticiones_diarias" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:HttpSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:Endpoints" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:EndpointsAuth" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:ControlExcepciones" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:TemplatesPath" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:Variables" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:PermisosEstados" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:Auth" ) );

        return services;
    }
}
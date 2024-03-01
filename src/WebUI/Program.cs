using Application;
using Infrastructure;
using WebUI;
using WebUI.Middleware;
using static AccesoDatosGrpcAse.Neg.DAL;
using static AccesoDatosPostgresql.Neg.DALPostgreSql;


var builder = WebApplication.CreateBuilder( args );

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddWebUiServices( builder.Configuration );

var grpc = builder.Configuration.GetSection( "ApiSettings:GrpcSettings" );
var urlSybase = grpc.GetValue<string>( "client_grpc_sybase" );
var urlPostgres = grpc.GetValue<string>( "client_grpc_postgres" );

builder.Services.AddGrpcClient<DALClient>( o =>
{
    o.Address = new Uri( urlSybase! );
} ).ConfigureChannel( c =>
{
    c.HttpHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds( 20 ),
        KeepAlivePingTimeout = TimeSpan.FromSeconds( 60 ),
        EnableMultipleHttp2Connections = true
    };
} );

builder.Services.AddGrpcClient<DALPostgreSqlClient>( o => { o.Address = new Uri( urlPostgres! ); } ).ConfigureChannel( c =>
{
    c.HttpHandler = new SocketsHttpHandler
    {
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds( 20 ),
        KeepAlivePingTimeout = TimeSpan.FromSeconds( 60 ),
        EnableMultipleHttp2Connections = true
    };
} );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(
    c => { c.SwaggerEndpoint( "/swagger/v1/swagger.json", "Cuenta Linea COOPMEGO v1" ); }
);

app.UseCors( "CorsPolicy" );

app.UseAuthotizationMego();


app.UseAuthorization();

app.UseAuthentication();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
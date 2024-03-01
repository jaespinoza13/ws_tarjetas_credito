using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace wsMegomovil.Filters;

public class RequestFilter : IActionFilter
{
    private readonly IParametersInMemory _parameters;
    private readonly IFuncionalidadesMemory _funcionalidades;

    public RequestFilter(IParametersInMemory parameters, IOptionsMonitor<ApiSettings> options, IFuncionalidadesMemory funcionalidades)
    {
        this._parameters = parameters;
        this._funcionalidades = funcionalidades;
    }

    void IActionFilter.OnActionExecuting(ActionExecutingContext context)
    {
        ////VALIDACIÓN DE PARAMETROS
        _parameters.ValidaParametros();
        _funcionalidades.ValidaFuncionalidades();
    }

    void IActionFilter.OnActionExecuted(ActionExecutedContext context)
    {
        //No se realiza ningunaaccion
    }
}
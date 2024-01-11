using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ILogs
{
    Task SaveHeaderLogs(dynamic transaction, string str_operacion, string str_metodo, string str_clase);
    Task SaveResponseLogs(dynamic transaction, string str_operacion, string str_metodo, string str_clase);
    Task SaveExceptionLogs(dynamic transaction, string str_operacion, string str_metodo, string str_clase, Exception obj_error);
    Task SaveHttpErrorLogs(object obj_solicitud, string str_error, string str_id_transaccion);
    Task SaveErroresDb(dynamic transaction, string str_operacion, string str_metodo, string str_clase, Exception obj_error);
}
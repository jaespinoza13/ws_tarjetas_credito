namespace Application.Common.Models;

public class ApiSettings
{
    public string? DB_meg_megonline { get; set; }
    public string? DB_meg_atms { get; set; }
    public string? DB_meg_servicios { get; set; }
    public string? DB_meg_sistemas { get; set; }
    public string? DB_meg_buro { get; set; }
    public string? DB_meg_tarjetas_credito { get; set; }

    //Agregados
    public string url_logs { get; set; } = string.Empty;

    public string auth_logs { get; set; } = string.Empty;

    public string? client_grpc_sybase { get; set; }
    public string? client_grpc_mongo { get; set; }
    public string? client_grpc_postgres { get; set; }
    public int timeoutGrpcSybase { get; set; } = 10;
    public int delayOutGrpcSybase { get; set; } = 10;
    public int timeoutGrpcMongo { get; set; } = 5;
    public int delayOutGrpcMongo { get; set; } = 5;


    public string nombre_base_mongo { get; set; } = string.Empty;
    public string coll_peticiones { get; set; } = string.Empty;
    public string coll_errores { get; set; } = string.Empty;
    public string coll_amenazas { get; set; } = string.Empty;
    public string coll_respuesta { get; set; } = string.Empty;
    public string coll_peticiones_diarias { get; set; } = string.Empty;
    public string coll_promedio_peticiones_diarias { get; set; } = string.Empty;
    public string coll_errores_db { get; set; } = string.Empty;
    public string coll_errores_http { get; set; } = string.Empty;


    public string logs_path_peticiones { get; set; } = string.Empty;
    public string logs_path_errores { get; set; } = string.Empty;
    public string logs_path_errores_db { get; set; } = string.Empty;
    public string logs_path_amenazas { get; set; } = string.Empty;
    public string logs_path_errores_http { get; set; } = string.Empty;

    public List<string> lst_nombres_parametros { get; set; } = new();
    public List<string> lst_canales_encriptar { get; set; } = new();

    public bool valida_peticiones_diarias { get; set; }
    public int timeOutHttp { get; set; }

    public string auth_ws_tarjetas_credito { get; set; } = string.Empty;
    public string auth_ws_otp { get; set; } = string.Empty;

    public int mostrar_descripcion_badrequest { get; set; }

    #region Variables 

    public string estado_creado { get; set; } = string.Empty;
    public string estado_entregado { get; set; } = string.Empty;
    public string estado_analisis_gestor { get; set; } = string.Empty;
    public string estado_anulado { get; set; } = string.Empty;
    public int int_id_sistema { get; set; }
    public int fun_tipo_accion { get; set; }
    public string rango_tc_clasica { get; set; } = string.Empty;
    public string rango_tc_black { get; set; } = string.Empty;
    public string rango_tc_gold { get; set; } = string.Empty;
    public string tarjeta_gold { get; set; } = string.Empty;
    public string tarjeta_black { get; set; } = string.Empty;
    public string tarjeta_clasica { get; set; } = string.Empty;
    #endregion

    #region PermisosEstados
    public string enviarUAC { get; set; } = string.Empty;
    public List<string> permisosVisualizacion { get; set; } = new();
    public List<string> permisosAccion { get; set; } = new();
    public List<string> estadosSolTC { get; set; } = new();
    #endregion
}

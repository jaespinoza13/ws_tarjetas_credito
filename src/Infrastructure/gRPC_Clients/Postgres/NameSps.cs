namespace Infrastructure.gRPC_Clients.Postgres
{
    public static class NameSps
    {
        public const string addSolicitudTC = "add_solicitud_tc";
        public const string getSolicitudesTc = "get_solicitudes_tc";
        public const string getProspectosTc = "get_prospectos_tc";
        public const string getParametros = "get_parametros_tc";
        public const string getFuncPermisos = "get_func_permisos";
        public const string addComentarioProceso = "add_comentario_proceso";
        public const string getFlujoSolicitud = "get_flujo_solicitud";
        public const string updSolicitudTC = "upd_solicitud_tc";
        public const string addProspeccionTC = "add_prospeccion_tc";
        public const string getCatalogoAgencias = "get_catalogo_agencias_tc";
        public const string getInfCliente = "get_inf_cliente_tc";
        public const string getIngEgrSoc = "get_ing_egr_soc_tc";
        public const string getSitFinSoc = "get_sit_fin_soc_tc";
        public const string getActPasSoc = "get_act_pas_soc_tc";
        public const string getCredVigSoc = "get_cred_vig_cob_tc";
        public const string getGarConsSoc = "get_garantias_constituidas_tc";
        public const string getParametrosInformeTc = "get_parametros_informe_tc";
        public const string updComentariosAsesorTc = "upd_comentarios_asesor_tc";
        public const string getComentariosAsesorTc = "get_comentariosf_tc";
        public const string getAnalistasCredito = "get_analistas_tc";
        public const string addAnalistaSolicitud = "add_analista_solicitud";
        public const string addInformeTc = "add_informe_tc";
        public const string getInformesTC = "get_informes_tc";
        public const string getResolicionesTC = "get_resoluciones_solicitud";
        public const string addResolicionesTC = "add_resolucion_solicitud";
        public const string updResolicionesTC = "update_resolucion_solicitud";
        //Variable para obtener los resultado de TC en proceso 
        public const string getSolicitudEnProceso = "get_solicitud_en_proceso_2";

        //Se agregan varibales para duplicar SP's
        public const string addComentarioProceso_2 = "add_comentario_proceso_2";
        public const string getFlujoSolicitud_2 = "get_flujo_solicitud_2";
        public const string addSolicitudTC_2 = "add_solicitud_tc_2";

        //Ordenes Tarjetas Credito
        public const string getOrdenesTC = "get_ordenes_tc";
        public const string getTarjetasCredito = "get_tarjetas_credito";
    }
}

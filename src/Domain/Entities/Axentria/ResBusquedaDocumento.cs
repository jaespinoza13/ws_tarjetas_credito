namespace Domain.Entities.Axentria
{
    public class ResBusquedaDocumento
    {
        public string str_tipo_doc { get; set; } = string.Empty;
        public string str_name_doc { get; set; } = string.Empty;
        public string str_version { get; set; } = string.Empty;
        public DateTime dt_ult_mod { get; set; }
        public string str_id_solicitud { get; set; } = string.Empty;
        public string str_login_carga { get; set; } = string.Empty;
        public string str_codigo_documento { get; set; } = string.Empty;
    }
}

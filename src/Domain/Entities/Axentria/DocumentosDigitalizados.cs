namespace Domain.Entities.Axentria
{
    public class DocumentosDigitalizados
    {
        public string str_id_doc { get; set; } = string.Empty;
        public string str_tipo_actor { get; set; } = string.Empty;
        public string str_cod_escanear { get; set; } = string.Empty;
        public string str_identificacion { get; set; } = string.Empty;
        public int int_id_tipo { get; set; }
        public string str_tipo_doc { get; set; } = string.Empty;
        public string str_usu_carga { get; set; } = string.Empty;
        public string str_fecha_carga { get; set; } = string.Empty;
        public string str_nombres { get; set; } = string.Empty;
        public int int_ente { get; set; }
    }
}

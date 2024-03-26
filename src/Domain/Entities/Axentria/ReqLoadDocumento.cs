namespace Domain.Entities.Axentria
{
    public class ReqLoadDocumento
    {
        public ImportarArchivo loadfile { get; set; }
        public long lng_id_carpeta { get; set; }
        public int int_solicitud { get; set; }
        public string str_num_identifica { get; set; } = string.Empty;
        public int int_tipo_ide { get; set; }
        public string str_tipo_doc { get; set; } = string.Empty;
        public int int_sistema { get; set; }
        public string str_usuario { get; set; } = string.Empty;
        public int int_perfil { get; set; }
        public int int_oficina { get; set; }
        public int int_canal { get; set; }
        public string str_nombre_canal { get; set; } = string.Empty;
        public string str_ciudad { get; set; } = string.Empty;

        public ReqLoadDocumento() { }
    }
}

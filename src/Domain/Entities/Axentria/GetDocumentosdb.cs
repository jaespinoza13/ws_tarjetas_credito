namespace Domain.Entities.Axentria
{
    public class GetDocumentosdb
    {

        public string codigo_alfresco { get; set; } = string.Empty;
        public string referencia { get; set; } = string.Empty;
        public int ente { get; set; }
        public string tipo_documento { get; set; } = string.Empty;
        public int sistema { get; set; }
        public string login_carga { get; set; } = string.Empty;
        public DateTime fecha_carga { get; set; }
        public int estado { get; set; }
        public string num_identifica { get; set; } = string.Empty;
        public int tipo_identifica { get; set; }
        public int admin_doc { get; set; }

    }
}

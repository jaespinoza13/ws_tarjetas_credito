
namespace Domain.Entities.Axentria
{
    public class ReqDocumentosSolicitud
    {
        public int int_id_solicitud { get; set; }
        public string str_ced { get; set; } = string.Empty;
        public int int_tipo_ide { get; set; }
        public string str_tipo_doc { get; set; } = string.Empty;
        public string str_filter_user { get; set; } = string.Empty;
        public int int_num_filas { get; set; }
        public int int_actual { get; set; }
        public string str_ente { get; set; } = string.Empty;

        public ReqDocumentosSolicitud() { }
    }
}

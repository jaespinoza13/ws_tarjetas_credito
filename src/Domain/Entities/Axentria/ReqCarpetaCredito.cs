namespace Domain.Entities.Axentria
{
    public class ReqCarpetaCredito
    {
        public string str_cedula_socio { get; set; } = string.Empty;
        public string str_path_root { get; set; } = string.Empty;
        public int int_id_solicitud { get; set; }
        public string str_name_folder { get; set; } = string.Empty;
        public ReqCarpetaCredito() { }
    }
}

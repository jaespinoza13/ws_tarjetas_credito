namespace Domain.Entities.Axentria
{
    public class GarantiasSolicitud
    {
        public string gre_cod_cobis { get; set; } = string.Empty;
        public int gre_ente_propietario { get; set; }
        public string gre_nombre_prop { get; set; } = string.Empty;
        public string gre_descripcion { get; set; } = string.Empty;
        public string gre_tipo { get; set; } = string.Empty;
        public decimal gre_avaluo { get; set; }
        public string gre_fecha_avaluo { get; set; } = string.Empty;
        public decimal gre_cobertura { get; set; }
        public string gre_estado { get; set; } = string.Empty;
        // public decimal gre_valor_actual { get; set; } = 0;
    }
}

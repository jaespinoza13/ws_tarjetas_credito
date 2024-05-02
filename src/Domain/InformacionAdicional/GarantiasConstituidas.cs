namespace Domain.InformacionAdicional;

public class GarantiasConstituidas
{
    public string str_detalle_garantia { get; set; } = String.Empty;
    public Decimal dcm_patrimonio { get; set; }
    public Decimal dcm_avaluo { get; set; }
    public Decimal dcm_ahorro_neto { get; set; }
    public DateTime dtt_fecha_avaluo { get; set; }
}

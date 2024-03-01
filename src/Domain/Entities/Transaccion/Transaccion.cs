namespace Domain.Entities.Transaccion;


public class Transaccion
{
    public int int_id { get; set; }
    public string dtt_fecha_pago { get; set; } = string.Empty;
    public string str_convenio { get; set; } = string.Empty;
    public string str_servicio { get; set; } = string.Empty;
    public string str_num_doc_cont { get; set; } = string.Empty;
    public string str_nom_contribuyente { get; set; } = string.Empty;
    public string mon_valor { get; set; } = string.Empty;
    public string str_estado { get; set; } = string.Empty;
    public string str_origen { get; set; } = string.Empty;

}

public class EstadoPago
{

    public int int_estado { get; set; }
    public string str_nombre { get; set; } = string.Empty;

}

public class PagosError
{

    public int int_pago_err { get; set; }

}
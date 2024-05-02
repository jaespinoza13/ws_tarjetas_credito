namespace Domain.Entities.ComentariosAsesorCredito;

public class Informes
{
    public int int_id_parametro { get; set; }
    public string str_tipo { get; set; } = string.Empty;
    public string str_descripcion { get; set; } = string.Empty;
    public string str_detalle { get; set; } = string.Empty;


    public class ResInformes
    {
        public string json_res_inf { get; set; } = String.Empty;
    }
}

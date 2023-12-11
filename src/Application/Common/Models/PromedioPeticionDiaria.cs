namespace Application.Common.Models;

public class PromedioPeticionDiaria
{
    public string str_id { get; set; } = string.Empty;
    public double dbl_promedio_peticion { get; set; }
    public string str_operacion { get; set; } = string.Empty;
    public string str_fecha_actualizacion { get; set; } = string.Empty;
}
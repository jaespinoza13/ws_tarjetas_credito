namespace Domain.Entities.Informacion_Financiera
{
    public class Egresos
    {
        public int int_codigo { get; set; }
        public string? str_descripcion { get; set; } = string.Empty;
        public Decimal dcm_valor { get; set; }
    }
}

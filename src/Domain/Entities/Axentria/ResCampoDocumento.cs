namespace Domain.Entities.Axentria
{
    public class ResCampoDocumento
    {
        public List<CamposDb> campos { get; set; } = new List<CamposDb>();
        public int int_total_campos { get; set; }
    }
}

namespace Domain.Entities.Axentria
{
    public class Campo
    {
        public int Id { get; set; }
        public int IdMC { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Tipo { get; set; }
        public int TipoPicklist { get; set; }
    }

    public class CamposTodos
    {
        public List<int> ids { get; set; } = new List<int>();
    }

    public class Indice
    {
        public int int_id { get; set; }
        public string str_nombre { get; set; } = string.Empty;
        public int int_tipo { get; set; }
    }
}

namespace Domain.Entities.Axentria
{
    public class ActorSolicitud
    {
        public string Tipo { get; set; } = string.Empty;
        public string TipoId { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public int Ente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdCliente { get; set; }
        public string dve_opc_sexo { get; set; } = string.Empty;
    }
}

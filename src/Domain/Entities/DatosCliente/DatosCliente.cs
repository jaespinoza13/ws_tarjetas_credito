namespace Domain.Entities.DatosCliente
{
    public class DatosCliente
    {
        public int ente { get; set; }
        public string? nombres { get; set; } = string.Empty;
        public string? apellido_paterno { get; set; } = string.Empty;
        public string? apellido_materno { get; set; } = string.Empty;
        public string? fecha_nacimiento { get; set; } = string.Empty;
        public string? nivel_educacion { get; set; } = string.Empty;
        public string? codigo_profesion { get; set; } = string.Empty;
        public string? actividad_economica { get; set; } = string.Empty;
        public string? ocupacion { get; set; } = string.Empty;
        public string? estado_civil { get; set; } = string.Empty;
        public string? nacionalidad { get; set; } = string.Empty;
        public string? sexo { get; set; } = string.Empty;
        public string? sector { get; set; } = string.Empty;
        public string? subsector { get; set; } = string.Empty;
        public string? tipo_persona { get; set; } = string.Empty;
        public string? medio_informacion { get; set; } = string.Empty;
        public string? calificacion_riesgo { get; set; } = string.Empty;
    }
}

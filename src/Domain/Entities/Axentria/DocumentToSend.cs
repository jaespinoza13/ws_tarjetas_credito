namespace Domain.Entities.Axentria
{
    public class DocumentoToSend
    {
        public List<ImportarArchivoCampo> fields { get; set; } = new List<ImportarArchivoCampo>();
        public string str_path_file { get; set; } = string.Empty;
        public List<PropertyDocument> properties { get; set; } = new List<PropertyDocument>();
        public string str_name_group { get; set; } = string.Empty;
    }
}

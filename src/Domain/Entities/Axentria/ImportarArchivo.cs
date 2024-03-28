namespace Domain.Entities.Axentria
{
    public class ImportarArchivo
    {
        public byte[] file { get; set; } = new byte[0];
        public List<ImportarArchivoCampo> fields { get; set; } = new List<ImportarArchivoCampo>();
        public List<PropertyDocument> properties { get; set; } = new List<PropertyDocument>();
        public object policies { get; set; } = new object();
    }

    public class ImportarArchivoCampo
    {
        public int DocumentTypeId { get; set; }
        public int DocumentId { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string DataValue { get; set; } = string.Empty;
        public int FieldType { get; set; }
    }

    public class PropertyDocument
    {
        public string Id { get; set; } = string.Empty;
        public object LocalName { get; set; } = new object();
        public object LocalNamespace { get; set; } = new object();
        public string DisplayName { get; set; } = string.Empty;
        public object QueryName { get; set; } = new object();
        public object Description { get; set; } = new object();
        public bool Queryable { get; set; }
        public object PropertyTypeName { get; set; } = new object();
        public bool Inherited { get; set; }
        public bool Required { get; set; }
        public bool Orderable { get; set; }
        public bool OpenChoice { get; set; }
        public List<object> DefaultValue { get; set; } = new List<object>();
    }
}

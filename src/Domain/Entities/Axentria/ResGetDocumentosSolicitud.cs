namespace Domain.Entities.Axentria
{
    public class ResGetDocumentosSolicitud
    {
        public List<DocumentosDigitalizados> documentos { get; set; } = new List<DocumentosDigitalizados>();
        public int int_paginas { get; set; }
        public int int_actual { get; set; }
        public List<DocumentoSeparadores> separadores { get; set; } = new List<DocumentoSeparadores>();
    }
}

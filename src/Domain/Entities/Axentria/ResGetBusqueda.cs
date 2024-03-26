namespace Domain.Entities.Axentria
{
    public class ResGetBusqueda
    {
        public List<ResBusquedaDocumento> documentos { get; set; } = new List<ResBusquedaDocumento>();
        public int int_paginas { get; set; }
        public int int_actual { get; set; }
    }
}

namespace Application.Common.Models;

public class SolicitudTransaccion
{
    public Cabecera cabecera { get; set; }
    public object obj_cuerpo { get; set; }

    public SolicitudTransaccion(Cabecera cabecera, object obj_cuerpo)
    {
        this.cabecera = cabecera;
        this.obj_cuerpo = obj_cuerpo;
    }
}
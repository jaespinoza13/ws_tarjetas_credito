namespace Application.Common.Models;

public class RespuestaTransaccion
{
    public object obj_cuerpo { get; set; } = new();//12
    public string str_cuerpo { get; set; } 
    public string str_codigo { get; set; } = string.Empty;//27

    public Dictionary<string, string> diccionario { get; set; } = new();//23
}
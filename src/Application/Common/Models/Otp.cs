﻿namespace Application.Common.Models;

public class Otp
{
    public string str_nombre_socio { get; set; } = string.Empty;
    public string str_correo { get; set; } = string.Empty;
    public string str_celular { get; set; } = string.Empty;
    public int int_longitud_otp { get; set; }
    public int int_intentos_fallidos_otp { get; set; }
    public int int_tiempo_duracion_otp { get; set; }
    public int int_tiempo_regenerar_otp { get; set; }
}
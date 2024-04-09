using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.InformacionAdicional;

public class CreditosVigentes
{
    public string str_tipo_deuda {  get; set; } = String.Empty;
    public string str_linea_credito {  get; set; } = String.Empty; 
    public string str_operacion { get; set; } = String.Empty;
    public Decimal dcm_saldo { get; set; }
    public string str_estado { get; set; } = String.Empty;
    public int int_dias_mora { get; set; }
    public Decimal dcm_prom_dias_mora { get; set; }
    public char chr_calificacion { get; set; }
    public string str_nombre_producto { get; set; } = String.Empty;
    public Decimal dcm_monto { get; set; }
    public int int_mora_maxima { get; set; }
    public DateTime dtt_fecha_vencimiento { get; set; }
    public int int_plazo { get; set; }
    public string str_tipo_garantia {  get; set; } = String.Empty;
    public Decimal dcm_avaluo_garantia { get; set; }
    public int int_operacionT {  get; set; }
    public int int_ente_titular { get; set; }
    public int int_nOrden { get; set; }
    public string str_id_cartera {  get; set; } = String.Empty;

}

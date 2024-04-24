using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Memoria_Cache;

public class InformacionMemoriaSol
{
    //Se agrega campos json 
    public string str_act_soc_json { get; set; } = string.Empty;
    public string str_pas_soc_json { get; set; } = string.Empty;
    public string str_dpfs_json { get; set; } = string.Empty;
    public string str_cred_hist_json { get; set; } = string.Empty;
    public string str_ingr_soc_json { get; set; } = string.Empty;
    public string str_egr_soc_json { get; set; } = string.Empty;
    public string str_cred_vig_json { get; set; } = string.Empty;
    public string str_gar_cns_json { get; set; } = string.Empty;
}

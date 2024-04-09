
using Domain.Parameters;

namespace Application.Common.Interfaces.Dat;
public interface IParametersInMemory
{
    void ValidaParametros();
    void LoadConfiguration();
    List<Parametro> FindParametros(string str_tipo_parametro);
    Parametro FindParametroNemonico(string str_nemonico);
    Parametro FindParametroValorFin(string str_valor_fin);
    Parametro FindParametroId(int int_id_param);
    string getMensajeProceso(string str_codigo, string str_mensaje = "");

}
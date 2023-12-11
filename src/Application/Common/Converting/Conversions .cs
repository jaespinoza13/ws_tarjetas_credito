using Application.Common.Models;

namespace Application.Common.Converting;

public static class Conversions
{
    #region Métodos "Conversión de Conjunto de Datos a un objeto/Lista de una Clase"

    /// <summary>
    /// Convierte un Conjunto de datos a una lista de una Clase específica
    /// </summary>
    /// <param name="conjuntoDatos"></param>
    /// <returns></returns>
    public static List<T> ConvertConjuntoDatosToListClass<T>(ConjuntoDatos conjuntoDatos)
    {
        return conjuntoDatos.LstTablas[0].LstFilas
            .Select( item => (T)Converting.MapDictToObj( item.NombreValor, typeof(T) ) ).ToList();
    }


    /// <summary>
    /// Convierte un Conjunto de datos a un objeto de una Clase específica
    /// </summary>
    /// <param name="conjuntoDatos"></param>
    /// <returns></returns>
    public static T ConvertConjuntoDatosToClass<T>(ConjuntoDatos conjuntoDatos)
    {
        var obj = default(T);
        foreach (var item in conjuntoDatos.LstTablas[0].LstFilas)
        {
            obj = (T)Converting.MapDictToObj( item.NombreValor, typeof(T) );
        }

        return obj!;
    }

    public static T ConvertConjuntoDatosToClass<T>(ConjuntoDatos conjuntoDatos, int posicion)
    {
        var obj = default(T);
        foreach (var item in conjuntoDatos.LstTablas[posicion].LstFilas)
        {
            obj = (T)Converting.MapDictToObj( item.NombreValor, typeof(T) );
        }

        return obj!;
    }

    /// <summary>
    /// Convierte un Conjunto de datos a una lista de una Clase específica se puede enviar la tabla(0,1..) a convertir
    /// </summary>
    /// <param name="conjuntoDatos"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public static IEnumerable<T> ConvertToListClassDynamic<T>(ConjuntoDatos conjuntoDatos, int table) =>
        conjuntoDatos.LstTablas[table].LstFilas
            .Select( item => (T)Converting.MapDictToObj( item.NombreValor, typeof(T) ) ).ToList();

    #endregion
}
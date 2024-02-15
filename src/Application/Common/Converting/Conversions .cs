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
    public static List<T> ConvertConjuntoDatosToListClassPos0<T>(ConjuntoDatos conjuntoDatos)
    {
        return conjuntoDatos.lst_tablas[0].lst_filas
            .Select( item => (T)Converting.MapDictToObj( item.nombre_valor, typeof(T) ) ).ToList();
    }
    public static List<T> ConvertConjuntoDatosToListClassPos1<T>(ConjuntoDatos conjuntoDatos)
    {
        return conjuntoDatos.lst_tablas[1].lst_filas
            .Select( item => (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) ) ).ToList();
    }
    public static List<T> ConvertConjuntoDatosToListClassPos2<T>(ConjuntoDatos conjuntoDatos)
    {
        return conjuntoDatos.lst_tablas[2].lst_filas
            .Select( item => (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) ) ).ToList();
    }


    /// <summary>
    /// Convierte un Conjunto de datos a un objeto de una Clase específica
    /// </summary>
    /// <param name="conjuntoDatos"></param>
    /// <returns></returns>
    public static T ConvertConjuntoDatosToClass<T>(ConjuntoDatos conjuntoDatos)
    {
        var obj = default(T);
        foreach (var item in conjuntoDatos.lst_tablas[0].lst_filas)
        {
            obj = (T)Converting.MapDictToObj( item.nombre_valor, typeof(T) );
        }

        return obj!;
    }

    public static T ConvertConjuntoDatosToClass<T>(ConjuntoDatos conjuntoDatos, int posicion)
    {
        var obj = default(T);
        foreach (var item in conjuntoDatos.lst_tablas[posicion].lst_filas)
        {
            obj = (T)Converting.MapDictToObj( item.nombre_valor, typeof(T) );
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
        conjuntoDatos.lst_tablas[table].lst_filas
            .Select( item => (T)Converting.MapDictToObj( item.nombre_valor, typeof(T) ) ).ToList();

    #endregion
}
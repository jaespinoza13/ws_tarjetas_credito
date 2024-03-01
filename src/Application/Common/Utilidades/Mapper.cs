using Application.Common.Models;

namespace Application.Common.Utilidades
{
    public static class Mapper
    {

        #region Métodos "Conversión de Conjunto de Datos a un objeto/Lista de una Clase"
        /// <summary>
        /// Convierte un Conjunto de datos a una lista de una Clase específica
        /// </summary>
        /// <param name="cuerpo"></param>
        /// <returns></returns>
        public static List<T> ConvertConjuntoDatosToListClass<T>(object objData, int int_posicion = 0)
        {
            List<T> lst_array = new();
            var conjuntoDatos = (ConjuntoDatos)objData;

            foreach (var item in conjuntoDatos.lst_tablas[int_posicion].lst_filas!)
            {
                T obj = (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) );
                lst_array.Add( obj );
            }

            return lst_array;
        }

        public static T ConvertConjuntoDatosToClass<T>(object objData, int int_posicion = 0)
        {

            T? obj = default( T );
            var conjuntoDatos = (ConjuntoDatos)objData;

            foreach (var item in conjuntoDatos.lst_tablas[int_posicion].lst_filas!)
            {
                obj = (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) );
            }

            return obj!;
        }

        public static dynamic getValueCampoClass(object objData, string str_nom_campo, int int_tabla = 0)
        {

            var conjuntoDatos = (ConjuntoDatos)objData;

            var valor_campo = conjuntoDatos.lst_tablas[int_tabla].lst_filas[0];

            return valor_campo.nombre_valor[str_nom_campo];
        }

        /// <summary>
        /// Convierte un Conjunto de datos a una lista de una Clase específica se puede enviar la tabla(0,1..) a convertir
        /// </summary>
        /// <param name="cuerpo"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConvertConjuntoDatosToListClassDynamic<T>(ConjuntoDatos cuerpo, int table)
        {
            return cuerpo.lst_tablas[table].lst_filas!.Select( item => (T)Converting.MapDictToObj( item.nombre_valor, typeof( T ) ) ).ToList();
        }
        #endregion
    }
}

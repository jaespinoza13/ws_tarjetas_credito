using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using System.Runtime;

namespace Application.Common.Utilidades
{
    public static class Validaciones
    {
        public static (int, string) obtener_nuevo_estado_proceso(int int_estado, IParametersInMemory parametersInMemory, bool bl_regresa)
        {
            int estado_nuevo = 0;
            string estado = "";

            string nem_estado_actual = parametersInMemory.FindParametroId( int_estado ).str_nemonico;
            int estado_actual = Convert.ToInt32( parametersInMemory.FindParametroId( int_estado ).str_valor_fin );
            estado_actual = bl_regresa == true ? estado_actual - 1 : estado_actual + 1;
            if (estado_actual > 0)
            {
                estado = parametersInMemory.nuevoEstado( estado_actual.ToString() ).str_nemonico;

                estado_nuevo = parametersInMemory.FindParametroNemonico( estado ).int_id_parametro;
            }
            //Proceso para saber si el perfil tiene acceso a ese permiso
            return (estado_nuevo, estado);
        }

        public static bool ValidarEstado(string estado, ApiSettings _settings, IFuncionalidadesInMemory _funcionalidadesMemory, string perfil)
        {
            bool permiso = false;
            string func_nombre = "";
            int int_funcionalidad;

            for (int i = 0; i < _settings.permisosAccion.Count; i++)
            {
                if (estado == _settings.estadosSolTC[i]) func_nombre = _settings.permisosAccion[i];

                if (func_nombre != "")
                {
                    int_funcionalidad = _funcionalidadesMemory.FindFuncionalidadNombre( func_nombre ).fun_id;

                    if (_funcionalidadesMemory.FindPermisoPerfil( Convert.ToInt32( perfil ), int_funcionalidad ))
                    {
                        permiso = true;
                        break;
                    }
                }
            }
            return permiso;
        }
    }
}

using Domain.Funcionalidades;

namespace Application.Common.Interfaces.Dat
{
    public interface IFuncionalidadesInMemory
    {
        void ValidaFuncionalidades();
        void LoadFuncionalidades();
        bool FindPermisoPerfil(int int_perfil, int funcionalidad);
        Funcionalidad FindFuncionalidadNombre(string str_nombre_func);
    }
}

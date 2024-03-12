using Domain.Funcionalidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Dat
{
    public interface IFuncionalidadesMemory
    {
        void ValidaFuncionalidades();
        void LoadFuncionalidades();
        bool FindPermisoPerfil(int int_perfil, int funcionalidad);
        Funcionalidad FindFuncionalidadNombre(string str_nombre_func);
    }
}

using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Dat
{
    public interface IFuncionalidades
    {
        Task<RespuestaTransaccion> getFuncionalidades(int int_id_sistema, int int_tipo_funcionalidad = -1, int int_id_perfil = -1);
    }
}

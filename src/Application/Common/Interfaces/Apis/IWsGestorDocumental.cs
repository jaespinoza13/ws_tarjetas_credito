using Application.Common.Models;
using Domain.Entities.Axentria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Apis
{
    public interface IWsGestorDocumental
    {
        string addDocumento(ReqLoadDocumento reqLoadDocumento, string str_id_transaccion);
    }
}

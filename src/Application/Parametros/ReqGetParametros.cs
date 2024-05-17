using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parametros;

public class ReqGetParametros : ResGetParametros, IRequest<ResGetParametros>
{
    public int int_id_sis {  get; set; }
}

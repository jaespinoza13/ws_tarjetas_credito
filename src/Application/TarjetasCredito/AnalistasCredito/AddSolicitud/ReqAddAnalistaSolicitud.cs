using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TarjetasCredito.AnalistasCredito.AddSolicitud
{
    public class ReqAddAnalistaSolicitud : Header, IRequest<ResAddAnalistaSolicitud>
    {
        public string str_id_analista {  get; set; } = string.Empty;
        public string str_analista {  get; set; } = string.Empty;
        public int int_id_solicitud { get; set; }
    }
}

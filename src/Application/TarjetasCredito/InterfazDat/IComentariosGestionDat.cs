using Application.TarjetasCredito.ComentariosGestion;

namespace Application.TarjetasCredito.InterfazDat
{
    public interface IComentariosGestionDat
    {
        Task<ResGetComentGestion> get_coment_gest_sol(ReqGetComentGestion req);
    }
}



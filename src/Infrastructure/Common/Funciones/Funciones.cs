using AccesoDatosPostgresql.Neg;
using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Infrastructure.Common.Funciones;

public static class Funciones
{
    public static ConjuntoDatos ObtenerDatos(DatosRespuesta resultado)
    {
        var conjuntoDatos = new ConjuntoDatos();
        var lstTablas = new List<Tabla>();
        foreach (var t in resultado.ListaTablas)
        {
            var lstFilas = new List<Application.Common.Models.Fila>();
            foreach (var t1 in t.ListaFilas)
            {
                Application.Common.Models.Fila fila = new();

                foreach (var t2 in t1.ListaColumnas)
                {
                    fila.NombreValor.Add( t2.NombreCampo, t2.Valor );
                }

                lstFilas.Add( new Application.Common.Models.Fila { NombreValor = fila.NombreValor } );
            }

            lstTablas.Add( new Tabla { LstFilas = lstFilas } );
        }

        conjuntoDatos.LstTablas = lstTablas;

        return conjuntoDatos;
        //ConjuntoDatos cd = new();
        //var lst_tablas = new List<Tabla>();
        //for (int k = 0; k < resultado.ListaTablas.Count; k++)
        //{
        //    var lst_filas = new List<Application.Common.Models.Fila>();
        //    for (int i = 0; i < resultado.ListaTablas[k].ListaFilas.Count; i++)
        //    {
        //        Application.Common.Models.Fila fila = new();

        //        for (int j = 0; j < resultado.ListaTablas[k].ListaFilas[i].ListaColumnas.Count; j++)
        //        {

        //            fila.NombreValor.Add( resultado.ListaTablas[k].ListaFilas[i].ListaColumnas[j].NombreCampo, resultado.ListaTablas[k].ListaFilas[i].ListaColumnas[j].Valor );

        //        }
        //        lst_filas.Add( new Application.Common.Models.Fila { NombreValor = fila.NombreValor } );
        //    }
        //    lst_tablas.Add( new Tabla { LstFilas = lst_filas } );
        //}
        //cd.LstTablas = lst_tablas;
        //return cd;
    }

    public static void llenar_datos_auditoria_salida(DatosSolicitud ds, Header header)
    {
        // Parámtros de entrada de auditoría
      //  ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_sistema", TipoDato = TipoDato.Integer, ObjValue = header.str_id_sistema } );
      //  ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_login", TipoDato = TipoDato.CharacterVarying, ObjValue = header.str_login } );
      //  ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_oficina", TipoDato = TipoDato.Integer, ObjValue = header.str_id_oficina } );
      //  ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nemonico_canal", TipoDato = TipoDato.CharacterVarying, ObjValue = header.str_nemonico_canal } );
      //  ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_ip_dispositivo", TipoDato = TipoDato.CharacterVarying, ObjValue = header.str_ip_dispositivo } );
        // Parametros de salida
        //ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.CharacterVarying } );
        //ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );
    }
}
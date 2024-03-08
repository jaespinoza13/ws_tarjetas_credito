using Application.Common.Interfaces.Dat;
using Application.Common.Models;
using Application.Common.Utilidades;
using Domain.Funcionalidades;
using Domain.Parameters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MemoryCache
{
    internal class FuncionalidadesInMemory : IFuncionalidadesMemory
    {
        public readonly ApiSettings _settings;
        public readonly IFuncionalidadesDat _funcionalidadesDat;
        public readonly IMemoryCache _memoryCache;
        public DateTime dt_fecha_func;

        public FuncionalidadesInMemory(IOptionsMonitor<ApiSettings> options, IFuncionalidadesDat funcionalidadesDat, IMemoryCache memoryCache)
        {
            this._settings = options.CurrentValue;
            this._funcionalidadesDat = funcionalidadesDat;
            this._memoryCache = memoryCache;
        }

        public void LoadFuncionalidades()
        {
            try
            {
                var lst_funcionalidaes_accion = new List<Funcionalidad>();
                var lst_funcionalidades = new List<Funcionalidad>();

                RespuestaTransaccion resTran = _funcionalidadesDat.getFuncionalidades( Convert.ToInt32(_settings.int_id_sistema) ).Result;

                switch (resTran.codigo)
                {
                    case "000":
                        lst_funcionalidades = Mapper.ConvertConjuntoDatosToListClass<Funcionalidad>( resTran.cuerpo );
                        foreach (var item in lst_funcionalidades)
                        {
                            if (item.fun_tipo == _settings.fun_tipo_accion)
                                lst_funcionalidaes_accion.Add( item );
                        }


                        _memoryCache.Set( "funcionalidades", lst_funcionalidaes_accion );
                        break;
                    case "002":
                        lst_funcionalidaes_accion = new List<Funcionalidad>();
                        lst_funcionalidades = Mapper.ConvertConjuntoDatosToListClass<Funcionalidad>( resTran.cuerpo );
                        foreach (var item in lst_funcionalidades)
                        {
                            if (item.fun_tipo == _settings.fun_tipo_accion)
                                lst_funcionalidaes_accion.Add( item );
                        }

                        _memoryCache.Set( "funcionalidades", lst_funcionalidaes_accion );

                        var lst_permisos = Mapper.ConvertConjuntoDatosToListClass<PermisoPerfil>( resTran.cuerpo, 1 );
                        _memoryCache.Set( "permiso_perfil", lst_permisos );
                        break;
                    default:
                        throw new ArgumentException( "Sin funcionalidades" );
                }
                
                dt_fecha_func = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.Message );
            }
        }

        public void ValidaFuncionalidades()
        {
            if (DateTime.Compare( DateTime.Now, dt_fecha_func.AddDays( 1 ) ) > 0)
            {
                LoadFuncionalidades();
            }
        }

        public bool FindPermisoPerfil(int int_perfil, int funcionalidad)
        {
            bool bl_permiso = false;
            var lst_permisos = _memoryCache.Get<List<PermisoPerfil>>( "permiso_perfil" );

            var permiso_perfil = lst_permisos!.Find( x => x.prm_fk_perfil == int_perfil && x.prm_fk_funcionalidad == funcionalidad )!;

            return bl_permiso = permiso_perfil != null ? true : false;
        }

        public Funcionalidad FindFuncionalidadNombre(string str_nombre)
        {
            var lst_funcionalidades = _memoryCache.Get<List<Funcionalidad>>( "funcionalidades" );
            return lst_funcionalidades!.Find( x => x.fun_nombre == str_nombre )!;
        }
    }
}

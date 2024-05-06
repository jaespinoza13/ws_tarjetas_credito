using Application.Common.Interfaces.Apis;
using Application.Common.Models;
using Application.TarjetasCredito.AgregarSolicitudTc;
using Domain.Entities.Axentria;
using Infrastructure.Common.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infraestructure.InternalApis
{
    public class ValidacionesBuro : IValidacionesBuro
    {
        private readonly IHttpService _httpService;
        private readonly ApiSettings _config;

        public ValidacionesBuro(IOptionsMonitor<ApiSettings> options, IHttpService httpService)
        {
            this._httpService = httpService;
            this._config = options.CurrentValue;
        }

        //public async Task<RespuestaTransaccion> getVerificaConsulta(ReqConsultaBuro reqConsultaBuro)
        //{
        //    RespuestaTransaccion respuesta = new();
        //    Dictionary<string, object> _dic_headers = new();
        //    SolicitarServicio _sol_servicio = new();
        //    try
        //    {
        //        _dic_headers.Add("Authorization-Mego", _config.auth_validaciones_buro);

        //        _sol_servicio.urlServicio = _config.url_validaciones_buro + "CONSULTA_BURO";
        //        _sol_servicio.idTransaccion = reqConsultaBuro.str_id_transaccion!;
        //        _sol_servicio.objSolicitud = reqConsultaBuro;
        //        _sol_servicio.dcyHeadersAdicionales = _dic_headers;

        //        string str_result_srv = await _httpService.solicitar_servicio(_sol_servicio);

        //        dynamic result = JObject.Parse(str_result_srv)!;

        //        respuesta.str_codigo = result.str_res_codigo;
        //        respuesta.str_mensaje = result.str_res_info_adicional;
        //        respuesta.dcc_variables["str_url_contrato"] = result.str_url_contrato.ToString();
        //        respuesta.dcc_variables["int_cliente"] = result.int_cliente.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }
        //    return respuesta;
        //}

        //public async Task<RespuestaTransaccion> stpAddConsulta(ReqConsultaBuro reqConsultaBuro)
        //{
        //    RespuestaTransaccion respuesta = new();
        //    Dictionary<string, object> _dic_headers = new();
        //    SolicitarServicio _sol_servicio = new();
        //    reqConsultaBuro.str_estado = _config.estado_consultado;
        //    try
        //    {
        //        _dic_headers.Add("Authorization-Mego", _config.auth_validaciones_buro);

        //        _sol_servicio.urlServicio = _config.url_validaciones_buro + "ADD_LOG_CONSULTA";
        //        _sol_servicio.idTransaccion = reqConsultaBuro.str_id_transaccion!;
        //        _sol_servicio.objSolicitud = reqConsultaBuro;
        //        _sol_servicio.dcyHeadersAdicionales = _dic_headers;

        //        string str_result_srv = await _httpService.solicitar_servicio(_sol_servicio);

        //        var result = JsonSerializer.Deserialize<ResConsultaBuro>(str_result_srv)!;

        //        respuesta.str_codigo = result.str_res_codigo == "1" ? "000" : "001";
        //        respuesta.str_mensaje = result.str_res_info_adicional;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }
        //    return respuesta;
        //}

        //public async Task<RespuestaTransaccion> stpAddAutorizacion(ReqAddAutorizacion reqAddAutorizacion)
        //{
        //    RespuestaTransaccion respuesta = new();
        //    Dictionary<string, object> _dic_headers = new();
        //    SolicitarServicio _sol_servicio = new();
        //    try
        //    {
        //        _dic_headers.Add("Authorization-Mego", _config.auth_validaciones_buro);

        //        _sol_servicio.urlServicio = _config.url_validaciones_buro + "ADD_AUTORIZACION";
        //        _sol_servicio.idTransaccion = reqAddAutorizacion.str_id_transaccion!;
        //        _sol_servicio.objSolicitud = reqAddAutorizacion;
        //        _sol_servicio.dcyHeadersAdicionales = _dic_headers;

        //        string str_result_srv = await _httpService.solicitar_servicio(_sol_servicio);

        //        var result = JsonSerializer.Deserialize<ResConsultaBuro>(str_result_srv)!;

        //        respuesta.str_codigo = result.str_res_codigo == "1" ? "000" : "001";
        //        respuesta.str_mensaje = result.str_res_info_adicional;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException(ex.Message);
        //    }
        //    return respuesta;
        //}

        public RespuestaTransaccion addDocumento(string str_id_documento, ReqLoadDocumento reqLoadDocumento, string str_id_transaccion)
        {
            RespuestaTransaccion respuesta = new();
            Dictionary<string, object> _dic_headers = new();
            SolicitarServicio _sol_servicio = new();
            try
            {

                _dic_headers.Add( "Authorization-Mego", _config.auth_validaciones_buro );

                _sol_servicio.urlServicio = _config.url_validaciones_buro + "ADD_DOCUMENTO";
                _sol_servicio.idTransaccion = str_id_transaccion!;

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>( JsonSerializer.Serialize<object>( reqLoadDocumento ) );
                data!.Add( "str_cod_alfresco", str_id_documento! );

                _sol_servicio.dcyHeadersAdicionales = _dic_headers;
                _sol_servicio.objSolicitud = reqLoadDocumento;

                string str_result_srv = _httpService.solicitar_servicio( _sol_servicio ).Result;

                var result = JsonSerializer.Deserialize<ResAddSolicitudTc>( str_result_srv )!;

                respuesta.codigo = result.str_res_codigo == "1" ? "000" : "001";
                respuesta.diccionario.Add( "str_error", result.str_res_info_adicional );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.Message );
            }
            return respuesta;
        }

        public RespuestaTransaccion updateDocumento(string str_cod_documento, ReqLoadDocumento reqLoadDocumento, string str_id_transaccion)
        {
            RespuestaTransaccion respuesta = new();
            Dictionary<string, object> _dic_headers = new();
            SolicitarServicio _sol_servicio = new();
            try
            {

                _dic_headers.Add( "Authorization-Mego", _config.auth_validaciones_buro );

                _sol_servicio.urlServicio = _config.url_validaciones_buro + "UPD_DOCUMENTO";
                _sol_servicio.idTransaccion = str_id_transaccion!;

                var data = JsonSerializer.Deserialize<Dictionary<string, object>>( JsonSerializer.Serialize<object>( reqLoadDocumento ) );
                data!.Add( "str_cod_alfresco", str_cod_documento! );

                _sol_servicio.dcyHeadersAdicionales = _dic_headers;
                _sol_servicio.objSolicitud = reqLoadDocumento;

                string str_result_srv = _httpService.solicitar_servicio( _sol_servicio ).Result;

                var result = JsonSerializer.Deserialize<ResAddSolicitudTc>( str_result_srv )!;

                respuesta.codigo = result.str_res_codigo == "1" ? "000" : "001";
                respuesta.diccionario.Add( "str_error", result.str_res_info_adicional );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.Message );
            }
            return respuesta;
        }
    }
}

using Application.Common.Interfaces.Apis;
using Application.Common.Models;
using Domain.Entities.Axentria;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using wsGestorDocumentalSoap;

namespace Infraestructure.ExternalApis
{
    internal class wsGestorDocumental : IWsGestorDocumental
    {
        private readonly string str_clase;
        private readonly ApiSettings _settings;
        private readonly IValidacionesBuro _validacionesBuro;

        public wsGestorDocumental(IOptionsMonitor<ApiSettings> options, IValidacionesBuro validacionesBuro)
        {
            this.str_clase = GetType().FullName!;
            this._settings = options.CurrentValue;
            this._validacionesBuro = validacionesBuro;
        }

        public string addDocumento(ReqLoadDocumento reqLoadDocumento, string str_id_transaccion)
        {
            string str_error_publicacion = string.Empty;
            RespuestaTransaccion respuesta = new();
            ResGetCarpeta resGetCarpeta = new ResGetCarpeta();
            ReqCarpetaCredito reqCarpetaCredito = new ReqCarpetaCredito();
            try
            {

                //reqCarpetaCredito.str_cedula_socio = reqLoadDocumento.str_identificacion;
                reqCarpetaCredito.int_id_solicitud = Convert.ToInt32( _settings.id_gabinete );
                reqCarpetaCredito.str_name_folder = "SOLICITUD_TARJETA_CREDITO";
                resGetCarpeta = obtener_id_carpeta( reqCarpetaCredito );

                if (resGetCarpeta.lng_carpeta != 0)
                {
                    //publicar doc
                    Domain.Entities.Axentria.ImportarArchivoCampo importarArchivoCampo = new Domain.Entities.Axentria.ImportarArchivoCampo
                    {
                        DocumentId = 1,
                        DocumentTypeId = 1
                    };
                    reqLoadDocumento.loadfile.fields.Add( importarArchivoCampo );
                    reqLoadDocumento.int_solicitud = reqCarpetaCredito.int_id_solicitud;
                    reqLoadDocumento.lng_id_carpeta = resGetCarpeta.lng_carpeta;
                    bool bln_add_document = publicar_autorizacion( reqLoadDocumento );
                    if (bln_add_document)
                    {
                        string str_documento_id = get_Documento_publicado( reqLoadDocumento );
                        if (!str_documento_id.Contains( "Error" ) && str_documento_id != "0")
                        {
                            //almacenar db
                            respuesta = _validacionesBuro.addDocumento( str_documento_id, reqLoadDocumento, str_id_transaccion );
                            if (!string.IsNullOrEmpty( respuesta.diccionario["str_error"].ToString() ))
                            {
                                if (respuesta.diccionario["str_error"].ToString().Equals( "Error no se pudo crear el documento ya existe." ))
                                {
                                    int int_ente = Int32.Parse( reqLoadDocumento.loadfile.fields.First( e => e.FieldName.Equals( "Numero de Ente".ToUpper() ) ).DataValue );
                                    if (int_ente > 0)
                                    {
                                        //actualizar fecha
                                        respuesta = _validacionesBuro.updateDocumento( str_documento_id, reqLoadDocumento, str_id_transaccion );
                                        if (!string.IsNullOrEmpty( respuesta.diccionario["str_error"].ToString() ))
                                        {
                                            str_error_publicacion = "Error documento no actualizado id: " + str_documento_id + " bd: " + respuesta.diccionario["str_error"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        str_error_publicacion = "Error: No se pudo actualizar el valor del ente es incorrecto" + int_ente.ToString();
                                    }
                                }
                                else
                                {
                                    str_error_publicacion = "Error: No se pudo guardar en la BD " + respuesta.diccionario["str_error"].ToString();
                                }
                            }

                        }
                        else
                        {
                            str_error_publicacion = str_documento_id.Trim();
                        }
                    }
                    else { str_error_publicacion = "Error al publicar documento " + reqLoadDocumento.str_tipo_doc; }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return str_error_publicacion;
        }


        public Domain.Entities.Axentria.ImportarArchivoCampo generar_campo(int fieldId, string fieldName, string fieldvalue, int fieldtype, int documentId)
        {
            //Genero un objeto campo
            var campo = new Domain.Entities.Axentria.ImportarArchivoCampo();
            campo.FieldId = fieldId;
            campo.FieldName = fieldName;
            campo.DataValue = fieldvalue;
            campo.FieldType = fieldtype;
            campo.DocumentTypeId = documentId;
            return campo;
        }

        public string send_copy_file(string path, bool bln_separador, string str_name_doc)
        {
            string name = Path.GetFileNameWithoutExtension( path );
            string strPath = "\\PDFs\\";
            string path_cpy = strPath + str_name_doc + "(copy).pdf";
            string path_res = strPath + str_name_doc + ".pdf";

            try
            {
                if (bln_separador)
                {
                    File.Copy( path, path_cpy );
                    byte[] byte_data = File.ReadAllBytes( path_cpy );
                    PdfReader pdfReader = new PdfReader( byte_data );
                    int tpages = pdfReader.NumberOfPages;
                    if (tpages > 1)
                    {
                        pdfReader.SelectPages( "2-" + pdfReader.NumberOfPages.ToString() );
                        PdfStamper pdfStamper = new PdfStamper( pdfReader, new FileStream( path: path_res, mode: FileMode.Create ) );
                        pdfStamper.Close();
                        pdfReader.Close();
                    }
                    else
                    {
                        File.Copy( path, path_res );
                    }
                    File.Delete( path_cpy );
                }
                else
                {
                    File.Copy( path, path_res );
                }
                //File.Delete(path);
                return path_res;
            }
            catch (Exception ex)
            {
                Console.Write( ex.Message );
                return String.Empty;
            }
        }

        public ResGetCarpeta obtener_id_carpeta(ReqCarpetaCredito reqCarpetaCredito)
        {
            ResGetCarpeta resGetCarpeta = new ResGetCarpeta();
            srvAxentriaSoapClient srvAxentriaSoap = new( srvAxentriaSoapClient.EndpointConfiguration.srvAxentriaSoap );

            string str_folder_solicitud = "Sol_" + reqCarpetaCredito.int_id_solicitud.ToString();

            try
            {
                long carpeta_id = srvAxentriaSoap.obtener_carpeta( reqCarpetaCredito.str_cedula_socio, _settings.id_gabinete );

                if (carpeta_id == 0)
                {
                    //crear carpeta -> Socio 
                    long lng_socio;
                    lng_socio = srvAxentriaSoap.crear_carpeta( _settings.id_gabinete, reqCarpetaCredito.str_cedula_socio );
                    if (lng_socio > 0)
                    {
                        long lng_folder_credito = srvAxentriaSoap.crear_carpeta( lng_socio, reqCarpetaCredito.str_name_folder ); //carpeta Credito
                        if (lng_folder_credito > 0)
                        {
                            long lng_carpeta_solicitud = srvAxentriaSoap.crear_carpeta( lng_folder_credito, str_folder_solicitud ); //carpeta solicitud
                            if (lng_carpeta_solicitud > 0)
                            {
                                resGetCarpeta.lng_carpeta = lng_carpeta_solicitud;
                                resGetCarpeta.lng_carpeta_padre = lng_folder_credito;
                            }
                        }
                    }
                }
                else
                {
                    if (carpeta_id > 0)
                    {
                        //buscar carpeta credito
                        long lng_carpeta_credito = srvAxentriaSoap.obtener_carpeta( reqCarpetaCredito.str_name_folder, carpeta_id );
                        if (lng_carpeta_credito == 0)
                        {
                            long lng_folder_credit = srvAxentriaSoap.crear_carpeta( carpeta_id, reqCarpetaCredito.str_name_folder );
                            if (lng_folder_credit > 0)
                            {
                                long lng_carpeta_sol = srvAxentriaSoap.crear_carpeta( lng_folder_credit, str_folder_solicitud ); //carpeta solicitud
                                if (lng_carpeta_sol > 0)
                                {
                                    resGetCarpeta.lng_carpeta = lng_carpeta_sol;
                                    resGetCarpeta.lng_carpeta_padre = lng_folder_credit;
                                }
                            }
                        }
                        else
                        {
                            //obtener carpeta solciitud
                            long lng_carpeta_sol = srvAxentriaSoap.obtener_carpeta( str_folder_solicitud, lng_carpeta_credito );
                            if (lng_carpeta_sol > 0)
                            {
                                resGetCarpeta.lng_carpeta = lng_carpeta_sol;
                                resGetCarpeta.lng_carpeta_padre = lng_carpeta_credito;
                            }
                            else
                            {
                                long lng_carpeta_solicitud = srvAxentriaSoap.crear_carpeta( lng_carpeta_credito, str_folder_solicitud ); //carpeta solicitud
                                if (lng_carpeta_solicitud > 0)
                                {
                                    resGetCarpeta.lng_carpeta = lng_carpeta_solicitud;
                                    resGetCarpeta.lng_carpeta_padre = lng_carpeta_credito;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return resGetCarpeta;
        }

        public bool publicar_autorizacion(ReqLoadDocumento reqLoadDocumento)
        {
            bool autorizacion = false;
            srvAxentriaSoapClient srvAxentriaSoap = new( srvAxentriaSoapClient.EndpointConfiguration.srvAxentriaSoap );

            try
            {
                ReqPublicaDocumento reqPublicaDocumento = new ReqPublicaDocumento
                {
                    str_nombre_carpeta = "Sol_" + reqLoadDocumento.int_solicitud.ToString(),
                    lng_id_carpeta = reqLoadDocumento.lng_id_carpeta,
                    loadfile = get_load_file( reqLoadDocumento.loadfile )
                };

                bool bln_publicacion = srvAxentriaSoap.publicar_archivo( reqPublicaDocumento );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return autorizacion;
        }

        public string get_Documento_publicado(ReqLoadDocumento reqLoadDocumento)
        {
            srvAxentriaSoapClient srvAxentriaSoap = new( srvAxentriaSoapClient.EndpointConfiguration.srvAxentriaSoap );

            try
            {
                int int_tipo_documento = Int32.Parse( reqLoadDocumento.loadfile.properties[0].DefaultValue[0].ToString()! );
                //string str_nombre_archivo = reqloaddocumento.loadfile.properties[2].DefaultValue[0].ToString();
                string str_solicitud = reqLoadDocumento.int_solicitud.ToString();
                long id_doc = srvAxentriaSoap.obtener_id_documento( int_tipo_documento, str_solicitud );
                if (id_doc > 0)
                    return id_doc.ToString();
                else return "Error no se pudo obtener documento publicado de: " + str_solicitud;
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
        }

        public wsGestorDocumentalSoap.ImportarArchivo get_load_file(Domain.Entities.Axentria.ImportarArchivo loadfile)
        {
            wsGestorDocumentalSoap.ImportarArchivo imp_file = new wsGestorDocumentalSoap.ImportarArchivo();

            try
            {
                imp_file.fields = get_srv_fieldaxentria( loadfile.fields );
                imp_file.file = loadfile.file;
                imp_file.policies = loadfile.policies;
                imp_file.properties = get_properties( loadfile.properties );
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return imp_file;
        }

        public wsGestorDocumentalSoap.ImportarArchivoCampo[] get_srv_fieldaxentria(List<Domain.Entities.Axentria.ImportarArchivoCampo> fields)
        {
            wsGestorDocumentalSoap.ImportarArchivoCampo[] imp_campos = new wsGestorDocumentalSoap.ImportarArchivoCampo[fields.Count];
            int int_it_campo = 0;

            try
            {
                foreach (Domain.Entities.Axentria.ImportarArchivoCampo impc in fields)
                {
                    wsGestorDocumentalSoap.ImportarArchivoCampo nuevo_campo = new wsGestorDocumentalSoap.ImportarArchivoCampo
                    {
                        DataValue = impc.DataValue,
                        DocumentId = impc.DocumentId,
                        DocumentTypeId = impc.DocumentTypeId,
                        FieldId = impc.FieldId,
                        FieldName = impc.FieldName,
                        FieldType = impc.FieldType
                    };
                    imp_campos[int_it_campo] = nuevo_campo;
                    int_it_campo++;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return imp_campos;
        }

        public wsGestorDocumentalSoap.PropertyDocument[] get_properties(List<Domain.Entities.Axentria.PropertyDocument> properties)
        {
            wsGestorDocumentalSoap.PropertyDocument[] propiedades = new wsGestorDocumentalSoap.PropertyDocument[properties.Count];
            int int_it_prop = 0;

            try
            {
                foreach (Domain.Entities.Axentria.PropertyDocument iprop in properties)
                {
                    wsGestorDocumentalSoap.PropertyDocument nueva_propiedad = new wsGestorDocumentalSoap.PropertyDocument
                    {
                        DefaultValue = (ArrayOfAnyType)iprop.DefaultValue,
                        Description = iprop.Description,
                        DisplayName = iprop.DisplayName,
                        Id = iprop.Id,
                        Inherited = iprop.Inherited,
                        LocalName = iprop.LocalName,
                        LocalNamespace = iprop.LocalNamespace,
                        OpenChoice = iprop.OpenChoice,
                        Orderable = iprop.Orderable,
                        PropertyTypeName = iprop.PropertyTypeName,
                        Queryable = iprop.Queryable,
                        QueryName = iprop.QueryName,
                        Required = iprop.Required
                    };
                    propiedades[int_it_prop] = nueva_propiedad;
                    int_it_prop++;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.ToString() );
            }
            return propiedades;
        }
    }
}

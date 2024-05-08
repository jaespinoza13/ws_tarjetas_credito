using Domain.Entities.Orden;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Application.Common.Utilidades
{
    public class GenerarReporte
    {
        
        

        public static byte[] ReportePDF(Orden orden)
        {
            byte[] pdfBytes = [];

            try
            {   
                MemoryStream datos = new MemoryStream();
                PdfWriter writer = new PdfWriter( datos );
                PdfDocument pdfDocument = new PdfDocument( writer );
                Document documento = new Document( pdfDocument );

                UtilidadesGenerarReporte.TableHeaderEventHandler handler = new UtilidadesGenerarReporte.TableHeaderEventHandler( documento );
                pdfDocument.AddEventHandler( PdfDocumentEvent.END_PAGE, handler );

                // Calcula el margen superior para asegurarte de que la tabla se ajuste al margen.
                float topMargin = 20 + handler.GetTableHeight();
                documento.SetMargins( topMargin, 36, 36, 36 );



                PdfFont font_negrita = PdfFontFactory.CreateFont( StandardFonts.HELVETICA_BOLD );
                PdfFont font_normal = PdfFontFactory.CreateFont( StandardFonts.HELVETICA );

                // CAMPOS
                Paragraph fecha_reporte = UtilidadesGenerarReporte.AgregarParrafo( "Fecha: " + DateTime.Now.ToString( "MM/dd/yyyy HH:mm:ss" ), TextAlignment.LEFT, 9, [20, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph num_orden = UtilidadesGenerarReporte.AgregarParrafo( "Nro. Orden: " + orden.str_numero_orden, TextAlignment.LEFT, 9, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph descripcion = UtilidadesGenerarReporte.AgregarParrafo( "Descripcion: " + orden.str_descripcion, TextAlignment.LEFT, 9, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph total_tarjetas = UtilidadesGenerarReporte.AgregarParrafo( "Total de tarjetas: " + orden.str_cantidad, TextAlignment.LEFT, 9, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph agencia = UtilidadesGenerarReporte.AgregarParrafo( "Agencia: " + orden.str_agencia_solicita, TextAlignment.LEFT, 9, [00, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph firman = UtilidadesGenerarReporte.AgregarParrafo( "Firman", TextAlignment.CENTER, 9, [10, 0, 0, 0], [0, 0, 0, 0], font_negrita, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph firma_lineas = UtilidadesGenerarReporte.AgregarParrafo( "____________________________                                            ____________________________", TextAlignment.LEFT, 9, [30, 0, 0, 75], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );
                Paragraph firman_involucrados = UtilidadesGenerarReporte.AgregarParrafo( "Asistente de Operaciones                                                                    Jefe de Agencia", TextAlignment.LEFT, 9, [0, 0, 0, 88], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), iText.Kernel.Colors.ColorConstants.BLACK );

                documento.Add( fecha_reporte );
                documento.Add( num_orden );
                documento.Add( descripcion );
                documento.Add( total_tarjetas );
                documento.Add( agencia );


                //SECCION TABLA 
                Table table = new Table( 8 ).UseAllAvailableWidth();
                table.SetMarginTop( 20 );
                table.SetMarginBottom( 10 );

                Paragraph celdaCuenta = UtilidadesGenerarReporte.AgregarParrafo( "Cuenta", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaTipoIdentificacion = UtilidadesGenerarReporte.AgregarParrafo( "Tipo Identificación", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaIdentificacion = UtilidadesGenerarReporte.AgregarParrafo( "Identificación", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaEnte = UtilidadesGenerarReporte.AgregarParrafo( "Ente", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaNombre = UtilidadesGenerarReporte.AgregarParrafo( "Nombre", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaNombreImpreso = UtilidadesGenerarReporte.AgregarParrafo( "Nombre impreso", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaTipoProducto = UtilidadesGenerarReporte.AgregarParrafo( "Tipo Producto", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );
                Paragraph celdaCupoSolicitado = UtilidadesGenerarReporte.AgregarParrafo( "Cupo Solicitado", TextAlignment.CENTER, 7, [0, 0, 0, 0], [0, 0, 0, 0], font_normal, new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ), iText.Kernel.Colors.ColorConstants.WHITE );

                Cell headerCell1 = new Cell().SetWidth( UnitValue.CreatePercentValue( 15 ) ).Add(celdaCuenta ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell2 = new Cell().SetWidth( UnitValue.CreatePercentValue( 5 ) ).Add( celdaTipoIdentificacion ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell3 = new Cell().SetWidth( UnitValue.CreatePercentValue( 10 ) ).Add( celdaIdentificacion ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell4 = new Cell().SetWidth( UnitValue.CreatePercentValue( 5 ) ).Add( celdaEnte ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell5 = new Cell().SetWidth( UnitValue.CreatePercentValue( 22 ) ).Add( celdaNombre ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell6 = new Cell().SetWidth( UnitValue.CreatePercentValue( 22 ) ).Add( celdaNombreImpreso ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell7 = new Cell().SetWidth( UnitValue.CreatePercentValue( 11 ) ).Add( celdaTipoProducto ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 6 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell8 = new Cell().SetWidth( UnitValue.CreatePercentValue( 9 ) ).Add( celdaCupoSolicitado ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 7 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );


                table.AddHeaderCell( headerCell1 );
                table.AddHeaderCell( headerCell2 );
                table.AddHeaderCell( headerCell3 );
                table.AddHeaderCell( headerCell4 );
                table.AddHeaderCell( headerCell5 );
                table.AddHeaderCell( headerCell6 );
                table.AddHeaderCell( headerCell7 );
                table.AddHeaderCell( headerCell8 );


                foreach (Tarjeta_Solicitada item in orden.lst_tarjetas_solicitadas)
                {
                    int index = orden.lst_tarjetas_solicitadas.IndexOf( item );
                    table.AddCell( new Paragraph( item.str_cuenta ).SetFontSize( 7 ).SetTextAlignment( TextAlignment.CENTER ) );
                    table.AddCell( new Paragraph( item.str_tipo_identificacion ).SetFontSize( 7 ).SetTextAlignment( TextAlignment.CENTER ) );
                    table.AddCell( new Paragraph( item.str_identificacion ).SetFontSize( 7 ) );
                    table.AddCell( new Paragraph( item.str_ente ).SetFontSize( 7 ).SetTextAlignment( TextAlignment.CENTER ) );
                    table.AddCell( new Paragraph( item.str_nombre ).SetFontSize( 6 ) );
                    table.AddCell( new Paragraph( item.str_nombre_impreso ).SetFontSize( 6 ) );
                    table.AddCell( new Paragraph( item.str_tipo ).SetFontSize( 7 ) );
                    table.AddCell( new Paragraph( item.str_cupo ).SetFontSize( 7 ) ).SetTextAlignment( TextAlignment.CENTER );

                }
                documento.Add( table );
                documento.Add( firman );
                documento.Add( firma_lineas );
                documento.Add( firman_involucrados );
                documento.Close();


                pdfBytes = datos.ToArray();

                
                //debug para descargar reporte al escritorio
                /*
                string desktopPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
                string filePath = System.IO.Path.Combine( desktopPath, "prueba.pdf" );
                using (FileStream fileStream = new FileStream( filePath, FileMode.Create ))
                {
                    fileStream.Write( pdfBytes, 0, pdfBytes.Length );
                }
                */
                
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.Message );
            }

            return pdfBytes;
        }

    }
}

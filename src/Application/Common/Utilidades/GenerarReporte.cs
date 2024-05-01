using System;
using System.IO;
using Domain.Entities.Orden;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using static Application.TarjetasCredito.OrdenReporte.GetReporteOrdenHandler;

namespace Application.Common.Utilidades
{
    public class GenerarReporte
    {

        public static byte[] ReportePDF(Orden orden)
        {
            byte[] pdfBytes = [];

            try
            {
                MemoryStream baos = new MemoryStream();
                PdfWriter writer = new PdfWriter( baos );
                PdfDocument pdfDocument = new PdfDocument( writer );//writer.SetSmartMode( true ) );

                Document d = new Document( pdfDocument, iText.Kernel.Geom.PageSize.LETTER );


                // Establecer el título y la alineación del encabezado
                Paragraph title = new Paragraph( "COOPMEGO" ).SetTextAlignment( TextAlignment.CENTER ).SetFontSize( 13 );
                d.Add( title );

                //CAMPOS ADICIONALES
                Paragraph date = new Paragraph("Fecha: " + DateTime.Now.ToString( "MM/dd/yyyy HH:mm:ss" ) ).SetTextAlignment( TextAlignment.LEFT ).SetFontSize( 10 );
                d.Add( date );

                Paragraph num_orden = new Paragraph( "Nro. Orden: "+orden.str_numero_orden ).SetTextAlignment( TextAlignment.LEFT ).SetFontSize( 10 );
                d.Add( num_orden );

                Paragraph descripcion = new Paragraph( "Descripcion: " + orden.str_descripcion ).SetTextAlignment( TextAlignment.LEFT ).SetFontSize( 10 );
                d.Add( descripcion );

                Paragraph total_tarjetas = new Paragraph( "Total de tarjetas: " + orden.str_cantidad ).SetTextAlignment( TextAlignment.LEFT ).SetFontSize( 10 );
                d.Add( total_tarjetas );

                Paragraph agencia = new Paragraph( "Agencia: " + orden.str_agencia_solicita ).SetTextAlignment( TextAlignment.LEFT ).SetFontSize( 10 );
                d.Add(agencia);


                //SECCION TABLA 
                Table table = new Table( 8, true );


                // Establecer el color de fondo de la tabla iText.Kernel.Colors.ColorConstants.LIGHT_GRAY
                //table.SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) );

                // Añadir celdas de encabezado con estilos personalizados
                Cell headerCell1 = new Cell().Add( new Paragraph( "Cuenta" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell2 = new Cell().Add( new Paragraph( "Tipo Identificación" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell3 = new Cell().Add( new Paragraph( "Identificación" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell4 = new Cell().Add( new Paragraph( "Ente" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell5 = new Cell().Add( new Paragraph( "Nombre" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell6 = new Cell().Add( new Paragraph( "Nombre impreso" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell7 = new Cell().Add( new Paragraph( "Tipo Producto" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );
                Cell headerCell8 = new Cell().Add( new Paragraph( "Cupo Solicitado" ) ).SetBold().SetTextAlignment( TextAlignment.CENTER ).SetBackgroundColor( new iText.Kernel.Colors.DeviceRgb( 0, 76, 172 ) ).SetFontSize( 9 ).SetFontColor( iText.Kernel.Colors.ColorConstants.WHITE );

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
                    table.AddCell( new Paragraph( item.str_cuenta ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_tipo_identificacion ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_identificacion ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_ente ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_nombre ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_nombre_impreso ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_tipo ).SetFontSize( 9 ) );
                    table.AddCell( new Paragraph( item.str_cupo ).SetFontSize( 9 ) );
                }
                d.Add( table );
                d.Close();

                pdfBytes = baos.ToArray();

                string desktopPath = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
                string filePath = Path.Combine( desktopPath, "documento.pdf" );

                using (FileStream fileStream = new FileStream( filePath, FileMode.Create ))
                {
                    fileStream.Write( pdfBytes, 0, pdfBytes.Length );
                }

            } catch (Exception e){
                Console.WriteLine( e.ToString() );
            }

            return pdfBytes;
        }



    }
}

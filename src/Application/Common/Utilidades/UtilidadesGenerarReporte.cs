using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.IO.Font.Constants;

namespace Application.Common.Utilidades
{
    public class UtilidadesGenerarReporte
    {
        public static PdfFont font_negrita = PdfFontFactory.CreateFont( StandardFonts.HELVETICA_BOLD );
        public static PdfFont font_normal = PdfFontFactory.CreateFont( StandardFonts.HELVETICA );

        public class TableHeaderEventHandler : IEventHandler
        {
            private Table table;
            private float tableHeight;
            private Document doc;

            public TableHeaderEventHandler(Document doc)
            {
                this.doc = doc;
                InitTable();

                TableRenderer renderer = (TableRenderer)table.CreateRendererSubTree();
                renderer.SetParent( new DocumentRenderer( doc ) );

                LayoutResult result = renderer.Layout( new LayoutContext( new LayoutArea( 0, PageSize.A4 ) ) );
                tableHeight = result.GetOccupiedArea().GetBBox().GetHeight();


            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas( page.NewContentStreamBefore(), page.GetResources(), pdfDoc );
                PageSize pageSize = pdfDoc.GetDefaultPageSize();
                float coordX = pageSize.GetX() + doc.GetLeftMargin();
                float coordY = pageSize.GetTop() - doc.GetTopMargin();
                float width = pageSize.GetWidth() - doc.GetRightMargin() - doc.GetLeftMargin();
                float height = GetTableHeight();
                Rectangle rect = new Rectangle( coordX, coordY, width, height );


                new Canvas( canvas, rect )
                    .Add( table )
                    .Close();
            }

            public float GetTableHeight()
            {
                return tableHeight;
            }

            //METODO PARA GENERAR EL ENCABEZADO DE PAGINA (EJ https://github.com/itext/itext-publications-samples-dotnet/blob/develop/itext/itext.samples/itext/samples/sandbox/events/TableHeader.cs)
            private void InitTable()
            {
                table = new Table( 1 ).UseAllAvailableWidth();
                table.AddCell( "Cooperativa de Ahorro y Crédito Vicentina \n “Manuel Esteban Godoy Ortega” Ltda. \n CoopMego" ).SetTextAlignment( TextAlignment.CENTER )
                    .SetBorderBottom( new SolidBorder( new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), 1 ) )
                    .SetBorderTop( new SolidBorder( new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), 1 ) )
                    .SetBorderLeft( new SolidBorder( new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), 1 ) )
                    .SetBorderRight( new SolidBorder( new iText.Kernel.Colors.DeviceRgb( 255, 255, 255 ), 1 ) )
                    .SetFontSize( 11 ).SetFont( font_negrita );
                table.SetPaddingBottom( 10 );
                table.SetMarginBottom( 10 );
            }
        }

        public static Paragraph AgregarParrafo(String descripcion, TextAlignment textAlignment, float fontSize, float[] margin, float[] padding, PdfFont tipo_fuente, iText.Kernel.Colors.DeviceRgb colorFondoTexto, iText.Kernel.Colors.Color colorTexto)
        {

            Paragraph parrafo = new Paragraph( descripcion )
                .SetTextAlignment( textAlignment )
                .SetFontSize( fontSize )
                .SetMargins( margin[0], margin[1], margin[2], margin[3] )
                .SetPaddings( padding[0], padding[1], padding[2], padding[3] )
                .SetFont( tipo_fuente )
                .SetBackgroundColor( colorFondoTexto )
                .SetFontColor( colorTexto );
            return parrafo;
        }



    }
}

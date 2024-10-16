using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using IL = iText.Layout;

namespace SalesRestaurantSystem
{
    public class ReceiptController
    {
        string filePath = "ruta/del/archivo.pdf";

        public void BuildReceipt()
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                // Crear un PDF document
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    // Crear un documento
                    IL.Document document = new IL.Document(pdf);

                    // Agregar contenido (texto en este caso)
                    document.Add(new Paragraph("Comprobante de Venta"));

                    // Puedes agregar más elementos como tablas, imágenes, etc.
                    document.Add(new Paragraph("Fecha: " + DateTime.Now.ToString()));
                    document.Add(new Paragraph("Producto: XYZ"));
                    document.Add(new Paragraph("Cantidad: 1"));
                    document.Add(new Paragraph("Total: $100"));

                    // Cerrar el documento
                    document.Close();
                }
            }
        }

    }
}

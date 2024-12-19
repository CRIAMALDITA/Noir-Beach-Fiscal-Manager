using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using Microsoft.Win32;
using RestaurantData;

namespace SalesRestaurantSystem
{
    public static class ReceiptController
    {
        public static void MakeReceipt(ReceiptData data)
        {
            string newFile = Path.Combine(Path.GetTempPath(), $"Invoice_{Guid.NewGuid()}.pdf");
            using (PdfWriter writer = new PdfWriter(newFile))
                using (PdfDocument pdf = new PdfDocument(writer))
                using (Document document = new Document(pdf))
                {
                    document.Add(new Paragraph(data.Top.CompanyName)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph(data.Top.Address)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12));
                    document.Add(new Paragraph(data.Top.Id)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12));

                    document.Add(new LineSeparator(new SolidLine()).SetMarginBottom(10));

                    document.Add(new Paragraph("Customer Details")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph($"{data.Client.ClientName}\n{data.Client.ClientId}\n{data.Client.ClientAddress}\n{data.Client.ClientMail}")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12));

                    document.Add(new LineSeparator(new SolidLine()).SetMarginBottom(10));

                    document.Add(new Paragraph("Simplified Invoice")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph($"Invoice Number: {data.Invoice.InvoiceNum}\nRegister: {data.Invoice.POS}\nDate: {data.Invoice.CreationDate}\nEmployee:{data.Invoice.UserName}")
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER));

                    document.Add(new LineSeparator(new SolidLine()).SetMarginBottom(10));

                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 2, 2 }))
                        .UseAllAvailableWidth();

                    table.AddHeaderCell(new Cell().Add(new Paragraph("Count")));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Product")));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Price")));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Subtotal")));

                    PopulateTable(table, data.Invoice.Elements);
                    document.Add(table);

                    document.Add(new LineSeparator(new SolidLine()).SetMarginTop(10).SetMarginBottom(10));

                    Table paymentTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 3, 3, 3 }))
                        .UseAllAvailableWidth();
                    paymentTable.AddHeaderCell(new Cell().Add(new Paragraph("Payment type")));
                    paymentTable.AddHeaderCell(new Cell().Add(new Paragraph("Pays with")));
                    paymentTable.AddHeaderCell(new Cell().Add(new Paragraph("Total")));
                    paymentTable.AddHeaderCell(new Cell().Add(new Paragraph("Change")));

                    paymentTable.AddCell(new Cell().Add(new Paragraph(data.Pay.PayType)));
                    paymentTable.AddCell(new Cell().Add(new Paragraph(data.Pay.PaysWith)));
                    paymentTable.AddCell(new Cell().Add(new Paragraph(data.Pay.Total)));
                    paymentTable.AddCell(new Cell().Add(new Paragraph(data.Pay.Change)));

                    document.Add(paymentTable);

                    document.Add(new LineSeparator(new SolidLine()).SetMarginTop(10).SetMarginBottom(10));
                    document.Add(new Paragraph(data.Messagge).SetTextAlignment(TextAlignment.CENTER).SetFontSize(12));
                }

                Console.WriteLine("Documento PDF creado correctamente.");
                OpenFile(newFile);
        }

        private static void PopulateTable(Table table, List<ReceiptData.ReceiptElements> elements)
        {
            foreach (var element in elements)
            {
                table.AddCell(new Cell().Add(new Paragraph(element.Count.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(element.Name)));
                table.AddCell(new Cell().Add(new Paragraph($"${element.Price}")));
                table.AddCell(new Cell().Add(new Paragraph($"${element.SubTotal}")));
            }
        }

        private static string ShowSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Guardar archivo PDF",
                Filter = "PDF File (*.pdf)|*.pdf|All Files (*.*)|*.*",
                DefaultExt = "pdf",
                AddExtension = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = "Invoice"
            };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        private static void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
                Console.WriteLine("El archivo se abrió correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar abrir el archivo: {ex.Message}");
            }
        }
    }
}

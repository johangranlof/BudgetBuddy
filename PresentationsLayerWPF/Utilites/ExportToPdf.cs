using EntityLayer;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using PdfSharp.UniversalAccessibility.Drawing;

namespace PresentationsLayerWPF.Utilites
{
    public static class PdfExporter
    {
        [Obsolete]
        public static void ExportExpensesToPdf(IEnumerable<Expense> expenses)
        {
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Verdana", 12);

            XStringFormat format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;

            int yPosition = 50;
            int xDate = 50;
            int xType = 150;
            int xAmount = 250;
            int xNote = 350;

            gfx.DrawString("Date", font, XBrushes.Black, new XRect(xDate, yPosition, page.Width, page.Height), format);
            gfx.DrawString("Type", font, XBrushes.Black, new XRect(xType, yPosition, page.Width, page.Height), format);
            gfx.DrawString("Amount", font, XBrushes.Black, new XRect(xAmount, yPosition, page.Width, page.Height), format);
            gfx.DrawString("Note", font, XBrushes.Black, new XRect(xNote, yPosition, page.Width, page.Height), format);

            yPosition += 20;

            foreach (var expense in expenses)
            {
                gfx.DrawString(expense.Date.ToString("yyyy-MM-dd"), font, XBrushes.Black,
                    new XRect(xDate, yPosition, page.Width, page.Height), format);
                gfx.DrawString(expense.Type.ToString(), font, XBrushes.Black,
                    new XRect(xType, yPosition, page.Width, page.Height), format);
                gfx.DrawString(expense.Amount.ToString("F2"), font, XBrushes.Black,
                    new XRect(xAmount, yPosition, page.Width, page.Height), format);
                if (expense.Note != null)
                {
                    gfx.DrawString(expense.Note, font, XBrushes.Black,
                    new XRect(xNote, yPosition, page.Width, page.Height), format);
                }

                yPosition += 20;

                if (yPosition > page.Height - 50)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPosition = 50;

                    gfx.DrawString("Date", font, XBrushes.Black, new XRect(xDate, yPosition, page.Width, page.Height), format);
                    gfx.DrawString("Type", font, XBrushes.Black, new XRect(xType, yPosition, page.Width, page.Height), format);
                    gfx.DrawString("Amount", font, XBrushes.Black, new XRect(xAmount, yPosition, page.Width, page.Height), format);
                    gfx.DrawString("Note", font, XBrushes.Black, new XRect(xNote, yPosition, page.Width, page.Height), format);

                    yPosition += 20;
                }
            }

            string pdfFilename = Path.Combine(Path.GetTempPath(), "Expenses.pdf");
            document.Save(pdfFilename);
            document.Close();

            OpenPdfFile(pdfFilename);
        }

        public static void OpenPdfFile(string pdfFilename)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pdfFilename,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

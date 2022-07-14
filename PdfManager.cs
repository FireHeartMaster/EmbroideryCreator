using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class PdfManager
    {
        private XSolidBrush pageSetupBrush = XBrushes.DarkBlue;

        private string title = "Title";
        private string secondTitle = "Second Title";
        private string subtitle = "Subtitle";
        private string leftText = "Left Text";
        private string rightText = "Right Text";
        private string footerText = "Footer Text";

        private XFont titleFont = new XFont("Verdana", 20, XFontStyle.Regular);
        private XFont subtitleFont = new XFont("Verdana", 10, XFontStyle.Regular);
        private XFont footerFont = new XFont("Verdana", 5, XFontStyle.Regular);
        private XFont sideTextFont = new XFont("Verdana", 5, XFontStyle.Regular);

        private XFont pageNumberFont = new XFont("Verdana", 12, XFontStyle.Regular);



        public void CreatePdf()
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Eduardo testando PDF";

            PdfPage page = document.AddPage();

            PreparePage(page);

            document.Save("D:/Downloads/test.pdf");
        }

        private void PreparePage(PdfPage page)
        {
            XGraphics pdfGraphics = XGraphics.FromPdfPage(page);


            //title
            pdfGraphics.DrawString(title, titleFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.1f), XStringFormats.Center);
            //second title
            pdfGraphics.DrawString(secondTitle, titleFont, pageSetupBrush, new XRect(0, 25, page.Width, page.Height * 0.1f), XStringFormats.Center);

            //subtitle
            pdfGraphics.DrawString(subtitle, subtitleFont, pageSetupBrush, new XRect(0, 50, page.Width, page.Height * 0.1f), XStringFormats.Center);

            //bottom
            pdfGraphics.DrawString(footerText, footerFont, pageSetupBrush, new XRect(0, page.Height * 0.95f, page.Width, page.Height * 0.05f), XStringFormats.Center);

            //left text
            pdfGraphics.RotateAtTransform(-90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
            pdfGraphics.DrawString(leftText, sideTextFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.33f), XStringFormats.Center);
            pdfGraphics.RotateAtTransform(90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));

            //right text
            pdfGraphics.RotateAtTransform(90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
            pdfGraphics.DrawString(rightText, sideTextFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.33f), XStringFormats.Center);
            pdfGraphics.RotateAtTransform(-90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
        }
    }
}

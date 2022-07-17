using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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



        private XPoint startingPointForDrawings = new XPoint(50, 100);
        private double sizeOfEachSquare = 8;

        private readonly int maxHorizontalNumberOfSquares = 56;
        private readonly int maxVerticalNumberOfSquares = 85;

        private readonly XPen gridPen = new XPen(XColors.Gray, 0.5);
        private readonly XPen thickGridPen = new XPen(XColors.Black, 1);

        public PdfManager()
        {
            gridPen.LineCap = XLineCap.Round;
            thickGridPen.LineCap = XLineCap.Round;
        }

        public void CreatePdf(List<Bitmap> images, string pathToSave)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Eduardo testando PDF";

            foreach(var image in images)
            {
                PdfPage page = document.AddPage();

                XGraphics pageGraphics = XGraphics.FromPdfPage(page);
                PreparePage(page, pageGraphics);

                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                XImage xImage = XImage.FromStream(stream);

                pageGraphics.DrawImage(xImage, page.Width * 0.1f, page.Height * 0.3f, page.Width * (1 - 2* 0.1f), page.Height * (1 - 2 * 0.3f));


                //CreateAllDrawingPages(document, )
            }

            try
            {
                document.Save(pathToSave);
            }
            catch (IOException exception)
            {
                Console.WriteLine("IOException");
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(exception.Message);
            }
        }

        public void CreatePdfStitches(string pathToSave, int[,] matrixOfNewColors, List<Color> colorMeans)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Eduardo testando PDF";

            CreateAllDrawingPages(document, matrixOfNewColors, colorMeans);

            try
            {
                document.Save(pathToSave);
            }
            catch (IOException exception)
            {
                Console.WriteLine("IOException");
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(exception.Message);
            }
        }

        private void PreparePage(PdfPage page, XGraphics pageGraphics)
        {
            //XGraphics pageGraphics = XGraphics.FromPdfPage(page);


            //title
            pageGraphics.DrawString(title, titleFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.1f), XStringFormats.Center);
            //second title
            pageGraphics.DrawString(secondTitle, titleFont, pageSetupBrush, new XRect(0, 25, page.Width, page.Height * 0.1f), XStringFormats.Center);

            //subtitle
            pageGraphics.DrawString(subtitle, subtitleFont, pageSetupBrush, new XRect(0, 50, page.Width, page.Height * 0.1f), XStringFormats.Center);

            //bottom
            pageGraphics.DrawString(footerText, footerFont, pageSetupBrush, new XRect(0, page.Height * 0.95f, page.Width, page.Height * 0.05f), XStringFormats.Center);

            //left text
            pageGraphics.RotateAtTransform(-90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
            pageGraphics.DrawString(leftText, sideTextFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.33f), XStringFormats.Center);
            pageGraphics.RotateAtTransform(90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));

            //right text
            pageGraphics.RotateAtTransform(90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
            pageGraphics.DrawString(rightText, sideTextFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.33f), XStringFormats.Center);
            pageGraphics.RotateAtTransform(-90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
        }

        private void CreateAllDrawingPages(PdfDocument document, int[,] matrixOfNewColors, List<Color> colorMeans)
        {
            int horizontalNumberOfTimes = matrixOfNewColors.GetLength(0) / maxHorizontalNumberOfSquares + 1;
            int verticalNumberOfTimes = matrixOfNewColors.GetLength(1) / maxVerticalNumberOfSquares + 1;

            for (int y = 0; y < verticalNumberOfTimes; y++)
            {
                for (int x = 0; x < horizontalNumberOfTimes; x++)
                {
                    PdfPage currentPage = document.AddPage();

                    XGraphics pageGraphics = XGraphics.FromPdfPage(currentPage);
                    PreparePage(currentPage, pageGraphics);

                    startingPointForDrawings.X = (currentPage.Width - (sizeOfEachSquare * maxHorizontalNumberOfSquares)) * 0.5f;

                    DrawStitchesOnPage(currentPage, pageGraphics, matrixOfNewColors, colorMeans, x, y);
                }
            }
        }

        private void DrawStitchesOnPage(PdfPage currentPage, XGraphics pageGraphics, int[,] matrixOfNewColors, List<Color> colorMeans, int x, int y)
        {
            int relativeIndexI = 0;
            int relativeIndexJ = 0;

            int lastRelativeIndexJ = 0;

            for (int j = y * maxVerticalNumberOfSquares; j < (y + 1) * maxVerticalNumberOfSquares; j++)
            {
                if (j >= matrixOfNewColors.GetLength(1)) break;

                relativeIndexI = 0;
                for (int i = x * maxHorizontalNumberOfSquares; i < (x + 1) * maxHorizontalNumberOfSquares; i++)
                {
                    if (i >= matrixOfNewColors.GetLength(0)) break;
                    
                    //Drawing stitch
                    DrawStitchAtPosition(pageGraphics, matrixOfNewColors, colorMeans, relativeIndexI, relativeIndexJ, i, j);

                    if (relativeIndexJ == maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1)
                    {
                        //Drawing current vertical line
                        DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, relativeIndexJ, relativeIndexI, i);
                    }

                    relativeIndexI++;
                }

                //Drawing current horizontal line
                DrawHorizontalGridLine(pageGraphics, matrixOfNewColors, relativeIndexJ, j, relativeIndexI);

                lastRelativeIndexJ = relativeIndexJ;
                relativeIndexJ++;
            }

            //Draw horizontal thick grid lines over the normal grid lines
            int startingJ = y * maxVerticalNumberOfSquares;
            while(startingJ % 10 != 0)
            {
                startingJ++;
            }
            relativeIndexJ = (y * maxVerticalNumberOfSquares) % 10;
            for (int j = startingJ; j < (y + 1) * maxVerticalNumberOfSquares; j += 10)
            {
                if (j >= matrixOfNewColors.GetLength(1)) break;

                DrawHorizontalGridLine(pageGraphics, matrixOfNewColors, relativeIndexJ, j, relativeIndexI);
                relativeIndexJ += 10;
            }

            //Draw vertical thick grid lines over the normal grid lines
            int startingI = x * maxHorizontalNumberOfSquares;
            relativeIndexI = 0;
            while (startingI % 10 != 0)
            {
                startingI++;
                relativeIndexI++;
            }
            relativeIndexJ = lastRelativeIndexJ;
            for (int i = startingI; i < (x + 1) * maxHorizontalNumberOfSquares; i += 10)
            {
                //if (i >= matrixOfNewColors.GetLength(0)) break;
                if (i >= matrixOfNewColors.GetLength(0))
                {
                    //Also draw last vertical thick line of the current page if we didn't reach yet the end of the page
                    if(relativeIndexI < maxHorizontalNumberOfSquares)
                    {
                        DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, relativeIndexJ, relativeIndexI, i);
                    }
                    break;
                }

                DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, relativeIndexJ, relativeIndexI, i);
                //if (i >= matrixOfNewColors.GetLength(0)) break;

                relativeIndexI += 10;
            }
        }

        private void DrawStitchAtPosition(XGraphics pageGraphics, int[,] matrixOfNewColors, List<Color> colorMeans, int relativeIndexI, int relativeIndexJ, int i, int j)
        {
            Color currentPositionColor = colorMeans[matrixOfNewColors[i, j]];
            XSolidBrush brush = new XSolidBrush(XColor.FromArgb(currentPositionColor.R, currentPositionColor.G, currentPositionColor.B));

            XRect positionToDrawRect = new XRect(   startingPointForDrawings.X + relativeIndexI * sizeOfEachSquare,
                                                    startingPointForDrawings.Y + relativeIndexJ * sizeOfEachSquare,
                                                    sizeOfEachSquare,
                                                    sizeOfEachSquare);

            pageGraphics.DrawRectangle(brush, positionToDrawRect);
        }

        private void DrawHorizontalGridLine(XGraphics pageGraphics, int[,] matrixOfNewColors, int relativeIndexJ, int j, int relativeIndexI)
        {
            XPen penToUse = j % 10 == 0 ? thickGridPen : gridPen;
            pageGraphics.DrawLine(penToUse,
                                    startingPointForDrawings.X,
                                    startingPointForDrawings.Y + relativeIndexJ * sizeOfEachSquare,
                                    startingPointForDrawings.X + relativeIndexI * sizeOfEachSquare,
                                    startingPointForDrawings.Y + relativeIndexJ * sizeOfEachSquare);

            //Also draw last horizontal line of the current page
            if (relativeIndexJ == maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1)
            {
                penToUse = (j + 1) % 10 == 0 ? thickGridPen : gridPen;
                pageGraphics.DrawLine(penToUse,
                                        startingPointForDrawings.X,
                                        startingPointForDrawings.Y + (relativeIndexJ + 1) * sizeOfEachSquare,
                                        startingPointForDrawings.X + relativeIndexI * sizeOfEachSquare,
                                        startingPointForDrawings.Y + (relativeIndexJ + 1) * sizeOfEachSquare);
            }
        }

        private void DrawVerticalGridLine(XGraphics pageGraphics, int[,] matrixOfNewColors, int x, int relativeIndexJ, int relativeIndexI, int i)
        {
            XPen penToUse = i % 10 == 0 ? thickGridPen : gridPen;
            //penToUse = gridPen;
            pageGraphics.DrawLine(penToUse,
                                    startingPointForDrawings.X + relativeIndexI * sizeOfEachSquare,
                                    startingPointForDrawings.Y,
                                    startingPointForDrawings.X + relativeIndexI * sizeOfEachSquare,
                                    startingPointForDrawings.Y + (relativeIndexJ + 1) * sizeOfEachSquare);

            if (relativeIndexI == (x + 1) * maxHorizontalNumberOfSquares - 1 || i == matrixOfNewColors.GetLength(0) - 1)
            {
                penToUse = (i + 1) % 10 == 0 ? thickGridPen : gridPen;
                //penToUse = gridPen;
                pageGraphics.DrawLine(penToUse,
                                        startingPointForDrawings.X + (relativeIndexI + 1) * sizeOfEachSquare,
                                        startingPointForDrawings.Y,
                                        startingPointForDrawings.X + (relativeIndexI + 1) * sizeOfEachSquare,
                                        startingPointForDrawings.Y + (relativeIndexJ + 1) * sizeOfEachSquare);
            }
        }
    }
}

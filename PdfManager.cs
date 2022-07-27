﻿using PdfSharp.Drawing;
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
        private XSolidBrush arrowBrush = XBrushes.DarkBlue;

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



        private XPoint startingPointForDrawings = new XPoint(50, 120);
        private double sizeOfEachSquare = 8;

        private readonly int maxHorizontalNumberOfSquares = 56;
        private readonly int maxVerticalNumberOfSquares = 85;

        //private readonly XPen gridPen = new XPen(XColors.Gray, 0.5);
        //private readonly XPen thickGridPen = new XPen(XColors.Black, 1);

        private readonly Color gridPenColor = Color.Gray;
        private readonly Color thickGridPenColor = Color.Black;

        private readonly double gridPenThickness = 0.5;
        private readonly double thickGridPenThickness = 2;

        private readonly double backstitchLineThickness = 2;

        public PdfManager()
        {
            //gridPen.LineCap = XLineCap.Round;
            //thickGridPen.LineCap = XLineCap.Round;
        }

        public PdfManager(string title, string secondTitle, string subtitle, string leftText, string rightText, string footerText)
        {
            this.title = title;
            this.secondTitle = secondTitle;
            this.subtitle = subtitle;
            this.leftText = leftText;
            this.rightText = rightText;
            this.footerText = footerText;

            //gridPen.LineCap = XLineCap.Round;
            //thickGridPen.LineCap = XLineCap.Round;
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

        public void CreatePdfStitches(string pathToSave, int[,] matrixOfNewColors, List<Color> colorMeans, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Eduardo testando PDF";

            CreateFirstPage(document, matrixOfNewColors, colorMeans, backstitchLines, backstitchColors);

            CreateAllDrawingPages(document, matrixOfNewColors, colorMeans, backstitchLines, backstitchColors);

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

        private void CreateFirstPage(PdfDocument document, int[,] matrixOfNewColors, List<Color> colorMeans, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors)
        {

            PdfPage currentPage = document.AddPage();

            XGraphics pageGraphics = XGraphics.FromPdfPage(currentPage);
            PreparePage(currentPage, pageGraphics);

            double maxWidth = maxHorizontalNumberOfSquares * sizeOfEachSquare;
            double maxHeight = maxVerticalNumberOfSquares * sizeOfEachSquare;

            ImageTransformations.RescaleImage(matrixOfNewColors.GetLength(0), matrixOfNewColors.GetLength(1), maxWidth, maxHeight, out double newWidth, out double newHeight);

            double firstPageSizeOfEachSquare = newWidth / matrixOfNewColors.GetLength(0);

            XPoint startingPoint = new XPoint((currentPage.Width - newWidth) * 0.5, startingPointForDrawings.Y);

            for (int y = 0; y < matrixOfNewColors.GetLength(1); y++)
            {
                for (int x = 0; x < matrixOfNewColors.GetLength(0); x++)
                {
                    DrawStitchAtPosition(pageGraphics, matrixOfNewColors, colorMeans, x, y, x, y, startingPoint, firstPageSizeOfEachSquare);

                    //top numbers
                    if(y == 0 && x % 10 == 0 && x != 0)
                    {
                        WriteGridNumber(pageGraphics, firstPageSizeOfEachSquare, x, startingPoint.X + x * firstPageSizeOfEachSquare, startingPoint.Y, true, 0);
                    }
                    //bottom numbers
                    if (y == matrixOfNewColors.GetLength(1) - 1 && x % 10 == 0 && x != 0)
                    {
                        WriteGridNumber(pageGraphics, firstPageSizeOfEachSquare, x, startingPoint.X + x * firstPageSizeOfEachSquare, startingPoint.Y + matrixOfNewColors.GetLength(1) * firstPageSizeOfEachSquare, false, 0);
                    }
                    //left numbers
                    if (x == 0 && y % 10 == 0 && y != 0)
                    {
                        WriteGridNumber(pageGraphics, firstPageSizeOfEachSquare, y, startingPoint.X, startingPoint.Y + y * firstPageSizeOfEachSquare, true, -90);
                    }
                    //right numbers
                    if (x == matrixOfNewColors.GetLength(0) - 1 && y % 10 == 0 && y != 0)
                    {
                        WriteGridNumber(pageGraphics, firstPageSizeOfEachSquare, y, startingPoint.X + matrixOfNewColors.GetLength(0) * firstPageSizeOfEachSquare, startingPoint.Y + y * firstPageSizeOfEachSquare, false, -90);
                    }

                    double distanceFactorFromGrid = 2;
                    if(y == 0 && x == (int)(matrixOfNewColors.GetLength(0) * 0.5))
                    {                   
                        //top arrow
                        DrawArrowAtPositionWithScale(   pageGraphics, 
                                                        x % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare),
                                                        0, 
                                                        0, 
                                                        startingPoint.X + x * firstPageSizeOfEachSquare, 
                                                        startingPoint.Y - distanceFactorFromGrid * firstPageSizeOfEachSquare, 
                                                        firstPageSizeOfEachSquare / sizeOfEachSquare);
                        //bottom arrow
                        DrawArrowAtPositionWithScale(   pageGraphics, 
                                                        x % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare), 
                                                        0, 
                                                        180, 
                                                        startingPoint.X + x * firstPageSizeOfEachSquare, 
                                                        startingPoint.Y + distanceFactorFromGrid * firstPageSizeOfEachSquare + matrixOfNewColors.GetLength(1) * firstPageSizeOfEachSquare, 
                                                        firstPageSizeOfEachSquare / sizeOfEachSquare);
                    }

                    if (y == (int)(matrixOfNewColors.GetLength(1) * 0.5) && x == 0)
                    {
                        //left arrow
                        DrawArrowAtPositionWithScale(   pageGraphics,
                                                        0,
                                                        y % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare),
                                                        -90,
                                                        startingPoint.X - distanceFactorFromGrid * firstPageSizeOfEachSquare,
                                                        startingPoint.Y + y * firstPageSizeOfEachSquare,
                                                        firstPageSizeOfEachSquare / sizeOfEachSquare);
                        //right arrow
                        DrawArrowAtPositionWithScale(   pageGraphics,
                                                        0,
                                                        y % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare),
                                                        90,
                                                        startingPoint.X + distanceFactorFromGrid * firstPageSizeOfEachSquare + matrixOfNewColors.GetLength(0) * firstPageSizeOfEachSquare,
                                                        startingPoint.Y + y * firstPageSizeOfEachSquare,
                                                        firstPageSizeOfEachSquare / sizeOfEachSquare);
                    }

                    if (y == matrixOfNewColors.GetLength(1) - 1)
                    {
                        //Drawing current vertical line
                        DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, y, x, x, startingPoint, firstPageSizeOfEachSquare, firstPageSizeOfEachSquare / sizeOfEachSquare);
                    }
                }
                //Drawing current horizontal line
                DrawHorizontalGridLine(pageGraphics, matrixOfNewColors, y, y, matrixOfNewColors.GetLength(0), startingPoint, firstPageSizeOfEachSquare, firstPageSizeOfEachSquare / sizeOfEachSquare);
            }


            //drawing thick grid lines
            for (int y = 0; y <= matrixOfNewColors.GetLength(1); y += 10)
            {
                //Drawing current horizontal line
                DrawHorizontalGridLine(pageGraphics, matrixOfNewColors, y, y, matrixOfNewColors.GetLength(0), startingPoint, firstPageSizeOfEachSquare, firstPageSizeOfEachSquare / sizeOfEachSquare);
            }

            for (int x = 0; x <= matrixOfNewColors.GetLength(0); x += 10)
            {
                //Drawing current vertical line
                DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, matrixOfNewColors.GetLength(1) - 1, x, x, startingPoint, firstPageSizeOfEachSquare, firstPageSizeOfEachSquare / sizeOfEachSquare);
            }

            //Drawing backstitch lines
            foreach (int backstitchLineIndex in backstitchLines.Keys)
            {
                foreach (BackstitchLine backstitch in backstitchLines[backstitchLineIndex])
                {
                    double firstPageBackstitchThickness = backstitchLineThickness * (firstPageSizeOfEachSquare / sizeOfEachSquare);
                    XPen backstitchPen = GetRoundedPenFromColorAndThickness(backstitchColors[backstitchLineIndex], firstPageBackstitchThickness);
                    pageGraphics.DrawLine(backstitchPen,
                                    startingPoint.X + backstitch.startingPosition.Item1 * firstPageSizeOfEachSquare,
                                    startingPoint.Y + backstitch.startingPosition.Item2 * firstPageSizeOfEachSquare,
                                    startingPoint.X + backstitch.endingPosition.Item1 * firstPageSizeOfEachSquare,
                                    startingPoint.Y + backstitch.endingPosition.Item2 * firstPageSizeOfEachSquare);
                }
            }
        }

        private void WriteGridNumber(XGraphics pageGraphics, double squareSize, int number, double positionX, double positionY, bool isUpperNumber, double angle)
        {
            XFont lineNumberFont = new XFont("Verdana", 10 * (squareSize / sizeOfEachSquare), XFontStyle.Regular);
            XBrush lineNumberBrush = XBrushes.Black;
            XRect lineNumberRect;
            if (isUpperNumber)
            {
                lineNumberRect = new XRect(positionX - lineNumberFont.Size, positionY - 0.3 * squareSize - 1.5 * lineNumberFont.Size, 2 * lineNumberFont.Size, lineNumberFont.Size);
            }
            else
            {
                lineNumberRect = new XRect(positionX - lineNumberFont.Size, positionY + 0.3 * squareSize + 0.5 * lineNumberFont.Size, 2 * lineNumberFont.Size, lineNumberFont.Size);
            }
            pageGraphics.RotateAtTransform(angle, new XPoint(positionX, positionY));
            pageGraphics.DrawString(number.ToString(), lineNumberFont, lineNumberBrush, lineNumberRect, XStringFormats.Center);
            pageGraphics.RotateAtTransform(-angle, new XPoint(positionX, positionY));
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

        private void CreateAllDrawingPages(PdfDocument document, int[,] matrixOfNewColors, List<Color> colorMeans, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors)
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

                    DrawStitchesOnPage(currentPage, pageGraphics, matrixOfNewColors, colorMeans, x, y, backstitchLines, backstitchColors);
                }
            }
        }

        private void DrawStitchesOnPage(PdfPage currentPage, XGraphics pageGraphics, int[,] matrixOfNewColors, List<Color> colorMeans, int x, int y, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors)
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
                    DrawStitchAtPosition(pageGraphics, matrixOfNewColors, colorMeans, relativeIndexI, relativeIndexJ, i, j, startingPointForDrawings, sizeOfEachSquare);
                    
                    //top numbers
                    if (j == y * maxVerticalNumberOfSquares && i % 10 == 0 && i != x * maxHorizontalNumberOfSquares)
                    {
                        WriteGridNumber(pageGraphics, sizeOfEachSquare, i, startingPointForDrawings.X + (i - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare, startingPointForDrawings.Y, true, 0);
                    }
                    //bottom numbers
                    if ((j == (y + 1) * maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1) && i % 10 == 0 && i != x * maxHorizontalNumberOfSquares)
                    {
                        WriteGridNumber(pageGraphics, sizeOfEachSquare, i, startingPointForDrawings.X + (i - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare, startingPointForDrawings.Y + (j + 1 - y * maxVerticalNumberOfSquares) * sizeOfEachSquare, false, 0);
                    }
                    //left numbers
                    if (i == x * maxHorizontalNumberOfSquares && j % 10 == 0 && j != y * maxVerticalNumberOfSquares)
                    {
                        WriteGridNumber(pageGraphics, sizeOfEachSquare, j, startingPointForDrawings.X, startingPointForDrawings.Y + (j - y * maxVerticalNumberOfSquares) * sizeOfEachSquare, true, -90);
                    }
                    //right numbers
                    if ((i == (x + 1) * maxHorizontalNumberOfSquares - 1 || i == matrixOfNewColors.GetLength(0) - 1) && j % 10 == 0 && j != y * maxVerticalNumberOfSquares)
                    {
                        WriteGridNumber(pageGraphics, sizeOfEachSquare, j, startingPointForDrawings.X + (i + 1 - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare, startingPointForDrawings.Y + (j - y * maxVerticalNumberOfSquares) * sizeOfEachSquare, false, -90);
                    }

                    double distanceFactorFromGrid = 2;
                    //top arrow
                    if (j == y * maxVerticalNumberOfSquares && i == (int)(matrixOfNewColors.GetLength(0) * 0.5))
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        i % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        0,
                                                        0,
                                                        startingPointForDrawings.X + i * sizeOfEachSquare,
                                                        startingPointForDrawings.Y - distanceFactorFromGrid * sizeOfEachSquare,
                                                        1);
                    }

                    //bottom arrow
                    if ((j == (y + 1) * maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1) && i == (int)(matrixOfNewColors.GetLength(0) * 0.5))
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        i % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        0,
                                                        180,
                                                        startingPointForDrawings.X + i * sizeOfEachSquare,
                                                        startingPointForDrawings.Y + distanceFactorFromGrid * sizeOfEachSquare + (j + 1 - y * maxVerticalNumberOfSquares) * sizeOfEachSquare,
                                                        1);
                    }

                    //left arrow
                    if (j == (int)(matrixOfNewColors.GetLength(1) * 0.5) && i == x * maxHorizontalNumberOfSquares)
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        0,
                                                        j % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        -90,
                                                        startingPointForDrawings.X - distanceFactorFromGrid * sizeOfEachSquare,
                                                        startingPointForDrawings.Y + j * sizeOfEachSquare,
                                                        1);
                    }

                    //right arrow
                    if (j == (int)(matrixOfNewColors.GetLength(1) * 0.5) && (i == (x + 1) * maxHorizontalNumberOfSquares - 1 || i == matrixOfNewColors.GetLength(0) - 1))
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        0,
                                                        j % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        90,
                                                        startingPointForDrawings.X + distanceFactorFromGrid * sizeOfEachSquare + (i + 1 - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare,
                                                        startingPointForDrawings.Y + j * sizeOfEachSquare,
                                                        1);
                    }

                    if (relativeIndexJ == maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1)
                    {
                        //Drawing current vertical line
                        DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, relativeIndexJ, relativeIndexI, i, startingPointForDrawings, sizeOfEachSquare);
                    }

                    relativeIndexI++;
                }

                //Drawing current horizontal line
                DrawHorizontalGridLine(pageGraphics, matrixOfNewColors, relativeIndexJ, j, relativeIndexI, startingPointForDrawings, sizeOfEachSquare);

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

                DrawHorizontalGridLine(pageGraphics, matrixOfNewColors, relativeIndexJ, j, relativeIndexI, startingPointForDrawings, sizeOfEachSquare);
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
                        DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, relativeIndexJ, relativeIndexI, i, startingPointForDrawings, sizeOfEachSquare);
                    }
                    break;
                }

                DrawVerticalGridLine(pageGraphics, matrixOfNewColors, x, relativeIndexJ, relativeIndexI, i, startingPointForDrawings, sizeOfEachSquare);
                //if (i >= matrixOfNewColors.GetLength(0)) break;

                relativeIndexI += 10;
            }

            //Drawing backstitch lines
            foreach (int backstitchLineIndex in backstitchLines.Keys)
            {
                foreach (BackstitchLine backstitch in backstitchLines[backstitchLineIndex])
                {
                    XPen backstitchPen = GetRoundedPenFromColorAndThickness(backstitchColors[backstitchLineIndex], backstitchLineThickness);

                    List<Tuple<double, double>> currentBackstitchPoints = ImageTransformations.FindIntersectionsOfLineAndSquare(
                                                                                                        backstitch.startingPosition.Item1, backstitch.startingPosition.Item2,
                                                                                                        backstitch.endingPosition.Item1, backstitch.endingPosition.Item2,
                                                                                                        x * maxHorizontalNumberOfSquares, y * maxVerticalNumberOfSquares,
                                                                                                        maxHorizontalNumberOfSquares, maxVerticalNumberOfSquares);
                    if(currentBackstitchPoints.Count == 2)
                    {
                        pageGraphics.DrawLine(backstitchPen,
                                    startingPointForDrawings.X + (currentBackstitchPoints[0].Item1 - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare,
                                    startingPointForDrawings.Y + (currentBackstitchPoints[0].Item2 - y * maxVerticalNumberOfSquares) * sizeOfEachSquare,
                                    startingPointForDrawings.X + (currentBackstitchPoints[1].Item1 - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare,
                                    startingPointForDrawings.Y + (currentBackstitchPoints[1].Item2 - y * maxVerticalNumberOfSquares) * sizeOfEachSquare);
                    }
                }
            }
        }

        private void DrawStitchAtPosition(XGraphics pageGraphics, int[,] matrixOfNewColors, List<Color> colorMeans, int relativeIndexI, int relativeIndexJ, int i, int j,
                                            XPoint startingPoint, double squareSize)
        {
            Color currentPositionColor = colorMeans[matrixOfNewColors[i, j]];
            XSolidBrush brush = new XSolidBrush(XColor.FromArgb(currentPositionColor.R, currentPositionColor.G, currentPositionColor.B));

            XRect positionToDrawRect = new XRect(   startingPoint.X + relativeIndexI * squareSize,
                                                    startingPoint.Y + relativeIndexJ * squareSize,
                                                    squareSize,
                                                    squareSize);

            pageGraphics.DrawRectangle(brush, positionToDrawRect);
        }

        private void DrawHorizontalGridLine(XGraphics pageGraphics, int[,] matrixOfNewColors, int relativeIndexJ, int j, int relativeIndexI,
                                            XPoint startingPoint, double squareSize, double thicknessMultiplicationFactor = 1.0)
        {
            XPen penToUse = GetPenBasedOnPosition(j, thicknessMultiplicationFactor);
            pageGraphics.DrawLine(penToUse,
                                    startingPoint.X,
                                    startingPoint.Y + relativeIndexJ * squareSize,
                                    startingPoint.X + relativeIndexI * squareSize,
                                    startingPoint.Y + relativeIndexJ * squareSize);

            //Also draw last horizontal line of the current page
            if (relativeIndexJ == maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1)
            {
                penToUse = GetPenBasedOnPosition(j + 1, thicknessMultiplicationFactor);
                pageGraphics.DrawLine(penToUse,
                                        startingPoint.X,
                                        startingPoint.Y + (relativeIndexJ + 1) * squareSize,
                                        startingPoint.X + relativeIndexI * squareSize,
                                        startingPoint.Y + (relativeIndexJ + 1) * squareSize);
            }
        }

        private void DrawVerticalGridLine(XGraphics pageGraphics, int[,] matrixOfNewColors, int x, int relativeIndexJ, int relativeIndexI, int i,
                                            XPoint startingPoint, double squareSize, double thicknessMultiplicationFactor = 1.0)
        {
            XPen penToUse = GetPenBasedOnPosition(i, thicknessMultiplicationFactor);
            //penToUse = gridPen;
            pageGraphics.DrawLine(penToUse,
                                    startingPoint.X + relativeIndexI * squareSize,
                                    startingPoint.Y,
                                    startingPoint.X + relativeIndexI * squareSize,
                                    startingPoint.Y + (relativeIndexJ + 1) * squareSize);

            if (relativeIndexI == (x + 1) * maxHorizontalNumberOfSquares - 1 || i == matrixOfNewColors.GetLength(0) - 1)
            {
                penToUse = GetPenBasedOnPosition(i + 1, thicknessMultiplicationFactor);
                //penToUse = gridPen;
                pageGraphics.DrawLine(penToUse,
                                        startingPoint.X + (relativeIndexI + 1) * squareSize,
                                        startingPoint.Y,
                                        startingPoint.X + (relativeIndexI + 1) * squareSize,
                                        startingPoint.Y + (relativeIndexJ + 1) * squareSize);
            }
        }

        //Draws an arrow pointing down
        private void DrawArrow(XGraphics pageGraphics, XPoint pointOfTheArrow, double sizeMultiplicationFactor = 1)
        {
            XPoint baseOfTheArrow = new XPoint(pointOfTheArrow.X, pointOfTheArrow.Y - sizeOfEachSquare * 1.5 * sizeMultiplicationFactor);
            XPoint leftCorner = new XPoint(baseOfTheArrow.X - sizeOfEachSquare * 0.5 * sizeMultiplicationFactor, baseOfTheArrow.Y);
            XPoint rightCorner = new XPoint(baseOfTheArrow.X + sizeOfEachSquare * 0.5 * sizeMultiplicationFactor, baseOfTheArrow.Y);

            XPoint[] trianglePoints = { pointOfTheArrow, leftCorner, rightCorner };

            pageGraphics.DrawPolygon(arrowBrush, trianglePoints, XFillMode.Alternate);

            XPoint arrowLineBase = new XPoint(baseOfTheArrow.X, baseOfTheArrow.Y - sizeOfEachSquare * 0.7 * sizeMultiplicationFactor);
            XPen pen = GetRoundedPenFromColorAndThickness(arrowBrush.Color, gridPenThickness * 2 * sizeMultiplicationFactor);
            pen.LineCap = XLineCap.Flat;
            pageGraphics.DrawLine(pen, baseOfTheArrow, arrowLineBase);
        }

        private void DrawArrowAtPositionWithScale(XGraphics pageGraphics, double xOffset, double yOffset, double angle, double xPosition, double yPosition, double multiplicationFactor = 1)
        {
            XPoint pointOfTheArrow = new XPoint(xPosition + xOffset,
                                                yPosition + yOffset);
            pageGraphics.RotateAtTransform(angle, pointOfTheArrow);
            DrawArrow(pageGraphics, pointOfTheArrow, multiplicationFactor);
            pageGraphics.RotateAtTransform(-angle, pointOfTheArrow);
        }

        private XPen GetPenBasedOnPosition(int number, double multiplicationFactor = 1.0)
        {
            Color colorToUse = number % 10 == 0 ? thickGridPenColor : gridPenColor;
            double thicknessToUse = (number % 10 == 0 ? thickGridPenThickness : gridPenThickness) * multiplicationFactor;

            XPen penToUse = GetRoundedPenFromColorAndThickness(colorToUse, thicknessToUse);
            return penToUse;
        }

        private XPen GetRoundedPenFromColorAndThickness(Color color, double thickness)
        {
            XColor xColor = XColor.FromArgb(color.R, color.G, color.B);
            XPen pen = new XPen(xColor, thickness);
            pen.LineCap = XLineCap.Round;

            return pen;
        }

        private XPen GetRoundedPenFromColorAndThickness(XColor xColor, double thickness)
        {
            XPen pen = new XPen(xColor, thickness);
            pen.LineCap = XLineCap.Round;

            return pen;
        }
    }

    public enum Alignement
    {
        Top,
        Bottom,
        Left,
        Right
    }
}

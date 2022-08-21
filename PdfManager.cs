using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
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
        List<PdfPage> allPages = new List<PdfPage>();
        private List<XGraphics> graphicsOfAllPages = new List<XGraphics>();
        
        private XColor mainColor = XColor.FromArgb(34, 62, 64);
        private XSolidBrush pageCountBrush;/*new XSolidBrush(XColor.FromArgb(105, 105, 105));*/
        private XFont pageCountFont = new XFont("Agency FB", 14, XFontStyle.Regular);

        private XSolidBrush pageSetupBrush;
        private XSolidBrush arrowBrush = XBrushes.DarkBlue;
        private XSolidBrush observationAboutRoundingBrush;

        XFont patternSizeFont = new XFont("Alike", 10, XFontStyle.Regular);
        XBrush patternSizeBrush;

        private XImage topLogo;
        private string title = "Title";
        private string secondTitle = "Second Title";
        private string subtitle = "Subtitle";
        private string leftText = "Left Text";
        private string rightText = "Right Text";
        private string footerText = "Footer Text";
        private string footerLink = "https://phinalia.com";
        private string secondFooterText = "Second Footer Text";
        private string[] socialMediaLinks;
        private XImage[] socialMediaImages;
        private string[] socialMediaNames;
        private XImage crossStitchSymbol;

        private XFont titleFont = new XFont("Amicale", 16.6, XFontStyle.Bold);
        private XFont subtitleFont = new XFont("Amicale", 8.3, XFontStyle.Bold);
        private XFont footerFont = new XFont("Amicale", 11.4, XFontStyle.Bold);
        private XFont socialMediaFont = new XFont("Alike", 8.3, XFontStyle.Regular);
        private XFont sideTextFont = new XFont("Verdana", 6.6, XFontStyle.Regular);

        private XFont pageNumberFont = new XFont("Verdana", 12, XFontStyle.Regular);


        private XFont listOfColorsTitleFont = new XFont("Verdana", 20, XFontStyle.Regular);
        private XFont listOfColorsFont = new XFont("Verdana", 10, XFontStyle.Regular);
        private XFont obesrvationAboutRoundingFont = new XFont("Verdana", 7, XFontStyle.Regular);
        string observationsAboutRoundingNumberOfSkeins = "The number of skeins is an approximated value assuming a 16-count Aida cloth with two strands of floss, it can vary depending on several factors.";



        private XPoint startingPointForDrawings = new XPoint(50, 210);
        private double sizeOfEachSquare = 8;

        private readonly int maxHorizontalNumberOfSquares = 56;
        private readonly int maxVerticalNumberOfSquares = 59;

        //private readonly XPen gridPen = new XPen(XColors.Gray, 0.5);
        //private readonly XPen thickGridPen = new XPen(XColors.Black, 1);

        private readonly Color gridPenColor = Color.Gray;
        private readonly Color thickGridPenColor = Color.Black;

        private readonly double gridPenThickness = 0.5;
        private readonly double thickGridPenThickness = 2;

        private readonly double backstitchLineThickness = 2;

        private readonly double crossStitchListOriginalTitleHeight = 40;
        private readonly double crossStitchListHeaderHeight = 20;
        private readonly double crossStitchListRowHeight = 15;
        private readonly int listsOfColorsNumberOfColumns = 1;
        private readonly double verticalPaddingBetweenTables = 15;

        public PdfManager()
        {
            //gridPen.LineCap = XLineCap.Round;
            //thickGridPen.LineCap = XLineCap.Round;
        }

        public PdfManager(Bitmap topLogo, string title, string secondTitle, string subtitle, string leftText, string rightText, string footerText, string footerLink, string secondFooterText, string[] socialMediaLinks, Bitmap[] socialMediaImages, string[] socialMediaNames)
        {
            this.topLogo = ConvertBitmapToXimage(topLogo);

            this.title = title;
            this.secondTitle = secondTitle;
            this.subtitle = subtitle;
            this.leftText = leftText;
            this.rightText = rightText;
            this.footerText = footerText;
            this.footerLink = footerLink;
            this.secondFooterText = secondFooterText;
            this.socialMediaLinks = socialMediaLinks;
            this.socialMediaImages = new XImage[socialMediaImages.Length];
            for (int i = 0; i < socialMediaImages.Length; i++)
            {                
                this.socialMediaImages[i] = ConvertBitmapToXimage(socialMediaImages[i]);
            }
            this.socialMediaNames = socialMediaNames;
            this.crossStitchSymbol = ConvertBitmapToXimage(Properties.Resources.CrossStitchSymbol);

            //gridPen.LineCap = XLineCap.Round;
            //thickGridPen.LineCap = XLineCap.Round;

            pageCountBrush = new XSolidBrush(mainColor);
            pageSetupBrush = new XSolidBrush(mainColor);
            patternSizeBrush = new XSolidBrush(mainColor);
        }

        public void CreatePdf(List<Bitmap> images, string pathToSave)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Eduardo testando PDF";

            foreach(var image in images)
            {
                PdfPage page = document.AddPage();
                allPages.Add(page);

                XGraphics pageGraphics = XGraphics.FromPdfPage(page);
                graphicsOfAllPages.Add(pageGraphics);
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

        public void CreatePdfStitches(string pathToSave, int[,] matrixOfNewColors, List<Color> colorMeans, Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, Dictionary<int, Bitmap> dictionaryOfSymbolByColor)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = title;

            Dictionary<int, XImage> dictionaryOfXimageByIndex = new Dictionary<int, XImage>();

            foreach (KeyValuePair<int, Bitmap> pair in dictionaryOfSymbolByColor)
            {
                dictionaryOfXimageByIndex.Add(pair.Key, PdfManager.ConvertBitmapToXimage(pair.Value));
            }

            CreateFirstPage(document, matrixOfNewColors, colorMeans, backstitchLines, backstitchColors, dictionaryOfXimageByIndex);

            CreateAllDrawingPages(document, matrixOfNewColors, colorMeans, backstitchLines, backstitchColors, dictionaryOfXimageByIndex);

            //CreatePagesWithListsOfColors(document, colorMeans, backstitchColors, dictionaryOfXimageByIndex);
            CreatePagesWithListsOfColors(document, colorMeans, positionsOfEachColor, backstitchLines, backstitchColors, dictionaryOfXimageByIndex, true);

            
            for (int i = 0; i < graphicsOfAllPages.Count; i++)
            {
                //graphicsOfAllPages
                double xFactorPosition = 0.9;
                double yFactorPosition = 0.92;
                XRect pageCountRect = new XRect(xFactorPosition * allPages[i].Width, yFactorPosition * allPages[i].Height, (1 - xFactorPosition) * allPages[i].Width, (1 - yFactorPosition) * allPages[i].Height);
                graphicsOfAllPages[i].DrawString((i + 1).ToString() + " / " + graphicsOfAllPages.Count.ToString(), pageCountFont, pageCountBrush, pageCountRect, XStringFormats.CenterLeft);
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

        private void CreateFirstPage(PdfDocument document, int[,] matrixOfNewColors, List<Color> colorMeans, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, Dictionary<int, XImage> dictionaryOfXimageByIndex)
        {
            XGraphics pageGraphics = AddPageAndPrepare(document, out PdfPage currentPage);
            
            double maxWidth = maxHorizontalNumberOfSquares * sizeOfEachSquare;
            double maxHeight = maxVerticalNumberOfSquares * sizeOfEachSquare;

            ImageTransformations.RescaleImage(matrixOfNewColors.GetLength(0), matrixOfNewColors.GetLength(1), maxWidth, maxHeight, out double newWidth, out double newHeight);

            double firstPageSizeOfEachSquare = newWidth / matrixOfNewColors.GetLength(0);
            //double firstPageSizeOfEachSquare = sizeOfEachSquare;

            XPoint startingPoint = new XPoint((currentPage.Width - newWidth) * 0.5, startingPointForDrawings.Y);

            for (int y = 0; y < matrixOfNewColors.GetLength(1); y++)
            {
                for (int x = 0; x < matrixOfNewColors.GetLength(0); x++)
                {
                    DrawStitchAtPosition(pageGraphics, matrixOfNewColors, colorMeans, x, y, x, y, startingPoint, firstPageSizeOfEachSquare, dictionaryOfXimageByIndex, true, false);

                    //top numbers
                    if(y == 0 && x % 10 == 0 && x != 0)
                    {
                        WriteGridNumber(pageGraphics, /*firstPageSizeOfEachSquare*/sizeOfEachSquare, x, startingPoint.X + x * firstPageSizeOfEachSquare, startingPoint.Y, true, 0);
                    }
                    //bottom numbers
                    if (y == matrixOfNewColors.GetLength(1) - 1 && x % 10 == 0 && x != 0)
                    {
                        WriteGridNumber(pageGraphics, /*firstPageSizeOfEachSquare*/sizeOfEachSquare, x, startingPoint.X + x * firstPageSizeOfEachSquare, startingPoint.Y + matrixOfNewColors.GetLength(1) * firstPageSizeOfEachSquare, false, 0);
                    }
                    //left numbers
                    if (x == 0 && y % 10 == 0 && y != 0)
                    {
                        WriteGridNumber(pageGraphics, /*firstPageSizeOfEachSquare*/sizeOfEachSquare, y, startingPoint.X, startingPoint.Y + y * firstPageSizeOfEachSquare, true, -90);
                    }
                    //right numbers
                    if (x == matrixOfNewColors.GetLength(0) - 1 && y % 10 == 0 && y != 0)
                    {
                        WriteGridNumber(pageGraphics, /*firstPageSizeOfEachSquare*/sizeOfEachSquare, y, startingPoint.X + matrixOfNewColors.GetLength(0) * firstPageSizeOfEachSquare, startingPoint.Y + y * firstPageSizeOfEachSquare, false, -90);
                    }

                    double distanceFactorFromGrid = 2;
                    //double sizeFactor = firstPageSizeOfEachSquare / sizeOfEachSquare;
                    double sizeFactor = 1.0;
                    if (y == 0 && x == (int)(matrixOfNewColors.GetLength(0) * 0.5))
                    {                   
                        //top arrow
                        DrawArrowAtPositionWithScale(   pageGraphics,
                                                        matrixOfNewColors.GetLength(0) % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare),
                                                        0, 
                                                        0, 
                                                        startingPoint.X + x * firstPageSizeOfEachSquare, 
                                                        startingPoint.Y - distanceFactorFromGrid * /*firstPageSizeOfEachSquare*/sizeOfEachSquare,
                                                        sizeFactor);
                        //bottom arrow
                        DrawArrowAtPositionWithScale(   pageGraphics,
                                                        matrixOfNewColors.GetLength(0) % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare), 
                                                        0, 
                                                        180, 
                                                        startingPoint.X + x * firstPageSizeOfEachSquare, 
                                                        startingPoint.Y + distanceFactorFromGrid * /*firstPageSizeOfEachSquare*/sizeOfEachSquare + matrixOfNewColors.GetLength(1) * firstPageSizeOfEachSquare,
                                                        sizeFactor);
                    }

                    if (y == (int)(matrixOfNewColors.GetLength(1) * 0.5) && x == 0)
                    {
                        //left arrow
                        DrawArrowAtPositionWithScale(   pageGraphics,
                                                        0,
                                                        matrixOfNewColors.GetLength(1) % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare),
                                                        -90,
                                                        startingPoint.X - distanceFactorFromGrid * /*firstPageSizeOfEachSquare*/sizeOfEachSquare,
                                                        startingPoint.Y + y * firstPageSizeOfEachSquare,
                                                        sizeFactor);
                        //right arrow
                        DrawArrowAtPositionWithScale(   pageGraphics,
                                                        0,
                                                        matrixOfNewColors.GetLength(1) % 2 == 0 ? 0 : (0.5 * firstPageSizeOfEachSquare),
                                                        90,
                                                        startingPoint.X + distanceFactorFromGrid * /*firstPageSizeOfEachSquare*/sizeOfEachSquare + matrixOfNewColors.GetLength(0) * firstPageSizeOfEachSquare,
                                                        startingPoint.Y + y * firstPageSizeOfEachSquare,
                                                        sizeFactor);
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

            //Size of the pattern in number of stitches
            string patternSizeText = "Design size: " + matrixOfNewColors.GetLength(0).ToString() + " x " + matrixOfNewColors.GetLength(1).ToString() + " stitches";
            double yPosition = startingPoint.Y + maxHeight + 3.25 * patternSizeFont.Size;
            XRect patternSizeRect = new XRect(0, yPosition, currentPage.Width, currentPage.Height * 0.88 - yPosition);
            pageGraphics.DrawString(patternSizeText, patternSizeFont, patternSizeBrush, patternSizeRect, XStringFormats.Center);
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
            //Drawing lines
            XPen pen = GetRoundedPenFromColorAndThickness(pageSetupBrush.Color, 1.0);
            pageGraphics.DrawLine(pen, page.Width * 0.05, page.Height * 0.13, page.Width * 0.95, page.Height * 0.13);
            pageGraphics.DrawLine(pen, page.Width * 0.05, page.Height * 0.88, page.Width * 0.95, page.Height * 0.88);

            //Top logo
            double topLogoSquareSize = 200;
            XRect topLogoRect = new XRect((page.Width - topLogoSquareSize) * 0.5, page.Height * 0.1f - 0.55 * topLogoSquareSize, topLogoSquareSize, topLogoSquareSize);
            pageGraphics.DrawImage(topLogo, topLogoRect);

            //title
            pageGraphics.DrawString(title, titleFont, pageSetupBrush, new XRect(0, page.Height * 0.1557, page.Width, titleFont.Size), XStringFormats.Center);
            //second title
            pageGraphics.DrawString(secondTitle, titleFont, pageSetupBrush, new XRect(0, page.Height * 0.1557 + titleFont.Size, page.Width, titleFont.Size), XStringFormats.Center);

            //subtitle
            pageGraphics.DrawString(subtitle, subtitleFont, pageSetupBrush, new XRect(0, page.Height * 0.186, page.Width, subtitleFont.Size), XStringFormats.Center);

            //left text
            pageGraphics.RotateAtTransform(-90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
            pageGraphics.DrawString(leftText, sideTextFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.33f), XStringFormats.Center);
            pageGraphics.RotateAtTransform(90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));

            //right text
            pageGraphics.RotateAtTransform(90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));
            pageGraphics.DrawString(rightText, sideTextFont, pageSetupBrush, new XRect(0, 0, page.Width, page.Height * 0.33f), XStringFormats.Center);
            pageGraphics.RotateAtTransform(-90, new XPoint(page.Width * 0.5f, page.Height * 0.5f));

            //bottom
            PrepareFooterOfPage(page, pageGraphics);
        }

        private void PrepareFooterOfPage(PdfPage page, XGraphics pageGraphics)
        {
            //pageGraphics.DrawString(footerText, footerFont, pageSetupBrush, new XRect(0, page.Height * 0.95f, page.Width, page.Height * 0.05f), XStringFormats.Center);
            double totalFooterWidth = (footerText.Length + footerLink.Length) * footerFont.Size * 0.5;

            double bottomY = page.Height * 0.88f;
            XRect footerTextRect = new XRect((page.Width - totalFooterWidth) * 0.5, bottomY, page.Width, page.Height * 0.05f);
            pageGraphics.DrawString(footerText, footerFont, pageSetupBrush, footerTextRect, XStringFormats.CenterLeft);

            XRect footerLinkRect = new XRect((page.Width - totalFooterWidth) * 0.5 + footerText.Length * footerFont.Size * 0.5, footerTextRect.Y, /*page.Width*/footerLink.Length * footerFont.Size * 0.5, page.Height * 0.05f);
            pageGraphics.DrawString(footerLink, footerFont, pageSetupBrush, footerLinkRect, XStringFormats.CenterLeft);

            PdfRectangle linkRect = new PdfRectangle(pageGraphics.Transformer.WorldToDefaultPage(footerLinkRect));
            page.AddWebLink(linkRect, footerLink);

            double verticalPadding = footerFont.Size * 0.5;
            XRect secondFooterRect = new XRect(0, footerTextRect.Y + footerFont.Size + verticalPadding, page.Width, page.Height * 0.05f);
            pageGraphics.DrawString(secondFooterText, footerFont, pageSetupBrush, secondFooterRect, XStringFormats.Center);

            double socialMediaIconSquareSize = socialMediaFont.Size * 2.4;
            double socialMediaY = secondFooterRect.Y + secondFooterRect.Height/* + verticalPadding*/ - socialMediaFont.Size * 0.5;
            double totalLengthOfSocialMediaNames = 0;
            for (int i = 0; i < socialMediaLinks.Length; i++)
            {
                totalLengthOfSocialMediaNames += socialMediaNames[i].Length;
            }
            double horizontalPadding = socialMediaFont.Size;
            double socialMediaX = (page.Width - (socialMediaLinks.Length * socialMediaIconSquareSize + totalLengthOfSocialMediaNames * socialMediaFont.Size * 0.5 + horizontalPadding * (3 * socialMediaLinks.Length - 1))) * 0.5;

            for (int i = 0; i < socialMediaLinks.Length; i++)
            {
                XRect socialMediaLogoRect = new XRect(socialMediaX, socialMediaY, socialMediaIconSquareSize, socialMediaIconSquareSize);
                pageGraphics.DrawImage(socialMediaImages[i], socialMediaLogoRect);

                XRect socialMediaNameRect = new XRect(socialMediaX + socialMediaIconSquareSize + horizontalPadding, socialMediaY, socialMediaNames[i].Length * socialMediaFont.Size * 0.5, socialMediaIconSquareSize);
                pageGraphics.DrawString(socialMediaNames[i], socialMediaFont, pageSetupBrush, socialMediaNameRect, XStringFormats.Center);

                XRect socialMediaLinkRect = new XRect(socialMediaX, socialMediaY, socialMediaLogoRect.Width + socialMediaNameRect.Width, socialMediaIconSquareSize);
                PdfRectangle socialMediaLinkPdfRect = new PdfRectangle(pageGraphics.Transformer.WorldToDefaultPage(socialMediaLinkRect));
                page.AddWebLink(socialMediaLinkPdfRect, socialMediaLinks[i]);

                socialMediaX += socialMediaLogoRect.Width + horizontalPadding + socialMediaNameRect.Width + 2 * horizontalPadding;
            }
        }

        private void CreateAllDrawingPages(PdfDocument document, int[,] matrixOfNewColors, List<Color> colorMeans, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, Dictionary<int, XImage> dictionaryOfXimageByIndex)
        {
            int horizontalNumberOfTimes = matrixOfNewColors.GetLength(0) / maxHorizontalNumberOfSquares + 1;
            int verticalNumberOfTimes = matrixOfNewColors.GetLength(1) / maxVerticalNumberOfSquares + 1;

            for (int y = 0; y < verticalNumberOfTimes; y++)
            {
                for (int x = 0; x < horizontalNumberOfTimes; x++)
                {
                    XGraphics pageGraphics = AddPageAndPrepare(document, out PdfPage currentPage);

                    startingPointForDrawings.X = (currentPage.Width - (sizeOfEachSquare * maxHorizontalNumberOfSquares)) * 0.5f;

                    DrawStitchesOnPage(currentPage, pageGraphics, matrixOfNewColors, colorMeans, x, y, backstitchLines, backstitchColors, dictionaryOfXimageByIndex);
                }
            }
        }

        private void CreatePagesWithListsOfColors(PdfDocument document, List<Color> colorMeans, Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, Dictionary<int, XImage> dictionaryOfXimageByIndex, bool convertToDmcColors = true)
        {
            //throw new NotImplementedException();

            double startingHeight = startingPointForDrawings.Y;
            XGraphics pageGraphics = AddPageAndPrepare(document, out _);
            //cross stitch list
            DrawListOfColors(document, ref pageGraphics, ref startingHeight, colorMeans, positionsOfEachColor, backstitchLines, backstitchColors, dictionaryOfXimageByIndex, true, convertToDmcColors, true);
            
            //backstitch list
            DrawListOfColors(document, ref pageGraphics, ref startingHeight, colorMeans, positionsOfEachColor, backstitchLines, backstitchColors, dictionaryOfXimageByIndex, false, convertToDmcColors, true);
        }

        private void DrawListOfColors(PdfDocument document, ref XGraphics pageGraphics, ref double startingHeight, List<Color> colorMeans, Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, Dictionary<int, XImage> dictionaryOfXimageByIndex, bool isCrossStitchAndNotBackstitch = true, bool convertToDmcColors = true, bool includeNumberOfStitches = true, bool includeNumberOfSkeins = true)
        {
            double crossStitchListTitleHeight = isCrossStitchAndNotBackstitch ? 135 : crossStitchListOriginalTitleHeight;

            double heightToStartList = startingHeight + crossStitchListTitleHeight;
            double maxHeightToEndList = startingPointForDrawings.Y + maxVerticalNumberOfSquares * sizeOfEachSquare;
            double listWidth = maxHorizontalNumberOfSquares * sizeOfEachSquare;

            double widthOfEachColumn = listWidth / listsOfColorsNumberOfColumns;

            int totalAmountOfColors = isCrossStitchAndNotBackstitch ? colorMeans.Count : backstitchColors.Count;

            int indexOfTheFirstColorOfTheCurrentPage = 0;
            if(colorMeans.Count > 0 && colorMeans[0].A == 0)
            {
                indexOfTheFirstColorOfTheCurrentPage = 1; //skip the first color because it is the empty color
            }

            if (totalAmountOfColors == 0) return;

            while(indexOfTheFirstColorOfTheCurrentPage < totalAmountOfColors)
            {
                //if starting at a position too down in the page, create a new one and start from there
                if(startingHeight + crossStitchListTitleHeight + crossStitchListHeaderHeight + crossStitchListRowHeight > maxHeightToEndList)
                {
                    pageGraphics = AddPageAndPrepare(document, out _);
                    startingHeight = startingPointForDrawings.Y;
                    heightToStartList = startingHeight + crossStitchListTitleHeight;
                }

                heightToStartList += crossStitchListHeaderHeight;

                XRect titleRect = new XRect(startingPointForDrawings.X, startingHeight, listWidth + 3 * sizeOfEachSquare, isCrossStitchAndNotBackstitch ? crossStitchListOriginalTitleHeight : crossStitchListTitleHeight);
                pageGraphics.DrawString(isCrossStitchAndNotBackstitch ? "Cross Stitch" : "Backstitch", listOfColorsTitleFont, pageSetupBrush, titleRect, XStringFormats.Center);
                if (isCrossStitchAndNotBackstitch)
                {
                    double listOfColorsBorderThickness = thickGridPenThickness * 0.5;
                    Color penColor = Color.FromArgb(pageSetupBrush.Color.R, pageSetupBrush.Color.G, pageSetupBrush.Color.B);
                    XPen pen = GetRoundedPenFromColorAndThickness(penColor, listOfColorsBorderThickness);
                    pageGraphics.DrawLine(pen, titleRect.X, titleRect.Y + titleRect.Height, titleRect.X + titleRect.Width, titleRect.Y + titleRect.Height);

                    double crossStitchSymbolWidth = titleRect.Width * 0.7;
                    double crossStitchSymbolHeight = crossStitchSymbol.Height * (crossStitchSymbolWidth / crossStitchSymbol.Width);
                    XRect crossStitchSymbolRect = new XRect(titleRect.X + (titleRect.Width - crossStitchSymbolWidth) * 0.5, titleRect.Y + crossStitchListOriginalTitleHeight + 0.05 * crossStitchSymbolHeight, crossStitchSymbolWidth, crossStitchSymbolHeight);
                    pageGraphics.DrawImage(crossStitchSymbol, crossStitchSymbolRect);
                }

                int amountOfColorsInTheCurrentPage;
                if (((totalAmountOfColors - indexOfTheFirstColorOfTheCurrentPage) / listsOfColorsNumberOfColumns) * crossStitchListRowHeight > (maxHeightToEndList - heightToStartList))
                {
                    //not all remaining colors fit in the current page
                    amountOfColorsInTheCurrentPage = (int)(((maxHeightToEndList - heightToStartList) / crossStitchListRowHeight) * listsOfColorsNumberOfColumns);
                }
                else
                {
                    //all remaining colors fit in the current page
                    amountOfColorsInTheCurrentPage = totalAmountOfColors - indexOfTheFirstColorOfTheCurrentPage;
                }

                int amountOfColorsPerColumn = (int)Math.Ceiling(((double)amountOfColorsInTheCurrentPage) / listsOfColorsNumberOfColumns);
                for (int column = 0; column < listsOfColorsNumberOfColumns; column++)
                {
                    //draw column header
                    List<string> headerStrings;

                    if (convertToDmcColors)
                    {
                        headerStrings = new List<string>() { "Symbol", "Dmc Color", "Name" };
                    }
                    else
                    {
                        headerStrings = new List<string>() { "Symbol", "Color" };
                    }

                    if (includeNumberOfStitches && isCrossStitchAndNotBackstitch)
                    {
                        headerStrings.Add("N° stitches");
                    }

                    if (includeNumberOfSkeins)
                    {
                        headerStrings.Add("N° skeins");
                    }

                    DrawListOfColorsRow(pageGraphics, headerStrings, startingPointForDrawings.X + column * widthOfEachColumn, heightToStartList - crossStitchListHeaderHeight, widthOfEachColumn, crossStitchListHeaderHeight, Color.Empty, false);

                    int startingIndex = indexOfTheFirstColorOfTheCurrentPage + amountOfColorsPerColumn * column;
                    for (int i = startingIndex; i - startingIndex < amountOfColorsPerColumn && i < totalAmountOfColors; i++)
                    {
                        //draw row
                        List< string> rowStrings;

                        //get color
                        Color currentColor = isCrossStitchAndNotBackstitch ? colorMeans[i] : backstitchColors[i];
                        if (convertToDmcColors)
                        {
                            DmcColor dmcColor = ColorsConverter.ConvertColorToDmc(currentColor);
                            rowStrings = new List<string>() { dmcColor.Number, dmcColor.Name };
                        }
                        else
                        {
                            string hexColor = "#" + currentColor.R.ToString("X2") + currentColor.G.ToString("X2") + currentColor.B.ToString("X2");
                            rowStrings = new List<string>() { hexColor };
                        }

                        if (includeNumberOfStitches && isCrossStitchAndNotBackstitch)
                        {
                            rowStrings.Add(positionsOfEachColor[i].Count.ToString());
                        }

                        if (includeNumberOfSkeins)
                        {
                            double numberOfSkeins;
                            if (isCrossStitchAndNotBackstitch)
                            {
                                numberOfSkeins = Math.Round(CalculateNumberOfSkeinsForCrossStitch(positionsOfEachColor[i].Count), 1);
                            }
                            else
                            {
                                numberOfSkeins = Math.Round(CalculateNumberOfSkeinsForBackstitch(backstitchLines[i], 16, 2), 1);
                            }
                            rowStrings.Add(numberOfSkeins.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture));
                        }

                        try
                        {
                            var xImage = dictionaryOfXimageByIndex[i];
                        }
                        catch (Exception e)
                        {

                        }

                        DrawListOfColorsRow(pageGraphics, rowStrings, startingPointForDrawings.X + column * widthOfEachColumn, heightToStartList + (i - startingIndex) * crossStitchListRowHeight, widthOfEachColumn, crossStitchListRowHeight, currentColor, true, isCrossStitchAndNotBackstitch ? dictionaryOfXimageByIndex[i] : null, isCrossStitchAndNotBackstitch);
                    }
                }

                DrawListOfColorsBorders(startingHeight, crossStitchListTitleHeight, crossStitchListHeaderHeight, crossStitchListRowHeight, listWidth, pageGraphics, amountOfColorsPerColumn);
                indexOfTheFirstColorOfTheCurrentPage += amountOfColorsInTheCurrentPage;
                startingHeight += crossStitchListTitleHeight + crossStitchListHeaderHeight + amountOfColorsPerColumn * crossStitchListRowHeight;

                //write observation about the number of skeins
                startingHeight += crossStitchListRowHeight;
                double observationHeight = 3 * listOfColorsFont.Size;
                XRect observationAboutRoundingRect = new XRect(startingPointForDrawings.X, startingHeight, listWidth + 3 * sizeOfEachSquare, observationHeight);                
                XTextFormatter textFormatter = new XTextFormatter(pageGraphics);
                textFormatter.DrawString(observationsAboutRoundingNumberOfSkeins, obesrvationAboutRoundingFont, pageSetupBrush, observationAboutRoundingRect, XStringFormats.TopLeft);

                startingHeight += observationHeight + crossStitchListRowHeight + verticalPaddingBetweenTables;
            }
        }

        private double CalculateNumberOfSkeinsForCrossStitch(int numberOfStitches, int aidaCount = 16, int numberOfStrandsUsed = 2)
        {
            double stitchesPerSkein = 17 * (15.0 / (6.0 / aidaCount)) * (6.0 / numberOfStrandsUsed);
            return numberOfStitches / stitchesPerSkein;
        }

        private double CalculateNumberOfSkeinsForBackstitch(HashSet<BackstitchLine> backstitchLines, int aidaCount = 16, int numberOfStrandsUsed = 2)
        {
            double lengthInInchesPerSkein = 17 * 18 * (6.0 / numberOfStrandsUsed); //length IN INCHES per skein
            double backstitchLengthInAbsoluteUnits = 0;

            foreach (BackstitchLine currentLine in backstitchLines)
            {
                double xGap = currentLine.startingPosition.Item1 - currentLine.endingPosition.Item1;
                double yGap = currentLine.startingPosition.Item2 - currentLine.endingPosition.Item2;

                backstitchLengthInAbsoluteUnits += Math.Sqrt(xGap * xGap + yGap * yGap);
            }

            double backstitchLengthInInches = backstitchLengthInAbsoluteUnits / aidaCount;

            return backstitchLengthInInches / lengthInInchesPerSkein;
        }

        private XGraphics AddPageAndPrepare(PdfDocument document, out PdfPage currentPage)
        {
            currentPage = document.AddPage();
            allPages.Add(currentPage);

            XGraphics pageGraphics = XGraphics.FromPdfPage(currentPage);
            graphicsOfAllPages.Add(pageGraphics);
            PreparePage(currentPage, pageGraphics);
            return pageGraphics;
        }

        private void DrawListOfColorsBorders(double startingHeight, double crossStitchListTitleHeight, double crossStitchListHeaderHeight, double crossStitchListRowHeight, double listWidth, XGraphics pageGraphics, int amountOfColorsPerColumn)
        {
            double listOfColorsBorderThickness = thickGridPenThickness * 0.5;
            double listOfColorsBorderWidth = listWidth + 3 * sizeOfEachSquare;
            Color penColor = Color.FromArgb(pageSetupBrush.Color.R, pageSetupBrush.Color.G, pageSetupBrush.Color.B);
            XPen pen = GetRoundedPenFromColorAndThickness(penColor, listOfColorsBorderThickness);
            pageGraphics.DrawLine(pen, startingPointForDrawings.X, startingHeight + crossStitchListTitleHeight, startingPointForDrawings.X + listOfColorsBorderWidth, startingHeight + crossStitchListTitleHeight);
            pageGraphics.DrawLine(pen, startingPointForDrawings.X, startingHeight + crossStitchListTitleHeight + crossStitchListHeaderHeight, startingPointForDrawings.X + listOfColorsBorderWidth, startingHeight + crossStitchListTitleHeight + crossStitchListHeaderHeight);
            DrawRectangleWithLines(pageGraphics, startingPointForDrawings.X, startingHeight, listOfColorsBorderWidth, crossStitchListTitleHeight + crossStitchListHeaderHeight + amountOfColorsPerColumn * crossStitchListRowHeight, listOfColorsBorderThickness, penColor);
        }

        private void DrawListOfColorsRow(XGraphics pageGraphics, List<string> rowStrings, double rowPositionX, double rowPositionY, double columnWidth, double columnHeight, Color color, bool drawSymbol = true, XImage symbol = null, bool isCrossStitchAndNotBackstitch = true)
        {
            //throw new NotImplementedException();

            int additionalFieldValue = (drawSymbol ? 1 : 0);

            int numberOfFields = rowStrings.Count + additionalFieldValue;

            double fieldWidth = columnWidth / numberOfFields;

            if (drawSymbol)
            {
                if (isCrossStitchAndNotBackstitch)
                {
                    XSolidBrush brush = new XSolidBrush(XColor.FromArgb(color.R, color.G, color.B));

                    pageGraphics.DrawImage(symbol, rowPositionX + 0.5 * (fieldWidth - sizeOfEachSquare) - 0.8 * sizeOfEachSquare, rowPositionY + 0.5 * (columnHeight - sizeOfEachSquare), sizeOfEachSquare, sizeOfEachSquare);

                    double xSquarePosition = rowPositionX + 0.5 * (fieldWidth - sizeOfEachSquare) + 0.8 * sizeOfEachSquare;
                    double ySquarePosition = rowPositionY + 0.5 * (columnHeight - sizeOfEachSquare);

                    pageGraphics.DrawRectangle(brush, xSquarePosition, ySquarePosition, sizeOfEachSquare, sizeOfEachSquare);

                    DrawRectangleWithLines(pageGraphics, xSquarePosition, ySquarePosition, sizeOfEachSquare, sizeOfEachSquare, gridPenThickness, thickGridPenColor);

                    pageGraphics.DrawImage(symbol, rowPositionX + 0.5 * (fieldWidth - sizeOfEachSquare) + 0.8 * sizeOfEachSquare, rowPositionY + 0.5 * (columnHeight - sizeOfEachSquare), sizeOfEachSquare, sizeOfEachSquare);
                }
                else
                {
                    XPen pen = GetRoundedPenFromColorAndThickness(color, thickGridPenThickness);
                    pageGraphics.DrawLine(pen,  rowPositionX + 0.5 * fieldWidth - 1.5 * sizeOfEachSquare,
                                                rowPositionY + 0.5 * columnHeight,
                                                rowPositionX + 0.5 * fieldWidth + 1.5 * sizeOfEachSquare,
                                                rowPositionY + 0.5 * columnHeight);
                }
            }

            for (int i = additionalFieldValue; i < numberOfFields; i++)
            {
                double fieldPositionX = rowPositionX + i * fieldWidth;
                XRect rect = new XRect(fieldPositionX, rowPositionY, fieldWidth, columnHeight);


                pageGraphics.DrawString(rowStrings[i - additionalFieldValue], listOfColorsFont, pageSetupBrush, rect, XStringFormats.Center);
            }

        }

        private void DrawRectangleWithLines(XGraphics pageGraphics, double xPosition, double yPosition, double width, double height, double thickness, Color penColor)
        {
            XPen pen = GetRoundedPenFromColorAndThickness(penColor, thickness);

            //top line
            pageGraphics.DrawLine(pen, xPosition, yPosition, xPosition + width, yPosition);
            //bottom line
            pageGraphics.DrawLine(pen, xPosition, yPosition + height, xPosition + width, yPosition + height);
            //left line
            pageGraphics.DrawLine(pen, xPosition, yPosition, xPosition, yPosition + height);
            //right line
            pageGraphics.DrawLine(pen, xPosition + width, yPosition, xPosition + width, yPosition + height);
        }

        private void DrawStitchesOnPage(PdfPage currentPage, XGraphics pageGraphics, int[,] matrixOfNewColors, List<Color> colorMeans, int x, int y, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, Dictionary<int, XImage> dictionaryOfXimageByIndex)
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
                    DrawStitchAtPosition(pageGraphics, matrixOfNewColors, colorMeans, relativeIndexI, relativeIndexJ, i, j, startingPointForDrawings, sizeOfEachSquare, dictionaryOfXimageByIndex, true);
                    
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
                                                        matrixOfNewColors.GetLength(0) % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        0,
                                                        0,
                                                        startingPointForDrawings.X + (i - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare,
                                                        startingPointForDrawings.Y - distanceFactorFromGrid * sizeOfEachSquare,
                                                        1);
                    }

                    //bottom arrow
                    if ((j == (y + 1) * maxVerticalNumberOfSquares - 1 || j == matrixOfNewColors.GetLength(1) - 1) && i == (int)(matrixOfNewColors.GetLength(0) * 0.5))
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        matrixOfNewColors.GetLength(0) % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        0,
                                                        180,
                                                        startingPointForDrawings.X + (i - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare,
                                                        startingPointForDrawings.Y + distanceFactorFromGrid * sizeOfEachSquare + (j + 1 - y * maxVerticalNumberOfSquares) * sizeOfEachSquare,
                                                        1);
                    }

                    //left arrow
                    if (j == (int)(matrixOfNewColors.GetLength(1) * 0.5) && i == x * maxHorizontalNumberOfSquares)
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        0,
                                                        matrixOfNewColors.GetLength(1) % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        -90,
                                                        startingPointForDrawings.X - distanceFactorFromGrid * sizeOfEachSquare,
                                                        startingPointForDrawings.Y + (j - y * maxVerticalNumberOfSquares) * sizeOfEachSquare,
                                                        1);
                    }

                    //right arrow
                    if (j == (int)(matrixOfNewColors.GetLength(1) * 0.5) && (i == (x + 1) * maxHorizontalNumberOfSquares - 1 || i == matrixOfNewColors.GetLength(0) - 1))
                    {
                        DrawArrowAtPositionWithScale(pageGraphics,
                                                        0,
                                                        matrixOfNewColors.GetLength(1) % 2 == 0 ? 0 : (0.5 * sizeOfEachSquare),
                                                        90,
                                                        startingPointForDrawings.X + distanceFactorFromGrid * sizeOfEachSquare + (i + 1 - x * maxHorizontalNumberOfSquares) * sizeOfEachSquare,
                                                        startingPointForDrawings.Y + (j - y * maxVerticalNumberOfSquares) * sizeOfEachSquare,
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
                    if(i == matrixOfNewColors.GetLength(0) && relativeIndexI < maxHorizontalNumberOfSquares)
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
                                            XPoint startingPoint, double squareSize, Dictionary<int, XImage> dictionaryOfXimageByIndex, bool drawColor = true, bool drawSymbol = true)
        {
            Color currentPositionColor = colorMeans[matrixOfNewColors[i, j]];
            XSolidBrush brush = new XSolidBrush(XColor.FromArgb(currentPositionColor.A, currentPositionColor.R, currentPositionColor.G, currentPositionColor.B));

            XRect positionToDrawRect = new XRect(   startingPoint.X + relativeIndexI * squareSize,
                                                    startingPoint.Y + relativeIndexJ * squareSize,
                                                    squareSize,
                                                    squareSize);
            if (drawColor)
            {
                pageGraphics.DrawRectangle(brush, positionToDrawRect);
            }

            if (drawSymbol)
            {
                //Bitmap iconImage = new Bitmap(dictionaryOfSymbolByColor[matrixOfNewColors[i, j]], 16, 16);
                //XImage xImageIcon = ConvertBitmapToXimage(iconImage);
                //pageGraphics.DrawImage(xImageIcon, positionToDrawRect); 
                pageGraphics.DrawImage(dictionaryOfXimageByIndex[matrixOfNewColors[i, j]], positionToDrawRect);
            }
        }

        private static XImage ConvertBitmapToXimage(Bitmap iconImage)
        {
            MemoryStream stream = new MemoryStream();
            iconImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return XImage.FromStream(stream);
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

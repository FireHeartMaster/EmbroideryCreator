using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class ImageAndOperationsData
    {
        private Bitmap originalImage;
        //private Bitmap pixelatedImage;
        //private Bitmap colorReducedImage;
        //private Bitmap augmentedImage;
        //private Bitmap withGridImage;
        //private Bitmap withBorderImage;
        public Bitmap ResultingImage { get; private set; }
        public Bitmap ThreadImage { get; private set; }
        public Bitmap SymbolsImage { get; private set; }
        public Bitmap BackstitchImage { get; private set; }
        public Bitmap GridImage { get; private set; }
        public Bitmap BorderImage { get; private set; }

        public int newWidth = 100;
        public int numberOfColors = 10;
        public int numberOfIterations = 10;

        private int newPixelSize = 32;
        
        private List<Color> colorMeans;
        private Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor = new Dictionary<int, List<Tuple<int, int>>>();
        private int[,] matrixOfNewColors;
        public List<bool> colorIsBackgroundList = new List<bool>();

        private Dictionary<int, HashSet<BackstitchLine>> backstitchLines = new Dictionary<int, HashSet<BackstitchLine>>();
        private Dictionary<int, Color> backstitchColors = new Dictionary<int, Color>();

        public List<Color> GetCrossStitchColors() => colorMeans;
        public List<Color> GetBackstitchColors() => backstitchColors.Values.ToList<Color>();
        public Dictionary<int, List<Tuple<int, int>>> GetPositionsOfEachColor() => positionsOfEachColor;

        public int BorderThicknessInNumberOfPixels { get; private set; } = 1;
        public int GridThicknessInNumberOfPixels { get; private set; } = 1;

        Dictionary<int, Bitmap> dictionaryOfColoredCrossByIndex = new Dictionary<int, Bitmap>();
        Dictionary<int, Bitmap> dictionaryOfSymbolByIndex = new Dictionary<int, Bitmap>();

        IconImagesManager iconsManager = new IconImagesManager();

        public void ChangeColorByIndex(int indexToUpdate, Color newColor)
        {
            if(indexToUpdate >= 0 && indexToUpdate < colorMeans.Count)
            {
                colorMeans[indexToUpdate] = newColor;
                PaintNewColorOnImage(indexToUpdate, newColor, ResultingImage);

                //Update color of cross
                UpdateColorOfCross(indexToUpdate);
                PaintCrossOfNewColorOnImage(indexToUpdate, ThreadImage);
            }
        }

        private void UpdateColorOfCross(int indexToUpdate)
        {
            if (!dictionaryOfColoredCrossByIndex.ContainsKey(indexToUpdate))
            {
                dictionaryOfColoredCrossByIndex.Add(indexToUpdate, GenerateCrossOfSelectedColor(colorMeans[indexToUpdate]));
            }
            else
            {
                dictionaryOfColoredCrossByIndex[indexToUpdate] = GenerateCrossOfSelectedColor(colorMeans[indexToUpdate]);
            }
        }

        private void PaintNewColorOnImage(int indexToUpdate, Color newColor, Bitmap image)
        {            
            
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (Tuple<int, int> position in positionsOfEachColor[indexToUpdate])
                {
                    FillPixelAtCoordinate(newColor, graphics, position);
                }
            }
        }

        private void PaintCrossOfNewColorOnImage(int indexToUpdate, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (Tuple<int, int> position in positionsOfEachColor[indexToUpdate])
                {
                    FillCrossAtCoordinate(indexToUpdate, graphics, position);
                }
            }
        }

        private void PaintNewSymbolOnImage(int indexToUpdate, Color newColor, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (Tuple<int, int> position in positionsOfEachColor[indexToUpdate])
                {
                    FillSymbolAtCoordinate(indexToUpdate, graphics, position);
                }
            }
        }

        private void PaintNewColorOnPixelPosition(Tuple<int, int> position, Color newColor, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                FillPixelAtCoordinate(newColor, graphics, position);
            }
        }

        private void PaintCrossOnPixelPosition(Tuple<int, int> position, int indexToUpdate, Bitmap crossStitchResultingImage)
        {
            using (var graphics = Graphics.FromImage(crossStitchResultingImage))
            {
                FillCrossAtCoordinate(indexToUpdate, graphics, position);
            }
        }

        private void PaintSymbolOnPixelPosition(Tuple<int, int> position, int indexToUpdate, Bitmap symbolsImage)
        {
            using (var graphics = Graphics.FromImage(symbolsImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                FillSymbolAtCoordinate(indexToUpdate, graphics, position);
            }
        }

        private void PaintNewColorOnSeveralPixelPositions(List<Tuple<int, int>> positions, Color newColor, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var position in positions)
                {
                    FillPixelAtCoordinate(newColor, graphics, position);
                }
            }
        }

        private void PaintCrossOnSeveralPixelPositions(List<Tuple<int, int>> positions, int colorIndexToUpdate, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var position in positions)
                {
                    FillCrossAtCoordinate(colorIndexToUpdate, graphics, position);
                }
            }
        }

        private void PaintSymbolOnSeveralPixelPositions(List<Tuple<int, int>> positions, int indexToUpdate, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var position in positions)
                {
                    FillSymbolAtCoordinate(indexToUpdate, graphics, position);
                }
            }
        }

        private void FillPixelAtCoordinate(Color newColor, Graphics graphics, Tuple<int, int> position)
        {
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphics.FillRectangle(new SolidBrush(newColor),
                                                        BorderThicknessInNumberOfPixels + (position.Item1) * (newPixelSize) - GridThicknessInNumberOfPixels,
                                                        BorderThicknessInNumberOfPixels + (position.Item2) * (newPixelSize) - GridThicknessInNumberOfPixels,
                                                        newPixelSize/* - GridThicknessInNumberOfPixels*/,
                                                        newPixelSize/* - GridThicknessInNumberOfPixels*/);
        }

        private void FillCrossAtCoordinate(int indexToUpdate, Graphics graphics, Tuple<int, int> position)
        {
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            Bitmap crossOfSelectedColor;
            if (!dictionaryOfColoredCrossByIndex.ContainsKey(indexToUpdate))
            {
                dictionaryOfColoredCrossByIndex.Add(indexToUpdate, GenerateCrossOfSelectedColor(colorMeans[indexToUpdate]));
            }

            crossOfSelectedColor = dictionaryOfColoredCrossByIndex[indexToUpdate];
            graphics.DrawImage(crossOfSelectedColor, 
                                                        BorderThicknessInNumberOfPixels + (position.Item1) * (newPixelSize) - GridThicknessInNumberOfPixels,
                                                        BorderThicknessInNumberOfPixels + (position.Item2) * (newPixelSize) - GridThicknessInNumberOfPixels,
                                                        newPixelSize/* - GridThicknessInNumberOfPixels*/,
                                                        newPixelSize/* - GridThicknessInNumberOfPixels*/);
        }

        private void FillSymbolAtCoordinate(int indexToUpdate, Graphics graphics, Tuple<int, int> position)
        {
            //throw new NotImplementedException();
            Bitmap symbolForSelectedColor;
            if (!dictionaryOfSymbolByIndex.ContainsKey(indexToUpdate))
            {
                dictionaryOfSymbolByIndex.Add(indexToUpdate, GetNextSymbol());  
            }

            symbolForSelectedColor = dictionaryOfSymbolByIndex[indexToUpdate];
            graphics.DrawImage(symbolForSelectedColor,
                                                        BorderThicknessInNumberOfPixels + (position.Item1) * (newPixelSize) - GridThicknessInNumberOfPixels,
                                                        BorderThicknessInNumberOfPixels + (position.Item2) * (newPixelSize) - GridThicknessInNumberOfPixels,
                                                        newPixelSize/* - GridThicknessInNumberOfPixels*/,
                                                        newPixelSize/* - GridThicknessInNumberOfPixels*/);
        }

        public void ChangeSymbolByIndex(int indexToUpdate, Bitmap newSymbol)
        {
            if (indexToUpdate >= 0 && indexToUpdate < colorMeans.Count)
            {
                if (dictionaryOfSymbolByIndex.ContainsKey(indexToUpdate))
                {
                    dictionaryOfSymbolByIndex[indexToUpdate] = newSymbol;
                }
                else
                {
                    dictionaryOfSymbolByIndex.Add(indexToUpdate, newSymbol);
                }
                PaintNewSymbolOnImage(indexToUpdate, colorMeans[indexToUpdate], SymbolsImage);
            }
        }

        private void FillAllNotFilledSymbols()
        {
            if (dictionaryOfSymbolByIndex.Count > 0) return;

            for (int i = 0; i < colorMeans.Count; i++)
            {
                if (!dictionaryOfSymbolByIndex.ContainsKey(i))
                {
                    dictionaryOfSymbolByIndex.Add(i, GetNextSymbol());
                }
            }
        }

        //private Bitmap GenerateCrossOfSelectedColor(Color color)
        //{
        //    //TODO: make a proper function for the generation of thread crosses
        //    int alphaIntensity = 100;
        //    Bitmap coloredCross = new Bitmap(Properties.Resources.ThreadCross);

        //    using(Graphics graphics = Graphics.FromImage(coloredCross))
        //    {
        //        graphics.FillRectangle(new SolidBrush(Color.FromArgb(alphaIntensity, color)), 0, 0, coloredCross.Width, coloredCross.Height);
        //    }

        //    Color noColor = Color.FromArgb(0, 0, 0, 0);
        //    for (int x = 0; x < coloredCross.Width; x++)
        //    {
        //        for (int y = 0; y < coloredCross.Height; y++)
        //        {
        //            if(Properties.Resources.ThreadCross.GetPixel(x, y).A == 0)
        //            {
        //                coloredCross.SetPixel(x, y, noColor);
        //            }
        //            else
        //            {
        //                coloredCross.SetPixel(x, y, Color.FromArgb(Properties.Resources.ThreadCross.GetPixel(x, y).A, coloredCross.GetPixel(x, y)));
        //            }
        //        }
        //    }

        //    return coloredCross;
        //}

        private Bitmap GenerateCrossOfSelectedColor(Color color)
        {
            Bitmap redColoredCross = new Bitmap(Properties.Resources.RedThreadCross);

            Color baseColor = Color.FromArgb(72, 4, 9);
            //Color baseColor = redColoredCross.GetPixel((int)(redColoredCross.Width * 0.5), (int)(redColoredCross.Height * 0.5));

            Bitmap newColoredCross = new Bitmap(redColoredCross.Width, redColoredCross.Height);

            Color noColor = Color.FromArgb(0, 0, 0, 0);
            for (int x = 0; x < newColoredCross.Width; x++)
            {
                for (int y = 0; y < newColoredCross.Height; y++)
                {
                    if (redColoredCross.GetPixel(x, y).A == 0)
                    {
                        newColoredCross.SetPixel(x, y, noColor);
                    }
                    else
                    {
                        Color transformedColor = ImageTransformations.TransformColor(baseColor, color, redColoredCross.GetPixel(x, y));
                        newColoredCross.SetPixel(x, y, Color.FromArgb(redColoredCross.GetPixel(x, y).A, transformedColor));
                    }
                }
            }

            return newColoredCross;
        }

        private Bitmap GetNextSymbol()
        {
            //TODO: make a proper function for the symbols
            //Random rnd = new Random();
            //return GenerateCrossOfSelectedColor(Color.FromArgb(255, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));
            return iconsManager.GetNextIcon();
        }

        private void PaintBackstitchLine(Color colorToPaint, BackstitchLine backstitchLine)
        {
            using(Graphics graphics = Graphics.FromImage(BackstitchImage))
            {
                Tuple<int, int> startingPositionOnImagePixels = ConvertFromCoordinatesIncludingHalfValuesToGeneralPositionOnImage(backstitchLine.startingPosition);
                Tuple<int, int> endingPositionOnImagePixels = ConvertFromCoordinatesIncludingHalfValuesToGeneralPositionOnImage(backstitchLine.endingPosition);

                Color penColor = colorToPaint;
                float backstitchLineThickness = GridThicknessInNumberOfPixels * 3;
                Pen pen = new Pen(penColor, backstitchLineThickness);
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

                graphics.DrawLine(pen, startingPositionOnImagePixels.Item1 - GridThicknessInNumberOfPixels * 0.5f,
                                        startingPositionOnImagePixels.Item2 - GridThicknessInNumberOfPixels * 0.5f,
                                        endingPositionOnImagePixels.Item1 - GridThicknessInNumberOfPixels * 0.5f,
                                        endingPositionOnImagePixels.Item2 - GridThicknessInNumberOfPixels * 0.5f);
            }
        }

        private void PaintAllBackstitchLines()
        {
            BackstitchImage = new Bitmap(ResultingImage.Width, ResultingImage.Height);

            for (int i = 0; i < backstitchLines.Count; i++)
            {
                foreach (BackstitchLine backstitchLine in backstitchLines[i])
                {
                    PaintBackstitchLine(backstitchColors[i], backstitchLine);
                }
            }
        }

        public ImageAndOperationsData(Bitmap importedImage)
        {
            originalImage = new Bitmap(importedImage);
        }

        public ImageAndOperationsData(Bitmap originalImage, Bitmap resultingImage, Bitmap threadImage, Bitmap symbolsImage, Bitmap backstitchImage, Bitmap gridImage, Bitmap borderImage, int newWidth, int numberOfColors, int numberOfIterations, int newPixelSize, List<Color> colorMeans, Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, int[,] matrixOfNewColors, List<bool> colorIsBackgroundList, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, int borderThicknessInNumberOfPixels, int gridThicknessInNumberOfPixels) : this(originalImage)
        {
            ResultingImage = resultingImage;
            ThreadImage = threadImage;
            SymbolsImage = symbolsImage;
            BackstitchImage = backstitchImage;
            GridImage = gridImage;
            BorderImage = borderImage;
            this.newWidth = newWidth;
            this.numberOfColors = numberOfColors;
            this.numberOfIterations = numberOfIterations;
            this.newPixelSize = newPixelSize;
            this.colorMeans = colorMeans;
            this.positionsOfEachColor = positionsOfEachColor;
            this.matrixOfNewColors = matrixOfNewColors;
            this.colorIsBackgroundList = colorIsBackgroundList;
            this.backstitchLines = backstitchLines;
            this.backstitchColors = backstitchColors;
            BorderThicknessInNumberOfPixels = borderThicknessInNumberOfPixels;
            GridThicknessInNumberOfPixels = gridThicknessInNumberOfPixels;

            FillAllNotFilledSymbols();
        }

        public void SerializeData(string filePath)
        {
            ImageAndOperationsDataSerialized serializableData = new ImageAndOperationsDataSerialized(originalImage, ResultingImage, ThreadImage, SymbolsImage, BackstitchImage, GridImage, BorderImage, newWidth, numberOfColors, numberOfIterations, newPixelSize, colorMeans, positionsOfEachColor, matrixOfNewColors, colorIsBackgroundList, backstitchLines, backstitchColors, BorderThicknessInNumberOfPixels, GridThicknessInNumberOfPixels);

            SerializerHelper.WriteToFile<ImageAndOperationsDataSerialized>(filePath, serializableData);
        }

        private Bitmap PixelateImage(Bitmap originalImage)
        {
            Bitmap pixelatedImage = ImageTransformations.Pixelate(originalImage, newWidth);
            return pixelatedImage;
        }

        private Bitmap PixelateImageExactlyAccordingToOriginal(Bitmap originalImage)
        {
            Bitmap pixelatedImageExactly = ImageTransformations.PixelateExactlyAccordingToOriginal(originalImage, newWidth);
            return pixelatedImageExactly;
        }

        private Bitmap PixelateImageAlternateOrder(Bitmap originalImage)
        {
            Bitmap pixelatedImage = ImageTransformations.PixelateAlternateOrder(originalImage, newWidth, ref colorMeans, ref positionsOfEachColor);
            return pixelatedImage;
        }

        private Bitmap ReduceNumberOfColors(Bitmap pixelatedImage, int numberOfIterations = 10)
        {
            Bitmap colorReducedImage = ImageTransformations.ReduceNumberOfColors(pixelatedImage, numberOfColors, numberOfIterations, 
                                                                                out colorMeans, out positionsOfEachColor, out matrixOfNewColors);
            return colorReducedImage;
        }

        private Bitmap SetColorsWithoutReducingNumberOfColors(Bitmap pixelatedImage, int numberOfIterations = 10)
        {
            Bitmap colorReducedImage = ImageTransformations.SetColorsWithoutReducingNumberOfColors(pixelatedImage, out colorMeans, out positionsOfEachColor, out matrixOfNewColors);
            return colorReducedImage;
        }

        private Bitmap ResizingImage(Bitmap colorReducedImage)
        {
            //bool largerIsWidth = colorReducedImage.Width > colorReducedImage.Height;
            Bitmap augmentedImage = ImageTransformations.ResizeBitmap(colorReducedImage, newPixelSize);
            return augmentedImage;
        }

        private Bitmap AddGridToImage(Bitmap imageToAddGrid, int numberOfVerticalGridLines, int numberOfHorizontalGridLines)
        {
            int intervalForDarkerLines = 10;
            using (var graphics = Graphics.FromImage(imageToAddGrid))
            {
                //vertical lines
                for (int x = 0; x <= numberOfVerticalGridLines; x++)
                {
                    Color penColor = x % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    //GridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, x * newPixelSize/* - newPixelSize*0.5f*/, 0, x * newPixelSize/* - newPixelSize*0.5f*/, imageToAddGrid.Height - 1);
                }

                //horizontal lines
                for (int y = 0; y <= numberOfHorizontalGridLines; y++)
                {
                    Color penColor = y % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    //GridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, 0, y * newPixelSize/* - newPixelSize*0.5f*/, imageToAddGrid.Width - 1, y * newPixelSize/* - newPixelSize*0.5f*/);
                }
            }
            return imageToAddGrid;
        }

        private Bitmap AddGridResizingImage(Bitmap colorReducedImage)
        {
            //bool largerIsWidth = colorReducedImage.Width > colorReducedImage.Height;
            Bitmap augmentedImage = ImageTransformations.ResizeBitmap(colorReducedImage, newPixelSize);

            Bitmap withGridImage = new Bitmap(augmentedImage);
            int intervalForDarkerLines = 10;
            using (var graphics = Graphics.FromImage(withGridImage))
            {
                //vertical lines
                for (int x = 0; x <= colorReducedImage.Width; x++)
                {
                    Color penColor = x % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    //GridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, x * newPixelSize/* - newPixelSize*0.5f*/, 0, x * newPixelSize/* - newPixelSize*0.5f*/, withGridImage.Height - 1);
                }

                //horizontal lines
                for (int y = 0; y <= colorReducedImage.Height; y++)
                {
                    Color penColor = y % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    //GridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, 0, y * newPixelSize/* - newPixelSize*0.5f*/, withGridImage.Width - 1, y * newPixelSize/* - newPixelSize*0.5f*/);
                }
            }
            return withGridImage;
        }

        private Bitmap AddPaddingToImage(Bitmap smallerImage)
        {
            SetPenAndBorderThickness(out _, out _);

            Bitmap imageWithPadding = new Bitmap(smallerImage.Width + 2 * BorderThicknessInNumberOfPixels - GridThicknessInNumberOfPixels, smallerImage.Height + 2 * BorderThicknessInNumberOfPixels - GridThicknessInNumberOfPixels);
            //I'm subtracting the grid thickness here to add a small offset to hide the first top horizontal and the first left vertical lines of the grid,
            //otherwise we end up with an asymmetric grid starting with black lines at the top and at the left but with no lines on the right and at the bottom

            using (var graphics = Graphics.FromImage(imageWithPadding))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                //Let's add this offset to hide the first top horizontal and the first left vertical lines of the grid, otherwise we end up with an asymmetric grid
                //starting with black lines at the top and at the left but with no lines on the right and at the bottom
                int offset = GridThicknessInNumberOfPixels;
                graphics.DrawImage(smallerImage, BorderThicknessInNumberOfPixels - offset, BorderThicknessInNumberOfPixels - offset, smallerImage.Width, smallerImage.Height);
            }
            return imageWithPadding;
        }

        private Bitmap AddBorder(Bitmap imageToAddBorder)
        {
            SetPenAndBorderThickness(out int penSize, out Pen pen);

            //Bitmap withoutBorderImage = new Bitmap(withoutBorderImage_.Width + 2 * BorderThicknessInNumberOfPixels - GridThicknessInNumberOfPixels, withoutBorderImage_.Height + 2 * BorderThicknessInNumberOfPixels - GridThicknessInNumberOfPixels);
            //I'm subtracting the grid thickness here to add a small offset to hide the first top horizontal and the first left vertical lines of the grid,
            //otherwise we end up with an asymmetric grid starting with black lines at the top and at the left but with no lines on the right and at the bottom

            using (var graphics = Graphics.FromImage(imageToAddBorder))
            {
                int offset = GridThicknessInNumberOfPixels;

                int offsetForBorder = (int)(penSize % 2 == 0 ? penSize * 0.5f : (penSize + 1) * 0.5f);
                graphics.DrawLine(pen, 0, offsetForBorder, imageToAddBorder.Width, offsetForBorder); //upper border
                graphics.DrawLine(pen, offsetForBorder, 0, offsetForBorder, imageToAddBorder.Height); //left border
                int bottomBorderOffset = 1; //without this exact value, the line isn't draw at the precise position we need. I don't know yet why this exact value. It doesn't seem to depend on the size of the image
                graphics.DrawLine(pen, 0, imageToAddBorder.Height - offsetForBorder + 1 - bottomBorderOffset, imageToAddBorder.Width, imageToAddBorder.Height - offsetForBorder + 1 - bottomBorderOffset); //bottom border
                int rightBorderOffset = 6; //without this exact value, the line isn't draw at the precise position we need. I don't know yet why this exact value. It doesn't seem to depend on the size of the image
                graphics.DrawLine(pen, imageToAddBorder.Width - offset - 1- rightBorderOffset, 0, imageToAddBorder.Width - offset - 1- rightBorderOffset, imageToAddBorder.Height); //right border
            }
            return imageToAddBorder;
        }

        private Bitmap AddBorderIncreasingSizeOfOriginalImageByAddingPadding(Bitmap withGridImage)
        {
            SetPenAndBorderThickness(out int penSize, out Pen pen);

            Bitmap withBorderImage = new Bitmap(withGridImage.Width + 2 * BorderThicknessInNumberOfPixels - GridThicknessInNumberOfPixels, withGridImage.Height + 2 * BorderThicknessInNumberOfPixels - GridThicknessInNumberOfPixels);
            //I'm subtracting the grid thickness here to add a small offset to hide the first top horizontal and the first left vertical lines of the grid,
            //otherwise we end up with an asymmetric grid starting with black lines at the top and at the left but with no lines on the right and at the bottom

            using (var graphics = Graphics.FromImage(withBorderImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                //int offset = (int)(penSize % 2 == 0 ? penSize * 0.5f : (penSize + 1) * 0.5f);
                //borderThicknessInNumberOfPixels = penSize;

                //Let's add this offset to hide the first top horizontal and the first left vertical lines of the grid, otherwise we end up with an asymmetric grid
                //starting with black lines at the top and at the left but with no lines on the right and at the bottom
                int offset = GridThicknessInNumberOfPixels;
                graphics.DrawImage(withGridImage, BorderThicknessInNumberOfPixels - offset, BorderThicknessInNumberOfPixels - offset, withGridImage.Width, withGridImage.Height);

                int offsetForBorder = (int)(penSize % 2 == 0 ? penSize * 0.5f : (penSize + 1) * 0.5f);
                graphics.DrawLine(pen, 0, offsetForBorder, withBorderImage.Width, offsetForBorder); //upper border
                graphics.DrawLine(pen, offsetForBorder, 0, offsetForBorder, withBorderImage.Height); //left border
                graphics.DrawLine(pen, 0, withBorderImage.Height - offsetForBorder + 1, withBorderImage.Width, withBorderImage.Height - offsetForBorder + 1); //bottom border
                graphics.DrawLine(pen, withBorderImage.Width - offset - 1, 0, withBorderImage.Width - offset - 1, withBorderImage.Height); //right border
            }
            return withBorderImage;
        }

        private void SetPenAndBorderThickness(out int penSize, out Pen pen)
        {
            Color penColor = Color.Black;
            penSize = (int)(newPixelSize * 0.5f)/* + 1*/;
            pen = new Pen(penColor, penSize);
            BorderThicknessInNumberOfPixels = penSize;
        }

        public void ProcessImage()
        {
            Bitmap processedImage = originalImage;
            processedImage = PixelateImage(processedImage);
            processedImage = ReduceNumberOfColors(processedImage);
            processedImage = AddGridResizingImage(processedImage);
            processedImage = AddBorderIncreasingSizeOfOriginalImageByAddingPadding(processedImage);
            //resultingImage = withBorderImage;
            ResultingImage = processedImage;
            BackstitchImage = new Bitmap(ResultingImage.Width, ResultingImage.Height);
            for (int i = 0; i < backstitchLines.Count; i++)
            {
                backstitchLines[i] = new HashSet<BackstitchLine>();
            }
        }

        //extremmely long time to execute and resource consuming
        public void ProcessImageAlternateOrder()
        {
            Bitmap processedImage = originalImage;
            processedImage = ReduceNumberOfColors(processedImage);
            processedImage = PixelateImageAlternateOrder(processedImage);
            processedImage = AddGridResizingImage(processedImage);
            processedImage = AddBorderIncreasingSizeOfOriginalImageByAddingPadding(processedImage);
            ResultingImage = processedImage;
            BackstitchImage = new Bitmap(ResultingImage.Width, ResultingImage.Height);
            for (int i = 0; i < backstitchLines.Count; i++)
            {
                backstitchLines[i] = new HashSet<BackstitchLine>();
            }
        }

        public void ProcessImageInSeparateLayers(int newPixelSize = 10, bool exactToSource = false)
        {
            this.newPixelSize = newPixelSize;

            Bitmap processedImage = originalImage;
            if (!exactToSource)
            {
                processedImage = PixelateImage(processedImage);
                processedImage = ReduceNumberOfColors(processedImage);
            }
            else
            {
                processedImage = PixelateImageExactlyAccordingToOriginal(processedImage);
                processedImage = SetColorsWithoutReducingNumberOfColors(processedImage);
            }
            //processedImage = AddGridResizingImage(processedImage);
            //processedImage = AddBorderIncreasingSizeOfOriginalImageByAddingPadding(processedImage);
            int reducedNumberOfColorsWidth = processedImage.Width;
            int reducedNumberOfColorsHeight = processedImage.Height;
            
            processedImage = ResizingImage(processedImage);
            GridImage = AddGridToImage(new Bitmap(processedImage.Width, processedImage.Height), reducedNumberOfColorsWidth, reducedNumberOfColorsHeight);
            
            //BorderImage = AddPaddingToImage(new Bitmap(processedImage.Width, processedImage.Height));
            GridImage = AddPaddingToImage(GridImage);
            ResultingImage = AddPaddingToImage(processedImage);

            dictionaryOfColoredCrossByIndex.Clear();
            dictionaryOfSymbolByIndex.Clear();

            RepaintMainImage(false, true, true);  //Setting thread image and symbols image

            BorderImage = AddBorder(new Bitmap(ResultingImage.Width, ResultingImage.Height));
            
            //resultingImage = withBorderImage;
            //ResultingImage = processedImage;
            BackstitchImage = new Bitmap(ResultingImage.Width, ResultingImage.Height);
            for (int i = 0; i < backstitchLines.Count; i++)
            {
                backstitchLines[i] = new HashSet<BackstitchLine>();
            }
        }

        public void MergeTwoColors(int firstIndex, int otherIndex)
        {
            //I can't simply remove the corresponding keys from the dictionary because the values of the keys also is used as the position in the colors list


            foreach (Tuple<int, int> position in positionsOfEachColor[otherIndex])
            {
                matrixOfNewColors[position.Item1, position.Item2] = firstIndex;
            }

            //Before starting removing and reordering everything, let's add all elements from the removed key to the list of the key that remains
            foreach (Tuple<int, int> position in positionsOfEachColor[otherIndex])
            {
                positionsOfEachColor[firstIndex].Add(position); //We don't need to remove it from the other list because it's already going to be overwritten right afterward
            }

            //Let's reorder the indexes and their lists of positions to counter the removal of one of them
            //If the key to remove actually corresponded to the last one, we don't need to worry, it simply means that
            //we can safely remove it without having to reorder things
            for (int i = otherIndex; i < colorMeans.Count - 1; i++)
            {
                positionsOfEachColor[i] = positionsOfEachColor[i + 1];

                foreach (Tuple<int, int> position in positionsOfEachColor[i])
                {
                    matrixOfNewColors[position.Item1, position.Item2] = i;
                }
            }

            //Now let's finally remove the last one from the dictionary once all others now have their positions/indexes corrected
            positionsOfEachColor.Remove(colorMeans.Count - 1);
            //Also let's remove it from the list of colors, for this one we can simply remove it without any reordering
            //That's why here we remove from the specified index instead of removing the last element
            colorMeans.RemoveAt(otherIndex);
        }

        public void PaintPixelInPositionWithColorOfIndex(Tuple<int, int> position, int colorIndexToPaint)
        {
            int originalColorIndex = matrixOfNewColors[position.Item1, position.Item2];
            matrixOfNewColors[position.Item1, position.Item2] = colorIndexToPaint;

            positionsOfEachColor[originalColorIndex].Remove(position);
            positionsOfEachColor[colorIndexToPaint].Add(position);
            
            PaintNewColorOnPixelPosition(position, colorMeans[colorIndexToPaint], ResultingImage);
            PaintCrossOnPixelPosition(position, colorIndexToPaint, ThreadImage);
            PaintSymbolOnPixelPosition(position, colorIndexToPaint, SymbolsImage);
        }

        private void RepaintMainImage(bool paintColors = true, bool paintCrosses = true, bool paintSymbols = true)
        {
            float aspectRatio = ((float)originalImage.Height) / originalImage.Width;
            ImageTransformations.GetNewSize(newWidth, (int)(newWidth * aspectRatio), newPixelSize, out int resultingImageWidth, out int resultingImageHeight);
            if (paintColors)
            {
                ResultingImage = AddPaddingToImage(new Bitmap(resultingImageWidth, resultingImageHeight));
            }
            if (paintCrosses)
            {
                ThreadImage = new Bitmap(ResultingImage.Width, ResultingImage.Height);
            }
            if (paintSymbols)
            {
                SymbolsImage = new Bitmap(ResultingImage.Width, ResultingImage.Height);
            }

            for (int i = 0; i < matrixOfNewColors.GetLength(0); i++)
            {
                for (int j = 0; j < matrixOfNewColors.GetLength(1); j++)
                {
                    Tuple<int, int> position = new Tuple<int, int>(i, j);
                    int colorIndexToPaint = matrixOfNewColors[position.Item1, position.Item2];

                    if (paintColors)
                    {
                        PaintNewColorOnPixelPosition(position, colorMeans[colorIndexToPaint], ResultingImage);
                    }
                    if (paintCrosses)
                    {
                        PaintCrossOnPixelPosition(position, colorIndexToPaint, ThreadImage);
                    }
                    if (paintSymbols)
                    {
                        PaintSymbolOnPixelPosition(position, colorIndexToPaint, SymbolsImage);
                    }
                }
            }
        }

        public void RepaintCrosses()
        {
            RepaintMainImage(false, true, false);
        }

        public void PaintNewColorOnGeneralPosition(Tuple<int, int> generalPosition, int colorIndexToPaint)
        {
            Tuple<int, int> coordinates = ConvertFromGeneralPositionOnImageToCoordinates(generalPosition);
            if(coordinates.Item1 < 0 || coordinates.Item1 >= matrixOfNewColors.GetLength(0) || coordinates.Item2 < 0 || coordinates.Item2 >= matrixOfNewColors.GetLength(1))
            {
                return;
            }

            if (matrixOfNewColors[coordinates.Item1, coordinates.Item2] == colorIndexToPaint) return;

            PaintPixelInPositionWithColorOfIndex(coordinates, colorIndexToPaint);
        }

        public void RemoveAlonePixels(int minAmountAround = 2)
        {
            for (int i = 0; i < matrixOfNewColors.GetLength(0); i++)
            {
                for (int j = 0; j < matrixOfNewColors.GetLength(1); j++)
                {
                    Tuple<int, int> coordinates = new Tuple<int, int>(i, j);

                    Dictionary<int, int> amountAroundPerIndex = new Dictionary<int, int>();

                    int indexWithHighestAmount = -1;
                    for (int x = i - 1; x <= i + 1; x++)
                    {
                        for (int y = j - 1; y <= j + 1; y++)
                        {
                            if (x == i && y == j) continue;
                            if (x < 0 || x >= matrixOfNewColors.GetLength(0) || y < 0 || y >= matrixOfNewColors.GetLength(1)) continue;

                            //amountAroundPerIndex[matrixOfNewColors[i,j]]
                            if(!amountAroundPerIndex.ContainsKey(matrixOfNewColors[x, y]))
                            {
                                amountAroundPerIndex.Add(matrixOfNewColors[x, y], 1);
                            }
                            else
                            {
                                amountAroundPerIndex[matrixOfNewColors[x, y]]++;
                            }

                            if(indexWithHighestAmount == -1)
                            {
                                indexWithHighestAmount = matrixOfNewColors[x, y];
                            }
                            else
                            {
                                if (amountAroundPerIndex[matrixOfNewColors[x, y]] > amountAroundPerIndex[indexWithHighestAmount])
                                {
                                    indexWithHighestAmount = matrixOfNewColors[x, y];
                                }
                            }
                        }
                    }

                    if(!amountAroundPerIndex.ContainsKey(matrixOfNewColors[i, j]) || amountAroundPerIndex[matrixOfNewColors[i, j]] < minAmountAround)
                    {
                        matrixOfNewColors[i, j] = indexWithHighestAmount;
                    }
                }
            }

            RepaintMainImage(true, true, true);
        }

        private Tuple<int, int> ConvertFromGeneralPositionOnImageToCoordinates(Tuple<int, int> generalPosition)
        {
            int x = (int)(((float)(generalPosition.Item1 - BorderThicknessInNumberOfPixels)) / newPixelSize);
            int y = (int)(((float)(generalPosition.Item2 - BorderThicknessInNumberOfPixels)) / newPixelSize);

            return new Tuple<int, int>(x, y);
        }

        private Tuple<float, float> ConvertFromGeneralPositionOnImageToFloatCoordinates(Tuple<int, int> generalPosition)
        {
            float x = ((float)(generalPosition.Item1 - BorderThicknessInNumberOfPixels)) / newPixelSize;
            float y = ((float)(generalPosition.Item2 - BorderThicknessInNumberOfPixels)) / newPixelSize;

            return new Tuple<float, float>(x, y);
        }

        public Tuple<float, float> ConvertFromGeneralPositionOnImageToCoordinatesIncludingHalfValues(Tuple<int, int> generalPosition)
        {
            double x = Math.Round(((generalPosition.Item1 - BorderThicknessInNumberOfPixels) * 2.0) / newPixelSize) / 2;
            double y = Math.Round(((generalPosition.Item2 - BorderThicknessInNumberOfPixels) * 2.0) / newPixelSize) / 2;

            return new Tuple<float, float>((float)x, (float)y);
        }

        private Tuple<int, int> ConvertFromCoordinatesIncludingHalfValuesToGeneralPositionOnImage(Tuple<float, float> coordinates)
        {
            int x = (int)(coordinates.Item1 * newPixelSize + BorderThicknessInNumberOfPixels);
            int y = (int)(coordinates.Item2 * newPixelSize + BorderThicknessInNumberOfPixels);

            return new Tuple<int, int>(x, y);
        }

        public Tuple<int, int> ConvertFromGeneralPositionOnImagesToRoundedGeneralPositionOnImageIncludingHalfValues(Tuple<int, int> generalPosition)
        {
            return ConvertFromCoordinatesIncludingHalfValuesToGeneralPositionOnImage(ConvertFromGeneralPositionOnImageToCoordinatesIncludingHalfValues(generalPosition));
        }

        public void AddNewColor(Color newColor)
        {
            int newColorIndex = colorMeans.Count;
            positionsOfEachColor.Add(newColorIndex, new List<Tuple<int, int>>());
            colorMeans.Add(newColor);

            if (!dictionaryOfSymbolByIndex.ContainsKey(newColorIndex))
            {
                dictionaryOfSymbolByIndex.Add(newColorIndex, GetNextSymbol());
            }

            if (!dictionaryOfColoredCrossByIndex.ContainsKey(newColorIndex))
            {
                dictionaryOfColoredCrossByIndex.Add(newColorIndex, GenerateCrossOfSelectedColor(colorMeans[newColorIndex]));
            }
        }

        public void FillRegionWithColorByPosition(Tuple<int, int> generalPosition, int colorIndexToPaint)
        {
            Tuple<int, int> coordinates = ConvertFromGeneralPositionOnImageToCoordinates(generalPosition);
            if (coordinates.Item1 < 0 || coordinates.Item1 >= matrixOfNewColors.GetLength(0) || coordinates.Item2 < 0 || coordinates.Item2 >= matrixOfNewColors.GetLength(1))
            {
                return;
            }

            int indexOfTheOriginalColor = matrixOfNewColors[coordinates.Item1, coordinates.Item2];

            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            List<Tuple<int, int>> listOfPositionsToChange = new List<Tuple<int, int>>();

            queue.Enqueue(coordinates);

            HashSet<Tuple<int, int>> positionsAlreadyAdded = new HashSet<Tuple<int, int>>();
            positionsAlreadyAdded.Add(coordinates);

            while (queue.Count > 0)
            {
                Tuple<int, int> firstElementOfTheQueue = queue.Dequeue();
                //positionsAlreadyAddedVisited.Add(firstElementOfTheQueue);

                if (firstElementOfTheQueue.Item1 >= 0 && firstElementOfTheQueue.Item1 < matrixOfNewColors.GetLength(0) &&
                    firstElementOfTheQueue.Item2 >= 0 && firstElementOfTheQueue.Item2 < matrixOfNewColors.GetLength(1))
                {
                    if(matrixOfNewColors[firstElementOfTheQueue.Item1, firstElementOfTheQueue.Item2] == indexOfTheOriginalColor)
                    {
                        listOfPositionsToChange.Add(firstElementOfTheQueue);

                        matrixOfNewColors[firstElementOfTheQueue.Item1, firstElementOfTheQueue.Item2] = colorIndexToPaint;

                        //enqueueing new not enqueued positions
                        Tuple<int, int> leftPosition = new Tuple<int, int>(firstElementOfTheQueue.Item1 - 1, firstElementOfTheQueue.Item2);
                        if (!positionsAlreadyAdded.Contains(leftPosition))
                        {
                            queue.Enqueue(leftPosition);
                            positionsAlreadyAdded.Add(leftPosition);
                        }

                        Tuple<int, int> upperPosition = new Tuple<int, int>(firstElementOfTheQueue.Item1, firstElementOfTheQueue.Item2 - 1);
                        if (!positionsAlreadyAdded.Contains(upperPosition))
                        {
                            queue.Enqueue(upperPosition);
                            positionsAlreadyAdded.Add(upperPosition);
                        }

                        Tuple<int, int> rightPosition = new Tuple<int, int>(firstElementOfTheQueue.Item1 + 1, firstElementOfTheQueue.Item2);
                        if (!positionsAlreadyAdded.Contains(rightPosition))
                        {
                            queue.Enqueue(rightPosition);
                            positionsAlreadyAdded.Add(rightPosition);
                        }

                        Tuple<int, int> bottomPosition = new Tuple<int, int>(firstElementOfTheQueue.Item1, firstElementOfTheQueue.Item2 + 1);
                        if (!positionsAlreadyAdded.Contains(bottomPosition))
                        {
                            queue.Enqueue(bottomPosition);
                            positionsAlreadyAdded.Add(bottomPosition);
                        }
                    }
                }
            }


            //remove all positions that are changing their colors from one list of positions and add it to the other one (the one of the new color)
            foreach (Tuple<int, int> position in listOfPositionsToChange)
            {
                positionsOfEachColor[indexOfTheOriginalColor].Remove(position);
                positionsOfEachColor[colorIndexToPaint].Add(position);
            }

            //repaint
            PaintNewColorOnSeveralPixelPositions(listOfPositionsToChange, colorMeans[colorIndexToPaint], ResultingImage);
            PaintCrossOnSeveralPixelPositions(listOfPositionsToChange, colorIndexToPaint, ThreadImage);
            PaintSymbolOnSeveralPixelPositions(listOfPositionsToChange, colorIndexToPaint, SymbolsImage);
        }

        public void AddNewBackstitchColor(Color newColor)
        {
            backstitchLines.Add(backstitchColors.Count, new HashSet<BackstitchLine>());
            backstitchColors.Add(backstitchColors.Count, newColor);
        }

        //public void RemoveBackstitchColor(Color colorToRemove)
        //{
        //    try
        //    {
        //        int indexToRemove = backstitchColors.First(element => element.Value == colorToRemove).Key;
        //        backstitchLines.Remove(indexToRemove);
        //        backstitchColors.Remove(indexToRemove);
                
        //        //TODO: repaint image without the lines of the removed color
        //    }
        //    catch (ArgumentNullException)
        //    {
                                   
        //    }
        //}

        public void RemoveBackstitchColorByIndex(int indexToRemove)
        {
            if(indexToRemove >= 0 && indexToRemove < backstitchColors.Count)
            {
                backstitchLines.Remove(indexToRemove);
                backstitchColors.Remove(indexToRemove);

                PaintAllBackstitchLines();
            }
        }

        public void AddNewBackstitchLine(int indexToAddLine, Tuple<float, float> startingPosition, Tuple<float, float> endingPosition)
        {
            if (startingPosition.Item1 >= 0 && startingPosition.Item1 < matrixOfNewColors.GetLength(0) &&
                    endingPosition.Item2 >= 0 && endingPosition.Item2 < matrixOfNewColors.GetLength(1))
            {
                HashSet<BackstitchLine> a = backstitchLines[indexToAddLine];
                BackstitchLine newBackstitchLine = new BackstitchLine(startingPosition, endingPosition);
                backstitchLines[indexToAddLine].Add(newBackstitchLine);

                PaintBackstitchLine(backstitchColors[indexToAddLine], newBackstitchLine);
            }
        }


        public void RemoveBackstitchLineClicked(Tuple<int, int> positionClickedInPixels)
        {
            Tuple<int, BackstitchLine> indexAndLine = FindBackstitchLineByPosition(positionClickedInPixels);

            if(indexAndLine.Item1 != -1)
            {
                RemoveBackstitchLine(indexAndLine.Item1, indexAndLine.Item2);
            }
        }

        private Tuple<int, BackstitchLine> FindBackstitchLineByPosition(Tuple<int, int> generalPixelPosition)
        {
            int indexFound = -1;
            BackstitchLine backstitchLineFound = new BackstitchLine();

            float distanceToConsiderCloseEnough = (3.0f * GridThicknessInNumberOfPixels / newPixelSize) * 0.5f;
            Tuple<float, float> imagePosition = ConvertFromGeneralPositionOnImageToFloatCoordinates(generalPixelPosition);

            for (int i = 0; i < backstitchLines.Count; i++)
            {
                //TODO: Verify if the reference point is between the starting and ending points of a backstitch line

                foreach (BackstitchLine currentBackstitchLineToVerifyDistance in backstitchLines[i])
                {
                    //float distanceToLine = ImageTransformations.CalculateDistanceOfPointToLine(imagePosition, currentBackstitchLineToVerifyDistance.startingPosition, currentBackstitchLineToVerifyDistance.endingPosition);

                    //if(distanceToLine <= distanceToConsiderCloseEnough)
                    if(ImageTransformations.IsPointCloseEnoughToLineDefinedByTwoPoints(imagePosition, currentBackstitchLineToVerifyDistance.startingPosition, currentBackstitchLineToVerifyDistance.endingPosition, distanceToConsiderCloseEnough))
                    {
                        indexFound = i;
                        backstitchLineFound = currentBackstitchLineToVerifyDistance;
                        
                        return new Tuple<int, BackstitchLine>(indexFound, backstitchLineFound);
                    }
                }
            }

            return new Tuple<int, BackstitchLine>(indexFound, backstitchLineFound);
        }

        private void RemoveBackstitchLine(int indexToRemoveLine, BackstitchLine backstitchLine)
        {
            if (backstitchLine.startingPosition.Item1 >= 0 && backstitchLine.startingPosition.Item1 < matrixOfNewColors.GetLength(0) &&
                    backstitchLine.endingPosition.Item2 >= 0 && backstitchLine.endingPosition.Item2 < matrixOfNewColors.GetLength(1))
            {
                backstitchLines[indexToRemoveLine].Remove(backstitchLine);
            }

            PaintAllBackstitchLines();
        }

        public void UpdateBackstitchColorByIndex(int indexToUpdate, Color newColor)
        {
            if(indexToUpdate >= 0 && indexToUpdate < backstitchColors.Count)
            {
                backstitchColors[indexToUpdate] = newColor;

                foreach (BackstitchLine backstitchLineOfSelectedColor in backstitchLines[indexToUpdate])
                {
                    PaintBackstitchLine(backstitchColors[indexToUpdate], backstitchLineOfSelectedColor);
                }
            }
        }

        public void SavePdf(string pathToSave, Bitmap topLogo, string title, string secondTitle, string subtitle, string leftText, string rightText, string footerText, string footerLink, string secondFooterText, string[] socialMediaLinks, Bitmap[] socialMediaImages, string[] socialMediaNames)
        {
            PdfManager pdfManager = new PdfManager(topLogo, title, secondTitle, subtitle, leftText, rightText, footerText, footerLink, secondFooterText, socialMediaLinks, socialMediaImages, socialMediaNames);
            pdfManager.CreatePdfStitches(pathToSave, matrixOfNewColors, colorMeans, backstitchLines, backstitchColors, dictionaryOfSymbolByIndex);
        }

        public void CreateMachinePath(string pathToSaveWithoutExtension)
        {
            MachineEmbroidery machineEmbroidery = new MachineEmbroidery();
            //machineEmbroidery.CreatePath(positionsOfEachColor);
            Dictionary<int, List<Tuple<int, int>>> positionsOfEachColorToEmbroider = new Dictionary<int, List<Tuple<int, int>>>();
            int threadIndex = 0;
            foreach (KeyValuePair<int, List<Tuple<int, int>>> indexAndPositions in positionsOfEachColor)
            {
                if (!colorIsBackgroundList[indexAndPositions.Key])
                {
                    positionsOfEachColorToEmbroider.Add(threadIndex, indexAndPositions.Value);
                    //foreach (Tuple<int, int> position in indexAndPositions.Value)
                    //{
                    //    positionsToEmbroider[threadIndex].Add(new Tuple<float, float>(position.Item1, position.Item2));
                    //}
                    threadIndex++;
                }
            }

            //foreach (var indexAndLines in backstitchLines)
            //{

            //}

            machineEmbroidery.CreatePathAndDstFile(pathToSaveWithoutExtension, positionsOfEachColorToEmbroider, 30, matrixOfNewColors.GetLength(0), matrixOfNewColors.GetLength(1));
        }
    }

    [Serializable]public struct BackstitchLine
    {
        public Tuple<float, float> startingPosition;
        public Tuple<float, float> endingPosition;

        public BackstitchLine(Tuple<float, float> startingPosition, Tuple<float, float> endingPosition)
        {
            this.startingPosition = startingPosition;
            this.endingPosition = endingPosition;
        }
    }
}

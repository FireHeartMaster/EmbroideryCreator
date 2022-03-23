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
        private Bitmap pixelatedImage;
        private Bitmap colorReducedImage;
        private Bitmap augmentedImage;
        private Bitmap withGridImage;
        public Bitmap resultingImage { get; private set; }

        public int newWidth = 100;
        public int numberOfColors = 10;
        public int numberOfIterations = 10;

        private int newPixelSize = 10;

        private int[,] matrixOfNewColors;
        private Color[] colorMeans;
        private Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor = new Dictionary<int, List<Tuple<int, int>>>();

        public int[,] GetMatrixOfColors() => matrixOfNewColors;
        public Color[] GetColors() => colorMeans;
        public Dictionary<int, List<Tuple<int, int>>> GetPositionsOfEachColor() => positionsOfEachColor;


        private int borderThicknessInNumberOfPixels = 1;
        private int gridThicknessInNumberOfPixels = 1;

        public void UpdateColorByIndex(int indexToUpdate, Color newColor)
        {
            if(indexToUpdate >= 0 && indexToUpdate < colorMeans.Length)
            {
                colorMeans[indexToUpdate] = newColor;
                PaintNewColorOnImage(indexToUpdate, newColor);
            }
        }

        private void PaintNewColorOnImage(int indexToUpdate, Color newColor)
        {
            foreach (Tuple<int, int> position in positionsOfEachColor[indexToUpdate])
            {
                using (var graphics = Graphics.FromImage(resultingImage))
                {
                    //Color penColor = newColor;
                    //int penSize = newPixelSize;
                    //Pen pen = new Pen(penColor, penSize);
                    //graphics.DrawRectangle(pen, borderThicknessInNumberOfPixels + position.Item1 * (newPixelSize + gridThicknessInNumberOfPixels),
                    //                            borderThicknessInNumberOfPixels + position.Item2 * (newPixelSize + gridThicknessInNumberOfPixels),
                    //                            newPixelSize * 0.5f,
                    //                            newPixelSize * 0.5f);
                    //pen = new Pen(penColor, newPixelSize - gridThicknessInNumberOfPixels - 1);
                    ////graphics.DrawRectangle(pen, borderThicknessInNumberOfPixels + 12 * (newPixelSize) - (newPixelSize * 0.5f + 1),
                    ////                            borderThicknessInNumberOfPixels + 12 * (newPixelSize) - (newPixelSize * 0.5f + 1),
                    ////                            1,
                    ////                            1);
                    graphics.FillRectangle(new SolidBrush(newColor), 
                                                borderThicknessInNumberOfPixels + (position.Item1 - 1) * (newPixelSize),
                                                borderThicknessInNumberOfPixels + (position.Item2 - 1) * (newPixelSize),
                                                newPixelSize - gridThicknessInNumberOfPixels,
                                                newPixelSize - gridThicknessInNumberOfPixels);
                }
            }
        }

        public ImageAndOperationsData(Bitmap importedImage)
        {
            originalImage = new Bitmap(importedImage);
        }

        private void PixelateImage()
        {
            pixelatedImage = ImageTransformations.Pixelate(originalImage, newWidth);
        }

        private void ReduceNumberOfColors(int numberOfIterations = 10)
        {
            colorReducedImage = ImageTransformations.ReduceNumberOfColors(pixelatedImage, numberOfColors, numberOfIterations, out matrixOfNewColors, out colorMeans, out positionsOfEachColor);
        }

        private void AddGrid()
        {
            bool largerIsWidth = colorReducedImage.Width > colorReducedImage.Height;
            augmentedImage = ImageTransformations.ResizeBitmap(colorReducedImage, (largerIsWidth ? colorReducedImage.Width : colorReducedImage.Height) * newPixelSize);

            withGridImage = new Bitmap(augmentedImage);
            int intervalForDarkerLines = 10;
            using (var graphics = Graphics.FromImage(withGridImage))
            {
                //vertical lines
                for (int x = 0; x <= colorReducedImage.Width; x++)
                {
                    Color penColor = x % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    gridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, x * newPixelSize - newPixelSize*0.5f, 0, x * newPixelSize - newPixelSize*0.5f, withGridImage.Height - 1);
                }

                //horizontal lines
                for (int y = 0; y <= colorReducedImage.Height; y++)
                {
                    Color penColor = y % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    gridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, 0, y * newPixelSize - newPixelSize*0.5f, withGridImage.Width - 1, y * newPixelSize - newPixelSize*0.5f);
                }
            }
        }

        private void AddBorder()
        {
            using (var graphics = Graphics.FromImage(withGridImage))
            {
                Color penColor = Color.Black;
                int penSize = (int)(newPixelSize * 0.5f) + 1;
                Pen pen = new Pen(penColor, penSize);
                int offset = (int)(penSize % 2 == 0 ? penSize * 0.5f : (penSize + 1) * 0.5f);
                borderThicknessInNumberOfPixels = penSize;

                graphics.DrawLine(pen, 0, offset, withGridImage.Width, offset); //upper border
                graphics.DrawLine(pen, offset, 0, offset, withGridImage.Height); //left border
                graphics.DrawLine(pen, 0, withGridImage.Height - offset + 1, withGridImage.Width, withGridImage.Height - offset + 1); //bottom border
                graphics.DrawLine(pen, withGridImage.Width - offset + 1, 0, withGridImage.Width - offset + 1, withGridImage.Height); //right border
            }
        }
        public void ProcessImage()
        {
            PixelateImage();
            ReduceNumberOfColors();
            AddGrid();
            AddBorder();
            resultingImage = withGridImage;
        }


    }
}

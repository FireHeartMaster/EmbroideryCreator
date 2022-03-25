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
        public Bitmap resultingImage { get; private set; }

        public int newWidth = 100;
        public int numberOfColors = 10;
        public int numberOfIterations = 10;

        private int newPixelSize = 10;
        
        private Color[] colorMeans;
        private Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor = new Dictionary<int, List<Tuple<int, int>>>();

        public Color[] GetColors() => colorMeans;
        public Dictionary<int, List<Tuple<int, int>>> GetPositionsOfEachColor() => positionsOfEachColor;


        private int borderThicknessInNumberOfPixels = 1;
        private int gridThicknessInNumberOfPixels = 1;

        public void UpdateColorByIndex(int indexToUpdate, Color newColor)
        {
            if(indexToUpdate >= 0 && indexToUpdate < colorMeans.Length)
            {
                colorMeans[indexToUpdate] = newColor;
                PaintNewColorOnImage(indexToUpdate, newColor, resultingImage);
            }
        }

        private void PaintNewColorOnImage(int indexToUpdate, Color newColor, Bitmap image)
        {            
            foreach (Tuple<int, int> position in positionsOfEachColor[indexToUpdate])
            {
                using (var graphics = Graphics.FromImage(image))
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
                                                borderThicknessInNumberOfPixels + (position.Item1/* - 1*/) * (newPixelSize),
                                                borderThicknessInNumberOfPixels + (position.Item2/* - 1*/) * (newPixelSize),
                                                newPixelSize - gridThicknessInNumberOfPixels,
                                                newPixelSize - gridThicknessInNumberOfPixels);
                }
            }
        }

        public ImageAndOperationsData(Bitmap importedImage)
        {
            originalImage = new Bitmap(importedImage);
        }

        private Bitmap PixelateImage(Bitmap originalImage)
        {
            Bitmap pixelatedImage = ImageTransformations.Pixelate(originalImage, newWidth);
            return pixelatedImage;
        }

        private Bitmap PixelateImageAlternateOrder(Bitmap originalImage)
        {
            Bitmap pixelatedImage = ImageTransformations.PixelateAlternateOrder(originalImage, newWidth, ref colorMeans, ref positionsOfEachColor);
            return pixelatedImage;
        }

        private Bitmap ReduceNumberOfColors(Bitmap pixelatedImage, int numberOfIterations = 10)
        {
            Bitmap colorReducedImage = ImageTransformations.ReduceNumberOfColors(pixelatedImage, numberOfColors, numberOfIterations, out colorMeans, out positionsOfEachColor);
            return colorReducedImage;
        }

        private Bitmap AddGrid(Bitmap colorReducedImage)
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
                    gridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, x * newPixelSize/* - newPixelSize*0.5f*/, 0, x * newPixelSize/* - newPixelSize*0.5f*/, withGridImage.Height - 1);
                }

                //horizontal lines
                for (int y = 0; y <= colorReducedImage.Height; y++)
                {
                    Color penColor = y % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    gridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, 0, y * newPixelSize/* - newPixelSize*0.5f*/, withGridImage.Width - 1, y * newPixelSize/* - newPixelSize*0.5f*/);
                }
            }
            return withGridImage;
        }

        private Bitmap AddBorder(Bitmap withGridImage)
        {
            Bitmap withBorderImage = (Bitmap)withGridImage.Clone();
            using (var graphics = Graphics.FromImage(withBorderImage))
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
            return withBorderImage;
        }

        private Bitmap AddBorderIncresingSizeOfOriginalImage(Bitmap withGridImage)
        {
            Color penColor = Color.Black;
            int penSize = (int)(newPixelSize * 0.5f)/* + 1*/;
            Pen pen = new Pen(penColor, penSize);
            borderThicknessInNumberOfPixels = penSize;

            Bitmap withBorderImage = new Bitmap(withGridImage.Width + 2 * borderThicknessInNumberOfPixels - gridThicknessInNumberOfPixels, withGridImage.Height + 2 * borderThicknessInNumberOfPixels - gridThicknessInNumberOfPixels);
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
                int offset = gridThicknessInNumberOfPixels;
                graphics.DrawImage(withGridImage, borderThicknessInNumberOfPixels - offset, borderThicknessInNumberOfPixels - offset, withGridImage.Width, withGridImage.Height);

                int offsetForBorder = (int)(penSize % 2 == 0 ? penSize * 0.5f : (penSize + 1) * 0.5f);
                graphics.DrawLine(pen, 0, offsetForBorder, withBorderImage.Width, offsetForBorder); //upper border
                graphics.DrawLine(pen, offsetForBorder, 0, offsetForBorder, withBorderImage.Height); //left border
                graphics.DrawLine(pen, 0, withBorderImage.Height - offsetForBorder + 1, withBorderImage.Width, withBorderImage.Height - offsetForBorder + 1); //bottom border
                graphics.DrawLine(pen, withBorderImage.Width - offset - 1, 0, withBorderImage.Width - offset - 1, withBorderImage.Height); //right border
            }
            return withBorderImage;
        }

        public void ProcessImage()
        {
            Bitmap processedImage = originalImage;
            processedImage = PixelateImage(processedImage);
            processedImage = ReduceNumberOfColors(processedImage);
            processedImage = AddGrid(processedImage);
            processedImage = AddBorderIncresingSizeOfOriginalImage(processedImage);
            //resultingImage = withBorderImage;
            resultingImage = processedImage;
        }

        //extremmely long time to execute and resource consuming
        public void ProcessImageAlternateOrder()
        {
            Bitmap processedImage = originalImage;
            processedImage = ReduceNumberOfColors(processedImage);
            processedImage = PixelateImageAlternateOrder(processedImage);
            processedImage = AddGrid(processedImage);
            processedImage = AddBorderIncresingSizeOfOriginalImage(processedImage);
            resultingImage = processedImage;
        }

        public void MergeTwoColors(int firstIndex, int otherIndex)
        {

        }

    }
}

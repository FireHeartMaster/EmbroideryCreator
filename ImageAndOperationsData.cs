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
        
        private List<Color> colorMeans;
        private Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor = new Dictionary<int, List<Tuple<int, int>>>();
        private int[,] matrixOfNewColors;

        public List<Color> GetColors() => colorMeans;
        public Dictionary<int, List<Tuple<int, int>>> GetPositionsOfEachColor() => positionsOfEachColor;

        private int borderThicknessInNumberOfPixels = 1;
        private int gridThicknessInNumberOfPixels = 1;

        public void ChangeColorByIndex(int indexToUpdate, Color newColor)
        {
            if(indexToUpdate >= 0 && indexToUpdate < colorMeans.Count)
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
                    graphics.FillRectangle(new SolidBrush(newColor), 
                                                borderThicknessInNumberOfPixels + (position.Item1/* - 1*/) * (newPixelSize),
                                                borderThicknessInNumberOfPixels + (position.Item2/* - 1*/) * (newPixelSize),
                                                newPixelSize - gridThicknessInNumberOfPixels,
                                                newPixelSize - gridThicknessInNumberOfPixels);
                }
            }
        }

        private void PaintNewColorOnPixelPosition(Tuple<int, int> position, Color newColor, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.FillRectangle(new SolidBrush(newColor),
                                            borderThicknessInNumberOfPixels + (position.Item1) * (newPixelSize),
                                            borderThicknessInNumberOfPixels + (position.Item2) * (newPixelSize),
                                            newPixelSize - gridThicknessInNumberOfPixels,
                                            newPixelSize - gridThicknessInNumberOfPixels);
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
            Bitmap colorReducedImage = ImageTransformations.ReduceNumberOfColors(pixelatedImage, numberOfColors, numberOfIterations, 
                                                                                out colorMeans, out positionsOfEachColor, out matrixOfNewColors);
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

            //TODO:remove this position from its original group of positions and put it on the group of the new index
            positionsOfEachColor[originalColorIndex].Remove(position);
            positionsOfEachColor[colorIndexToPaint].Add(position);

            //TODO:repaint that pixel position
            PaintNewColorOnPixelPosition(position, colorMeans[colorIndexToPaint], resultingImage);
        }

    }
}

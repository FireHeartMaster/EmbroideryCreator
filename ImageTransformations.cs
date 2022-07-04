using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmbroideryCreator
{
    public static class ImageTransformations
    {
        private static Random rnd = new Random();

        public static Bitmap Pixelate(Bitmap originalImage, int newWidthSize)
        {
            float aspectRatio = ((float)originalImage.Height) / originalImage.Width;
            Bitmap pixelatedImage = new Bitmap(originalImage, newWidthSize, (int)(newWidthSize * aspectRatio));
            return pixelatedImage;
        }

        public static Bitmap PixelateAlternateOrder(Bitmap originalImage, int newWidthSize, 
            ref List<Color> means, ref Dictionary<int, List<Tuple<int, int>>> clustersOfColors)
        {
            float aspectRatio = ((float)originalImage.Height) / originalImage.Width;
            int newHeightSize = (int)(newWidthSize * aspectRatio);
            Bitmap pixelatedImage = new Bitmap(newWidthSize, newHeightSize);

            clustersOfColors = new Dictionary<int, List<Tuple<int, int>>>();
            for (int i = 0; i < means.Count; i++)
            {
                clustersOfColors.Add(i, new List<Tuple<int, int>>());
            }

            for (int x = 0; x < pixelatedImage.Width; x++)
            {
                for (int y = 0; y < pixelatedImage.Height; y++)
                {

                    int redSum = 0;
                    int greenSum = 0;
                    int blueSum = 0;

                    int amountOfPixels = 0;

                    for (int xOriginalImage = (int)(x * (((float)originalImage.Width) / newWidthSize)); xOriginalImage < (x + 1) * (((float)originalImage.Width) / newWidthSize); xOriginalImage++)
                    {
                        for (int yOriginalImage = (int)(y * (((float)originalImage.Height) / newHeightSize)); yOriginalImage < (y + 1) * (((float)originalImage.Height) / newHeightSize); yOriginalImage++)
                        {
                            Color currentPixelColor = originalImage.GetPixel(xOriginalImage, yOriginalImage);
                            redSum += currentPixelColor.R;
                            greenSum += currentPixelColor.G;
                            blueSum += currentPixelColor.B;

                            amountOfPixels++;
                        }
                    }

                    Color colorMean = Color.FromArgb((int)(((float)redSum) / amountOfPixels), (int)(((float)greenSum) / amountOfPixels), (int)(((float)blueSum) / amountOfPixels));

                    int closestColorIndex = FindClosestColor(colorMean, means);
                    pixelatedImage.SetPixel(x, y, means[closestColorIndex]);
                    clustersOfColors[closestColorIndex].Add(new Tuple<int, int>(x, y));
                }
            }

            return pixelatedImage;
        }

        private static int FindClosestColor(Color originalColor, List<Color> colors)
        {
            int minSquaredDistance = int.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < colors.Count; i++)
            {
                int redDistance = originalColor.R - colors[i].R;
                int greenDistance = originalColor.G - colors[i].G;
                int blueDistance = originalColor.B - colors[i].B;

                int squaredDistance = redDistance * redDistance + greenDistance * greenDistance + blueDistance * blueDistance;
                if(squaredDistance < minSquaredDistance)
                {
                    minSquaredDistance = squaredDistance;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }
        
        public static Bitmap ReduceNumberOfColors(Bitmap imageToReduceColors, int newNumberOfColors, int numberOfIterations, out List<Color> means, 
            out Dictionary<int, List<Tuple<int, int>>> clustersOfColors, out int[,] matrixOfNewColors)
        {
            matrixOfNewColors = new int[imageToReduceColors.Width, imageToReduceColors.Height];
            //Color[] means = InitializeMeans(newNumberOfColors);
            means = InitializeMeansFromData(newNumberOfColors, imageToReduceColors);

            clustersOfColors = new Dictionary<int, List<Tuple<int, int>>>();
            for (int i = 0; i < means.Count; i++)
            {
                clustersOfColors.Add(i, new List<Tuple<int, int>>());
            }

            for (int i = 0; i < numberOfIterations; i++)
            {
                //clustersOfColors.Clear();

                //clearing each list of positions without clearing the whole dictionary, this way we keep track of the same indexes throughout the matrixOfNewColors, means and clustersOfColors
                for (int colorIndex = 0; colorIndex < clustersOfColors.Count; colorIndex++)
                {
                    clustersOfColors[colorIndex] = new List<Tuple<int, int>>();
                }
                for (int x = 0; x < imageToReduceColors.Width; x++)
                {
                    for (int y = 0; y < imageToReduceColors.Height; y++)
                    {
                        matrixOfNewColors[x, y] = FindNewNearestMean(means, imageToReduceColors.GetPixel(x, y));
                        //if(!clustersOfColors.ContainsKey(matrixOfNewColors[x, y]))
                        //{
                        //    clustersOfColors.Add(matrixOfNewColors[x, y], new List<Tuple<int, int>>());
                        //}
                        clustersOfColors[matrixOfNewColors[x, y]].Add(new Tuple<int, int>(x, y));
                    }
                }

                for (int meanIndex = 0; meanIndex < means.Count; meanIndex++)
                {
                    //throw new NotImplementedException();
                    if (clustersOfColors.ContainsKey(meanIndex))
                    {
                        means[meanIndex] = GetMeanColorOfCluster(clustersOfColors[meanIndex], imageToReduceColors); //Get new mean of all colors that are in this cluster
                    }
                }
            }

            for (int x = 0; x < imageToReduceColors.Width; x++)
            {
                for (int y = 0; y < imageToReduceColors.Height; y++)
                {
                    imageToReduceColors.SetPixel(x, y, means[matrixOfNewColors[x, y]]);
                }
            }

            return imageToReduceColors; //do I need to really return this image since the reference is already being modified here
        }

        private static Color GetMeanColorOfCluster(List<Tuple<int, int>> listOfPixelCoordinatesOnThisCluster, Bitmap imageToReduceColors)
        {
            //throw new NotImplementedException();
            if (listOfPixelCoordinatesOnThisCluster.Count == 0) return Color.White;

            int red = 0;
            int green = 0;
            int blue = 0;

            foreach (Tuple<int, int> coordinate in listOfPixelCoordinatesOnThisCluster)
            {
                red += imageToReduceColors.GetPixel(coordinate.Item1, coordinate.Item2).R;
                green += imageToReduceColors.GetPixel(coordinate.Item1, coordinate.Item2).G;
                blue += imageToReduceColors.GetPixel(coordinate.Item1, coordinate.Item2).B;
            }

            red /= listOfPixelCoordinatesOnThisCluster.Count;
            green /= listOfPixelCoordinatesOnThisCluster.Count;
            blue /= listOfPixelCoordinatesOnThisCluster.Count;

            return Color.FromArgb(red, green, blue);
        }

        private static int FindNewNearestMean(List<Color> means, Color color)
        {
            //throw new NotImplementedException();

            int minDistanceSquared = int.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < means.Count; i++)
            {
                int distanceSquared = (means[i].R - color.R) * (means[i].R - color.R) + (means[i].G - color.G) * (means[i].G - color.G) + (means[i].B - color.B) * (means[i].B - color.B);
                if(distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        private static Color[] InitializeMeans(int numberOfIterations)
        {
            //Initializing color means randomly (we could instead initialize them as taking k values from the colors in the image
            Color[] means = new Color[numberOfIterations];
            for (int i = 0; i < means.Length; i++)
            {
                means[i] = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            }

            return means;
        }

        private static List<Color> InitializeMeansFromData(int numberOfColors, Bitmap imageToReduceColors)
        {
            //throw new NotImplementedException();
            //Initializing color means randomly (we could instead initialize them as taking k values from the colors in the image
            //Color[] means = new Color[numberOfColors];
            List<Color> means = new List<Color>(new Color[numberOfColors]); //I need to initialize means 
                                                                            //explicitly specifying its size as numberOfColors
                                                                            //to guarantee we will have the desired number of colors
                                                                            //even if the original image itself doesn't have that many colors
            for (int i = 0; i < means.Count; i++)
            {
                //means[i] = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                means[i] = imageToReduceColors.GetPixel(rnd.Next(0, imageToReduceColors.Width), rnd.Next(0, imageToReduceColors.Height));
            }

            return means;
        }

        private static int[,] InitializeColorClusters(int width, int height, int newNumberOfColors)
        {
            int[,] matrixOfNewColors = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    matrixOfNewColors[i, j] = rnd.Next(0, newNumberOfColors);
                }
            }

            return matrixOfNewColors;
        }

        public static Bitmap RedimensionateImage(Bitmap imageBeforeRedimensioning, int newMaxSize)
        {            
            bool biggerIsWidth = imageBeforeRedimensioning.Width > imageBeforeRedimensioning.Height;

            if (biggerIsWidth)
            {
                return new Bitmap(imageBeforeRedimensioning, newMaxSize, (int)((((float)imageBeforeRedimensioning.Height) / imageBeforeRedimensioning.Width) * newMaxSize));
            }
            else
            {
                return new Bitmap(imageBeforeRedimensioning, (int)((((float)imageBeforeRedimensioning.Width) / imageBeforeRedimensioning.Height) * newMaxSize), newMaxSize);
            }
        }

        public static Bitmap ResizeBitmap(Bitmap sourceBMP, int newPixelSize)
        {
            bool biggerIsWidth = sourceBMP.Width > sourceBMP.Height;
            int newMaxSize = (biggerIsWidth ? sourceBMP.Width : sourceBMP.Height) * newPixelSize;
            int width; 
            int height;            
            if (biggerIsWidth)
            {
                width = newMaxSize;
                height = (int)((((float)sourceBMP.Height) / sourceBMP.Width) * newMaxSize);
            }
            else
            {
                width = (int)((((float)sourceBMP.Width) / sourceBMP.Height) * newMaxSize);
                height = newMaxSize;
            }

            Bitmap result = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                //int positionOffset = newPixelSize / 2; //Because of a bug with DrawImage, the source image isn't drawn in the correct position on the target image,
                                                         //it ended up slightly more to the top and to the left, that's why we need to use an offset or to set the pixel
                                                         //offset mode to high quality
                graphics.DrawImage(sourceBMP, /*positionOffset*/0, /*positionOffset*/0, width, height);
            }
            return result;
        }

        public static Bitmap CreateSolidColorBitmap(Color color, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(color))
            {
                graphics.FillRectangle(brush, 0, 0, width, height);
            }
            return bitmap;
        }

        public static Tuple<int, int> ConvertFromPictureBoxToRealImage(PictureBox pictureBox, Tuple<int, int> pictureBoxPosition)
        {
            bool biggerIsWidth = pictureBox.Image.Width > pictureBox.Image.Height;

            float ratio;
            if (biggerIsWidth)
            {
                ratio = ((float)pictureBox.Image.Width) / pictureBox.Size.Width;
            }
            else
            {
                ratio = ((float)pictureBox.Image.Height) / pictureBox.Size.Height;
            }
            int horizontalPosition = (int)(ratio * (pictureBoxPosition.Item1 - (pictureBox.Size.Width * 0.5f)) + pictureBox.Image.Width * 0.5f);
            int verticalPosition = (int)(ratio * (pictureBoxPosition.Item2 - (pictureBox.Size.Height * 0.5f)) + pictureBox.Image.Height * 0.5f);
            return new Tuple<int, int>(horizontalPosition, verticalPosition);
        }

        public static float CalculateDistanceOfPointToLine(Tuple<float, float> referencePoint, Tuple<float, float> lineStartingPoint, Tuple<float, float> lineEndingPoint)
        {
            float numerator = (lineEndingPoint.Item1 - lineStartingPoint.Item1) * (referencePoint.Item2 - lineStartingPoint.Item2) - (lineEndingPoint.Item2 - lineStartingPoint.Item2) * (referencePoint.Item1 - lineStartingPoint.Item1);
            float denominator = (float)Math.Sqrt((lineEndingPoint.Item1 - lineStartingPoint.Item1) * (lineEndingPoint.Item1 - lineStartingPoint.Item1) + (lineEndingPoint.Item2 - lineStartingPoint.Item2) * (lineEndingPoint.Item2 - lineStartingPoint.Item2));
            return Math.Abs(numerator) / denominator;
        }
    }
}

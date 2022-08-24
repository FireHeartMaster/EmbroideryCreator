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

        public static Bitmap PixelateExactlyAccordingToOriginal(Bitmap originalImage, int newWidthSize)
        {
            float aspectRatio = ((float)originalImage.Height) / originalImage.Width;
            Bitmap pixelatedImage = new Bitmap(newWidthSize, (int)(newWidthSize * aspectRatio));

            int step = originalImage.Width / newWidthSize;

            for (int x = 0; x < newWidthSize; x++)
            {
                for (int y = 0; y < pixelatedImage.Height; y++)
                {
                    pixelatedImage.SetPixel(x, y, originalImage.GetPixel(x * step, y * step));
                }
            }

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

        public static Bitmap SetColorsWithoutReducingNumberOfColors(Bitmap imageToReduceColors, out List<Color> means,
            out Dictionary<int, List<Tuple<int, int>>> clustersOfColors, out int[,] matrixOfNewColors)
        {
            matrixOfNewColors = new int[imageToReduceColors.Width, imageToReduceColors.Height];
            means = new List<Color>();
            clustersOfColors = new Dictionary<int, List<Tuple<int, int>>>();

            Dictionary<Tuple<int, int, int>, int> dictionaryOfColorsAndIndexes = new Dictionary<Tuple<int, int, int>, int>();

            for (int x = 0; x < imageToReduceColors.Width; x++)
            {
                for (int y = 0; y < imageToReduceColors.Height; y++)
                {
                    Color currentColor = imageToReduceColors.GetPixel(x, y);

                    var tupleKeyForCurrentColor = new Tuple<int, int, int>(currentColor.R, currentColor.G, currentColor.B);

                    if (!dictionaryOfColorsAndIndexes.ContainsKey(tupleKeyForCurrentColor))
                    {
                        int newIndex = dictionaryOfColorsAndIndexes.Count;
                        dictionaryOfColorsAndIndexes.Add(tupleKeyForCurrentColor, newIndex);
                        means.Add(currentColor);
                        clustersOfColors.Add(newIndex, new List<Tuple<int, int>>());
                    }
                    matrixOfNewColors[x, y] = dictionaryOfColorsAndIndexes[tupleKeyForCurrentColor];
                    clustersOfColors[dictionaryOfColorsAndIndexes[tupleKeyForCurrentColor]].Add(new Tuple<int, int>(x, y));
                }
            }

            return imageToReduceColors;
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

        public static int FindNewNearestMean(List<Color> means, Color color)
        {
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
            GetNewSize(sourceBMP.Width, sourceBMP.Height, newPixelSize, out int width, out int height);

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

        public static Bitmap CropOrAddPadding(Bitmap originalImage, int newWidth, int newHeight)
        {
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using(var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(originalImage, 0, 0);
            }

            return newImage;
        }

        public static void GetNewSize(int originalWidth, int originalHeight, int newPixelSize, out int width, out int height)
        {
            bool biggerIsWidth = originalWidth > originalHeight;
            int newMaxSize = (biggerIsWidth ? originalWidth : originalHeight) * newPixelSize;
            if (biggerIsWidth)
            {
                width = newMaxSize;
                height = (int)((((float)originalHeight) / originalWidth) * newMaxSize);
            }
            else
            {
                width = (int)((((float)originalWidth) / originalHeight) * newMaxSize);
                height = newMaxSize;
            }
        }

        public static Bitmap CombineImages(Bitmap baseImage, Bitmap topImage)
        {
            Bitmap imagesCombined = new Bitmap(baseImage);

            using(Graphics graphics = Graphics.FromImage(imagesCombined))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                graphics.DrawImage(topImage, 0, 0, topImage.Width, topImage.Height);
            }

            return imagesCombined;
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

        public static Tuple<int, int> ConvertFromPictureBoxToRealImage(PictureBox pictureBox, Tuple<int, int> pictureBoxPosition, bool roundToClosest = true)
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
            double horizontalPosition = (ratio * (pictureBoxPosition.Item1 - (pictureBox.Size.Width * 0.5f)) + pictureBox.Image.Width * 0.5f);
            double verticalPosition = (ratio * (pictureBoxPosition.Item2 - (pictureBox.Size.Height * 0.5f)) + pictureBox.Image.Height * 0.5f);

            int horizontalPositionRounded = (int)(roundToClosest ? Math.Round(horizontalPosition) : horizontalPosition);
            int verticalPositionRounded = (int)(roundToClosest ? Math.Round(verticalPosition) : verticalPosition);

            return new Tuple<int, int>(horizontalPositionRounded, verticalPositionRounded);
        }

        public static float CalculateDistanceOfPointToLine(Tuple<float, float> referencePoint, Tuple<float, float> lineStartingPoint, Tuple<float, float> lineEndingPoint)
        {
            float numerator = (lineEndingPoint.Item1 - lineStartingPoint.Item1) * (referencePoint.Item2 - lineStartingPoint.Item2) - (lineEndingPoint.Item2 - lineStartingPoint.Item2) * (referencePoint.Item1 - lineStartingPoint.Item1);
            float denominator = (float)Math.Sqrt((lineEndingPoint.Item1 - lineStartingPoint.Item1) * (lineEndingPoint.Item1 - lineStartingPoint.Item1) + (lineEndingPoint.Item2 - lineStartingPoint.Item2) * (lineEndingPoint.Item2 - lineStartingPoint.Item2));
            return Math.Abs(numerator) / denominator;
        }


        public static float DotProduct(Tuple<float, float> firstVector, Tuple<float, float> secondVector)
        {
            return firstVector.Item1 * secondVector.Item1 + firstVector.Item2 * secondVector.Item2;
        }

        public static float DotProduct(Tuple<float, float, float> firstVector, Tuple<float, float, float> secondVector)
        {
            return firstVector.Item1 * secondVector.Item1 + firstVector.Item2 * secondVector.Item2 + firstVector.Item3 * secondVector.Item3;
        }

        public static Tuple<float, float, float> CrossProduct(Tuple<float, float, float> firstVector, Tuple<float, float, float> secondVector)
        {
            float xResult = firstVector.Item2 * secondVector.Item3 - firstVector.Item3 * secondVector.Item2;
            float yResult = firstVector.Item3 * secondVector.Item1 - firstVector.Item1 * secondVector.Item3;
            float zResult = firstVector.Item1 * secondVector.Item2 - firstVector.Item2 * secondVector.Item1;

            return new Tuple<float, float, float>(xResult, yResult, zResult);
        }

        public static float NormOfVector(Tuple<float, float, float> vector)
        {
            return (float)Math.Sqrt(vector.Item1 * vector.Item1 + vector.Item2 * vector.Item2 + vector.Item3 * vector.Item3);
        }

        public static float AngleBetweenVectors(Tuple<float, float, float> firstVector, Tuple<float, float, float> secondVector)
        {
            float dotProductResult = DotProduct(firstVector, secondVector);
            float normFirstVector = NormOfVector(firstVector);
            float normSecondVector = NormOfVector(secondVector);

            float cosineOfAngle = dotProductResult / (normFirstVector * normSecondVector);

            return (float)Math.Acos(cosineOfAngle);
        }

        public static Tuple<float, float, float> RotateVectorAroundAxis(Tuple<float, float, float> vectorToRotate, Tuple<float, float, float> referenceAxis, float angleInRadians)
        {
            float axisNorm = NormOfVector(referenceAxis);
            Tuple<float, float, float> axisUnitVector = new Tuple<float, float, float>(referenceAxis.Item1 / axisNorm, referenceAxis.Item2 / axisNorm, referenceAxis.Item3 / axisNorm);

            float cosineOfAngle = (float)Math.Cos(angleInRadians);
            float sineOfAngle = (float)Math.Sin(angleInRadians);

            float uX = axisUnitVector.Item1;
            float uY = axisUnitVector.Item2;
            float uZ = axisUnitVector.Item3;

            float xResult = (cosineOfAngle + uX * uX * (1 - cosineOfAngle))     * vectorToRotate.Item1 +
                            (uX * uY * (1 - cosineOfAngle) - uZ * sineOfAngle)  * vectorToRotate.Item2 +
                            (uX * uZ * (1 - cosineOfAngle) + uY * sineOfAngle)  * vectorToRotate.Item3;

            float yResult = (uY * uX * (1 - cosineOfAngle) + uZ * sineOfAngle)  * vectorToRotate.Item1 +
                            (cosineOfAngle + uY * uY * (1 - cosineOfAngle))     * vectorToRotate.Item2 +
                            (uY * uZ * (1 - cosineOfAngle) - uX * sineOfAngle)  * vectorToRotate.Item3;

            float zResult = (uZ * uX * (1 - cosineOfAngle) - uY * sineOfAngle)  * vectorToRotate.Item1 +
                            (uZ * uY * (1 - cosineOfAngle) + uX * sineOfAngle)  * vectorToRotate.Item2 +
                            (cosineOfAngle + uZ * uZ * (1 - cosineOfAngle))     * vectorToRotate.Item3;

            return new Tuple<float, float, float>(xResult, yResult, zResult);
        }

        public static Color TransformColor(Color baseColor, Color targetColor, Color colorToTransform)
        {
            Tuple<float, float, float> differenceVector = new Tuple<float, float, float>(colorToTransform.R - baseColor.R,
                                                                                            colorToTransform.G - baseColor.G,
                                                                                            colorToTransform.B - baseColor.B);
            Tuple<float, float, float> baseVector = new Tuple<float, float, float>(baseColor.R, baseColor.G, baseColor.B);
            Tuple<float, float, float> targetVector = new Tuple<float, float, float>(targetColor.R, targetColor.G, targetColor.B);

            Tuple<float, float, float> axis = CrossProduct(baseVector, targetVector);

            float angleBetweenBaseAndTarget = AngleBetweenVectors(baseVector, targetVector);

            Tuple<float, float, float> rotatedDifferenceVector = RotateVectorAroundAxis(differenceVector, axis, angleBetweenBaseAndTarget);

            Tuple<float, float, float> newVector = new Tuple<float, float, float>(targetVector.Item1 + rotatedDifferenceVector.Item1,
                                                            targetVector.Item2 + rotatedDifferenceVector.Item2,
                                                            targetVector.Item3 + rotatedDifferenceVector.Item3);
            float newR = newVector.Item1;
            float newG = newVector.Item2;
            float newB = newVector.Item3;

            newR = NormalizeValueForColorLimits(newR);
            newG = NormalizeValueForColorLimits(newG);
            newB = NormalizeValueForColorLimits(newB);

            Color resultingColor;

            try
            {
                resultingColor = Color.FromArgb((int)newR, (int)newG, (int)newB);
            }
            catch(Exception exception)
            {
                resultingColor = targetColor;
            }

            return resultingColor;
        }

        private static float NormalizeValueForColorLimits(float colorValue)
        {
            if (colorValue < 0) colorValue = 0;
            if (colorValue > 255) colorValue = 255;
            return colorValue;
        }

        public static bool TwoVectorsAreFacingSameDirection(Tuple<float, float> firstVector, Tuple<float, float> secondVector)
        {
            return DotProduct(firstVector, secondVector) >= 0;
        }

        public static Tuple<float, float> GetUnitVector(Tuple<float, float> vector)
        {
            float norm = GetNormOfVector(vector);
            return new Tuple<float, float>(vector.Item1 / norm, vector.Item2 / norm);
        }

        private static float GetNormOfVector(Tuple<float, float> vector)
        {
            return (float)Math.Sqrt(DotProduct(vector, vector));
        }

        public static float GetSizeOfProjectionOnAnotherVector(Tuple<float, float> vectorToBeProjected, Tuple<float, float> referenceVector)
        {
            return DotProduct(vectorToBeProjected, GetUnitVector(referenceVector));
        }

        public static Tuple<float, float> GetProjectionOfVectorOntoAnother(Tuple<float, float> vectorToBeProjected, Tuple<float, float> referenceVector)
        {
            float sizeOfProjection = GetSizeOfProjectionOnAnotherVector(vectorToBeProjected, referenceVector);
            return new Tuple<float, float>(referenceVector.Item1 * sizeOfProjection, referenceVector.Item2 * sizeOfProjection);
        }

        public static bool IsPointBetweenTwoOthers(Tuple<float, float> referencePoint, Tuple<float, float> lineStartingPoint, Tuple<float, float> lineEndingPoint)
        {
            Tuple<float, float> referenceVector = new Tuple<float, float>(lineEndingPoint.Item1 - lineStartingPoint.Item1, lineEndingPoint.Item2 - lineStartingPoint.Item2);
            float sizeOfProjection = GetSizeOfProjectionOnAnotherVector(new Tuple<float, float>(referencePoint.Item1 - lineStartingPoint.Item1, referencePoint.Item2 - lineStartingPoint.Item2), referenceVector);

            return sizeOfProjection >= 0 && sizeOfProjection <= GetNormOfVector(referenceVector);
        }

        public static bool IsPointCloseEnoughToLineDefinedByTwoPoints(Tuple<float, float> referencePoint, Tuple<float, float> lineStartingPoint, Tuple<float, float> lineEndingPoint, float closeDistance)
        {
            float distance = CalculateDistanceOfPointToLine(referencePoint, lineStartingPoint, lineEndingPoint);

            if (distance > closeDistance) return false;

            Tuple<float, float> referenceVector = new Tuple<float, float>(lineEndingPoint.Item1 - lineStartingPoint.Item1, lineEndingPoint.Item2 - lineStartingPoint.Item2);
            Tuple<float, float> unitReferenceVector = GetUnitVector(referenceVector);
            Tuple<float, float> closeDistanceVector = new Tuple<float, float>(unitReferenceVector.Item1 * closeDistance, unitReferenceVector.Item2 * closeDistance);


            Tuple<float, float> auxStartingPoint = new Tuple<float, float>(lineStartingPoint.Item1 - closeDistanceVector.Item1, lineStartingPoint.Item2 - closeDistanceVector.Item2);
            Tuple<float, float> auxEndingPoint = new Tuple<float, float>(lineEndingPoint.Item1 + closeDistanceVector.Item1, lineEndingPoint.Item2 + closeDistanceVector.Item2);

            return IsPointBetweenTwoOthers(referencePoint, auxStartingPoint, auxEndingPoint);
        }

        public static void RescaleImageToMaximumSize(double width, double height, double maxRescaledWidth, double maxRescaledHeight, out double newWidth, out double newHeight)
       {
            double rescaleFactor = maxRescaledWidth / width;

            newHeight = rescaleFactor * height;
            if(newHeight < maxRescaledHeight)
            {
                newWidth = maxRescaledWidth;
            }
            else
            {
                rescaleFactor = maxRescaledHeight / height;
                newWidth = rescaleFactor * width;
                newHeight = maxRescaledHeight;
            }
        }

        public static void RescaleImageToMinimumSize(double width, double height, double minRescaledWidth, double minRescaledHeight, out double newWidth, out double newHeight)
        {
            double rescaleFactor = minRescaledWidth / width;

            newHeight = rescaleFactor * height;
            if (newHeight > minRescaledHeight)
            {
                newWidth = minRescaledWidth;
            }
            else
            {
                rescaleFactor = minRescaledHeight / height;
                newWidth = rescaleFactor * width;
                newHeight = minRescaledHeight;
            }
        }

        public static bool IsPointInsideRectangle(Tuple<double, double> pointToCheck, Tuple<double, double> rectangleTopLeftCorner, double width, double height)
        {
            return pointToCheck.Item1 >= rectangleTopLeftCorner.Item1 &&
                    pointToCheck.Item1 <= rectangleTopLeftCorner.Item1 + width &&
                    pointToCheck.Item2 >= rectangleTopLeftCorner.Item2 &&
                    pointToCheck.Item2 <= rectangleTopLeftCorner.Item2 + height;
        }

        //This function doesn't check the slope and alignment of the points
        public static bool IsPointBetweenTwoOthers(Tuple<double, double> pointToCheck, Tuple<double, double> pointA, Tuple<double, double> pointB)
        {
            bool xIsBetweenPoints = ((pointB.Item1 >= pointA.Item1) && (pointToCheck.Item1 >= pointA.Item1 && pointToCheck.Item1 <= pointB.Item1)) ||
                        ((pointA.Item1 >= pointB.Item1) && (pointToCheck.Item1 >= pointB.Item1 && pointToCheck.Item1 <= pointA.Item1));

            bool yIsBetweenPoints = ((pointB.Item2 >= pointA.Item2) && (pointToCheck.Item2 >= pointA.Item2 && pointToCheck.Item2 <= pointB.Item2)) ||
                        ((pointA.Item2 >= pointB.Item2) && (pointToCheck.Item2 >= pointB.Item2 && pointToCheck.Item2 <= pointA.Item2));

            return xIsBetweenPoints && yIsBetweenPoints;
        }

        public static List<Tuple<double, double>> ComputeIntersectionsOfLineAndRectangle(double xA, double yA, double xB, double yB, double xO, double yO, double width, double height)
        {
            double commonTerm = (yB - yA) * xA - (xB - xA) * yA;

            List<Tuple<double, double>> intersections = new List<Tuple<double, double>>();

            double x, y;

            //first intersection
            x = xO;
            y = (commonTerm - (yB - yA) * xO) / (xA - xB);
            intersections.Add(new Tuple<double, double>(x, y));

            //second intersection
            x = xO + width;
            y = (commonTerm - (yB - yA) * (xO + width)) / (xA - xB);
            intersections.Add(new Tuple<double, double>(x, y));

            //third intersection
            y = yO;
            x = (commonTerm + (xB - xA) * yO) / (yB - yA);
            intersections.Add(new Tuple<double, double>(x, y));

            //forth intersection
            y = yO + height;
            x = (commonTerm + (xB - xA) * (yO + height)) / (yB - yA);
            intersections.Add(new Tuple<double, double>(x, y));

            return intersections;
        }

        public static List<Tuple<double, double>> FindIntersectionsOfLineAndSquare(double xA, double yA, double xB, double yB, double xO, double yO, double width, double height)
        {
            List<Tuple<double, double>> pointsInsideSquare = new List<Tuple<double, double>>();

            bool aIsInside = IsPointInsideRectangle(new Tuple<double, double>(xA, yA), new Tuple<double, double>(xO, yO), width, height);
            bool bIsInside = IsPointInsideRectangle(new Tuple<double, double>(xB, yB), new Tuple<double, double>(xO, yO), width, height);


            if (aIsInside && bIsInside)
            {
                //both points inside the rectangle
                pointsInsideSquare.Add(new Tuple<double, double>(xA, yA));
                pointsInsideSquare.Add(new Tuple<double, double>(xB, yB));

                return pointsInsideSquare;
            }
            else if((aIsInside && !bIsInside) || (!aIsInside && bIsInside))
            {
                //only one point inside the rectangle
                Tuple<double, double> pointInside;
                Tuple<double, double> pointOutside;

                if (aIsInside && !bIsInside)
                {
                    pointInside = new Tuple<double, double>(xA, yA);
                    pointOutside = new Tuple<double, double>(xB, yB);
                }
                else
                {
                    pointInside = new Tuple<double, double>(xB, yB);
                    pointOutside = new Tuple<double, double>(xA, yA);
                }

                //verifying if we're dealing with horizontal or vertical lines, for which the intersection formula doesn't work the same way
                if(xA == xB)
                {
                    List<Tuple<double, double>> verticalPointsInsideRectangle = new List<Tuple<double, double>>();
                    verticalPointsInsideRectangle.Add(pointInside);
                    if (pointOutside.Item2 > pointInside.Item2)
                    {
                        verticalPointsInsideRectangle.Add(new Tuple<double, double>(pointInside.Item1, yO + height));
                    }
                    else
                    {
                        verticalPointsInsideRectangle.Add(new Tuple<double, double>(pointInside.Item1, yO));
                    }

                    return verticalPointsInsideRectangle;
                }

                if(yA == yB)
                {
                    List<Tuple<double, double>> horizontalPointsInsideRectangle = new List<Tuple<double, double>>();
                    horizontalPointsInsideRectangle.Add(pointInside);
                    if(pointOutside.Item1 > pointInside.Item1)
                    {
                        horizontalPointsInsideRectangle.Add(new Tuple<double, double>(xO + width, pointInside.Item2));
                    }
                    else
                    {
                        horizontalPointsInsideRectangle.Add(new Tuple<double, double>(xO, pointInside.Item2));
                    }

                    return horizontalPointsInsideRectangle;
                }

                List<Tuple<double, double>> intersectionsInside = GetIntersectionsWithRectangleInsideOfIt(xA, yA, xB, yB, xO, yO, width, height);
                bool direction = pointOutside.Item1 - pointInside.Item1 > 0;

                bool firstIntersectionDirection = intersectionsInside[0].Item1 - pointInside.Item1 > 0;

                List<Tuple<double, double>> result = new List<Tuple<double, double>>();
                result.Add(pointInside);

                if ((firstIntersectionDirection && direction) || (!firstIntersectionDirection && !direction))
                {
                    //first intersection is in the same direction as the point outside, so it should be the correct one
                    result.Add(intersectionsInside[0]);
                }
                else
                {
                    //first intersection is not in the same direction as the point outside, so the second point should be the correct one
                    result.Add(intersectionsInside[1]);
                }

                return result;
            }
            else
            {
                //no point inside the rectangle

                //verifying if we're dealing with horizontal or vertical lines, for which the intersection formula doesn't work the same way
                if (xA == xB)
                {
                    if (xA >= xO && xA <= xO + width)
                    {
                        List<Tuple<double, double>> verticalIntersections = new List<Tuple<double, double>>();
                        verticalIntersections.Add(new Tuple<double, double>(xA, yO));
                        verticalIntersections.Add(new Tuple<double, double>(xA, yO + height));

                        foreach (Tuple<double, double> intersection in verticalIntersections)
                        {
                            if(!IsPointBetweenTwoOthers(intersection, new Tuple<double, double>(xA, yA), new Tuple<double, double>(xB, yB)))
                            {
                                return new List<Tuple<double, double>>();
                            }
                        }

                        return verticalIntersections;
                    }
                    else
                    {
                        return new List<Tuple<double, double>>();
                    }
                }

                if (yA == yB)
                {
                    if (yA >= yO && yA <= yO + height)
                    {
                        List<Tuple<double, double>> horizontalIntersections = new List<Tuple<double, double>>();
                        horizontalIntersections.Add(new Tuple<double, double>(xO, yA));
                        horizontalIntersections.Add(new Tuple<double, double>(xO + width, yA));

                        foreach (Tuple<double, double> intersection in horizontalIntersections)
                        {
                            if (!IsPointBetweenTwoOthers(intersection, new Tuple<double, double>(xA, yA), new Tuple<double, double>(xB, yB)))
                            {
                                return new List<Tuple<double, double>>();
                            }
                        }

                        return horizontalIntersections;
                    }
                    else
                    {
                        return new List<Tuple<double, double>>();
                    }
                }

                //At this point we know that the points A and B aren't inside the rectangle and don't form an horizontal or vertical line
                List<Tuple<double, double>> pointsInside = GetIntersectionsWithRectangleInsideOfIt(xA, yA, xB, yB, xO, yO, width, height);

                if (pointsInside.Count >= 2)
                {
                    foreach (Tuple<double, double> intersection in pointsInside)
                    {
                        if (!IsPointBetweenTwoOthers(intersection, new Tuple<double, double>(xA, yA), new Tuple<double, double>(xB, yB)))
                        {
                            return new List<Tuple<double, double>>();
                        }
                    }

                    return pointsInside;
                }
                else
                {
                    return new List<Tuple<double, double>>();
                }

            }
        }

        public static List<Tuple<double, double>> GetIntersectionsWithRectangleInsideOfIt(double xA, double yA, double xB, double yB, double xO, double yO, double a, double b)
        {
            //A and B are the points that will define the possible intersections with the rectangle
            //O is the top left corner of the rectangle, while a and b are respectively the rectangle's width and height
            List<Tuple<double, double>> intersections = ComputeIntersectionsOfLineAndRectangle(xA, yA, xB, yB, xO, yO, a, b);

            List<Tuple<double, double>> pointsInside = new List<Tuple<double, double>>();

            foreach (Tuple<double, double> intersectionPoint in intersections)
            {
                if (IsPointInsideRectangle(new Tuple<double, double>(intersectionPoint.Item1, intersectionPoint.Item2), new Tuple<double, double>(xO, yO), a, b))
                {
                    if (!pointsInside.Contains(intersectionPoint))
                    {
                        pointsInside.Add(intersectionPoint);
                    }
                }
            }

            return pointsInside;
        }

        public static Tuple<double, double> ConvertPairType(Tuple<float, float> pair)
        {
            return new Tuple<double, double>(pair.Item1, pair.Item2);
        }

        public static Tuple<float, float> ConvertPairType(Tuple<double, double> pair)
        {
            return new Tuple<float, float>((float)pair.Item1, (float)pair.Item2);
        }
    }
}

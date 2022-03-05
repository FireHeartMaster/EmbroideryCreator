using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public static class ImageTransformations
    {
        public static Bitmap Pixelate(Bitmap originalImage, int newWidthSize)
        {
            float aspectRatio = ((float)originalImage.Height) / originalImage.Width;
            Bitmap pixelatedImage = new Bitmap(originalImage, newWidthSize, (int)(newWidthSize * aspectRatio));
            return pixelatedImage;
        }

        public static Bitmap ReduceNumberOfColors(Bitmap imageToReduceColors, int newNumberOfColors, int numberOfIterations)
        {
            int[][] matrixOfNewColors = InitializeColorClusters(imageToReduceColors.Width, imageToReduceColors.Height, newNumberOfColors);
            Color[] means = InitializeMeans(numberOfIterations);

            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int x = 0; x < imageToReduceColors.Width; x++)
                {
                    for (int y = 0; y < imageToReduceColors.Height; y++)
                    {
                        matrixOfNewColors[x][y] = FindNewNearestMean(means, imageToReduceColors.GetPixel(x, y));
                    }
                }

                for (int meanIndex = 0; meanIndex < means.Length; meanIndex++)
                {
                    throw new NotImplementedException();
                    //means[meanIndex] = //Get new mean of all colors that are in this cluster
                }
            }

            for (int x = 0; x < imageToReduceColors.Width; x++)
            {
                for (int y = 0; y < imageToReduceColors.Height; y++)
                {
                    imageToReduceColors.SetPixel(x, y, means[matrixOfNewColors[x][y]]);
                }
            }

            return imageToReduceColors; //do I need to really return this image since the reference is already being modified here
        }

        private static int FindNewNearestMean(Color[] means, Color color)
        {
            throw new NotImplementedException();
        }

        private static Color[] InitializeMeans(int numberOfIterations)
        {
            throw new NotImplementedException();
        }

        private static int[][] InitializeColorClusters(int width, int height, int newNumberOfColors)
        {
            throw new NotImplementedException();
        }
    }
}

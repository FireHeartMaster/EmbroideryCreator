﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Bitmap ReduceNumberOfColors(Bitmap imageToReduceColors, int newNumberOfColors, int numberOfIterations, out int[,] matrixOfNewColors, out Color[] means, 
            out Dictionary<int, List<Tuple<int, int>>> clustersOfColors)
        {
            matrixOfNewColors = InitializeColorClusters(imageToReduceColors.Width, imageToReduceColors.Height, newNumberOfColors);
            //Color[] means = InitializeMeans(newNumberOfColors);
            means = InitializeMeansFromData(newNumberOfColors, imageToReduceColors);

            clustersOfColors = new Dictionary<int, List<Tuple<int, int>>>();
            for (int i = 0; i < means.Length; i++)
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

                for (int meanIndex = 0; meanIndex < means.Length; meanIndex++)
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

        private static int FindNewNearestMean(Color[] means, Color color)
        {
            //throw new NotImplementedException();

            int minDistanceSquared = int.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < means.Length; i++)
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

        private static Color[] InitializeMeansFromData(int numberOfColors, Bitmap imageToReduceColors)
        {
            //throw new NotImplementedException();
            //Initializing color means randomly (we could instead initialize them as taking k values from the colors in the image
            Color[] means = new Color[numberOfColors];
            for (int i = 0; i < means.Length; i++)
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

        public static Bitmap ResizeBitmap(Bitmap sourceBMP, int newMaxSize)
        {
            int width; 
            int height;
            bool biggerIsWidth = sourceBMP.Width > sourceBMP.Height;
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
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(sourceBMP, 0, 0, width, height);
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
    }
}

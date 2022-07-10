using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    [Serializable]public class ImageAndOperationsDataSerialized
    {
        public Bitmap originalImage;
        public Bitmap resultingImage;
        public Bitmap threadImage;
        public Bitmap backstitchImage;
        public Bitmap gridImage;
        public Bitmap borderImage;

        public int newWidth;
        public int numberOfColors;
        public int numberOfIterations;

        public int newPixelSize;

        public List<Color> colorMeans;
        public Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor;
        public int[,] matrixOfNewColors;
        public List<bool> colorIsBackgroundList;

        public Dictionary<int, HashSet<BackstitchLine>> backstitchLines;
        public Dictionary<int, Color> backstitchColors;                

        public int borderThicknessInNumberOfPixels;
        public int gridThicknessInNumberOfPixels;

        public ImageAndOperationsDataSerialized(Bitmap originalImage, Bitmap resultingImage, Bitmap threadImage, Bitmap backstitchImage, Bitmap gridImage, Bitmap borderImage, int newWidth, int numberOfColors, int numberOfIterations, int newPixelSize, List<Color> colorMeans, Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, int[,] matrixOfNewColors, List<bool> colorIsBackgroundList, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, int borderThicknessInNumberOfPixels, int gridThicknessInNumberOfPixels)
        {
            this.originalImage = originalImage;
            this.resultingImage = resultingImage;
            this.threadImage = threadImage;
            this.gridImage = gridImage;
            this.borderImage = borderImage;
            this.backstitchImage = backstitchImage;
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
            this.borderThicknessInNumberOfPixels = borderThicknessInNumberOfPixels;
            this.gridThicknessInNumberOfPixels = gridThicknessInNumberOfPixels;
        }

        public static ImageAndOperationsData DeserializeData(string filePath)
        {
            ImageAndOperationsDataSerialized deserializedData = SerializerHelper.ReadFromFile<ImageAndOperationsDataSerialized>(filePath);

            return new ImageAndOperationsData(deserializedData.originalImage, deserializedData.resultingImage, deserializedData.threadImage, deserializedData.backstitchImage, deserializedData.gridImage, deserializedData.borderImage, deserializedData.newWidth, deserializedData.numberOfColors, deserializedData.numberOfIterations, deserializedData.newPixelSize, deserializedData.colorMeans, deserializedData.positionsOfEachColor, deserializedData.matrixOfNewColors, deserializedData.colorIsBackgroundList, deserializedData.backstitchLines, deserializedData.backstitchColors, deserializedData.borderThicknessInNumberOfPixels, deserializedData.gridThicknessInNumberOfPixels);
        }
    }


}

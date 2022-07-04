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
        public Bitmap BackstitchImage { get; private set; }

        public int newWidth = 100;
        public int numberOfColors = 10;
        public int numberOfIterations = 10;

        private int newPixelSize = 10;
        
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

        public void ChangeColorByIndex(int indexToUpdate, Color newColor)
        {
            if(indexToUpdate >= 0 && indexToUpdate < colorMeans.Count)
            {
                colorMeans[indexToUpdate] = newColor;
                PaintNewColorOnImage(indexToUpdate, newColor, ResultingImage);
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

        private void PaintNewColorOnPixelPosition(Tuple<int, int> position, Color newColor, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                FillPixelAtCoordinate(newColor, graphics, position);
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

        private void FillPixelAtCoordinate(Color newColor, Graphics graphics, Tuple<int, int> position)
        {
            graphics.FillRectangle(new SolidBrush(newColor),
                                                        BorderThicknessInNumberOfPixels + (position.Item1) * (newPixelSize),
                                                        BorderThicknessInNumberOfPixels + (position.Item2) * (newPixelSize),
                                                        newPixelSize - GridThicknessInNumberOfPixels,
                                                        newPixelSize - GridThicknessInNumberOfPixels);
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

        public ImageAndOperationsData(Bitmap originalImage, Bitmap resultingImage, Bitmap backstitchImage, int newWidth, int numberOfColors, int numberOfIterations, int newPixelSize, List<Color> colorMeans, Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor, int[,] matrixOfNewColors, List<bool> colorIsBackgroundList, Dictionary<int, HashSet<BackstitchLine>> backstitchLines, Dictionary<int, Color> backstitchColors, int borderThicknessInNumberOfPixels, int gridThicknessInNumberOfPixels) : this(originalImage)
        {
            ResultingImage = resultingImage;
            BackstitchImage = backstitchImage;
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
        }

        public void SerializeData(string filePath)
        {
            ImageAndOperationsDataSerialized serializableData = new ImageAndOperationsDataSerialized(originalImage, ResultingImage, BackstitchImage, newWidth, numberOfColors, numberOfIterations, newPixelSize, colorMeans, positionsOfEachColor, matrixOfNewColors, colorIsBackgroundList, backstitchLines, backstitchColors, BorderThicknessInNumberOfPixels, GridThicknessInNumberOfPixels);

            SerializerHelper.WriteToFile<ImageAndOperationsDataSerialized>(filePath, serializableData);
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
                    GridThicknessInNumberOfPixels = 1;
                    graphics.DrawLine(pen, x * newPixelSize/* - newPixelSize*0.5f*/, 0, x * newPixelSize/* - newPixelSize*0.5f*/, withGridImage.Height - 1);
                }

                //horizontal lines
                for (int y = 0; y <= colorReducedImage.Height; y++)
                {
                    Color penColor = y % intervalForDarkerLines == 0 ? Color.Black : Color.Gray;
                    Pen pen = new Pen(penColor, 1.0f);
                    GridThicknessInNumberOfPixels = 1;
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
                BorderThicknessInNumberOfPixels = penSize;

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
            BorderThicknessInNumberOfPixels = penSize;

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

        public void ProcessImage()
        {
            Bitmap processedImage = originalImage;
            processedImage = PixelateImage(processedImage);
            processedImage = ReduceNumberOfColors(processedImage);
            processedImage = AddGrid(processedImage);
            processedImage = AddBorderIncresingSizeOfOriginalImage(processedImage);
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
            processedImage = AddGrid(processedImage);
            processedImage = AddBorderIncresingSizeOfOriginalImage(processedImage);
            ResultingImage = processedImage;
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
            positionsOfEachColor.Add(colorMeans.Count, new List<Tuple<int, int>>());
            colorMeans.Add(newColor);
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
                    float distanceToLine = ImageTransformations.CalculateDistanceOfPointToLine(imagePosition, currentBackstitchLineToVerifyDistance.startingPosition, currentBackstitchLineToVerifyDistance.endingPosition);

                    if(distanceToLine <= distanceToConsiderCloseEnough)
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
                //TODO: Repaint image for the backstitch lines associated with this color/index
            }
        }

        public void CreateMachinePath()
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

            machineEmbroidery.CreatePathAndDstFile(positionsOfEachColorToEmbroider, 30, matrixOfNewColors.GetLength(0), matrixOfNewColors.GetLength(1));
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

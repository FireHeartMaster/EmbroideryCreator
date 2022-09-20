using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class SelectionToolData
    {
        public Tuple<int, Color>[,] MatrixOfIndexesAndColors { get; private set; }

        public Tuple<int, int> topLeftPoint;
        public Tuple<int, int> bottomRightPoint;

        public Tuple<int, int> currentPositionPoint;

        //Backstitches relative to the top left point
        public Dictionary<int, Tuple<Color, HashSet<BackstitchLine>>> backstitches = new Dictionary<int, Tuple<Color, HashSet<BackstitchLine>>>();

        public Bitmap GenerateDashedRectangle(ImageAndOperationsData imageAndOperationsData, int width, int height, int positionX, int positionY)
        {
            float[] dashValues = { imageAndOperationsData.NewPixelSize * 0.3f, imageAndOperationsData.NewPixelSize * 0.15f };
            Pen dashedPen = new Pen(Color.Blue, imageAndOperationsData.GridThicknessInNumberOfPixels);
            dashedPen.DashPattern = dashValues;

            Bitmap dashedRectangleImage = new Bitmap(width * imageAndOperationsData.NewPixelSize, height * imageAndOperationsData.NewPixelSize);

            using (Graphics graphics = Graphics.FromImage(dashedRectangleImage))
            {
                graphics.DrawRectangle(dashedPen, 0, 0, width * imageAndOperationsData.NewPixelSize - dashedPen.Width, height * imageAndOperationsData.NewPixelSize - dashedPen.Width);
            }

            Bitmap dashedRectangleCompleteImage = new Bitmap(imageAndOperationsData.ResultingImage.Width, imageAndOperationsData.ResultingImage.Height);

            int gap = imageAndOperationsData.BorderThicknessInNumberOfPixels/* - imageAndOperationsData.GridThicknessInNumberOfPixels*/;
            using (Graphics graphics = Graphics.FromImage(dashedRectangleCompleteImage))
            {
                graphics.DrawImage(dashedRectangleImage, gap + positionX * imageAndOperationsData.NewPixelSize,
                                                         gap + positionY * imageAndOperationsData.NewPixelSize,
                                                         width * imageAndOperationsData.NewPixelSize - dashedPen.Width,
                                                         height * imageAndOperationsData.NewPixelSize - dashedPen.Width);
            }

            return dashedRectangleCompleteImage;
        }

        public Bitmap GenerateDashedRectangle(int width, int height, int positionX, int positionY,
                                                                int borderThicknessInNumberOfPixels,
                                                                int gridThicknessInNumberOfPixels,
                                                                int newPixelSize,
                                                                int resultingImageWidth,
                                                                int resultingImageHeight)
        {
            float[] dashValues = { newPixelSize * 0.3f, newPixelSize * 0.15f };
            Pen dashedPen = new Pen(Color.Blue, gridThicknessInNumberOfPixels);
            dashedPen.DashPattern = dashValues;

            Bitmap dashedRectangleImage = new Bitmap(width * newPixelSize, height * newPixelSize);

            using (Graphics graphics = Graphics.FromImage(dashedRectangleImage))
            {
                graphics.DrawRectangle(dashedPen, 0, 0, width * newPixelSize - dashedPen.Width, height * newPixelSize - dashedPen.Width);
            }

            Bitmap dashedRectangleCompleteImage = new Bitmap(resultingImageWidth, resultingImageHeight);

            int gap = borderThicknessInNumberOfPixels/* - imageAndOperationsData.GridThicknessInNumberOfPixels*/;
            using (Graphics graphics = Graphics.FromImage(dashedRectangleCompleteImage))
            {
                graphics.DrawImage(dashedRectangleImage, gap + positionX * newPixelSize,
                                                         gap + positionY * newPixelSize,
                                                         width * newPixelSize - dashedPen.Width,
                                                         height * newPixelSize - dashedPen.Width);
            }

            return dashedRectangleCompleteImage;
        }

        public void SetData(ImageAndOperationsData imageAndOperationsData, Tuple<int, int> firstPoint, Tuple<int, int> secondPoint)
        {
            //Here I assume that both first and second points are diagonally opposed

            ImageTransformations.GetTopLeftAndBottomRight(firstPoint, secondPoint, out Tuple<int, int> topLeftPointFound, out Tuple<int, int> bottomRightPointFound);
            topLeftPoint = topLeftPointFound;
            bottomRightPoint = bottomRightPointFound;

            topLeftPoint = new Tuple<int, int>(ImageTransformations.Clamp(topLeftPoint.Item1, 0, imageAndOperationsData.GetSizeInPixels().Item1), ImageTransformations.Clamp(topLeftPoint.Item2, 0, imageAndOperationsData.GetSizeInPixels().Item2));
            bottomRightPoint = new Tuple<int, int>(ImageTransformations.Clamp(bottomRightPoint.Item1, 0, imageAndOperationsData.GetSizeInPixels().Item1), ImageTransformations.Clamp(bottomRightPoint.Item2, 0, imageAndOperationsData.GetSizeInPixels().Item2));

            //Setting cross stitches
            var colors = imageAndOperationsData.GetCrossStitchColors();

            MatrixOfIndexesAndColors = new Tuple<int, Color>[bottomRightPoint.Item1 - topLeftPoint.Item1, bottomRightPoint.Item2 - topLeftPoint.Item2];

            int localI = 0;
            for (int i = topLeftPoint.Item1; i < bottomRightPoint.Item1; i++, localI++)
            {
                int localJ = 0;
                for (int j = topLeftPoint.Item2; j < bottomRightPoint.Item2; j++, localJ++)
                {
                    int currentIndex = imageAndOperationsData.GetIndexFromPosition(i, j);
                    MatrixOfIndexesAndColors[localI, localJ] = new Tuple<int, Color>(currentIndex, colors[currentIndex]);
                }
            }

            //Setting backstitches
            Dictionary<int, HashSet<BackstitchLine>> backstitchLines = imageAndOperationsData.GetBackstitchLines();
            List<Color> backstitchColors = imageAndOperationsData.GetBackstitchColors();

            backstitches = new Dictionary<int, Tuple<Color, HashSet<BackstitchLine>>>();

            for (int index = 0; index < backstitchLines.Keys.Count; index++)
            {
                Color currentBackstitchColor = backstitchColors[index];

                List<BackstitchLine> backstitchLinesToRemove = new List<BackstitchLine>();
                List<Tuple<int, Tuple<float, float>, Tuple<float, float>>> backstitchLinesToAdd = new List<Tuple<int, Tuple<float, float>, Tuple<float, float>>>();

                foreach (BackstitchLine currentBackstitchLine in backstitchLines[index])
                {
                    bool isStartingPointInside = ImageTransformations.IsPointInsideRectangle(
                        ImageTransformations.ConvertPairType(currentBackstitchLine.startingPosition),
                        ImageTransformations.ConvertPairType(topLeftPoint),
                        bottomRightPoint.Item1 - topLeftPoint.Item1,
                        bottomRightPoint.Item2 - topLeftPoint.Item2);

                    bool isEndingPointInside = ImageTransformations.IsPointInsideRectangle(
                        ImageTransformations.ConvertPairType(currentBackstitchLine.endingPosition),
                        ImageTransformations.ConvertPairType(topLeftPoint),
                        bottomRightPoint.Item1 - topLeftPoint.Item1,
                        bottomRightPoint.Item2 - topLeftPoint.Item2);

                    if(isStartingPointInside && isEndingPointInside)
                    {
                        backstitchLinesToRemove.Add(currentBackstitchLine);

                        if (!backstitches.ContainsKey(index))
                        {
                            backstitches.Add(index, new Tuple<Color, HashSet<BackstitchLine>>(currentBackstitchColor, new HashSet<BackstitchLine>()));
                        }
                        backstitches[index].Item2.Add(new BackstitchLine(
                            new Tuple<float, float>(currentBackstitchLine.startingPosition.Item1 - topLeftPoint.Item1, currentBackstitchLine.startingPosition.Item2 - topLeftPoint.Item2),
                            new Tuple<float, float>(currentBackstitchLine.endingPosition.Item1 - topLeftPoint.Item1, currentBackstitchLine.endingPosition.Item2 - topLeftPoint.Item2)
                            ));
                    }
                    else if((isStartingPointInside && !isEndingPointInside) || (!isStartingPointInside && isEndingPointInside))
                    {
                        //only one point inside the rectangle
                        Tuple<float, float> pointInside;
                        Tuple<float, float> pointOutside;

                        if (isStartingPointInside && !isEndingPointInside)
                        {
                            pointInside = currentBackstitchLine.startingPosition;
                            pointOutside = currentBackstitchLine.endingPosition;
                        }
                        else
                        {
                            pointInside = currentBackstitchLine.endingPosition;
                            pointOutside = currentBackstitchLine.startingPosition;
                        }

                        var intersections = ImageTransformations.FindIntersectionsOfLineAndSquare(
                            currentBackstitchLine.startingPosition.Item1,
                            currentBackstitchLine.startingPosition.Item2,
                            currentBackstitchLine.endingPosition.Item1,
                            currentBackstitchLine.endingPosition.Item2,
                            topLeftPoint.Item1,
                            topLeftPoint.Item2,
                            bottomRightPoint.Item1 - topLeftPoint.Item1,
                            bottomRightPoint.Item2 - topLeftPoint.Item2);

                        //removing from imageAndOperationsData
                        backstitchLinesToRemove.Add(currentBackstitchLine);

                        var intersectionThatIsNotThePointInside = ((intersections[0].Item1 == pointInside.Item1) && (intersections[0].Item2 == pointInside.Item2)) ? intersections[1] : intersections[0];

                        if (!backstitches.ContainsKey(index))
                        {
                            backstitches.Add(index, new Tuple<Color, HashSet<BackstitchLine>>(currentBackstitchColor, new HashSet<BackstitchLine>()));
                        }

                        //Adding to the data being set the part inside the selection rectangle
                        //The backstitch here is relative to the top left point
                        backstitches[index].Item2.Add(new BackstitchLine(
                            new Tuple<float, float>((float)intersections[0].Item1 - topLeftPoint.Item1, (float)intersections[0].Item2 - topLeftPoint.Item2),
                            new Tuple<float, float>((float)intersections[1].Item1 - topLeftPoint.Item1, (float)intersections[1].Item2 - topLeftPoint.Item2)
                            ));

                        backstitchLinesToAdd.Add(new Tuple<int, Tuple<float, float>, Tuple<float, float>>(
                            index,
                            ImageTransformations.ConvertPairType(intersectionThatIsNotThePointInside),
                            pointOutside));
                    }
                    else
                    {
                        var intersections = ImageTransformations.FindIntersectionsOfLineAndSquare(
                            currentBackstitchLine.startingPosition.Item1,
                            currentBackstitchLine.startingPosition.Item2,
                            currentBackstitchLine.endingPosition.Item1,
                            currentBackstitchLine.endingPosition.Item2,
                            topLeftPoint.Item1,
                            topLeftPoint.Item2,
                            bottomRightPoint.Item1 - topLeftPoint.Item1,
                            bottomRightPoint.Item2 - topLeftPoint.Item2);
                        
                        if(intersections.Count == 2)
                        {

                            //removing from imageAndOperationsData
                            backstitchLinesToRemove.Add(currentBackstitchLine);

                            //Adding to the data being set the part inside the selection rectangle
                            //The backstitch here is relative to the top left point
                            if (!backstitches.ContainsKey(index))
                            {
                                backstitches.Add(index, new Tuple<Color, HashSet<BackstitchLine>>(currentBackstitchColor, new HashSet<BackstitchLine>()));
                            }

                            backstitches[index].Item2.Add(new BackstitchLine(
                                new Tuple<float, float>((float)intersections[0].Item1 - topLeftPoint.Item1, (float)intersections[0].Item2 - topLeftPoint.Item2),
                                new Tuple<float, float>((float)intersections[1].Item1 - topLeftPoint.Item1, (float)intersections[1].Item2 - topLeftPoint.Item2)
                                ));

                            int indexOfClosestIntersectionToStartingPosition = 
                                ImageTransformations.CalculateDistanceBetweenPoints(ImageTransformations.ConvertPairType(intersections[0]), currentBackstitchLine.startingPosition) 
                                < 
                                ImageTransformations.CalculateDistanceBetweenPoints(ImageTransformations.ConvertPairType(intersections[1]), currentBackstitchLine.startingPosition) 
                                ? 0 : 1;

                            int indexOfClosestIntersectionToEndingPosition = (indexOfClosestIntersectionToStartingPosition - 1) * (-1); //Opposite of the other index

                            backstitchLinesToAdd.Add(new Tuple<int, Tuple<float, float>, Tuple<float, float>>(
                                index,
                                currentBackstitchLine.startingPosition,
                                ImageTransformations.ConvertPairType(intersections[indexOfClosestIntersectionToStartingPosition])));

                            backstitchLinesToAdd.Add(new Tuple<int, Tuple<float, float>, Tuple<float, float>>(
                                index,
                                ImageTransformations.ConvertPairType(intersections[indexOfClosestIntersectionToEndingPosition]),
                                currentBackstitchLine.endingPosition));
                        }
                    }
                }

                foreach (BackstitchLine backstitchLineToRemove in backstitchLinesToRemove)
                {
                    imageAndOperationsData.RemoveBackstitchLine(index, backstitchLineToRemove);
                }

                foreach (var indexAndStartingAndEndingPoint in backstitchLinesToAdd)
                {
                    //Adding to imageAndOperationsData the part outside the selection rectangle
                    imageAndOperationsData.AddNewBackstitchLine(indexAndStartingAndEndingPoint.Item1,
                        indexAndStartingAndEndingPoint.Item2,
                        indexAndStartingAndEndingPoint.Item3, 
                        false);
                }
            }

            //Repaint backstitch image
            imageAndOperationsData.PaintAllBackstitchLines();
        }



        public void PaintSelectedRegionWithEmptyColor(ImageAndOperationsData imageAndOperationsData, Tuple<int, int> topLeftPointOfTheSelection)
        {
            int localI = topLeftPointOfTheSelection.Item1;
            for (int i = 0; i < MatrixOfIndexesAndColors.GetLength(0); i++, localI++)
            {
                int localJ = topLeftPointOfTheSelection.Item2;
                for (int j = 0; j < MatrixOfIndexesAndColors.GetLength(1); j++, localJ++)
                {
                    Tuple<int, int> size = imageAndOperationsData.GetSizeInPixels();
                    if (localI < 0 || localI >= size.Item1 || localJ < 0 || localJ >= size.Item2) continue;

                    imageAndOperationsData.PaintPixelInPositionWithColorOfIndex(new Tuple<int, int>(localI, localJ), 0);
                }
            }
        }

        public Bitmap GenerateSelectionImage(ImageAndOperationsData imageAndOperationsData, List<Bitmap> images)
        {
            GC.Collect();

            Bitmap combinedImage = ImageTransformations.CombineImagesFromList(images);

            //Bitmap selectionImage = new Bitmap((bottomRightPoint.Item1 - topLeftPoint.Item1) * imageAndOperationsData.NewPixelSize, (bottomRightPoint.Item2 - topLeftPoint.Item2) * imageAndOperationsData.NewPixelSize);

            //Bitmap combinedImageNew = new Bitmap(combinedImage);

            int displacement = imageAndOperationsData.BorderThicknessInNumberOfPixels/* - imageAndOperationsData.GridThicknessInNumberOfPixels*/;
            Rectangle rectangleToCrop = new Rectangle(displacement + topLeftPoint.Item1 * imageAndOperationsData.NewPixelSize,
                                                        displacement + topLeftPoint.Item2 * imageAndOperationsData.NewPixelSize,
                                                        (bottomRightPoint.Item1 - topLeftPoint.Item1) * imageAndOperationsData.NewPixelSize,
                                                        (bottomRightPoint.Item2 - topLeftPoint.Item2) * imageAndOperationsData.NewPixelSize);

            Bitmap selectionImage = combinedImage.Clone(rectangleToCrop, combinedImage.PixelFormat);
            combinedImage.Dispose();

            Bitmap dashedRectangle = GenerateDashedRectangle(imageAndOperationsData, (bottomRightPoint.Item1 - topLeftPoint.Item1), (bottomRightPoint.Item2 - topLeftPoint.Item2), topLeftPoint.Item1, topLeftPoint.Item2);

            Bitmap selectionImageComplete = new Bitmap(imageAndOperationsData.ResultingImage.Width, imageAndOperationsData.ResultingImage.Height);

            int gap = imageAndOperationsData.BorderThicknessInNumberOfPixels/* - imageAndOperationsData.GridThicknessInNumberOfPixels*/;
            using (Graphics graphics = Graphics.FromImage(selectionImageComplete))
            {
                graphics.DrawImage(selectionImage, gap + topLeftPoint.Item1 * imageAndOperationsData.NewPixelSize,
                                                    gap + topLeftPoint.Item2 * imageAndOperationsData.NewPixelSize,
                                                    (bottomRightPoint.Item1 - topLeftPoint.Item1) * imageAndOperationsData.NewPixelSize,
                                                    (bottomRightPoint.Item2 - topLeftPoint.Item2) * imageAndOperationsData.NewPixelSize);
            }

            //combinedImageNew.Dispose();

            using (Graphics graphics = Graphics.FromImage(selectionImageComplete))
            {
                graphics.DrawImage(dashedRectangle, 0, 0, selectionImageComplete.Width, selectionImageComplete.Height);
            }

            dashedRectangle.Dispose();

            return selectionImageComplete;
        }

        public Bitmap GenerateSelectionImageFromRegisteredData(int positionX, int positionY,
                                                                int borderThicknessInNumberOfPixels,
                                                                int gridThicknessInNumberOfPixels,
                                                                int newPixelSize,
                                                                int resultingImageWidth,
                                                                int resultingImageHeight)
        {
            GC.Collect();

            if (MatrixOfIndexesAndColors == null) return null;

            //Bitmap combinedImage = ImageTransformations.CombineImagesFromList(images);

            ////Bitmap selectionImage = new Bitmap((bottomRightPoint.Item1 - topLeftPoint.Item1) * imageAndOperationsData.NewPixelSize, (bottomRightPoint.Item2 - topLeftPoint.Item2) * imageAndOperationsData.NewPixelSize);

            //Bitmap combinedImageNew = new Bitmap(combinedImage);

            //int displacement = borderThicknessInNumberOfPixels - gridThicknessInNumberOfPixels;
            //Rectangle rectangleToCrop = new Rectangle(displacement + topLeftPoint.Item1 * newPixelSize,
            //                                            displacement + topLeftPoint.Item2 * newPixelSize,
            //                                            (bottomRightPoint.Item1 - topLeftPoint.Item1) * newPixelSize,
            //                                            (bottomRightPoint.Item2 - topLeftPoint.Item2) * newPixelSize);

            Bitmap selectionImage = new Bitmap(MatrixOfIndexesAndColors.GetLength(0) * newPixelSize,
                                                MatrixOfIndexesAndColors.GetLength(1) * newPixelSize);

            topLeftPoint = new Tuple<int, int>(0, 0);
            bottomRightPoint = new Tuple<int, int>(MatrixOfIndexesAndColors.GetLength(0), MatrixOfIndexesAndColors.GetLength(1));

            currentPositionPoint = new Tuple<int, int>(0, 0);

            using (Graphics graphics = Graphics.FromImage(selectionImage))
            {
                for (int i = 0; i < MatrixOfIndexesAndColors.GetLength(0); i++)
                {
                    for (int j = 0; j < MatrixOfIndexesAndColors.GetLength(1); j++)
                    {
                        Color currentColor = MatrixOfIndexesAndColors[i, j].Item2;
                        Brush brush = new SolidBrush(currentColor);
                        graphics.FillRectangle(brush, i * newPixelSize, j * newPixelSize, newPixelSize, newPixelSize);
                    }
                }
            }

            //combinedImageNew.Dispose();

            Bitmap dashedRectangle = GenerateDashedRectangle((bottomRightPoint.Item1 - topLeftPoint.Item1), (bottomRightPoint.Item2 - topLeftPoint.Item2), positionX, positionY,
                                                                borderThicknessInNumberOfPixels,
                                                                gridThicknessInNumberOfPixels,
                                                                newPixelSize,
                                                                resultingImageWidth,
                                                                resultingImageHeight);

            Bitmap selectionImageComplete = new Bitmap(resultingImageWidth, resultingImageHeight);

            int gap = borderThicknessInNumberOfPixels/* - imageAndOperationsData.GridThicknessInNumberOfPixels*/;
            using (Graphics graphics = Graphics.FromImage(selectionImageComplete))
            {
                graphics.DrawImage(selectionImage, gap + positionX * newPixelSize,
                                                    gap + positionY * newPixelSize,
                                                    (bottomRightPoint.Item1 - topLeftPoint.Item1) * newPixelSize,
                                                    (bottomRightPoint.Item2 - topLeftPoint.Item2) * newPixelSize);
            }

            //combinedImageNew.Dispose();

            using (Graphics graphics = Graphics.FromImage(selectionImageComplete))
            {
                graphics.DrawImage(dashedRectangle, 0, 0, selectionImageComplete.Width, selectionImageComplete.Height);
            }

            dashedRectangle.Dispose();

            return selectionImageComplete;
        }

        public void PaintSelectionRectangle(MainForm mainForm, ImageAndOperationsData imageAndOperationsData, Tuple<int, int> currentTopLeftPositionOfTheSelection)
        {
            //Paint cross stitches
            int localI = currentTopLeftPositionOfTheSelection.Item1;
            for (int i = 0; i < MatrixOfIndexesAndColors.GetLength(0); i++, localI++)
            {
                int localJ = currentTopLeftPositionOfTheSelection.Item2;
                for (int j = 0; j < MatrixOfIndexesAndColors.GetLength(1); j++, localJ++)
                {
                    Tuple<int, int> size = imageAndOperationsData.GetSizeInPixels();
                    if (localI < 0 || localI >= size.Item1 || localJ < 0 || localJ >= size.Item2) continue;

                    int indexToPaint = MatrixOfIndexesAndColors[i, j].Item1;
                    Color registeredColor = MatrixOfIndexesAndColors[i, j].Item2;
                    //Color colorFromTheList = imageAndOperationsData.GetCrossStitchColors()[indexToPaint];
                    //Color colorFromTheList = Color.FromArgb(0, 0, 0, 0);

                    if (imageAndOperationsData.GetCrossStitchColors().Count <= indexToPaint)
                    {
                        indexToPaint = imageAndOperationsData.GetIndexFromCrossStitchColor(registeredColor);
                    }
                    else
                    {
                        Color colorFromTheList = imageAndOperationsData.GetCrossStitchColors()[indexToPaint];
                        if (registeredColor.A != colorFromTheList.A || registeredColor.R != colorFromTheList.R || registeredColor.G != colorFromTheList.G || registeredColor.B != colorFromTheList.B)
                        {
                            indexToPaint = imageAndOperationsData.GetIndexFromCrossStitchColor(registeredColor);
                        }
                    }

                    if (indexToPaint == 0 && registeredColor.A != 0)
                    {
                        //Add color that currently isn't in the list of colors, after that retrieve the index of the newly added color
                        mainForm.AddNewColor(registeredColor);
                        indexToPaint = imageAndOperationsData.GetIndexFromCrossStitchColor(registeredColor);
                    }

                    if (indexToPaint != -1 /*list of colors would be empty*/ && indexToPaint != 0 /*do not paint empty pixels from the selected rectangle onto other pixels*/)
                    {
                        imageAndOperationsData.PaintPixelInPositionWithColorOfIndex(new Tuple<int, int>(localI, localJ), indexToPaint);
                    }
                }
            }

            //Paint backstitches
            foreach (KeyValuePair<int, Tuple<Color, HashSet<BackstitchLine>>> backstitchIndexColorAndStitch in backstitches)
            {
                int indexToPaint = backstitchIndexColorAndStitch.Key;
                Color registeredColor = backstitchIndexColorAndStitch.Value.Item1;

                if (imageAndOperationsData.GetBackstitchColors().Count <= indexToPaint)
                {
                    indexToPaint = imageAndOperationsData.GetIndexFromBackstitchColor(registeredColor);
                }
                else
                {
                    Color colorFromTheList = imageAndOperationsData.GetBackstitchColors()[indexToPaint];
                    if (registeredColor.A != colorFromTheList.A || registeredColor.R != colorFromTheList.R || registeredColor.G != colorFromTheList.G || registeredColor.B != colorFromTheList.B)
                    {
                        indexToPaint = imageAndOperationsData.GetIndexFromBackstitchColor(registeredColor);
                    }
                }

                if (indexToPaint == -1)
                {
                    //Add color that currently isn't in the list of colors, after that retrieve the index of the newly added color
                    mainForm.AddNewBackstitchColor(registeredColor);
                    indexToPaint = imageAndOperationsData.GetIndexFromBackstitchColor(registeredColor);
                }

                if (indexToPaint != -1)
                {
                    //Adding new backstitches based on the current position of the selection rectangle,
                    //knowing the positions registered here are relative to the top left point

                    //Also need to get the intersections of the registered backstitch and the current total image borders
                    //because the image could have some parts outside the screen

                    foreach (BackstitchLine backstitchLineToAdd in backstitchIndexColorAndStitch.Value.Item2)
                    {
                        Tuple<float, float> absolutePositionStartingPoint = new Tuple<float, float>(currentTopLeftPositionOfTheSelection.Item1 + backstitchLineToAdd.startingPosition.Item1,
                                                                                                    currentTopLeftPositionOfTheSelection.Item2 + backstitchLineToAdd.startingPosition.Item2);
                        Tuple<float, float> absolutePositionEndingPoint = new Tuple<float, float>(currentTopLeftPositionOfTheSelection.Item1 + backstitchLineToAdd.endingPosition.Item1,
                                                                                                    currentTopLeftPositionOfTheSelection.Item2 + backstitchLineToAdd.endingPosition.Item2);
                        Tuple<int, int> size = imageAndOperationsData.GetSizeInPixels();
                        var intersections = ImageTransformations.FindIntersectionsOfLineAndSquare(
                            absolutePositionStartingPoint.Item1,
                            absolutePositionStartingPoint.Item2,
                            absolutePositionEndingPoint.Item1,
                            absolutePositionEndingPoint.Item2,
                            0,
                            0,
                            size.Item1,
                            size.Item2);

                        if(intersections.Count == 2)
                        {
                            imageAndOperationsData.AddNewBackstitchLine(indexToPaint,
                                                                        ImageTransformations.ConvertPairType(intersections[0]),
                                                                        ImageTransformations.ConvertPairType(intersections[1]),
                                                                        false);
                        }
                    }
                }
            }

            //Repaint backstitch image
            imageAndOperationsData.PaintAllBackstitchLines();
        }
    }
}

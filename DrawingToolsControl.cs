using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmbroideryCreator
{
    public partial class DrawingToolsControl : UserControl
    {
        public DrawingToolInUse currentDrawingTool { get; private set; } = DrawingToolInUse.Pencil;
        public SelectionToolData selectionToolData = new SelectionToolData();
        public SelectionToolState currentSelectionToolState = SelectionToolState.NothingSelected;

        PictureBox currentActiveToolPictureBox;

        public DrawingToolsControl()
        {
            InitializeComponent();
            //currentActiveToolPictureBox = pencilPictureBox;
            EnableNewTool(DrawingToolInUse.Pencil);
        }

        private void pencilPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Pencil);
        }

        private void bucketPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Bucket);
        }

        private void eraserPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Eraser);
        }

        private void movePictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Move);
        }

        private void colorPickerPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.ColorPicker);
        }

        private void selectionToolPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.SelectionTool);
        }

        public void EnableNewTool(DrawingToolInUse newSelectedDrawingTool)
        {
            DisableOtherTools(newSelectedDrawingTool);

            switch (newSelectedDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    currentDrawingTool = DrawingToolInUse.Pencil;
                    currentActiveToolPictureBox = pencilPictureBox;
                    break;

                case DrawingToolInUse.Bucket:
                    currentDrawingTool = DrawingToolInUse.Bucket;
                    currentActiveToolPictureBox = bucketPictureBox;
                    break;

                case DrawingToolInUse.Eraser:
                    currentDrawingTool = DrawingToolInUse.Eraser;
                    currentActiveToolPictureBox = eraserPictureBox;
                    break;
                case DrawingToolInUse.Move:
                    currentDrawingTool = DrawingToolInUse.Move;
                    currentActiveToolPictureBox = movePictureBox;
                    break;
                case DrawingToolInUse.ColorPicker:
                    currentDrawingTool = DrawingToolInUse.ColorPicker;
                    currentActiveToolPictureBox = colorPickerPictureBox;
                    break;
                case DrawingToolInUse.SelectionTool:
                    currentDrawingTool = DrawingToolInUse.SelectionTool;
                    currentActiveToolPictureBox = selectionToolPictureBox;
                    break;

                default:
                    break;
            }
            SetActiveCurrentToolImage();
        }

        private void DisableOtherTools(DrawingToolInUse drawingToolBeingActivated)
        {

        }

        //private void ResetCurrentToolsImage()
        //{
        //    currentActiveToolPictureBox.BackColor = unSelectedToolBorderColor;
        //    currentActiveToolPictureBox.Padding = new Padding(unSelectedToolBorderSize);
        //    currentActiveToolPictureBox.Invalidate();
        //}

        private void SetActiveCurrentToolImage()
        {
            //currentActiveToolPictureBox.BackColor = selectedToolBorderColor;
            //currentActiveToolPictureBox.Padding = new Padding(selectedToolBorderSize);
            //currentActiveToolPictureBox.Invalidate();
            selectedToolpictureBox.Location = new Point((int)(currentActiveToolPictureBox.Location.X - (selectedToolpictureBox.Size.Width - currentActiveToolPictureBox.Size.Width) * 0.5),
                                                        (int)(currentActiveToolPictureBox.Location.Y - (selectedToolpictureBox.Size.Height - currentActiveToolPictureBox.Size.Height) * 0.5));
        }
    }

    public enum DrawingToolInUse
    {
        Pencil,
        Bucket,
        Eraser,
        Move, 
        ColorPicker, 
        SelectionTool
    }

    public enum SelectionToolState
    {
        NothingSelected,
        Selecting,
        Moving,
        Selected
    }

    public class SelectionToolData
    {
        public Tuple<int, Color>[,] MatrixOfIndexesAndColors { get; private set; }

        public Tuple<int, int> topLeftPoint;
        public Tuple<int, int> bottomRightPoint;

        public Tuple<int, int> currentPositionPoint;

        public Bitmap GenerateDashedRectangle(ImageAndOperationsData imageAndOperationsData, int width, int height, int positionX, int positionY)
        {
            float[] dashValues = { imageAndOperationsData.NewPixelSize * 0.3f, imageAndOperationsData.NewPixelSize * 0.15f };
            Pen dashedPen = new Pen(Color.Blue, imageAndOperationsData.GridThicknessInNumberOfPixels);
            dashedPen.DashPattern = dashValues;

            Bitmap dashedRectangleImage = new Bitmap(width * imageAndOperationsData.NewPixelSize, height * imageAndOperationsData.NewPixelSize);

            using(Graphics graphics = Graphics.FromImage(dashedRectangleImage))
            {
                graphics.DrawRectangle(dashedPen, 0, 0, width * imageAndOperationsData.NewPixelSize - dashedPen.Width, height * imageAndOperationsData.NewPixelSize - dashedPen.Width);
            }

            Bitmap dashedRectangleCompleteImage = new Bitmap(imageAndOperationsData.ResultingImage.Width, imageAndOperationsData.ResultingImage.Height);

            int gap = imageAndOperationsData.BorderThicknessInNumberOfPixels/* - imageAndOperationsData.GridThicknessInNumberOfPixels*/;
            using(Graphics graphics = Graphics.FromImage(dashedRectangleCompleteImage))
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
            Rectangle rectangleToCrop = new Rectangle(  displacement + topLeftPoint.Item1 * imageAndOperationsData.NewPixelSize,
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
                graphics.DrawImage(selectionImage,  gap + topLeftPoint.Item1 * imageAndOperationsData.NewPixelSize,
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

            Bitmap selectionImage = new Bitmap( (bottomRightPoint.Item1 - topLeftPoint.Item1) * newPixelSize,
                                                (bottomRightPoint.Item2 - topLeftPoint.Item2) * newPixelSize);

            using(Graphics graphics = Graphics.FromImage(selectionImage))
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

        public void PaintMatrix(MainForm mainForm, ImageAndOperationsData imageAndOperationsData, Tuple<int, int> currentTopLeftPositionOfTheSelection)
        {
            int localI = currentTopLeftPositionOfTheSelection.Item1;
            for (int i = 0; i < MatrixOfIndexesAndColors.GetLength(0); i++, localI++)
            {
                int localJ = currentTopLeftPositionOfTheSelection.Item2;
                for (int j = 0; j < MatrixOfIndexesAndColors.GetLength(1); j++, localJ++)
                {
                    Tuple<int, int> size = imageAndOperationsData.GetSizeInPixels();
                    if (localI < 0 || localI >= size.Item1 || localJ < 0 || localJ >= size.Item2) continue;

                    int indexToPaint = MatrixOfIndexesAndColors[i,j].Item1;
                    Color registeredColor = MatrixOfIndexesAndColors[i, j].Item2;
                    //Color colorFromTheList = imageAndOperationsData.GetCrossStitchColors()[indexToPaint];
                    //Color colorFromTheList = Color.FromArgb(0, 0, 0, 0);

                    if (imageAndOperationsData.GetCrossStitchColors().Count <= indexToPaint)
                    {
                        indexToPaint = imageAndOperationsData.GetIndexFromColor(registeredColor);
                    }
                    else
                    {
                        Color colorFromTheList = imageAndOperationsData.GetCrossStitchColors()[indexToPaint];
                        if (registeredColor.A != colorFromTheList.A || registeredColor.R != colorFromTheList.R || registeredColor.G != colorFromTheList.G || registeredColor.B != colorFromTheList.B)
                        {
                            indexToPaint = imageAndOperationsData.GetIndexFromColor(registeredColor);
                        }
                    }

                    if (indexToPaint == 0 && registeredColor.A != 0)
                    {
                        //Add color that currently isn't in the list of colors, after that retrieve the index of the newly added color
                        mainForm.AddNewColor(registeredColor);
                        indexToPaint = imageAndOperationsData.GetIndexFromColor(registeredColor);
                    }

                    if(indexToPaint != -1 /*list of colors would be empty*/ && indexToPaint != 0 /*do not paint empty pixels from the selected rectangle onto other pixels*/)
                    {
                        imageAndOperationsData.PaintPixelInPositionWithColorOfIndex(new Tuple<int, int>(localI, localJ), indexToPaint);
                    }
                }
            }
        }
    }
}

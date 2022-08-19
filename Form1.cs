using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmbroideryCreator
{
    public partial class MainForm : Form
    {
        private ImageAndOperationsData imageAndOperationsData;
        private int defaultWidth = 100;
        private int defaultNumberOfColors = 10;
        private int defaultNumberOfIterations = 10;

        private bool isDrawing = false;

        private DrawingMode currentDrawingMode = DrawingMode.CrossStitch;

        public List<ReducedColorControl> selectedColorsControlsList = new List<ReducedColorControl>();

        public List<BackstitchColorControl> selectedBackstitchColorsControlsList = new List<BackstitchColorControl>();

        private Point pointMouseDown = new Point(0, 0);
        private Point pointMouseUp = new Point(0, 0);
        private Image imageBeforeDrawing = null;
        
        private Point lastBackstitchPointMouseUp = new Point(0, 0);
        private Point firstMoveToolPoint = new Point(0, 0);
        private Point originalPositionMoveTool = new Point(0, 0);   

        private Tuple<int, int> realImagePositionMouseDown = new Tuple<int, int>(0, 0);
        private Tuple<int, int> realImagePositionMouseUp = new Tuple<int, int>(0, 0);
        private Tuple<int, int> lastBackstitchRealImagePositionMouseUp = new Tuple<int, int>(0, 0);

        private Tuple<int, int> roundedRealImagePositionMouseDown = new Tuple<int, int>(0, 0);
        private Tuple<int, int> roundedRealImagePositionMouseUp = new Tuple<int, int>(0, 0);
        private Tuple<int, int> lastBackstitchRoundedRealImagePositionMouseUp = new Tuple<int, int>(0, 0);

        private Keys multipleSelectionKeyboardKey = Keys.Control;

        private List<PictureBox> pictureBoxesByVisibilityOrder = new List<PictureBox>();

        private Size oldFormSize;
        private Size pictureBoxUnscaledSize;
        private int currentScrollAmount = 0;

        public MainForm()
        {
            InitializeComponent();

            widthSizeTrackBar.Value = defaultWidth;
            numberOfColorsTrackBar.Value = defaultNumberOfColors;
            numberOfIterationsTrackBar.Value = defaultNumberOfIterations;

            crossStitchColorsRadioButton.Checked = true;
            backStitchColorsRadioButton.Checked = false;

            pictureBoxesByVisibilityOrder.Add(baseLayerPictureBox);
            pictureBoxesByVisibilityOrder.Add(mainPictureBox);
            pictureBoxesByVisibilityOrder.Add(threadPictureBox);
            pictureBoxesByVisibilityOrder.Add(symbolsPictureBox);
            pictureBoxesByVisibilityOrder.Add(gridPictureBox);
            pictureBoxesByVisibilityOrder.Add(borderPictureBox);
            pictureBoxesByVisibilityOrder.Add(backstitchPictureBox);

            if(pictureBoxesByVisibilityOrder.Count != 0)
            {
                pictureBoxUnscaledSize = pictureBoxesByVisibilityOrder[0].Size;
            }

            ResetOrderOfVisibilityOfPictureBoxes();

            imagesContainerPanel.MouseWheel += new MouseEventHandler(imagesContainerPanel_MouseWheel);
        }

        private void SetTransparentPictureBox(PictureBox transparentPictureBox, PictureBox solidPictureBox)
        {
            solidPictureBox.Controls.Add(transparentPictureBox);
            transparentPictureBox.Location = new Point(0, 0);
            transparentPictureBox.BackColor = Color.Transparent;
        }

        int GetNextVisiblePictureBox(int currentIndex, List<PictureBox> listOfPictureBoxes)
        {
            if (currentIndex <= 0) return 0;

            if(listOfPictureBoxes[currentIndex - 1].Visible)
            {
                return currentIndex - 1;
            }
            else
            {
                return GetNextVisiblePictureBox(currentIndex - 1, listOfPictureBoxes);
            }
        }
        private void ResetOrderOfVisibilityOfPictureBoxes()
        {
            for (int i = 1; i < pictureBoxesByVisibilityOrder.Count; i++)
            {
                int parentIndex = GetNextVisiblePictureBox(i, pictureBoxesByVisibilityOrder);
                SetTransparentPictureBox(pictureBoxesByVisibilityOrder[i], pictureBoxesByVisibilityOrder[parentIndex]);
            }
        }

        private Bitmap CombineImagesWithVisibilityOfPictureBoxes(List<PictureBox> pictureBoxesToCombine)
        {
            Bitmap imagesCombined = new Bitmap(pictureBoxesToCombine[0].Image);
            for (int i = 1; i < pictureBoxesToCombine.Count; i++)
            {
                if (pictureBoxesToCombine[i].Visible)
                {
                    imagesCombined = ImageTransformations.CombineImages(imagesCombined, new Bitmap(pictureBoxesToCombine[i].Image));
                }
            }

            return imagesCombined;
        }

        private void chooseNewImageButton_Click(object sender, EventArgs args)
        {
            SelectNewImageFile();
        }

        private void mainPictureBox_Click(object sender, EventArgs e)
        {
            //SelectNewImageFile();
        }

        private void mainPictureBox_DoubleClick(object sender, EventArgs e)
        {
            
        }

        internal void UpdateIfShouldPaintReducedColorBackground(int reducedColorIndex, bool isBackground)
        {
            if(reducedColorIndex >= 0 && reducedColorIndex < imageAndOperationsData.colorIsBackgroundList.Count)
            {
                imageAndOperationsData.colorIsBackgroundList[reducedColorIndex] = isBackground;
            }                        
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    drawingToolsControl.EnableNewTool((DrawingToolInUse)(1 - 1));
                    break;
                case Keys.D2:
                    drawingToolsControl.EnableNewTool((DrawingToolInUse)(2 - 1));
                    break;
                case Keys.B:
                    if (!ModifierKeys.HasFlag(Keys.Shift))
                    {
                        drawingToolsControl.EnableNewTool((DrawingToolInUse)(1 - 1));
                    }
                    else
                    {
                        drawingToolsControl.EnableNewTool((DrawingToolInUse)(2 - 1));
                    }
                    break;
                case Keys.D3:
                case Keys.E:
                    drawingToolsControl.EnableNewTool((DrawingToolInUse)(3 - 1));
                    break;
                case Keys.D4:
                case Keys.H:
                    drawingToolsControl.EnableNewTool((DrawingToolInUse)(4 - 1));
                    break;
                case Keys.D5:
                case Keys.I:
                    drawingToolsControl.EnableNewTool((DrawingToolInUse)(5 - 1));
                    break;

                default:
                    break;
            }
        }

        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;

            Point pointOnImage = e.Location;

            DrawOnPictureBox(pointOnImage);
        }

        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            SetBackstitchDrawMouseUp();
        }
        
        private void mainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                switch (currentDrawingMode)
                {
                    case DrawingMode.CrossStitch:
                        switch (drawingToolsControl.currentDrawingTool)
                        {
                            case DrawingToolInUse.Pencil:
                                DrawOnPictureBox(e.Location);
                                break;
                            case DrawingToolInUse.Move:
                                MoveTool(e.Location);
                                break;
                            case DrawingToolInUse.Eraser:
                                DrawOnPictureBox(e.Location);
                                break;

                            default:
                                break;
                        }
                        break;

                    case DrawingMode.Backstitch:
                        pointMouseUp = e.Location;
                        switch (drawingToolsControl.currentDrawingTool)
                        {
                            case DrawingToolInUse.Pencil:
                                PaintBackstitch();
                                break;

                            case DrawingToolInUse.Eraser:
                                TryToEraseBackstitchLineAtGeneralPosition(pointMouseUp);
                                break;
                            case DrawingToolInUse.Move:
                                MoveTool(e.Location);
                                break;

                            default:
                                break;
                        }
                        break;
                }
            }
        }

        private void MoveTool(Point secondMoveToolPoint)
        {
            if (pictureBoxesByVisibilityOrder.Count == 0 || currentScrollAmount == 0) return;

            PictureBox parentPictureBox = pictureBoxesByVisibilityOrder[0];

            int xMoveAmount = secondMoveToolPoint.X - firstMoveToolPoint.X;
            int yMoveAmount = secondMoveToolPoint.Y - firstMoveToolPoint.Y;            

            int xNewPosition = originalPositionMoveTool.X + xMoveAmount;
            int yNewPosition = originalPositionMoveTool.Y + yMoveAmount;

            CheckPictureBoxBordersAppearing(parentPictureBox, ref xNewPosition, ref yNewPosition);

            int xCurrentMove = xNewPosition - parentPictureBox.Left;
            int yCurrentMove = yNewPosition - parentPictureBox.Top;

            if (parentPictureBox.Left == xNewPosition && parentPictureBox.Top == yNewPosition) return;

            parentPictureBox.Left = xNewPosition;
            parentPictureBox.Top = yNewPosition;
            
            firstMoveToolPoint.X -= xCurrentMove;
            firstMoveToolPoint.Y -= yCurrentMove;
        }

        private void CheckPictureBoxBordersAppearing(PictureBox parentPictureBox, ref int xNewPosition, ref int yNewPosition)
        {
            if (xNewPosition > 0) xNewPosition = 0;
            if (xNewPosition < imagesContainerPanel.Width - parentPictureBox.Width) xNewPosition = imagesContainerPanel.Width - parentPictureBox.Width;

            if (yNewPosition > 0) yNewPosition = 0;
            if (yNewPosition < imagesContainerPanel.Height - parentPictureBox.Height) yNewPosition = imagesContainerPanel.Height - parentPictureBox.Height;
        }

        private void imagesContainerPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;
            
            ResizeImagesOnMouseWheel(e);
        }

        private void ResizeImagesOnMouseWheel(MouseEventArgs e)
        {
            if (pictureBoxesByVisibilityOrder.Count == 0) return;

            int rescaledDelta = e.Delta * SystemInformation.MouseWheelScrollLines / SystemInformation.MouseWheelScrollDelta;

            int amountPerScroll = 5;

            int newWidth = (int)((pictureBoxUnscaledSize.Width * (100 + currentScrollAmount)) / 100);
            int newHeight = (int)((pictureBoxUnscaledSize.Height * (100 + currentScrollAmount)) / 100);

            PictureBox parentPictureBox = pictureBoxesByVisibilityOrder[0];

            int newHorizontalPosition = (int)((((double)newWidth) / parentPictureBox.Width) * (parentPictureBox.Left - e.Location.X) + e.Location.X);
            int newVerticalPosition = (int)((((double)newHeight) / parentPictureBox.Height) * (parentPictureBox.Top - e.Location.Y) + e.Location.Y);

            currentScrollAmount += amountPerScroll * rescaledDelta;
            bool resetPosition = false;
            if (currentScrollAmount < 0)
            {
                currentScrollAmount = 0;
                resetPosition = true;
            }

            foreach (PictureBox pictureBox in pictureBoxesByVisibilityOrder)
            {
                pictureBox.Width = newWidth;
                pictureBox.Height = newHeight;
            }

            if (resetPosition)
            {
                parentPictureBox.Location = new Point(0, 0);
            }
            else
            {
                CheckPictureBoxBordersAppearing(parentPictureBox, ref newHorizontalPosition, ref newVerticalPosition);
                parentPictureBox.Location = new Point(newHorizontalPosition, newVerticalPosition);
            }
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            isDrawing = false;
            SetBackstitchDrawMouseUp();
        }

        private void DrawOnPictureBox(Point pointOnImage)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            switch (currentDrawingMode)
            {
                case DrawingMode.CrossStitch:
                    CrossStitchDraw(pointOnImage);
                    break;
                case DrawingMode.Backstitch:
                    SetBackstitchDrawMouseDown(pointOnImage);
                    break;
            }
        }

        private void SetBackstitchDrawMouseDown(Point pointOnImage)
        {
            switch (drawingToolsControl.currentDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    imageBeforeDrawing = new Bitmap(backstitchPictureBox.Image);
                    pointMouseDown = new Point(pointOnImage.X, pointOnImage.Y);
                    realImagePositionMouseDown = ImageTransformations.ConvertFromPictureBoxToRealImage(backstitchPictureBox, new Tuple<int, int>(pointMouseDown.X, pointMouseDown.Y));
                    roundedRealImagePositionMouseDown = imageAndOperationsData.ConvertFromGeneralPositionOnImagesToRoundedGeneralPositionOnImageIncludingHalfValues(realImagePositionMouseDown);
                    break;

                case DrawingToolInUse.Eraser:
                    TryToEraseBackstitchLineAtGeneralPosition(pointOnImage);
                    break;
                case DrawingToolInUse.Move:
                    firstMoveToolPoint = pointOnImage;
                    if (pictureBoxesByVisibilityOrder.Count > 0)
                    {
                        originalPositionMoveTool = pictureBoxesByVisibilityOrder[0].Location;
                    }
                    break;

                default:
                    break;
            }
        }

        private void TryToEraseBackstitchLineAtGeneralPosition(Point pointOnImage)
        {
            if (lastBackstitchPointMouseUp.X == pointOnImage.X && lastBackstitchPointMouseUp.Y == pointOnImage.Y) return;

            Tuple<int, int> realImagePosition = ImageTransformations.ConvertFromPictureBoxToRealImage(backstitchPictureBox, new Tuple<int, int>(pointOnImage.X, pointOnImage.Y));
            imageAndOperationsData.RemoveBackstitchLineClicked(realImagePosition);
            backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;

            lastBackstitchPointMouseUp.X = pointOnImage.X;
            lastBackstitchPointMouseUp.Y = pointOnImage.Y;
        }

        private void SetBackstitchDrawMouseUp()
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            switch (drawingToolsControl.currentDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    if (selectedBackstitchColorsControlsList.Count > 0)
                    {
                        Tuple<float, float> startingPosition = imageAndOperationsData.ConvertFromGeneralPositionOnImageToCoordinatesIncludingHalfValues(realImagePositionMouseDown);
                        Tuple<float, float> endingPosition = imageAndOperationsData.ConvertFromGeneralPositionOnImageToCoordinatesIncludingHalfValues(realImagePositionMouseUp);
                        imageAndOperationsData.AddNewBackstitchLine(selectedBackstitchColorsControlsList[0].backstitchColorIndex, startingPosition, endingPosition);
                        backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;
                    }
                    break;

                case DrawingToolInUse.Eraser:
                    break;

                default:
                    break;
            }
        }

        private void PaintBackstitch()
        {
            if (selectedBackstitchColorsControlsList.Count == 0) return;
            if (lastBackstitchPointMouseUp.X == pointMouseUp.X && lastBackstitchPointMouseUp.Y == pointMouseUp.Y) return;
            realImagePositionMouseUp = ImageTransformations.ConvertFromPictureBoxToRealImage(backstitchPictureBox, new Tuple<int, int>(pointMouseUp.X, pointMouseUp.Y));
            if (realImagePositionMouseUp.Item1 == lastBackstitchRealImagePositionMouseUp.Item1 && realImagePositionMouseUp.Item2 == lastBackstitchRealImagePositionMouseUp.Item2) return;

            roundedRealImagePositionMouseUp = imageAndOperationsData.ConvertFromGeneralPositionOnImagesToRoundedGeneralPositionOnImageIncludingHalfValues(realImagePositionMouseUp);

            if (roundedRealImagePositionMouseUp.Item1 == lastBackstitchRoundedRealImagePositionMouseUp.Item1 && roundedRealImagePositionMouseUp.Item2 == lastBackstitchRoundedRealImagePositionMouseUp.Item2) return;

            backstitchPictureBox.Image = imageBeforeDrawing.Clone() as Image;

            Graphics graphics = Graphics.FromImage(backstitchPictureBox.Image);

            Color penColor = selectedBackstitchColorsControlsList[0].color;
            float backstitchLineThickness = imageAndOperationsData.GridThicknessInNumberOfPixels * 3;
            Pen pen = new Pen(penColor, backstitchLineThickness);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            graphics.DrawLine(pen,  roundedRealImagePositionMouseDown.Item1 - imageAndOperationsData.GridThicknessInNumberOfPixels * 0.5f, 
                                    roundedRealImagePositionMouseDown.Item2 - imageAndOperationsData.GridThicknessInNumberOfPixels * 0.5f, 
                                    roundedRealImagePositionMouseUp.Item1 - imageAndOperationsData.GridThicknessInNumberOfPixels * 0.5f, 
                                    roundedRealImagePositionMouseUp.Item2 - imageAndOperationsData.GridThicknessInNumberOfPixels * 0.5f);

            lastBackstitchPointMouseUp.X = pointMouseUp.X;
            lastBackstitchPointMouseUp.Y = pointMouseUp.Y;

            lastBackstitchRealImagePositionMouseUp = new Tuple<int, int>(realImagePositionMouseUp.Item1, realImagePositionMouseUp.Item2);

            lastBackstitchRoundedRealImagePositionMouseUp = new Tuple<int, int>(roundedRealImagePositionMouseUp.Item1, roundedRealImagePositionMouseUp.Item2);
        }

        private void CrossStitchDraw(Point positionOnImage)
        {
            Tuple<int, int> realImagePosition = ImageTransformations.ConvertFromPictureBoxToRealImage(mainPictureBox, new Tuple<int, int>(positionOnImage.X, positionOnImage.Y), false);

            int indexOfTheFirstColorOfList = -1;
            if (flowLayoutPanelListOfCrossStitchColors.Controls.Count > 0)
            {
                indexOfTheFirstColorOfList = ((ReducedColorControl)flowLayoutPanelListOfCrossStitchColors.Controls[0]).reducedColorIndex;
            }
            int colorIndexToPaint = selectedColorsControlsList.Count > 0 ? selectedColorsControlsList[0].reducedColorIndex : indexOfTheFirstColorOfList;

            switch (drawingToolsControl.currentDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    if (colorIndexToPaint == -1) return;
                    imageAndOperationsData.PaintNewColorOnGeneralPosition(realImagePosition, colorIndexToPaint, false);
                    break;
                case DrawingToolInUse.Bucket:
                    if (colorIndexToPaint == -1) return;
                    imageAndOperationsData.FillRegionWithColorByPosition(realImagePosition, colorIndexToPaint, false);
                    isDrawing = false;
                    //SetBackstitchDrawMouseUp();
                    break;
                case DrawingToolInUse.Move:
                    firstMoveToolPoint = positionOnImage;
                    if (pictureBoxesByVisibilityOrder.Count > 0)
                    {
                        originalPositionMoveTool = pictureBoxesByVisibilityOrder[0].Location;
                    }
                    break;
                case DrawingToolInUse.Eraser:
                    imageAndOperationsData.PaintNewColorOnGeneralPosition(realImagePosition, 0, false);
                    break;
                case DrawingToolInUse.ColorPicker:
                    PickColor(realImagePosition, false);
                    isDrawing = false;
                    break;
                default:
                    break;
            }

            //reload picture box
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;
            threadPictureBox.Image = imageAndOperationsData.ThreadImage;
            symbolsPictureBox.Image = imageAndOperationsData.SymbolsImage;
        }

        private void PickColor(Tuple<int, int> realImagePosition, bool roundToClosest = true)
        {
            int colorIndex = imageAndOperationsData.GetColorIndexFromPosition(realImagePosition, roundToClosest);
            if (colorIndex != -1)
            {                
                //select the picked color
                UncheckAllOtherCrossStitchColorControlsAndCheckTheChosenOne(colorIndex);
            }
        }

        private void RemoveAlonePixels()
        {
            imageAndOperationsData.RemoveAlonePixels(removeAlonePixelsTrackBar.Value);

            //reload picture box
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;
            threadPictureBox.Image = imageAndOperationsData.ThreadImage;
            symbolsPictureBox.Image = imageAndOperationsData.SymbolsImage;
        }

        private void removeAlonePixelsButton_Click(object sender, EventArgs e)
        {
            RemoveAlonePixels();
        }

        private void SelectNewImageFile()
        {
            using (openNewImageFileDialog = new OpenFileDialog())
            {
                //openNewImageFileDialog.InitialDirectory = "c:\\";
                openNewImageFileDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif|All files|*.*";
                //openNewImageFileDialog.FilterIndex = 2;
                //openNewImageFileDialog.RestoreDirectory = true;

                if (openNewImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openNewImageFileDialog.FileName;

                    try
                    {
                        Bitmap imageFromFile = new Bitmap(filePath);
                        
                        SetImageAndData(imageFromFile);
                    }
                    catch (IOException exception)
                    {
                        Console.WriteLine("IOException");
                        Console.WriteLine(exception.Message);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Exception");
                        Console.WriteLine(exception.Message);
                    }
                }
            }
        }

        private void SetImageAndData(Bitmap originalImage)
        {
            mainPictureBox.Image = originalImage;
            threadPictureBox.Image = new Bitmap(mainPictureBox.Image.Width, mainPictureBox.Image.Height);
            symbolsPictureBox.Image = new Bitmap(mainPictureBox.Image.Width, mainPictureBox.Image.Height);
            gridPictureBox.Image = new Bitmap(mainPictureBox.Image.Width, mainPictureBox.Image.Height);
            borderPictureBox.Image = new Bitmap(mainPictureBox.Image.Width, mainPictureBox.Image.Height);
            backstitchPictureBox.Image = new Bitmap(mainPictureBox.Image.Width, mainPictureBox.Image.Height);

            baseLayerPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(Color.White, mainPictureBox.Image.Width, mainPictureBox.Image.Height);

            imageAndOperationsData = new ImageAndOperationsData(new Bitmap(mainPictureBox.Image));
        }

        private void processImageButton_Click(object sender, EventArgs e)
        {
            ProcessImage();
        }

        private void ProcessImage(int numberOfColors = -1)
        {
            if (imageAndOperationsData == null) return;

            imageAndOperationsData.newWidth = widthSizeTrackBar.Value;
            imageAndOperationsData.numberOfColors = numberOfColors == -1 ? numberOfColorsTrackBar.Value : numberOfColors;
            imageAndOperationsData.numberOfIterations = numberOfIterationsTrackBar.Value;

            //imageAndOperationsData.ProcessImage();
            imageAndOperationsData.ProcessImageInSeparateLayers(newPixelSizeTrackBar.Value, processImageExactToSourceCheckBox.Checked);
            //int newImageWidth = imageAndOperationsData.ResultingImage.Width;
            //int newImageHeight = imageAndOperationsData.ResultingImage.Height;
            ResetImagesAfterProcessing(true);
        }

        private void ResetImagesAfterProcessing(bool clearBackgroundList, bool clearBackstitchImage = true)
        {
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;/*ImageTransformations.ResizeBitmap(imageAndOperationsData.resultingImage, mainPictureBox.Width * 10);*/
            threadPictureBox.Image = imageAndOperationsData.ThreadImage;
            symbolsPictureBox.Image = imageAndOperationsData.SymbolsImage;
            gridPictureBox.Image = imageAndOperationsData.GridImage;
            borderPictureBox.Image = imageAndOperationsData.BorderImage;
            if (clearBackstitchImage)
            {
                backstitchPictureBox.Image = new Bitmap(mainPictureBox.Image.Width, mainPictureBox.Image.Height);
            }
            else
            {
                backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;
            }

            baseLayerPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(Color.White, imageAndOperationsData.ResultingImage.Width, imageAndOperationsData.ResultingImage.Height);


            FillListsOfColors(clearBackgroundList);

            ResetOrderOfVisibilityOfPictureBoxes();
        }

        private void FillListsOfColors(bool clearBackgroundList = true)
        {
            List<Color> crossStitchColorMeans = imageAndOperationsData.GetCrossStitchColors();
            List<Color> backstitchColors = imageAndOperationsData.GetBackstitchColors();

            flowLayoutPanelListOfCrossStitchColors.AutoScroll = true;
            flowLayoutPanelListOfCrossStitchColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfCrossStitchColors.WrapContents = false;

            flowLayoutPanelListOfBackstitchColors.AutoScroll = true;
            flowLayoutPanelListOfBackstitchColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfBackstitchColors.WrapContents = false;

            flowLayoutPanelListOfCrossStitchColors.Controls.Clear();
            selectedColorsControlsList.Clear();
            if (clearBackgroundList)
            {
                imageAndOperationsData.colorIsBackgroundList.Clear();
                imageAndOperationsData.colorIsBackgroundList.Add(true); // corresponding to empty color
            }

            flowLayoutPanelListOfBackstitchColors.Controls.Clear();
            selectedBackstitchColorsControlsList.Clear();

            for (int i = 1; i < crossStitchColorMeans.Count; i++)   //We start at 1 here to take the empty color into account
            {
                ReducedColorControl crossStitchColorControl = new ReducedColorControl();
                crossStitchColorControl.InitializeReducedColorControl(crossStitchColorMeans[i], i, this);
                flowLayoutPanelListOfCrossStitchColors.Controls.Add(crossStitchColorControl);
                if (clearBackgroundList)
                {
                    imageAndOperationsData.colorIsBackgroundList.Add(false);
                }
            }

            for (int i = 0; i < backstitchColors.Count; i++)
            {
                BackstitchColorControl backstitchColorControl = new BackstitchColorControl();
                backstitchColorControl.InitializeBackstitchColorControl(backstitchColors[i], i, this);
                flowLayoutPanelListOfBackstitchColors.Controls.Add(backstitchColorControl);
            }
        }

        public void UncheckAllOtherCrossStitchColorControls(int indexToRemainChecked)
        {
            foreach (object colorControl in flowLayoutPanelListOfCrossStitchColors.Controls)
            {
                ReducedColorControl reducedColorControl = (ReducedColorControl)colorControl;
                if(reducedColorControl.reducedColorIndex != indexToRemainChecked)
                {
                    reducedColorControl.ModifySelectionCheckBox(false);
                }
            }
        }

        public void UncheckAllOtherCrossStitchColorControlsAndCheckTheChosenOne(int indexToRemainChecked)
        {
            foreach (object colorControl in flowLayoutPanelListOfCrossStitchColors.Controls)
            {
                ReducedColorControl reducedColorControl = (ReducedColorControl)colorControl;
                if (reducedColorControl.reducedColorIndex != indexToRemainChecked)
                {
                    reducedColorControl.ModifySelectionCheckBox(false);
                }
                else
                {
                    reducedColorControl.ModifySelectionCheckBox(true);
                }
            }
        }

        public void UncheckAllOtherBackstitchColorControls(int indexToRemainChecked)
        {
            foreach (object colorControl in flowLayoutPanelListOfBackstitchColors.Controls)
            {
                BackstitchColorControl backstitchColorControl = (BackstitchColorControl)colorControl;
                if (backstitchColorControl.backstitchColorIndex != indexToRemainChecked)
                {
                    backstitchColorControl.ModifyBackstitchSelectionCheckBox(false);
                }
            }
        }

        private void TryToProcessImage()
        {
            if (ProcessAtAllChangesCheckBox.Checked)
            {
                ProcessImage();
            }
        }

        private void saveImageButton_Click(object sender, EventArgs e)
        {
            if(imageAndOperationsData != null && imageAndOperationsData.ResultingImage != null)
            {
                //saveImageFileDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif|All files|*.*";
                saveImageFileDialog.Filter = "Image files|*.png;*.jpg;*.jpeg;";
                string filePathWithoutExtension = "";

                if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap imageToSave;

                    try
                    {
                        imageToSave = CombineImagesWithVisibilityOfPictureBoxes(pictureBoxesByVisibilityOrder);
                    }catch(Exception exception)
                    {
                        newPixelSizeTrackBar.Value = 10;
                        ProcessImage();
                        imageToSave = CombineImagesWithVisibilityOfPictureBoxes(pictureBoxesByVisibilityOrder);
                    }

                    imageToSave.Save(saveImageFileDialog.FileNames[0]);
                    if(saveImageFileDialog.FileNames[0].LastIndexOf(".") != -1)
                    {
                        int lengthOfSubstring = saveImageFileDialog.FileNames[0].LastIndexOf(".");
                        filePathWithoutExtension = saveImageFileDialog.FileNames[0].Substring(0, lengthOfSubstring);
                        imageAndOperationsData.SerializeData(filePathWithoutExtension + ".edu");

                        lengthOfSubstring = filePathWithoutExtension.LastIndexOf(Path.DirectorySeparatorChar);
                        string title = filePathWithoutExtension.Substring(lengthOfSubstring + 1, filePathWithoutExtension.Length - lengthOfSubstring - 1);
                        imageAndOperationsData.SavePdf(filePathWithoutExtension + ".pdf", Properties.Resources.PhinaliaLogo, title, "", "COLORED CROSS STITCH", "2022 | Phinalia", "Phinalia Library Collection",
                            "Visit our website: ", "https://phinalia.com", "Join our community:", 
                            new string[] { "https://facebook.com", "https://instagram.com", "https://youtube.com", "https://pinterest.com" }, 
                            new Bitmap[] { Properties.Resources.FacebookLogo, Properties.Resources.InstagramLogo, Properties.Resources.YouTubeLogo, Properties.Resources.PinterestLogo }, 
                            new string[] { "Facebook", "Instagram", "YouTube", "Pinterest" });
                    }
                }

                imageAndOperationsData.CreateMachinePath(filePathWithoutExtension);
            }
        }

        private List<Bitmap> PrepareImagesForPdf()
        {
            List<PictureBox> firstImageIntoPdfListOfPictureBoxes = new List<PictureBox>();
            firstImageIntoPdfListOfPictureBoxes.Add(baseLayerPictureBox);
            firstImageIntoPdfListOfPictureBoxes.Add(mainPictureBox);
            firstImageIntoPdfListOfPictureBoxes.Add(threadPictureBox);
            firstImageIntoPdfListOfPictureBoxes.Add(symbolsPictureBox);
            firstImageIntoPdfListOfPictureBoxes.Add(gridPictureBox);
            firstImageIntoPdfListOfPictureBoxes.Add(borderPictureBox);
            firstImageIntoPdfListOfPictureBoxes.Add(backstitchPictureBox);

            Bitmap firstImageIntoPdf = CombineImagesWithVisibilityOfPictureBoxes(firstImageIntoPdfListOfPictureBoxes);

            List<PictureBox> secondImageIntoPdfListOfPictureBoxes = new List<PictureBox>();
            secondImageIntoPdfListOfPictureBoxes.Add(baseLayerPictureBox);
            secondImageIntoPdfListOfPictureBoxes.Add(mainPictureBox);
            secondImageIntoPdfListOfPictureBoxes.Add(threadPictureBox);
            secondImageIntoPdfListOfPictureBoxes.Add(symbolsPictureBox);
            secondImageIntoPdfListOfPictureBoxes.Add(gridPictureBox);
            secondImageIntoPdfListOfPictureBoxes.Add(borderPictureBox);
            secondImageIntoPdfListOfPictureBoxes.Add(backstitchPictureBox);

            Bitmap secondImageIntoPdf = CombineImagesWithVisibilityOfPictureBoxes(secondImageIntoPdfListOfPictureBoxes);

            List<Bitmap> imagesToGoIntoPdf = new List<Bitmap>();
            imagesToGoIntoPdf.Add(firstImageIntoPdf);
            imagesToGoIntoPdf.Add(secondImageIntoPdf);
            return imagesToGoIntoPdf;
        }

        private void RetrieveSavedFileButton_Click(object sender, EventArgs e)
        {
            retrieveSavedFileDialog.Filter = "Temporary Embroidery format (*.edu)|*.edu";

            if(retrieveSavedFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageAndOperationsData = ImageAndOperationsDataSerialized.DeserializeData(retrieveSavedFileDialog.FileName);
                mainPictureBox.Image = imageAndOperationsData.ResultingImage;
                threadPictureBox.Image = imageAndOperationsData.ThreadImage;
                symbolsPictureBox.Image = imageAndOperationsData.SymbolsImage;
                backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;
                gridPictureBox.Image = imageAndOperationsData.GridImage;
                borderPictureBox.Image = imageAndOperationsData.BorderImage;

                baseLayerPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(Color.White, mainPictureBox.Image.Width, mainPictureBox.Image.Height);

                FillListsOfColors();
                ResetOrderOfVisibilityOfPictureBoxes();
            }
        }

        private void widthSizeTrackBar_Scroll(object sender, EventArgs e)
        {
            widthTrackBarLabel.Text = widthSizeTrackBar.Value.ToString();
            TryToProcessImage();
        }

        private void numberOfColorsTrackBar_Scroll(object sender, EventArgs e)
        {
            numberOfColorsTrackBarLabel.Text = numberOfColorsTrackBar.Value.ToString();
            TryToProcessImage();
        }

        private void numberOfIterationsTrackBar_Scroll(object sender, EventArgs e)
        {
            numberOfIterationsTrackBarLabel.Text = numberOfIterationsTrackBar.Value.ToString();
            TryToProcessImage();
        }

        private void removeAlonePixelsTrackBar_Scroll(object sender, EventArgs e)
        {
            removeAlonePixelsLabel.Text = removeAlonePixelsTrackBar.Value.ToString();
            TryToProcessImage();
        }

        public void UpdateReducedColorByIndex(int index, Color newColor)
        {
            imageAndOperationsData.ChangeColorByIndex(index, newColor);
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;
            threadPictureBox.Image = imageAndOperationsData.ThreadImage;
            symbolsPictureBox.Image = imageAndOperationsData.SymbolsImage;
        }

        public void UpdateBackstitchColorByIndex(int index, Color newColor)
        {
            imageAndOperationsData.UpdateBackstitchColorByIndex(index, newColor);
            backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;
        }

        private void mergeColorsButton_Click(object sender, EventArgs e)
        {
            if (selectedColorsControlsList.Count < 2) return;

            //int firstIndex = selectedColorsControlsList[0].colorIndex;

            while(selectedColorsControlsList.Count > 1)
            {
                int firstIndex = selectedColorsControlsList[0].reducedColorIndex;
                int otherIndex = selectedColorsControlsList[1].reducedColorIndex;

                ReducedColorControl controlToRemove = selectedColorsControlsList[1];
                MergeTwoColorsByTheirIndexes(firstIndex, otherIndex, controlToRemove);
                selectedColorsControlsList.RemoveAt(1); //removing the second color selected, i.e. 1
            }
        }

        private void MergeTwoColorsByTheirIndexes(int firstIndex, int otherIndex, ReducedColorControl controlToRemove)
        {
            //The following code deals with the list and dictionary management of the indexes, but first let's paint the pixels of the removed index
            //with the color of the index that will stay
            UpdateReducedColorByIndex(otherIndex, imageAndOperationsData.GetCrossStitchColors()[firstIndex]);

            //Remove the desired index from the backend's list by merging it with the first index
            imageAndOperationsData.MergeTwoColors(firstIndex, otherIndex);

            //removing this index from the frontend
            //Redistribute index values to the list of the frontend
            foreach (object control in flowLayoutPanelListOfCrossStitchColors.Controls)
            {
                ReducedColorControl reducedColorControl = (ReducedColorControl)control;
                if (reducedColorControl.reducedColorIndex > otherIndex)
                {
                    reducedColorControl.reducedColorIndex--;
                }
            }

            ////Now I can remove the desired control from both the selection list and from the collection of controls of the panel
            if (controlToRemove != null)
            {
                flowLayoutPanelListOfCrossStitchColors.Controls.Remove(controlToRemove);
                controlToRemove.Dispose();
            }            
        }

        private ReducedColorControl GetReducedColorControlByIndex(int index)
        {
            foreach (object control in flowLayoutPanelListOfCrossStitchColors.Controls)
            {
                ReducedColorControl reducedColorControl = (ReducedColorControl)control;
                if (reducedColorControl.reducedColorIndex == index)
                {
                    return reducedColorControl;
                }
            }

            return null;
        }

        private void RemoveCrossStitchColor(int index)
        {
            if (index == 0) return;

            ReducedColorControl reducedColorControlToRemove = GetReducedColorControlByIndex(index);

            MergeTwoColorsByTheirIndexes(0, index, reducedColorControlToRemove);
        }

        private void AddNewColor(Color newColor)
        {
            ReducedColorControl colorControl = new ReducedColorControl();
            colorControl.InitializeReducedColorControl(newColor, flowLayoutPanelListOfCrossStitchColors.Controls.Count + 1, /*colorControl,*/ this);    //the "+1" here it to take the empty color into account
            flowLayoutPanelListOfCrossStitchColors.Controls.Add(colorControl);
            imageAndOperationsData.AddNewColor(newColor);
        }

        private void AddNewBackstitchColor(Color newColor)
        {
            int indexToAdd = imageAndOperationsData.AddNewBackstitchColor(newColor);

            BackstitchColorControl colorControl = new BackstitchColorControl();

            colorControl.InitializeBackstitchColorControl(newColor, indexToAdd, this);
            flowLayoutPanelListOfBackstitchColors.Controls.Add(colorControl);
        }

        private void addColorButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            if (addColorDialog.ShowDialog() == DialogResult.OK)
            {
                AddNewColor(addColorDialog.Color);
            }
        }

        private void mergeBackStitchColorsButton_Click(object sender, EventArgs e)
        {
            if (selectedBackstitchColorsControlsList.Count < 2) return;

            //int firstIndex = selectedColorsControlsList[0].colorIndex;

            while (selectedBackstitchColorsControlsList.Count > 1)
            {
                int firstIndex = selectedBackstitchColorsControlsList[0].backstitchColorIndex;
                int otherIndex = selectedBackstitchColorsControlsList[1].backstitchColorIndex;

                ////Remove the desired control from both the selection list and from the collection of controls of the panel
                flowLayoutPanelListOfBackstitchColors.Controls.Remove(selectedBackstitchColorsControlsList[1]);
                //selectedColorsControlsList[1].ModifySelectionCheckBox(false);
                BackstitchColorControl controlToRemove = selectedBackstitchColorsControlsList[1];
                controlToRemove.Dispose();
                selectedBackstitchColorsControlsList.RemoveAt(1);

                imageAndOperationsData.MergeTwoBackstitchColors(firstIndex, otherIndex, false);
            }

            //Repainting backstitch image
            imageAndOperationsData.PaintAllBackstitchLines();
            backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;
        }

        private void addBackstitchColorButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            if (addColorDialog.ShowDialog() == DialogResult.OK)
            {
                AddNewBackstitchColor(addColorDialog.Color);
            }
        }

        private void deleteBackstitchColorButton_Click(object sender, EventArgs e)
        {
            if (selectedBackstitchColorsControlsList.Count < 1) return;            

            while (selectedBackstitchColorsControlsList.Count > 0)
            {
                BackstitchColorControl controlToRemove = selectedBackstitchColorsControlsList[0];

                int firstIndex = controlToRemove.backstitchColorIndex;

                //Remove the desired control from both the selection list and from the collection of controls of the panel
                flowLayoutPanelListOfBackstitchColors.Controls.Remove(controlToRemove);
                //selectedColorsControlsList[1].ModifySelectionCheckBox(false);
                controlToRemove.Dispose();
                selectedBackstitchColorsControlsList.RemoveAt(0);

                imageAndOperationsData.RemoveBackstitchColorByIndex(firstIndex);
                backstitchPictureBox.Image = imageAndOperationsData.BackstitchImage;
            }
        }

        private void crossStitchColorsRadioButton_Clicked(object sender, EventArgs e)
        {
            crossStitchColorsRadioButton.Checked = true;
            backStitchColorsRadioButton.Checked = false;

            currentDrawingMode = DrawingMode.CrossStitch;

            currentStitchModeLabel.Text = "Cross Stitch";
        }

        private void backStitchColorsRadioButton_Clicked(object sender, EventArgs e)
        {
            crossStitchColorsRadioButton.Checked = false;
            backStitchColorsRadioButton.Checked = true;

            currentDrawingMode = DrawingMode.Backstitch;

            currentStitchModeLabel.Text = "Back Stitch";
        }

        public void IncreaseIndexesOfAllCrossStitchColors()
        {
            foreach (object colorControl in flowLayoutPanelListOfCrossStitchColors.Controls)
            {
                ReducedColorControl reducedColorControl = (ReducedColorControl)colorControl;
                reducedColorControl.reducedColorIndex++;
            }
        }

        private void mainPictureBox_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void mainPictureBox_Paint(object sender, PaintEventArgs e)
        {

        }

        public bool CheckIfMultipleSelectionIsActive()
        {            
            return ModifierKeys.HasFlag(multipleSelectionKeyboardKey);
        }

        private void mainImageVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ChangeVisibilityOfPictureBox(mainImageVisibleCheckBox, mainPictureBox);
        }

        private void threadImageVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ChangeVisibilityOfPictureBox(threadImageVisibleCheckBox, threadPictureBox);
        }

        private void symbolsVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ChangeVisibilityOfPictureBox(symbolsVisibleCheckBox, symbolsPictureBox);
        }

        private void gridVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ChangeVisibilityOfPictureBox(gridVisibleCheckBox, gridPictureBox);
        }

        private void borderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ChangeVisibilityOfPictureBox(borderVisibleCheckBox, borderPictureBox);
        }

        private void backstitchVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ChangeVisibilityOfPictureBox(backstitchVisibleCheckBox, backstitchPictureBox);
        }

        private void ChangeVisibilityOfPictureBox(CheckBox checkBoxOfPictureBoxToChangeVisibility, PictureBox pictureBoxToChangeVisibility)
        {
            pictureBoxToChangeVisibility.Visible = checkBoxOfPictureBoxToChangeVisibility.Checked;

            ResetOrderOfVisibilityOfPictureBoxes();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                ResizeControl(control, base.Size, oldFormSize);
            }

            oldFormSize = base.Size;

            if (pictureBoxesByVisibilityOrder.Count != 0)
            {
                pictureBoxUnscaledSize = pictureBoxesByVisibilityOrder[0].Size;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            oldFormSize = base.Size;
            KeyPreview = true;
        }

        private void ResizeControl(Control control, Size newSize, Size oldSize)
        {
            Size oldControlSize = control.Size;

            control.Left += (newSize.Width - oldSize.Width) * control.Left / oldSize.Width;
            control.Width += (newSize.Width - oldSize.Width) * control.Width / oldSize.Width;

            control.Top += (newSize.Height - oldSize.Height) * control.Top / oldSize.Height;
            control.Height += (newSize.Height - oldSize.Height) * control.Height / oldSize.Height;

            foreach (Control childControl in control.Controls)
            {
                ResizeControl(childControl, control.Size, oldControlSize);
            }
        }

        private void newPixelSizeTrackBar_Scroll(object sender, EventArgs e)
        {
            newPixelSizeTrackBarLabel.Text = newPixelSizeTrackBar.Value.ToString();
        }

        private void RepaintCrossesButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData != null && imageAndOperationsData.ResultingImage != null && imageAndOperationsData.ThreadImage != null)
            {
                imageAndOperationsData.RepaintCrosses();
                threadPictureBox.Image = imageAndOperationsData.ThreadImage;
            }
        }

        private void newCanvasButton_Click(object sender, EventArgs e)
        {
            Bitmap emptyImage = new Bitmap(100, 100);
            using (var graphics = Graphics.FromImage(emptyImage))
            {
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, emptyImage.Width, emptyImage.Height);
            }

            SetImageAndData(emptyImage);
            ProcessImage(1);
        }

        private void changeCanvasSizeButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            Tuple<int, int> currentSize = imageAndOperationsData.GetSizeInPixels();
            using (ChangeCanvasSizeDialog changeCanvasSizeDialog = new ChangeCanvasSizeDialog(currentSize.Item1, currentSize.Item2))
            {
                if (changeCanvasSizeDialog.ShowDialog() == DialogResult.OK)
                {
                    imageAndOperationsData.ChangeCanvasSize(changeCanvasSizeDialog.newWidth, changeCanvasSizeDialog.newHeight, newPixelSizeTrackBar.Value);

                    ResetImagesAfterProcessing(false, false);
                }
            }
        }

        private void removeUnusedCrossStichColorsButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            List<ReducedColorControl> reducedColorControlsToRemove = new List<ReducedColorControl>();
            foreach (object colorControl in flowLayoutPanelListOfCrossStitchColors.Controls)
            {
                ReducedColorControl reducedColorControl = (ReducedColorControl)colorControl;
                if(reducedColorControl.reducedColorIndex == 0)
                {
                    imageAndOperationsData.TryToAddEmptyColor();
                    IncreaseIndexesOfAllCrossStitchColors();
                }

                int amountForThisCrossStitchColor = imageAndOperationsData.GetAmountOfColorsForColorIndex(reducedColorControl.reducedColorIndex, out _);

                if(amountForThisCrossStitchColor == 0)
                {                    
                    reducedColorControlsToRemove.Add(reducedColorControl);
                }
            }

            foreach (var currentReducedColorControlToRemove in reducedColorControlsToRemove)
            {
                RemoveCrossStitchColor(currentReducedColorControlToRemove.reducedColorIndex);
                selectedColorsControlsList.RemoveAll(selectedReducedColor => selectedReducedColor.reducedColorIndex == currentReducedColorControlToRemove.reducedColorIndex);
            }
        }

        private void deleteCrossStitchColorButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.ResultingImage == null) return;

            while (selectedColorsControlsList.Count > 0)
            {
                ReducedColorControl selectedReducedColorToRemove = selectedColorsControlsList[0];
                RemoveCrossStitchColor(selectedReducedColorToRemove.reducedColorIndex);
                
                selectedColorsControlsList.RemoveAt(0); //removing the first color selected, i.e. 0
            }
        }
    }

    public enum DrawingMode
    {
        CrossStitch, 
        Backstitch
    }
}

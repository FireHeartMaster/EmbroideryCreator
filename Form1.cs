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
        //private PaintEventHandler backstitchPaintEventHandler = null;
        //private bool backstitchEventHandlerAlreadySubscribed = false;
        private Point lastBackstitchPointMouseUp = new Point(0, 0);

        private Tuple<int, int> realImagePositionMouseDown = new Tuple<int, int>(0, 0);
        private Tuple<int, int> realImagePositionMouseUp = new Tuple<int, int>(0, 0);
        private Tuple<int, int> lastBackstitchRealImagePositionMouseUp = new Tuple<int, int>(0, 0);

        private Tuple<int, int> roundedRealImagePositionMouseDown = new Tuple<int, int>(0, 0);
        private Tuple<int, int> roundedRealImagePositionMouseUp = new Tuple<int, int>(0, 0);
        private Tuple<int, int> lastBackstitchRoundedRealImagePositionMouseUp = new Tuple<int, int>(0, 0);

        private Keys multipleSelectionKeyboardKey = Keys.Control;

        public MainForm()
        {
            InitializeComponent();

            widthSizeTrackBar.Value = defaultWidth;
            numberOfColorsTrackBar.Value = defaultNumberOfColors;
            numberOfIterationsTrackBar.Value = defaultNumberOfIterations;

            crossStitchColorsRadioButton.Checked = true;
            backStitchColorsRadioButton.Checked = false;
        }

        private void chooseNewImageButton_Click(object sender, EventArgs args)
        {
            SelectNewImageFile();
        }

        private void mainPictureBox_Click(object sender, EventArgs e)
        {
            //SelectNewImageFile();
            //Console.WriteLine("Simple Click");
        }

        private void mainPictureBox_DoubleClick(object sender, EventArgs e)
        {
            //Console.WriteLine("Double Click");
        }

        internal void UpdateIfShouldPaintReducedColorBackground(int reducedColorIndex, bool isBackground)
        {
            if(reducedColorIndex >= 0 && reducedColorIndex < imageAndOperationsData.colorIsBackgroundList.Count)
            {
                imageAndOperationsData.colorIsBackgroundList[reducedColorIndex] = isBackground;
            }                        
        }

        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse Down");
            isDrawing = true;

            Point pointOnImage = e.Location;

            DrawOnPictureBox(pointOnImage);
        }

        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse Up");
            isDrawing = false;
            SetBackstitchDrawMouseUp();
            //Console.WriteLine(mainPictureBox.Size.Width + " " + mainPictureBox.Size.Height);
        }
        
        private void mainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                switch (currentDrawingMode)
                {
                    case DrawingMode.CrossStitch:
                        //Console.WriteLine(line + ": Drawing"); line++;
                        DrawOnPictureBox(e.Location);
                        break;

                    case DrawingMode.Backstitch:
                        //if (backstitchEventHandlerAlreadySubscribed)
                        //{
                        //    pointMouseUp = e.Location;
                        //}
                        pointMouseUp = e.Location;
                        PaintBackstitch();
                        break;
                }
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
            //imageBeforeDrawing = mainPictureBox.Image.Clone() as Image;
            imageBeforeDrawing = new Bitmap(mainPictureBox.Image);
            pointMouseDown = new Point(pointOnImage.X, pointOnImage.Y);
            realImagePositionMouseDown = ImageTransformations.ConvertFromPictureBoxToRealImage(mainPictureBox, new Tuple<int, int>(pointMouseDown.X, pointMouseDown.Y));
            roundedRealImagePositionMouseDown = imageAndOperationsData.ConvertFromGeneralPositionOnImagesToRoundedGeneralPositionOnImageIncludingHalfValues(realImagePositionMouseDown);
            //pointMouseUp = new Point(pointOnImage.X, pointOnImage.Y);

            //Subscribe to Paint event
            //if(backstitchPaintEventHandler == null)
            //{
            //    backstitchPaintEventHandler = new PaintEventHandler(PaintBackstitch);
            //}

            //if (!backstitchEventHandlerAlreadySubscribed)
            //{
            //    mainPictureBox.Paint += backstitchPaintEventHandler;
            //    backstitchEventHandlerAlreadySubscribed = true;
            //}
        }

        private void SetBackstitchDrawMouseUp()
        {
            //mainPictureBox.Image = imageBeforeDrawing.Clone() as Image;

            //TODO: set backstitch points in the backend

            //Unsubscribe to Paint event
            //if (backstitchEventHandlerAlreadySubscribed)
            //{
            //    mainPictureBox.Paint -= backstitchPaintEventHandler;
            //    backstitchEventHandlerAlreadySubscribed = false;
            //}
            if(selectedBackstitchColorsControlsList.Count > 0)
            {
                Tuple<float, float> startingPosition = imageAndOperationsData.ConvertFromGeneralPositionOnImageToCoordinatesIncludingHalfValues(realImagePositionMouseDown);
                Tuple<float, float> endingPosition = imageAndOperationsData.ConvertFromGeneralPositionOnImageToCoordinatesIncludingHalfValues(realImagePositionMouseUp);
                imageAndOperationsData.AddNewBackstitchLine(selectedBackstitchColorsControlsList[0].crossStitchColorIndex, startingPosition, endingPosition);
            }
        }

        private void PaintBackstitch()
        {
            if (selectedBackstitchColorsControlsList.Count == 0) return;
            if (lastBackstitchPointMouseUp.X == pointMouseUp.X && lastBackstitchPointMouseUp.Y == pointMouseUp.Y) return;
            realImagePositionMouseUp = ImageTransformations.ConvertFromPictureBoxToRealImage(mainPictureBox, new Tuple<int, int>(pointMouseUp.X, pointMouseUp.Y));
            if (realImagePositionMouseUp.Item1 == lastBackstitchRealImagePositionMouseUp.Item1 && realImagePositionMouseUp.Item2 == lastBackstitchRealImagePositionMouseUp.Item2) return;

            roundedRealImagePositionMouseUp = imageAndOperationsData.ConvertFromGeneralPositionOnImagesToRoundedGeneralPositionOnImageIncludingHalfValues(realImagePositionMouseUp);

            if (roundedRealImagePositionMouseUp.Item1 == lastBackstitchRoundedRealImagePositionMouseUp.Item1 && roundedRealImagePositionMouseUp.Item2 == lastBackstitchRoundedRealImagePositionMouseUp.Item2) return;

            mainPictureBox.Image = imageBeforeDrawing.Clone() as Image;

            Graphics graphics = Graphics.FromImage(mainPictureBox.Image);

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
            Tuple<int, int> realImagePosition = ImageTransformations.ConvertFromPictureBoxToRealImage(mainPictureBox, new Tuple<int, int>(positionOnImage.X, positionOnImage.Y));

            int colorIndexToPaint = selectedColorsControlsList.Count > 0 ? selectedColorsControlsList[0].reducedColorIndex : ((ReducedColorControl)flowLayoutPanelListOfCrossStitchColors.Controls[0]).reducedColorIndex;

            switch (drawingToolsControl.currentDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    imageAndOperationsData.PaintNewColorOnGeneralPosition(realImagePosition, colorIndexToPaint);
                    break;
                case DrawingToolInUse.Bucket:
                    imageAndOperationsData.FillRegionWithColorByPosition(realImagePosition, colorIndexToPaint);
                    isDrawing = false;
                    //SetBackstitchDrawMouseUp();
                    break;
                default:
                    break;
            }

            //reload picture box
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;
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
                        mainPictureBox.Image = new Bitmap(filePath);

                        imageAndOperationsData = new ImageAndOperationsData(new Bitmap(mainPictureBox.Image));
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

        private void processImageButton_Click(object sender, EventArgs e)
        {
            ProcessImage();
        }

        private void ProcessImage()
        {
            if (imageAndOperationsData == null) return;

            imageAndOperationsData.newWidth = widthSizeTrackBar.Value;
            imageAndOperationsData.numberOfColors = numberOfColorsTrackBar.Value;
            imageAndOperationsData.numberOfIterations = numberOfIterationsTrackBar.Value;

            imageAndOperationsData.ProcessImage();
            int newImageWidth = imageAndOperationsData.ResultingImage.Width;
            int newImageHeight = imageAndOperationsData.ResultingImage.Height;
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;/*ImageTransformations.ResizeBitmap(imageAndOperationsData.resultingImage, mainPictureBox.Width * 10);*/

            FillListsOfColors();
        }

        private void FillListsOfColors()
        {
            List<Color> colorMeans = imageAndOperationsData.GetColors();

            flowLayoutPanelListOfCrossStitchColors.AutoScroll = true;
            flowLayoutPanelListOfCrossStitchColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfCrossStitchColors.WrapContents = false;

            flowLayoutPanelListOfBackstitchColors.AutoScroll = true;
            flowLayoutPanelListOfBackstitchColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfBackstitchColors.WrapContents = false;

            flowLayoutPanelListOfCrossStitchColors.Controls.Clear();
            selectedColorsControlsList.Clear();
            imageAndOperationsData.colorIsBackgroundList.Clear();

            flowLayoutPanelListOfBackstitchColors.Controls.Clear();
            selectedBackstitchColorsControlsList.Clear();

            for (int i = 0; i < colorMeans.Count; i++)
            {
                ReducedColorControl colorControl = new ReducedColorControl();
                //Bitmap colorImage = new Bitmap(1, 1);
                //colorImage.SetPixel(0, 0, Color.Red);
                colorControl.InitializeReducedColorControl(colorMeans[i], i, colorControl, this);
                flowLayoutPanelListOfCrossStitchColors.Controls.Add(colorControl);
                imageAndOperationsData.colorIsBackgroundList.Add(false);
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

        public void UncheckAllOtherBackstitchColorControls(int indexToRemainChecked)
        {
            foreach (object colorControl in flowLayoutPanelListOfBackstitchColors.Controls)
            {
                BackstitchColorControl backstitchColorControl = (BackstitchColorControl)colorControl;
                if (backstitchColorControl.crossStitchColorIndex != indexToRemainChecked)
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


                if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imageAndOperationsData.ResultingImage.Save(saveImageFileDialog.FileNames[0]);
                    if(saveImageFileDialog.FileNames[0].LastIndexOf(".") != -1)
                    {
                        int lengthOfSubstring = saveImageFileDialog.FileNames[0].LastIndexOf(".");
                        string filePathWithoutExtension = saveImageFileDialog.FileNames[0].Substring(0, lengthOfSubstring);
                        imageAndOperationsData.SerializeData(filePathWithoutExtension + ".edu");
                    }
                }

                imageAndOperationsData.CreateMachinePath();
            }
        }

        private void RetrieveSavedFileButton_Click(object sender, EventArgs e)
        {
            retrieveSavedFileDialog.Filter = "Temporary Embroidery format (*.edu)|*.edu";

            if(retrieveSavedFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageAndOperationsData = ImageAndOperationsDataSerialized.DeserializeData(retrieveSavedFileDialog.FileName);
                mainPictureBox.Image = imageAndOperationsData.ResultingImage;

                FillListsOfColors();
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

        public void UpdateReducedColorByIndex(int index, Color newColor)
        {
            imageAndOperationsData.ChangeColorByIndex(index, newColor);
            mainPictureBox.Image = imageAndOperationsData.ResultingImage;
        }

        public void UpdateBackstitchColorByIndex(int index, Color newColor)
        {
            imageAndOperationsData.UpdateBackstitchColorByIndex(index, newColor);
            //TODO: Update the image in the main picture box with the new one after the update of color
        }

        private void mergeColorsButton_Click(object sender, EventArgs e)
        {
            if (selectedColorsControlsList.Count < 2) return;

            //int firstIndex = selectedColorsControlsList[0].colorIndex;

            while(selectedColorsControlsList.Count > 1)
            {
                int firstIndex = selectedColorsControlsList[0].reducedColorIndex;
                int otherIndex = selectedColorsControlsList[1].reducedColorIndex;

                //The following code deals with the list and dictionary management of the indexes, but first let's paint the pixels of the removed index
                //with the color of the index that will stay
                UpdateReducedColorByIndex(otherIndex, imageAndOperationsData.GetColors()[firstIndex]);

                //Remove the desired index from the backend's list
                imageAndOperationsData.MergeTwoColors(firstIndex, otherIndex);

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
                flowLayoutPanelListOfCrossStitchColors.Controls.Remove(selectedColorsControlsList[1]);
                imageAndOperationsData.colorIsBackgroundList.RemoveAt(selectedColorsControlsList[1].reducedColorIndex);
                //selectedColorsControlsList[1].ModifySelectionCheckBox(false);
                ReducedColorControl controlToRemove = selectedColorsControlsList[1];
                controlToRemove.Dispose();
                selectedColorsControlsList.RemoveAt(1);
            }
        }

        private void AddNewColor(Color newColor)
        {
            ReducedColorControl colorControl = new ReducedColorControl();
            colorControl.InitializeReducedColorControl(newColor, flowLayoutPanelListOfCrossStitchColors.Controls.Count, colorControl, this);
            flowLayoutPanelListOfCrossStitchColors.Controls.Add(colorControl);
            imageAndOperationsData.AddNewColor(newColor);
            imageAndOperationsData.colorIsBackgroundList.Add(false);
        }

        private void AddNewBackstitchColor(Color newColor)
        {
            BackstitchColorControl colorControl = new BackstitchColorControl();
            colorControl.InitializeBackstitchColorControl(newColor, flowLayoutPanelListOfBackstitchColors.Controls.Count, this);
            flowLayoutPanelListOfBackstitchColors.Controls.Add(colorControl);
            //imageAndOperationsData.AddNewColor(newColor);

            imageAndOperationsData.AddNewBackstitchColor(newColor);
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
                int firstIndex = selectedBackstitchColorsControlsList[0].crossStitchColorIndex;
                int otherIndex = selectedBackstitchColorsControlsList[1].crossStitchColorIndex;

                //The following code deals with the list and dictionary management of the indexes, but first let's paint the pixels of the removed index
                //with the color of the index that will stay
                //UpdateBackstitchColorByIndex(otherIndex, imageAndOperationsData.GetColors()[firstIndex]);
                //TODO: Update backstitch colors using the correct function (imageAndOperationsData.GetColors retrieves the reduced colors,
                //not the backstitch ones)

                //Remove the desired index from the backend's list
                //imageAndOperationsData.MergeTwoColors(firstIndex, otherIndex);
                //TODO: merge two backstitch colors in the backend

                //Redistribute index values to the list of the frontend
                foreach (object control in flowLayoutPanelListOfBackstitchColors.Controls)
                {
                    BackstitchColorControl backstitchColorControl = (BackstitchColorControl)control;
                    if (backstitchColorControl.crossStitchColorIndex > otherIndex)
                    {
                        backstitchColorControl.crossStitchColorIndex--;
                    }
                }

                ////Now I can remove the desired control from both the selection list and from the collection of controls of the panel
                flowLayoutPanelListOfBackstitchColors.Controls.Remove(selectedBackstitchColorsControlsList[1]);
                //selectedColorsControlsList[1].ModifySelectionCheckBox(false);
                BackstitchColorControl controlToRemove = selectedBackstitchColorsControlsList[1];
                controlToRemove.Dispose();
                selectedBackstitchColorsControlsList.RemoveAt(1);
            }
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

            //int firstIndex = selectedColorsControlsList[0].colorIndex;

            while (selectedBackstitchColorsControlsList.Count > 0)
            {
                int firstIndex = selectedBackstitchColorsControlsList[0].crossStitchColorIndex;

                //The following code deals with the list and dictionary management of the indexes, but first let's paint the pixels of the removed index
                //with the color of the index that will stay
                //UpdateBackstitchColorByIndex(otherIndex, imageAndOperationsData.GetColors()[firstIndex]);
                //TODO: Update backstitch colors using the correct function (imageAndOperationsData.GetColors retrieves the reduced colors,
                //not the backstitch ones)

                //Remove the desired index from the backend's list
                //imageAndOperationsData.MergeTwoColors(firstIndex, otherIndex);
                //TODO: merge two backstitch colors in the backend

                //Redistribute index values to the list of the frontend
                foreach (object control in flowLayoutPanelListOfBackstitchColors.Controls)
                {
                    BackstitchColorControl backstitchColorControl = (BackstitchColorControl)control;
                    if (backstitchColorControl.crossStitchColorIndex > firstIndex)
                    {
                        backstitchColorControl.crossStitchColorIndex--;
                    }
                }

                ////Now I can remove the desired control from both the selection list and from the collection of controls of the panel
                flowLayoutPanelListOfBackstitchColors.Controls.Remove(selectedBackstitchColorsControlsList[0]);
                //selectedColorsControlsList[1].ModifySelectionCheckBox(false);
                BackstitchColorControl controlToRemove = selectedBackstitchColorsControlsList[0];
                controlToRemove.Dispose();
                selectedBackstitchColorsControlsList.RemoveAt(0);
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
    }

    public enum DrawingMode
    {
        CrossStitch, 
        Backstitch
    }
}

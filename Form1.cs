﻿using System;
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

        public MainForm()
        {
            InitializeComponent();

            widthSizeTrackBar.Value = defaultWidth;
            numberOfColorsTrackBar.Value = defaultNumberOfColors;
            numberOfIterationsTrackBar.Value = defaultNumberOfIterations;
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

        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse Down");
            isDrawing = true;

            Point positionOnImage = e.Location;

            DrawOnPictureBox(positionOnImage);
        }

        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Mouse Up");
            isDrawing = false;
            //Console.WriteLine(mainPictureBox.Size.Width + " " + mainPictureBox.Size.Height);
        }
        
        private void mainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                //Console.WriteLine(line + ": Drawing"); line++;
                DrawOnPictureBox(e.Location);
            }
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            isDrawing = false;
        }

        private void DrawOnPictureBox(Point positionOnImage)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.resultingImage == null) return;

            Tuple<int, int> realImagePosition = ConvertFromPictureBoxToRealImage(new Tuple<int, int>(positionOnImage.X, positionOnImage.Y));

            int colorIndexToPaint = selectedColorsControlsList.Count > 0 ? selectedColorsControlsList[0].reducedColorIndex : ((ReducedColorControl)flowLayoutPanelListOfCrossStitchColors.Controls[0]).reducedColorIndex;

            switch (drawingToolsControl.currentDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    imageAndOperationsData.PaintNewColorOnGeneralPosition(realImagePosition, colorIndexToPaint);
                    break;
                case DrawingToolInUse.Bucket:
                    imageAndOperationsData.FillRegionWithColorByPosition(realImagePosition, colorIndexToPaint);
                    isDrawing = false;
                    break;
                default:
                    break;
            }

            //reload picture box
            mainPictureBox.Image = imageAndOperationsData.resultingImage;
        }

        private Tuple<int, int> ConvertFromPictureBoxToRealImage(Tuple<int, int> pictureBoxPosition)
        {
            bool biggerIsWidth = mainPictureBox.Image.Width > mainPictureBox.Image.Height;

            float ratio;
            if (biggerIsWidth)
            {
                ratio = ((float)mainPictureBox.Image.Width) / mainPictureBox.Size.Width;
            }
            else
            {
                ratio = ((float)mainPictureBox.Image.Height) / mainPictureBox.Size.Height;
            }
            int horizontalPosition = (int)(ratio * (pictureBoxPosition.Item1 - (mainPictureBox.Size.Width * 0.5f)) + mainPictureBox.Image.Width * 0.5f);
            int verticalPosition = (int)(ratio * (pictureBoxPosition.Item2 - (mainPictureBox.Size.Height * 0.5f)) + mainPictureBox.Image.Height * 0.5f);
            return new Tuple<int, int>(horizontalPosition, verticalPosition);
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
            int newImageWidth = imageAndOperationsData.resultingImage.Width;
            int newImageHeight = imageAndOperationsData.resultingImage.Height;
            mainPictureBox.Image = imageAndOperationsData.resultingImage;/*ImageTransformations.ResizeBitmap(imageAndOperationsData.resultingImage, mainPictureBox.Width * 10);*/
            List<Color> colorMeans = imageAndOperationsData.GetColors();

            flowLayoutPanelListOfCrossStitchColors.AutoScroll = true;
            flowLayoutPanelListOfCrossStitchColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfCrossStitchColors.WrapContents = false;

            flowLayoutPanelListOfBackstitchColors.AutoScroll = true;
            flowLayoutPanelListOfBackstitchColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfBackstitchColors.WrapContents = false;

            flowLayoutPanelListOfCrossStitchColors.Controls.Clear();
            selectedColorsControlsList.Clear();

            flowLayoutPanelListOfBackstitchColors.Controls.Clear();
            selectedBackstitchColorsControlsList.Clear();

            for (int i = 0; i < colorMeans.Count; i++)
            {
                ReducedColorControl colorControl = new ReducedColorControl();
                //Bitmap colorImage = new Bitmap(1, 1);
                //colorImage.SetPixel(0, 0, Color.Red);
                colorControl.InitializeReducedColorControl(colorMeans[i], i, colorControl, this);
                flowLayoutPanelListOfCrossStitchColors.Controls.Add(colorControl);
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
            if(imageAndOperationsData.resultingImage != null)
            {
                //saveImageFileDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif|All files|*.*";
                saveImageFileDialog.Filter = "Image files|*.png;*.jpg;*.jpeg;";


                if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imageAndOperationsData.resultingImage.Save(saveImageFileDialog.FileNames[0]);
                }

                imageAndOperationsData.CreateMachinePath();
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
            mainPictureBox.Image = imageAndOperationsData.resultingImage;
        }

        public void UpdateBackstitchColorByIndex(int index, Color newColor)
        {
            throw new NotImplementedException();
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
        }

        private void AddNewBackstitchColor(Color newColor)
        {
            BackstitchColorControl colorControl = new BackstitchColorControl();
            colorControl.InitializeBackstitchColorControl(newColor, flowLayoutPanelListOfBackstitchColors.Controls.Count, this);
            flowLayoutPanelListOfBackstitchColors.Controls.Add(colorControl);
            //imageAndOperationsData.AddNewColor(newColor);
            //TODO: Add new backstitch color to backend
        }

        private void addColorButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null || imageAndOperationsData.resultingImage == null) return;

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
            if (imageAndOperationsData == null || imageAndOperationsData.resultingImage == null) return;

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
    }

    public enum DrawingMode
    {
        CrossStitch, 
        Backstitch
    }
}

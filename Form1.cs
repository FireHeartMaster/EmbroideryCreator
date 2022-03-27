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

        public List<ReducedColorControl> selectedColorsControlsList = new List<ReducedColorControl>();

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
            Console.WriteLine("Simple Click");
        }

        private void mainPictureBox_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("Double Click");
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

            flowLayoutPanelListOfColors.AutoScroll = true;
            flowLayoutPanelListOfColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfColors.WrapContents = false;

            flowLayoutPanelListOfColors.Controls.Clear();
            selectedColorsControlsList.Clear();
            for (int i = 0; i < colorMeans.Count; i++)
            {
                ReducedColorControl colorControl = new ReducedColorControl();
                //Bitmap colorImage = new Bitmap(1, 1);
                //colorImage.SetPixel(0, 0, Color.Red);
                colorControl.InitializeReducedColorControl(colorMeans[i], i, colorControl, this);
                flowLayoutPanelListOfColors.Controls.Add(colorControl);
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
                saveImageFileDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif|All files|*.*";

                if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imageAndOperationsData.resultingImage.Save(saveImageFileDialog.FileNames[0]);
                }
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

        public void UpdateColorByIndex(int index, Color newColor)
        {
            imageAndOperationsData.ChangeColorByIndex(index, newColor);
            mainPictureBox.Image = imageAndOperationsData.resultingImage;
        }

        private void mergeColorsButton_Click(object sender, EventArgs e)
        {
            if (selectedColorsControlsList.Count < 2) return;

            //int firstIndex = selectedColorsControlsList[0].colorIndex;

            while(selectedColorsControlsList.Count > 1)
            {
                int firstIndex = selectedColorsControlsList[0].colorIndex;
                int otherIndex = selectedColorsControlsList[1].colorIndex;

                //The following code deals with the list and dictionary management of the indexes, but first let's paint the pixels of the removed index
                //with the color of the index that will stay
                UpdateColorByIndex(otherIndex, imageAndOperationsData.GetColors()[firstIndex]);

                //Remove the desired index from the backend's list
                imageAndOperationsData.MergeTwoColors(firstIndex, otherIndex);

                //Redistribute index values to the list of the frontend
                foreach (object control in flowLayoutPanelListOfColors.Controls)
                {
                    ReducedColorControl reducedColorControl = (ReducedColorControl)control;
                    if (reducedColorControl.colorIndex > otherIndex)
                    {
                        reducedColorControl.colorIndex--;
                    }
                }

                ////Now I can remove the desired control from both the selection list and from the collection of controls of the panel
                flowLayoutPanelListOfColors.Controls.Remove(selectedColorsControlsList[1]);
                //selectedColorsControlsList[1].ModifySelectionCheckBox(false);
                ReducedColorControl controlToRemove = selectedColorsControlsList[1];
                controlToRemove.Dispose();
                selectedColorsControlsList.RemoveAt(1);
            }
        }

        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse Down");
        }

        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse Up");
        }
    }
}

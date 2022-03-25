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
            SelectNewImageFile();
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

            imageAndOperationsData.ProcessImageAlternateOrder();
            int newImageWidth = imageAndOperationsData.resultingImage.Width;
            int newImageHeight = imageAndOperationsData.resultingImage.Height;
            mainPictureBox.Image = imageAndOperationsData.resultingImage;/*ImageTransformations.ResizeBitmap(imageAndOperationsData.resultingImage, mainPictureBox.Width * 10);*/
            int[,] matrixOfNewColors = imageAndOperationsData.GetMatrixOfColors();
            Color[] colorMeans = imageAndOperationsData.GetColors();

            flowLayoutPanelListOfColors.AutoScroll = true;
            flowLayoutPanelListOfColors.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelListOfColors.WrapContents = false;

            flowLayoutPanelListOfColors.Controls.Clear();
            for (int i = 0; i < colorMeans.Length; i++)
            {
                ReducedColorControl colorControl = new ReducedColorControl();
                //Bitmap colorImage = new Bitmap(1, 1);
                //colorImage.SetPixel(0, 0, Color.Red);
                colorControl.reducedColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(colorMeans[i], 30, 30);
                colorControl.indexInTheList = i;
                colorControl.myReferenceToMainForm = this;
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
            imageAndOperationsData.UpdateColorByIndex(index, newColor);
            mainPictureBox.Image = imageAndOperationsData.resultingImage;
        }
    }
}

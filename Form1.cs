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
        public MainForm()
        {
            InitializeComponent();
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

                    ////Read the contents of the file into a stream
                    //var fileStream = openNewImageFileDialog.OpenFile();

                    //using (StreamReader reader = new StreamReader(fileStream))
                    //{
                    //    fileContent = reader.ReadToEnd();
                    //}
                }
            }
        }

        private void processImageButton_Click(object sender, EventArgs e)
        {
            if (imageAndOperationsData == null) return;

            imageAndOperationsData.newWidth = widthSizeTrackBar.Value;
            imageAndOperationsData.numberOfColors = numberOfColorsTrackBar.Value;

            imageAndOperationsData.PixelateImage();
            imageAndOperationsData.ReduceNumberOfColors();
            int newImageWidth = imageAndOperationsData.colorReducedImage.Width;
            int newImageHeight = imageAndOperationsData.colorReducedImage.Height;
            //mainPictureBox.Image = ImageTransformations.RedimensionateImage(imageAndOperationsData.colorReducedImage, mainPictureBox.Width*10);
            mainPictureBox.Image = ImageTransformations.ResizeBitmap(imageAndOperationsData.colorReducedImage, mainPictureBox.Width*10);
        }

        private void saveImageButton_Click(object sender, EventArgs e)
        {
            if(imageAndOperationsData.colorReducedImage != null)
            {
                saveImageFileDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*.tif|All files|*.*";

                if (saveImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //imageAndOperationsData.colorReducedImage.Save(saveImageFileDialog.FileNames[0]);
                    ImageTransformations.RedimensionateImage(imageAndOperationsData.colorReducedImage, mainPictureBox.Width * 10).Save(saveImageFileDialog.FileNames[0]);
                    //if ((myStream = saveImageFileDialog.OpenFile()) != null)
                    //{
                    //    // Code to write the stream goes here.
                    //    myStream.Close();
                    //}
                }
            }
        }

        private void widthSizeTrackBar_Scroll(object sender, EventArgs e)
        {
            widthTrackBarLabel.Text = widthSizeTrackBar.Value.ToString();
        }

        private void numberOfColorsTrackBar_Scroll(object sender, EventArgs e)
        {
            numberOfColorsTrackBarLabel.Text = numberOfColorsTrackBar.Value.ToString();
        }
    }
}

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
        public Bitmap originalImage { get; private set; }
        public Bitmap pixelatedImage { get; private set; }
        public Bitmap colorReducedImage { get; private set; }

        public int newWidth = 100;
        public int numberOfColors = 10;

        public ImageAndOperationsData(Bitmap importedImage)
        {
            originalImage = new Bitmap(importedImage);
        }

        public void PixelateImage()
        {
            pixelatedImage = ImageTransformations.Pixelate(originalImage, newWidth);
        }

        public void ReduceNumberOfColors()
        {
            colorReducedImage = ImageTransformations.ReduceNumberOfColors(pixelatedImage, numberOfColors, 10);
        }


    }
}

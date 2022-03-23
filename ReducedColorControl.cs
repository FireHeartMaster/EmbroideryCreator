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
    public partial class ReducedColorControl : UserControl
    {
        public ReducedColorControl()
        {
            InitializeComponent();

            reducedColorPictureBox = pictureBoxColor;
        }

        public PictureBox reducedColorPictureBox { get; private set; }
        public int indexInTheList;
        public MainForm myReferenceToMainForm;

        private void pictureBoxColor_Click(object sender, EventArgs e)
        {
            if(newColorDialog.ShowDialog() == DialogResult.OK)
            {
                myReferenceToMainForm.UpdateColorByIndex(indexInTheList, newColorDialog.Color);
                this.reducedColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(newColorDialog.Color, 30, 30);
            }
        }
    }
}

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
    public partial class BackstitchColorControl : UserControl
    {
        public BackstitchColorControl()
        {
            InitializeComponent();

            backstitchColorPictureBox = threadColorPictureBox;
        }

        public PictureBox backstitchColorPictureBox { get; private set; }
        public int crossStitchColorIndex;
        public MainForm myReferenceToMainForm;
        public Color color;

        private void threadColorPictureBox_Click(object sender, EventArgs e)
        {
            newBackstitchColorDialog.Color = color;
            if (newBackstitchColorDialog.ShowDialog() == DialogResult.OK)
            {
                color = newBackstitchColorDialog.Color;
                this.backstitchColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(color, 40, 5);
                PaintNewBackstitchColor();
            }
        }

        private void PaintNewBackstitchColor() => myReferenceToMainForm.UpdateBackstitchColorByIndex(crossStitchColorIndex, color);

        public void InitializeBackstitchColorControl(Color newColor, int i,/* BackstitchColorControl colorControl, */MainForm referenceToMainForm)
        {
            backstitchColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(newColor, 40, 5);
            crossStitchColorIndex = i;
            color = newColor;
            myReferenceToMainForm = referenceToMainForm;
        }

        private void backstitchColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (backstitchColorCheckBox.Checked)
            {
                AddThisControlToListOfSelectedBackstitchColorControls();
            }
            else
            {
                RemoveThisControlFromListOfSelectedBackstitchColorControls();
            }
        }

        private void AddThisControlToListOfSelectedBackstitchColorControls()
        {
            myReferenceToMainForm.selectedBackstitchColorsControlsList.Add(this);
        }

        private void RemoveThisControlFromListOfSelectedBackstitchColorControls()
        {
            myReferenceToMainForm.selectedBackstitchColorsControlsList.Remove(this);
        }

        public void ModifyBackstitchSelectionCheckBox(bool state)
        {
            backstitchColorCheckBox.Checked = state;
        }
    }
}

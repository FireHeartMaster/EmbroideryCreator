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
        public int reducedColorIndex;
        public MainForm myReferenceToMainForm;
        public Color color;
        public bool isBackground = false;

        private void pictureBoxColor_Click(object sender, EventArgs e)
        {
            newColorDialog.Color = color;
            if (newColorDialog.ShowDialog() == DialogResult.OK)
            {
                color = newColorDialog.Color;
                this.reducedColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(color, 30, 30);
                PaintNewColor();
            }
        }

        private void PaintNewColor() => myReferenceToMainForm.UpdateReducedColorByIndex(reducedColorIndex, color);

        public void InitializeReducedColorControl(Color newColor, int i,/* ReducedColorControl colorControl, */MainForm referenceToMainForm)
        {
            reducedColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(newColor, 30, 30);
            reducedColorIndex = i;
            color = newColor;
            myReferenceToMainForm = referenceToMainForm;
        }

        private void selectionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (selectionCheckBox.Checked)
            {
                AddThisControlToListOfSelectedControls();
                if (!myReferenceToMainForm.CheckIfMultipleSelectionIsActive())
                {
                    myReferenceToMainForm.UncheckAllOtherCrossStitchColorControls(reducedColorIndex);
                }
            }
            else
            {
                RemoveThisControlFromListOfSelectedControls();
            }
        }

        private void AddThisControlToListOfSelectedControls()
        {
            myReferenceToMainForm.selectedColorsControlsList.Add(this);
        }

        private void RemoveThisControlFromListOfSelectedControls()
        {
            myReferenceToMainForm.selectedColorsControlsList.Remove(this);
        }

        public void ModifySelectionCheckBox(bool state)
        {
            selectionCheckBox.Checked = state;
        }

        private void IsBackgroundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            isBackground = IsBackgroundCheckBox.Checked;
            myReferenceToMainForm.UpdateIfShouldPaintReducedColorBackground(reducedColorIndex, isBackground);
        }
    }
}

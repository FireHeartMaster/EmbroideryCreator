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
        public int backstitchColorIndex;
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

        private void PaintNewBackstitchColor() => myReferenceToMainForm.UpdateBackstitchColorByIndex(backstitchColorIndex, color);

        public void InitializeBackstitchColorControl(Color newColor, int backstitchIndex,MainForm referenceToMainForm)
        {
            backstitchColorPictureBox.Image = ImageTransformations.CreateSolidColorBitmap(newColor, 40, 5);
            backstitchColorIndex = backstitchIndex;
            color = newColor;
            myReferenceToMainForm = referenceToMainForm;
        }

        private void backstitchColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (backstitchColorCheckBox.Checked)
            {
                AddThisControlToListOfSelectedBackstitchColorControls();
                if (!myReferenceToMainForm.CheckIfMultipleSelectionIsActive())
                {
                    myReferenceToMainForm.UncheckAllOtherBackstitchColorControls(backstitchColorIndex);
                }
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

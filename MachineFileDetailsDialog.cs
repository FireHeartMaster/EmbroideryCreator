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
    public partial class MachineFileDetailsDialog : Form
    {
        public int squareSize = 18; //in 0.1 milimeters
        public int numberOfRepetitionsPerStitch = 1;

        private List<RadioButton> allAidaCountRadioButtons;

        public MachineFileDetailsDialog()
        {
            InitializeComponent();
        }

        public MachineFileDetailsDialog(int squareSize = 18, int numberOfRepetitionsPerStitch = 1)
        {
            InitializeComponent();

            this.squareSize = squareSize;
            this.numberOfRepetitionsPerStitch = numberOfRepetitionsPerStitch;

            allAidaCountRadioButtons = new List<RadioButton>() { aidaCount11RadioButton, aidaCount12RadioButton,
                                                                    aidaCount13RadioButton, aidaCount14RadioButton,
                                                                    aidaCount16RadioButton, aidaCount18RadioButton, otherAidaCountRadioButton };

            squareSizeTextBox.Text = this.squareSize.ToString();
            numberOfRepetitionsPerStitchTrackBar.Value = this.numberOfRepetitionsPerStitch;
            valueOfNumberOfRepetitionsPerStitchLabel.Text = this.numberOfRepetitionsPerStitch.ToString();
        }

        private void squareSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(squareSizeTextBox.Text, out int parsedSquareSize))
            {
                this.squareSize = parsedSquareSize;
                if (uncheckAll)
                {
                    UncheckAllRadioButtons();
                }
            }
            else
            {
                squareSizeTextBox.Text = this.squareSize.ToString();
            }
        }

        private void UncheckAllRadioButtonsExceptOne(RadioButton radioButtonToNotUncheck)
        {
            foreach (RadioButton radioButton in allAidaCountRadioButtons)
            {
                if (radioButton != radioButtonToNotUncheck)
                {
                    radioButton.Checked = false;
                }
            }
        }

        private void UncheckAllRadioButtons()
        {
            foreach (RadioButton radioButton in allAidaCountRadioButtons)
            {
                radioButton.Checked = false;
            }
        }

        private void CheckingRadioButton(RadioButton radioButtonJustChecked)
        {
            UncheckAllRadioButtonsExceptOne(radioButtonJustChecked);

            int aidaCount = 14;

            if (radioButtonJustChecked == aidaCount11RadioButton)
            {
                aidaCount = 11;
            }
            else if (radioButtonJustChecked == aidaCount12RadioButton)
            {
                aidaCount = 12;
            }
            else if (radioButtonJustChecked == aidaCount13RadioButton)
            {
                aidaCount = 13;
            }
            else if (radioButtonJustChecked == aidaCount14RadioButton)
            {
                aidaCount = 14;
            }
            else if (radioButtonJustChecked == aidaCount16RadioButton)
            {
                aidaCount = 16;
            }
            else if (radioButtonJustChecked == aidaCount18RadioButton)
            {
                aidaCount = 18;
            }

            double squareSizeInInches = 1.0 / aidaCount;
            this.squareSize = (int)(squareSizeInInches * 254);

            uncheckAll = false;
            squareSizeTextBox.Text = this.squareSize.ToString();
            uncheckAll = true;
        }
        bool uncheckAll = true;

        private void aidaCount11RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckingRadioButtonIfBeingChecked(sender);
        }

        private void aidaCount12RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckingRadioButtonIfBeingChecked(sender);
        }

        private void aidaCount13RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckingRadioButtonIfBeingChecked(sender);
        }

        private void aidaCount14RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckingRadioButtonIfBeingChecked(sender);
        }

        private void aidaCount16RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckingRadioButtonIfBeingChecked(sender);
        }

        private void aidaCount18RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckingRadioButtonIfBeingChecked(sender);
        }

        private void CheckingRadioButtonIfBeingChecked(object sender)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked == true)
            {
                CheckingRadioButton(radioButton);
            }
        }

        private void numberOfRepetitionsPerStitchTrackBar_Scroll(object sender, EventArgs e)
        {
            this.numberOfRepetitionsPerStitch = numberOfRepetitionsPerStitchTrackBar.Value;
            valueOfNumberOfRepetitionsPerStitchLabel.Text = this.numberOfRepetitionsPerStitch.ToString();
        }

        private void otherAidaCountRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

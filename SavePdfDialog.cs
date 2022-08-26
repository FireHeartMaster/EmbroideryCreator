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
    public partial class SavePdfDialog : Form
    {
        public string collection;
        public string title;
        public string subtitle;
        public string alternativeTitle;

        public int collectionTextFormattingFactor = 7;
        public int titleFirstPageFormattingFactor = 7;
        public int subtitleFirstPageFormattingFactor = 7;

        public int collectionCharacterLengthToCheck = 9;
        public int titleCharacterLengthToCheck = 18;

        public SavePdfDialog()
        {
            InitializeComponent();
        }

        public SavePdfDialog(string collection, string title, string subtitle, string alternativeTitle,
            int collectionTextFormattingFactor = 7,
            int titleFirstPageFormattingFactor = 7,
            int subtitleFirstPageFormattingFactor = 7,
            int collectionCharacterLengthToCheck = 9,
            int titleCharacterLengthToCheck = 18)
        {
            InitializeComponent();

            collectionTextBox.Text = collection;
            titleTextBox.Text = title;
            subtitleTextBox.Text = subtitle;
            alternativeTitleTextBox.Text = alternativeTitle;

            this.collection = collection;
            this.title = title;
            this.subtitle = subtitle;
            this.alternativeTitle = alternativeTitle;

            this.collectionTextFormattingFactor = collectionTextFormattingFactor;
            this.titleFirstPageFormattingFactor = titleFirstPageFormattingFactor;
            this.subtitleFirstPageFormattingFactor = subtitleFirstPageFormattingFactor;
            this.collectionCharacterLengthToCheck = collectionCharacterLengthToCheck;
            this.titleCharacterLengthToCheck = titleCharacterLengthToCheck;

            collectionFactorTrackBar.Value = collectionTextFormattingFactor;
            titleFactorTrackBar.Value = titleFirstPageFormattingFactor;
            subtitleFactorTrackBar.Value = subtitleFirstPageFormattingFactor;
            collectionLengthTrackBar.Value = collectionCharacterLengthToCheck;
            titleLengthTrackBar.Value = titleCharacterLengthToCheck;

            collectionFactorValueLabel.Text = collectionTextFormattingFactor.ToString();
            titleFactorValueLabel.Text = titleFirstPageFormattingFactor.ToString();
            subtitleFactorValueLabel.Text = subtitleFirstPageFormattingFactor.ToString();
            collectionLengthValueLabel.Text = collectionCharacterLengthToCheck.ToString();
            titleLengthValueLabel.Text = titleCharacterLengthToCheck.ToString();
        }

        private void collectionTextBox_TextChanged(object sender, EventArgs e)
        {
            collection = collectionTextBox.Text;
        }

        private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            title = titleTextBox.Text;
        }

        private void subtitleTextBox_TextChanged(object sender, EventArgs e)
        {
            subtitle = subtitleTextBox.Text;
        }

        private void alternativeTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            alternativeTitle = alternativeTitleTextBox.Text;
        }

        private void showMoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            int newWidth;

            if (showMoreCheckBox.Checked)
            {
                newWidth = 700;
            }
            else
            {
                newWidth = 350;
            }

            this.Size = new Size(newWidth, this.Size.Height);
        }

        private void collectionFactorTrackBar_Scroll(object sender, EventArgs e)
        {
            collectionTextFormattingFactor = collectionFactorTrackBar.Value;
            collectionFactorValueLabel.Text = collectionFactorTrackBar.Value.ToString();
        }

        private void titleFactorTrackBar_Scroll(object sender, EventArgs e)
        {
            titleFirstPageFormattingFactor = titleFactorTrackBar.Value;
            titleFactorValueLabel.Text = titleFactorTrackBar.Value.ToString();
        }

        private void subtitleFactorTrackBar_Scroll(object sender, EventArgs e)
        {
            subtitleFirstPageFormattingFactor = subtitleFactorTrackBar.Value;
            subtitleFactorValueLabel.Text = subtitleFactorTrackBar.Value.ToString();

        }

        private void collectionLengthTrackBar_Scroll(object sender, EventArgs e)
        {
            collectionCharacterLengthToCheck = collectionLengthTrackBar.Value;
            collectionLengthValueLabel.Text = collectionLengthTrackBar.Value.ToString();
        }

        private void titleLengthTrackBar_Scroll(object sender, EventArgs e)
        {
            titleCharacterLengthToCheck = titleLengthTrackBar.Value;
            titleLengthValueLabel.Text = titleLengthTrackBar.Value.ToString();
        }
    }
}

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

        public SavePdfDialog()
        {
            InitializeComponent();
        }

        public SavePdfDialog(string collection, string title, string subtitle, string alternativeTitle)
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
    }
}

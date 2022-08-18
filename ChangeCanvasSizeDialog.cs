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
    public partial class ChangeCanvasSizeDialog : Form
    {
        public int newWidth;
        public int newHeight;

        public ChangeCanvasSizeDialog()
        {
            InitializeComponent();
        }

        public ChangeCanvasSizeDialog(int width, int height)
        {
            InitializeComponent();

            this.newWidth = width;
            this.newHeight = height;

            widthTextBox.Text = width.ToString();
            heightTextBox.Text = height.ToString();
        }

        private void widthTextBox_TextChanged(object sender, EventArgs e)
        {
            if(int.TryParse(widthTextBox.Text, out int parsedWidth))
            {
                newWidth = parsedWidth;
            }
            else
            {
                widthTextBox.Text = newWidth.ToString();
            }
        }

        private void heightTextBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(heightTextBox.Text, out int parsedHeight))
            {
                newHeight = parsedHeight;
            }
            else
            {
                heightTextBox.Text = newHeight.ToString();
            }
        }
    }
}

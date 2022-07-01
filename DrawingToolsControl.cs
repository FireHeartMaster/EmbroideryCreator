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
    public partial class DrawingToolsControl : UserControl
    {
        public DrawingToolInUse currentDrawingTool { get; private set; } = DrawingToolInUse.Pencil;

        PictureBox currentActiveToolPictureBox;

        public DrawingToolsControl()
        {
            InitializeComponent();
            //currentActiveToolPictureBox = pencilPictureBox;
            EnableNewTool(DrawingToolInUse.Pencil);
        }

        private void pencilPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Pencil);
        }

        private void bucketPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Bucket);
        }

        private void EnableNewTool(DrawingToolInUse newSelectedDrawingTool)
        {
            //ResetCurrentToolsImage();
            switch (newSelectedDrawingTool)
            {
                case DrawingToolInUse.Pencil:
                    currentDrawingTool = DrawingToolInUse.Pencil;
                    currentActiveToolPictureBox = pencilPictureBox;
                    break;

                case DrawingToolInUse.Bucket:
                    currentDrawingTool = DrawingToolInUse.Bucket;
                    currentActiveToolPictureBox = bucketPictureBox;
                    break;

                default:
                    break;
            }
            SetActiveCurrentToolImage();
        }

        //private void ResetCurrentToolsImage()
        //{
        //    currentActiveToolPictureBox.BackColor = unSelectedToolBorderColor;
        //    currentActiveToolPictureBox.Padding = new Padding(unSelectedToolBorderSize);
        //    currentActiveToolPictureBox.Invalidate();
        //}

        private void SetActiveCurrentToolImage()
        {
            //currentActiveToolPictureBox.BackColor = selectedToolBorderColor;
            //currentActiveToolPictureBox.Padding = new Padding(selectedToolBorderSize);
            //currentActiveToolPictureBox.Invalidate();
            selectedToolpictureBox.Location = new Point((int)(currentActiveToolPictureBox.Location.X - (selectedToolpictureBox.Size.Width - currentActiveToolPictureBox.Size.Width) * 0.5),
                                                        (int)(currentActiveToolPictureBox.Location.Y - (selectedToolpictureBox.Size.Height - currentActiveToolPictureBox.Size.Height) * 0.5));
        }
    }

    public enum DrawingToolInUse
    {
        Pencil,
        Bucket
    }
}

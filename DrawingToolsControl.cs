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
        public SelectionToolData selectionToolData = new SelectionToolData();
        public SelectionToolState currentSelectionToolState = SelectionToolState.NothingSelected;

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

        private void eraserPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Eraser);
        }

        private void movePictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.Move);
        }

        private void colorPickerPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.ColorPicker);
        }

        private void selectionToolPictureBox_Click(object sender, EventArgs e)
        {
            EnableNewTool(DrawingToolInUse.SelectionTool);
        }

        public void EnableNewTool(DrawingToolInUse newSelectedDrawingTool)
        {
            DisableOtherTools(newSelectedDrawingTool);

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

                case DrawingToolInUse.Eraser:
                    currentDrawingTool = DrawingToolInUse.Eraser;
                    currentActiveToolPictureBox = eraserPictureBox;
                    break;
                case DrawingToolInUse.Move:
                    currentDrawingTool = DrawingToolInUse.Move;
                    currentActiveToolPictureBox = movePictureBox;
                    break;
                case DrawingToolInUse.ColorPicker:
                    currentDrawingTool = DrawingToolInUse.ColorPicker;
                    currentActiveToolPictureBox = colorPickerPictureBox;
                    break;
                case DrawingToolInUse.SelectionTool:
                    currentDrawingTool = DrawingToolInUse.SelectionTool;
                    currentActiveToolPictureBox = selectionToolPictureBox;
                    break;

                default:
                    break;
            }
            SetActiveCurrentToolImage();
        }

        private void DisableOtherTools(DrawingToolInUse drawingToolBeingActivated)
        {

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
        Bucket,
        Eraser,
        Move, 
        ColorPicker, 
        SelectionTool
    }

    public enum SelectionToolState
    {
        NothingSelected,
        Selecting,
        Moving,
        Selected
    }
}


namespace EmbroideryCreator
{
    partial class DrawingToolsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.eraserPictureBox = new System.Windows.Forms.PictureBox();
            this.bucketPictureBox = new System.Windows.Forms.PictureBox();
            this.pencilPictureBox = new System.Windows.Forms.PictureBox();
            this.selectedToolpictureBox = new System.Windows.Forms.PictureBox();
            this.movePictureBox = new System.Windows.Forms.PictureBox();
            this.colorPickerPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.eraserPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bucketPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pencilPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedToolpictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.movePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // eraserPictureBox
            // 
            this.eraserPictureBox.Image = global::EmbroideryCreator.Properties.Resources.EraserIcon;
            this.eraserPictureBox.Location = new System.Drawing.Point(93, 6);
            this.eraserPictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.eraserPictureBox.Name = "eraserPictureBox";
            this.eraserPictureBox.Size = new System.Drawing.Size(30, 30);
            this.eraserPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.eraserPictureBox.TabIndex = 3;
            this.eraserPictureBox.TabStop = false;
            this.eraserPictureBox.Click += new System.EventHandler(this.eraserPictureBox_Click);
            // 
            // bucketPictureBox
            // 
            this.bucketPictureBox.Image = global::EmbroideryCreator.Properties.Resources.BucketIcon;
            this.bucketPictureBox.Location = new System.Drawing.Point(51, 6);
            this.bucketPictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.bucketPictureBox.Name = "bucketPictureBox";
            this.bucketPictureBox.Size = new System.Drawing.Size(30, 30);
            this.bucketPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bucketPictureBox.TabIndex = 1;
            this.bucketPictureBox.TabStop = false;
            this.bucketPictureBox.Click += new System.EventHandler(this.bucketPictureBox_Click);
            // 
            // pencilPictureBox
            // 
            this.pencilPictureBox.Image = global::EmbroideryCreator.Properties.Resources.PencilIcon;
            this.pencilPictureBox.Location = new System.Drawing.Point(9, 6);
            this.pencilPictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.pencilPictureBox.Name = "pencilPictureBox";
            this.pencilPictureBox.Size = new System.Drawing.Size(30, 30);
            this.pencilPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pencilPictureBox.TabIndex = 0;
            this.pencilPictureBox.TabStop = false;
            this.pencilPictureBox.Click += new System.EventHandler(this.pencilPictureBox_Click);
            // 
            // selectedToolpictureBox
            // 
            this.selectedToolpictureBox.Image = global::EmbroideryCreator.Properties.Resources.SelectedToolIcon;
            this.selectedToolpictureBox.Location = new System.Drawing.Point(4, 1);
            this.selectedToolpictureBox.Name = "selectedToolpictureBox";
            this.selectedToolpictureBox.Size = new System.Drawing.Size(40, 40);
            this.selectedToolpictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.selectedToolpictureBox.TabIndex = 2;
            this.selectedToolpictureBox.TabStop = false;
            // 
            // movePictureBox
            // 
            this.movePictureBox.Image = global::EmbroideryCreator.Properties.Resources.MoveIcon;
            this.movePictureBox.Location = new System.Drawing.Point(135, 6);
            this.movePictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.movePictureBox.Name = "movePictureBox";
            this.movePictureBox.Size = new System.Drawing.Size(30, 30);
            this.movePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.movePictureBox.TabIndex = 4;
            this.movePictureBox.TabStop = false;
            this.movePictureBox.Click += new System.EventHandler(this.movePictureBox_Click);
            // 
            // colorPickerPictureBox
            // 
            this.colorPickerPictureBox.Image = global::EmbroideryCreator.Properties.Resources.ColorPickerIcon;
            this.colorPickerPictureBox.Location = new System.Drawing.Point(176, 6);
            this.colorPickerPictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.colorPickerPictureBox.Name = "colorPickerPictureBox";
            this.colorPickerPictureBox.Size = new System.Drawing.Size(30, 30);
            this.colorPickerPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.colorPickerPictureBox.TabIndex = 5;
            this.colorPickerPictureBox.TabStop = false;
            this.colorPickerPictureBox.Click += new System.EventHandler(this.colorPickerPictureBox_Click);
            // 
            // DrawingToolsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.colorPickerPictureBox);
            this.Controls.Add(this.movePictureBox);
            this.Controls.Add(this.eraserPictureBox);
            this.Controls.Add(this.bucketPictureBox);
            this.Controls.Add(this.pencilPictureBox);
            this.Controls.Add(this.selectedToolpictureBox);
            this.Name = "DrawingToolsControl";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(215, 45);
            ((System.ComponentModel.ISupportInitialize)(this.eraserPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bucketPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pencilPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedToolpictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.movePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPickerPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pencilPictureBox;
        private System.Windows.Forms.PictureBox bucketPictureBox;
        private System.Windows.Forms.PictureBox selectedToolpictureBox;
        private System.Windows.Forms.PictureBox eraserPictureBox;
        private System.Windows.Forms.PictureBox movePictureBox;
        private System.Windows.Forms.PictureBox colorPickerPictureBox;
    }
}

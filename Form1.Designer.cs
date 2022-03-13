
namespace EmbroideryCreator
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.widthSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.numberOfColorsTrackBar = new System.Windows.Forms.TrackBar();
            this.chooseNewImageButton = new System.Windows.Forms.Button();
            this.processImageButton = new System.Windows.Forms.Button();
            this.widthSizeLabel = new System.Windows.Forms.Label();
            this.numberOfColorsLabel = new System.Windows.Forms.Label();
            this.openNewImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveImageButton = new System.Windows.Forms.Button();
            this.saveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.numberOfColorsTrackBarLabel = new System.Windows.Forms.Label();
            this.widthTrackBarLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfColorsTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Image = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.mainPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.mainPictureBox.Location = new System.Drawing.Point(301, 12);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(529, 529);
            this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            this.mainPictureBox.Click += new System.EventHandler(this.mainPictureBox_Click);
            // 
            // widthSizeTrackBar
            // 
            this.widthSizeTrackBar.Location = new System.Drawing.Point(12, 119);
            this.widthSizeTrackBar.Maximum = 300;
            this.widthSizeTrackBar.Minimum = 20;
            this.widthSizeTrackBar.Name = "widthSizeTrackBar";
            this.widthSizeTrackBar.Size = new System.Drawing.Size(283, 45);
            this.widthSizeTrackBar.TabIndex = 1;
            this.widthSizeTrackBar.Value = 100;
            this.widthSizeTrackBar.Scroll += new System.EventHandler(this.widthSizeTrackBar_Scroll);
            // 
            // numberOfColorsTrackBar
            // 
            this.numberOfColorsTrackBar.Location = new System.Drawing.Point(12, 220);
            this.numberOfColorsTrackBar.Maximum = 50;
            this.numberOfColorsTrackBar.Minimum = 2;
            this.numberOfColorsTrackBar.Name = "numberOfColorsTrackBar";
            this.numberOfColorsTrackBar.Size = new System.Drawing.Size(283, 45);
            this.numberOfColorsTrackBar.TabIndex = 2;
            this.numberOfColorsTrackBar.Value = 10;
            this.numberOfColorsTrackBar.Scroll += new System.EventHandler(this.numberOfColorsTrackBar_Scroll);
            // 
            // chooseNewImageButton
            // 
            this.chooseNewImageButton.Location = new System.Drawing.Point(13, 518);
            this.chooseNewImageButton.Name = "chooseNewImageButton";
            this.chooseNewImageButton.Size = new System.Drawing.Size(139, 23);
            this.chooseNewImageButton.TabIndex = 3;
            this.chooseNewImageButton.Text = "Choose New Image";
            this.chooseNewImageButton.UseVisualStyleBackColor = true;
            this.chooseNewImageButton.Click += new System.EventHandler(this.chooseNewImageButton_Click);
            // 
            // processImageButton
            // 
            this.processImageButton.Location = new System.Drawing.Point(158, 518);
            this.processImageButton.Name = "processImageButton";
            this.processImageButton.Size = new System.Drawing.Size(137, 23);
            this.processImageButton.TabIndex = 4;
            this.processImageButton.Text = "Process Image";
            this.processImageButton.UseVisualStyleBackColor = true;
            this.processImageButton.Click += new System.EventHandler(this.processImageButton_Click);
            // 
            // widthSizeLabel
            // 
            this.widthSizeLabel.AutoSize = true;
            this.widthSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.widthSizeLabel.Location = new System.Drawing.Point(12, 85);
            this.widthSizeLabel.Name = "widthSizeLabel";
            this.widthSizeLabel.Size = new System.Drawing.Size(113, 25);
            this.widthSizeLabel.TabIndex = 5;
            this.widthSizeLabel.Text = "New Width:";
            // 
            // numberOfColorsLabel
            // 
            this.numberOfColorsLabel.AutoSize = true;
            this.numberOfColorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfColorsLabel.Location = new System.Drawing.Point(12, 192);
            this.numberOfColorsLabel.Name = "numberOfColorsLabel";
            this.numberOfColorsLabel.Size = new System.Drawing.Size(175, 25);
            this.numberOfColorsLabel.TabIndex = 6;
            this.numberOfColorsLabel.Text = "Number Of Colors:";
            // 
            // openNewImageFileDialog
            // 
            this.openNewImageFileDialog.FileName = "openNewImageFileDialog";
            // 
            // saveImageButton
            // 
            this.saveImageButton.Location = new System.Drawing.Point(158, 489);
            this.saveImageButton.Name = "saveImageButton";
            this.saveImageButton.Size = new System.Drawing.Size(137, 23);
            this.saveImageButton.TabIndex = 7;
            this.saveImageButton.Text = "Save Image";
            this.saveImageButton.UseVisualStyleBackColor = true;
            this.saveImageButton.Click += new System.EventHandler(this.saveImageButton_Click);
            // 
            // numberOfColorsTrackBarLabel
            // 
            this.numberOfColorsTrackBarLabel.AutoSize = true;
            this.numberOfColorsTrackBarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfColorsTrackBarLabel.Location = new System.Drawing.Point(193, 192);
            this.numberOfColorsTrackBarLabel.Name = "numberOfColorsTrackBarLabel";
            this.numberOfColorsTrackBarLabel.Size = new System.Drawing.Size(34, 25);
            this.numberOfColorsTrackBarLabel.TabIndex = 8;
            this.numberOfColorsTrackBarLabel.Text = "10";
            // 
            // widthTrackBarLabel
            // 
            this.widthTrackBarLabel.AutoSize = true;
            this.widthTrackBarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.widthTrackBarLabel.Location = new System.Drawing.Point(131, 85);
            this.widthTrackBarLabel.Name = "widthTrackBarLabel";
            this.widthTrackBarLabel.Size = new System.Drawing.Size(45, 25);
            this.widthTrackBarLabel.TabIndex = 9;
            this.widthTrackBarLabel.Text = "100";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 553);
            this.Controls.Add(this.widthTrackBarLabel);
            this.Controls.Add(this.numberOfColorsTrackBarLabel);
            this.Controls.Add(this.saveImageButton);
            this.Controls.Add(this.numberOfColorsLabel);
            this.Controls.Add(this.widthSizeLabel);
            this.Controls.Add(this.processImageButton);
            this.Controls.Add(this.chooseNewImageButton);
            this.Controls.Add(this.numberOfColorsTrackBar);
            this.Controls.Add(this.widthSizeTrackBar);
            this.Controls.Add(this.mainPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "EmbroideryCreator";
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfColorsTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.TrackBar widthSizeTrackBar;
        private System.Windows.Forms.TrackBar numberOfColorsTrackBar;
        private System.Windows.Forms.Button chooseNewImageButton;
        private System.Windows.Forms.Button processImageButton;
        private System.Windows.Forms.Label widthSizeLabel;
        private System.Windows.Forms.Label numberOfColorsLabel;
        private System.Windows.Forms.OpenFileDialog openNewImageFileDialog;
        private System.Windows.Forms.Button saveImageButton;
        private System.Windows.Forms.SaveFileDialog saveImageFileDialog;
        private System.Windows.Forms.Label numberOfColorsTrackBarLabel;
        private System.Windows.Forms.Label widthTrackBarLabel;
    }
}


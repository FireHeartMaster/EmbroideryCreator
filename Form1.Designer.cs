
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
            this.numberOfIterationsTrackBar = new System.Windows.Forms.TrackBar();
            this.numberOfIterationsLabel = new System.Windows.Forms.Label();
            this.numberOfIterationsTrackBarLabel = new System.Windows.Forms.Label();
            this.ProcessAtAllChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelListOfColors = new System.Windows.Forms.FlowLayoutPanel();
            this.ListOfColorsLabel = new System.Windows.Forms.Label();
            this.mergeColorsButton = new System.Windows.Forms.Button();
            this.addColorButton = new System.Windows.Forms.Button();
            this.addColorDialog = new System.Windows.Forms.ColorDialog();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.drawingToolsControl = new EmbroideryCreator.DrawingToolsControl();
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfColorsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfIterationsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // widthSizeTrackBar
            // 
            this.widthSizeTrackBar.Location = new System.Drawing.Point(12, 119);
            this.widthSizeTrackBar.Maximum = 300;
            this.widthSizeTrackBar.Minimum = 2;
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
            // numberOfIterationsTrackBar
            // 
            this.numberOfIterationsTrackBar.Location = new System.Drawing.Point(12, 312);
            this.numberOfIterationsTrackBar.Maximum = 100;
            this.numberOfIterationsTrackBar.Name = "numberOfIterationsTrackBar";
            this.numberOfIterationsTrackBar.Size = new System.Drawing.Size(283, 45);
            this.numberOfIterationsTrackBar.TabIndex = 10;
            this.numberOfIterationsTrackBar.Value = 10;
            this.numberOfIterationsTrackBar.Scroll += new System.EventHandler(this.numberOfIterationsTrackBar_Scroll);
            // 
            // numberOfIterationsLabel
            // 
            this.numberOfIterationsLabel.AutoSize = true;
            this.numberOfIterationsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfIterationsLabel.Location = new System.Drawing.Point(12, 284);
            this.numberOfIterationsLabel.Name = "numberOfIterationsLabel";
            this.numberOfIterationsLabel.Size = new System.Drawing.Size(192, 25);
            this.numberOfIterationsLabel.TabIndex = 11;
            this.numberOfIterationsLabel.Text = "Number of Iterations:";
            // 
            // numberOfIterationsTrackBarLabel
            // 
            this.numberOfIterationsTrackBarLabel.AutoSize = true;
            this.numberOfIterationsTrackBarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfIterationsTrackBarLabel.Location = new System.Drawing.Point(210, 284);
            this.numberOfIterationsTrackBarLabel.Name = "numberOfIterationsTrackBarLabel";
            this.numberOfIterationsTrackBarLabel.Size = new System.Drawing.Size(34, 25);
            this.numberOfIterationsTrackBarLabel.TabIndex = 12;
            this.numberOfIterationsTrackBarLabel.Text = "10";
            // 
            // ProcessAtAllChangesCheckBox
            // 
            this.ProcessAtAllChangesCheckBox.AutoSize = true;
            this.ProcessAtAllChangesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcessAtAllChangesCheckBox.Location = new System.Drawing.Point(12, 372);
            this.ProcessAtAllChangesCheckBox.Name = "ProcessAtAllChangesCheckBox";
            this.ProcessAtAllChangesCheckBox.Size = new System.Drawing.Size(212, 21);
            this.ProcessAtAllChangesCheckBox.TabIndex = 13;
            this.ProcessAtAllChangesCheckBox.Text = "Process image at all changes";
            this.ProcessAtAllChangesCheckBox.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelListOfColors
            // 
            this.flowLayoutPanelListOfColors.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.flowLayoutPanelListOfColors.Location = new System.Drawing.Point(847, 85);
            this.flowLayoutPanelListOfColors.Name = "flowLayoutPanelListOfColors";
            this.flowLayoutPanelListOfColors.Size = new System.Drawing.Size(98, 398);
            this.flowLayoutPanelListOfColors.TabIndex = 14;
            // 
            // ListOfColorsLabel
            // 
            this.ListOfColorsLabel.AutoSize = true;
            this.ListOfColorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListOfColorsLabel.Location = new System.Drawing.Point(842, 59);
            this.ListOfColorsLabel.Name = "ListOfColorsLabel";
            this.ListOfColorsLabel.Size = new System.Drawing.Size(75, 25);
            this.ListOfColorsLabel.TabIndex = 15;
            this.ListOfColorsLabel.Text = "Colors:";
            // 
            // mergeColorsButton
            // 
            this.mergeColorsButton.Location = new System.Drawing.Point(848, 489);
            this.mergeColorsButton.Name = "mergeColorsButton";
            this.mergeColorsButton.Size = new System.Drawing.Size(98, 23);
            this.mergeColorsButton.TabIndex = 16;
            this.mergeColorsButton.Text = "Merge colors";
            this.mergeColorsButton.UseVisualStyleBackColor = true;
            this.mergeColorsButton.Click += new System.EventHandler(this.mergeColorsButton_Click);
            // 
            // addColorButton
            // 
            this.addColorButton.Location = new System.Drawing.Point(848, 518);
            this.addColorButton.Name = "addColorButton";
            this.addColorButton.Size = new System.Drawing.Size(98, 23);
            this.addColorButton.TabIndex = 17;
            this.addColorButton.Text = "Add color";
            this.addColorButton.UseVisualStyleBackColor = true;
            this.addColorButton.Click += new System.EventHandler(this.addColorButton_Click);
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
            this.mainPictureBox.DoubleClick += new System.EventHandler(this.mainPictureBox_DoubleClick);
            this.mainPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseDown);
            this.mainPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseMove);
            this.mainPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseUp);
            // 
            // drawingToolsControl
            // 
            this.drawingToolsControl.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.drawingToolsControl.Location = new System.Drawing.Point(848, 13);
            this.drawingToolsControl.Name = "drawingToolsControl";
            this.drawingToolsControl.Padding = new System.Windows.Forms.Padding(3);
            this.drawingToolsControl.Size = new System.Drawing.Size(89, 45);
            this.drawingToolsControl.TabIndex = 18;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 553);
            this.Controls.Add(this.drawingToolsControl);
            this.Controls.Add(this.addColorButton);
            this.Controls.Add(this.mergeColorsButton);
            this.Controls.Add(this.ListOfColorsLabel);
            this.Controls.Add(this.flowLayoutPanelListOfColors);
            this.Controls.Add(this.ProcessAtAllChangesCheckBox);
            this.Controls.Add(this.numberOfIterationsTrackBarLabel);
            this.Controls.Add(this.numberOfIterationsLabel);
            this.Controls.Add(this.numberOfIterationsTrackBar);
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
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfColorsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfIterationsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
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
        private System.Windows.Forms.TrackBar numberOfIterationsTrackBar;
        private System.Windows.Forms.Label numberOfIterationsLabel;
        private System.Windows.Forms.Label numberOfIterationsTrackBarLabel;
        private System.Windows.Forms.CheckBox ProcessAtAllChangesCheckBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelListOfColors;
        private System.Windows.Forms.Label ListOfColorsLabel;
        private System.Windows.Forms.Button mergeColorsButton;
        private System.Windows.Forms.Button addColorButton;
        private System.Windows.Forms.ColorDialog addColorDialog;
        private DrawingToolsControl drawingToolsControl;
    }
}



namespace EmbroideryCreator
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.widthSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.NumberOfColorsTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfColorsTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(301, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(529, 529);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // widthSizeTrackBar
            // 
            this.widthSizeTrackBar.Location = new System.Drawing.Point(12, 119);
            this.widthSizeTrackBar.Name = "widthSizeTrackBar";
            this.widthSizeTrackBar.Size = new System.Drawing.Size(283, 45);
            this.widthSizeTrackBar.TabIndex = 1;
            this.widthSizeTrackBar.Scroll += new System.EventHandler(this.widthSizeTrackBar_Scroll);
            // 
            // NumberOfColorsTrackBar
            // 
            this.NumberOfColorsTrackBar.Location = new System.Drawing.Point(12, 220);
            this.NumberOfColorsTrackBar.Name = "NumberOfColorsTrackBar";
            this.NumberOfColorsTrackBar.Size = new System.Drawing.Size(283, 45);
            this.NumberOfColorsTrackBar.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 553);
            this.Controls.Add(this.NumberOfColorsTrackBar);
            this.Controls.Add(this.widthSizeTrackBar);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "EmbroideryCreator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfColorsTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar widthSizeTrackBar;
        private System.Windows.Forms.TrackBar NumberOfColorsTrackBar;
    }
}


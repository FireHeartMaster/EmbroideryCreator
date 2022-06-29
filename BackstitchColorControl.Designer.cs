
namespace EmbroideryCreator
{
    partial class BackstitchColorControl
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
            this.backstitchColorCheckBox = new System.Windows.Forms.CheckBox();
            this.threadColorPictureBox = new System.Windows.Forms.PictureBox();
            this.newBackstitchColorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.threadColorPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // backstitchColorCheckBox
            // 
            this.backstitchColorCheckBox.AutoSize = true;
            this.backstitchColorCheckBox.Location = new System.Drawing.Point(13, 20);
            this.backstitchColorCheckBox.Name = "backstitchColorCheckBox";
            this.backstitchColorCheckBox.Size = new System.Drawing.Size(15, 14);
            this.backstitchColorCheckBox.TabIndex = 0;
            this.backstitchColorCheckBox.UseVisualStyleBackColor = true;
            this.backstitchColorCheckBox.CheckedChanged += new System.EventHandler(this.backstitchColorCheckBox_CheckedChanged);
            // 
            // threadColorPictureBox
            // 
            this.threadColorPictureBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.threadColorPictureBox.Location = new System.Drawing.Point(33, 24);
            this.threadColorPictureBox.Name = "threadColorPictureBox";
            this.threadColorPictureBox.Size = new System.Drawing.Size(40, 5);
            this.threadColorPictureBox.TabIndex = 1;
            this.threadColorPictureBox.TabStop = false;
            this.threadColorPictureBox.Click += new System.EventHandler(this.threadColorPictureBox_Click);
            // 
            // BackstitchColorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.threadColorPictureBox);
            this.Controls.Add(this.backstitchColorCheckBox);
            this.Name = "BackstitchColorControl";
            this.Size = new System.Drawing.Size(85, 52);
            ((System.ComponentModel.ISupportInitialize)(this.threadColorPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox backstitchColorCheckBox;
        private System.Windows.Forms.PictureBox threadColorPictureBox;
        private System.Windows.Forms.ColorDialog newBackstitchColorDialog;
    }
}

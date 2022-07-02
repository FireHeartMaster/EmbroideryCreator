
namespace EmbroideryCreator
{
    partial class ReducedColorControl
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
            this.newColorDialog = new System.Windows.Forms.ColorDialog();
            this.pictureBoxColor = new System.Windows.Forms.PictureBox();
            this.selectionCheckBox = new System.Windows.Forms.CheckBox();
            this.IsBackgroundCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxColor
            // 
            this.pictureBoxColor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxColor.Location = new System.Drawing.Point(33, 10);
            this.pictureBoxColor.Margin = new System.Windows.Forms.Padding(10);
            this.pictureBoxColor.Name = "pictureBoxColor";
            this.pictureBoxColor.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxColor.TabIndex = 0;
            this.pictureBoxColor.TabStop = false;
            this.pictureBoxColor.Click += new System.EventHandler(this.pictureBoxColor_Click);
            // 
            // selectionCheckBox
            // 
            this.selectionCheckBox.AutoSize = true;
            this.selectionCheckBox.Location = new System.Drawing.Point(10, 18);
            this.selectionCheckBox.Name = "selectionCheckBox";
            this.selectionCheckBox.Size = new System.Drawing.Size(15, 14);
            this.selectionCheckBox.TabIndex = 1;
            this.selectionCheckBox.UseVisualStyleBackColor = true;
            this.selectionCheckBox.CheckedChanged += new System.EventHandler(this.selectionCheckBox_CheckedChanged);
            // 
            // IsBackgroundCheckBox
            // 
            this.IsBackgroundCheckBox.AutoSize = true;
            this.IsBackgroundCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IsBackgroundCheckBox.Location = new System.Drawing.Point(43, 41);
            this.IsBackgroundCheckBox.Name = "IsBackgroundCheckBox";
            this.IsBackgroundCheckBox.Size = new System.Drawing.Size(96, 17);
            this.IsBackgroundCheckBox.TabIndex = 2;
            this.IsBackgroundCheckBox.Text = "is background:";
            this.IsBackgroundCheckBox.UseVisualStyleBackColor = true;
            this.IsBackgroundCheckBox.CheckedChanged += new System.EventHandler(this.IsBackgroundCheckBox_CheckedChanged);
            // 
            // ReducedColorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.IsBackgroundCheckBox);
            this.Controls.Add(this.selectionCheckBox);
            this.Controls.Add(this.pictureBoxColor);
            this.Name = "ReducedColorControl";
            this.Size = new System.Drawing.Size(142, 58);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog newColorDialog;
        private System.Windows.Forms.PictureBox pictureBoxColor;
        private System.Windows.Forms.CheckBox selectionCheckBox;
        private System.Windows.Forms.CheckBox IsBackgroundCheckBox;
    }
}

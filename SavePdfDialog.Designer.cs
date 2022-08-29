
namespace EmbroideryCreator
{
    partial class SavePdfDialog
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
            this.collectionTextBox = new System.Windows.Forms.TextBox();
            this.collectionLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.subtitleTextBox = new System.Windows.Forms.TextBox();
            this.confirmButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.alternativeTitleLabel = new System.Windows.Forms.Label();
            this.alternativeTitleTextBox = new System.Windows.Forms.TextBox();
            this.showMoreCheckBox = new System.Windows.Forms.CheckBox();
            this.formattingLabel = new System.Windows.Forms.Label();
            this.collectionFactorLabel = new System.Windows.Forms.Label();
            this.titleFactorLabel = new System.Windows.Forms.Label();
            this.subtitleFactorLabel = new System.Windows.Forms.Label();
            this.collectionFactorTrackBar = new System.Windows.Forms.TrackBar();
            this.collectionLengthTrackBar = new System.Windows.Forms.TrackBar();
            this.titleFactorTrackBar = new System.Windows.Forms.TrackBar();
            this.subtitleFactorTrackBar = new System.Windows.Forms.TrackBar();
            this.titleLengthTrackBar = new System.Windows.Forms.TrackBar();
            this.collectionFactorValueLabel = new System.Windows.Forms.Label();
            this.titleFactorValueLabel = new System.Windows.Forms.Label();
            this.subtitleFactorValueLabel = new System.Windows.Forms.Label();
            this.collectionLengthValueLabel = new System.Windows.Forms.Label();
            this.titleLengthValueLabel = new System.Windows.Forms.Label();
            this.convertColorsLabel = new System.Windows.Forms.Label();
            this.dmcConvertColorRadioButton = new System.Windows.Forms.RadioButton();
            this.anchorConvertColorRadioButton = new System.Windows.Forms.RadioButton();
            this.noConvertColorRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.collectionFactorTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionLengthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleFactorTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subtitleFactorTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleLengthTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // collectionTextBox
            // 
            this.collectionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.collectionTextBox.Location = new System.Drawing.Point(143, 16);
            this.collectionTextBox.Name = "collectionTextBox";
            this.collectionTextBox.Size = new System.Drawing.Size(165, 26);
            this.collectionTextBox.TabIndex = 6;
            this.collectionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.collectionTextBox.TextChanged += new System.EventHandler(this.collectionTextBox_TextChanged);
            // 
            // collectionLabel
            // 
            this.collectionLabel.AutoSize = true;
            this.collectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.collectionLabel.Location = new System.Drawing.Point(12, 19);
            this.collectionLabel.Name = "collectionLabel";
            this.collectionLabel.Size = new System.Drawing.Size(86, 20);
            this.collectionLabel.TabIndex = 6;
            this.collectionLabel.Text = "Collection: ";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(12, 51);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(46, 20);
            this.titleLabel.TabIndex = 7;
            this.titleLabel.Text = "Title: ";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleTextBox.Location = new System.Drawing.Point(143, 48);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(165, 26);
            this.titleTextBox.TabIndex = 8;
            this.titleTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.titleTextBox.TextChanged += new System.EventHandler(this.titleTextBox_TextChanged);
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.AutoSize = true;
            this.subtitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subtitleLabel.Location = new System.Drawing.Point(12, 83);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(71, 20);
            this.subtitleLabel.TabIndex = 9;
            this.subtitleLabel.Text = "Subtitle: ";
            // 
            // subtitleTextBox
            // 
            this.subtitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subtitleTextBox.Location = new System.Drawing.Point(143, 80);
            this.subtitleTextBox.Name = "subtitleTextBox";
            this.subtitleTextBox.Size = new System.Drawing.Size(165, 26);
            this.subtitleTextBox.TabIndex = 10;
            this.subtitleTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.subtitleTextBox.TextChanged += new System.EventHandler(this.subtitleTextBox_TextChanged);
            // 
            // confirmButton
            // 
            this.confirmButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmButton.Location = new System.Drawing.Point(12, 156);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(146, 30);
            this.confirmButton.TabIndex = 13;
            this.confirmButton.Text = "OK";
            this.confirmButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(164, 156);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(146, 30);
            this.cancelButton.TabIndex = 14;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // alternativeTitleLabel
            // 
            this.alternativeTitleLabel.AutoSize = true;
            this.alternativeTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alternativeTitleLabel.Location = new System.Drawing.Point(12, 111);
            this.alternativeTitleLabel.Name = "alternativeTitleLabel";
            this.alternativeTitleLabel.Size = new System.Drawing.Size(125, 20);
            this.alternativeTitleLabel.TabIndex = 11;
            this.alternativeTitleLabel.Text = "Alternative Title: ";
            // 
            // alternativeTitleTextBox
            // 
            this.alternativeTitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alternativeTitleTextBox.Location = new System.Drawing.Point(143, 112);
            this.alternativeTitleTextBox.Name = "alternativeTitleTextBox";
            this.alternativeTitleTextBox.Size = new System.Drawing.Size(165, 26);
            this.alternativeTitleTextBox.TabIndex = 12;
            this.alternativeTitleTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.alternativeTitleTextBox.TextChanged += new System.EventHandler(this.alternativeTitleTextBox_TextChanged);
            // 
            // showMoreCheckBox
            // 
            this.showMoreCheckBox.AutoSize = true;
            this.showMoreCheckBox.Location = new System.Drawing.Point(314, 12);
            this.showMoreCheckBox.Name = "showMoreCheckBox";
            this.showMoreCheckBox.Size = new System.Drawing.Size(15, 14);
            this.showMoreCheckBox.TabIndex = 15;
            this.showMoreCheckBox.UseVisualStyleBackColor = true;
            this.showMoreCheckBox.CheckedChanged += new System.EventHandler(this.showMoreCheckBox_CheckedChanged);
            // 
            // formattingLabel
            // 
            this.formattingLabel.AutoSize = true;
            this.formattingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formattingLabel.Location = new System.Drawing.Point(350, 7);
            this.formattingLabel.Name = "formattingLabel";
            this.formattingLabel.Size = new System.Drawing.Size(96, 20);
            this.formattingLabel.TabIndex = 16;
            this.formattingLabel.Text = "Formatting";
            // 
            // collectionFactorLabel
            // 
            this.collectionFactorLabel.AutoSize = true;
            this.collectionFactorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.collectionFactorLabel.Location = new System.Drawing.Point(336, 37);
            this.collectionFactorLabel.Name = "collectionFactorLabel";
            this.collectionFactorLabel.Size = new System.Drawing.Size(78, 20);
            this.collectionFactorLabel.TabIndex = 17;
            this.collectionFactorLabel.Text = "Collection";
            // 
            // titleFactorLabel
            // 
            this.titleFactorLabel.AutoSize = true;
            this.titleFactorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleFactorLabel.Location = new System.Drawing.Point(474, 37);
            this.titleFactorLabel.Name = "titleFactorLabel";
            this.titleFactorLabel.Size = new System.Drawing.Size(38, 20);
            this.titleFactorLabel.TabIndex = 18;
            this.titleFactorLabel.Text = "Title";
            // 
            // subtitleFactorLabel
            // 
            this.subtitleFactorLabel.AutoSize = true;
            this.subtitleFactorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subtitleFactorLabel.Location = new System.Drawing.Point(580, 37);
            this.subtitleFactorLabel.Name = "subtitleFactorLabel";
            this.subtitleFactorLabel.Size = new System.Drawing.Size(63, 20);
            this.subtitleFactorLabel.TabIndex = 19;
            this.subtitleFactorLabel.Text = "Subtitle";
            // 
            // collectionFactorTrackBar
            // 
            this.collectionFactorTrackBar.LargeChange = 2;
            this.collectionFactorTrackBar.Location = new System.Drawing.Point(340, 61);
            this.collectionFactorTrackBar.Maximum = 20;
            this.collectionFactorTrackBar.Minimum = 2;
            this.collectionFactorTrackBar.Name = "collectionFactorTrackBar";
            this.collectionFactorTrackBar.Size = new System.Drawing.Size(111, 45);
            this.collectionFactorTrackBar.TabIndex = 20;
            this.collectionFactorTrackBar.Value = 7;
            this.collectionFactorTrackBar.Scroll += new System.EventHandler(this.collectionFactorTrackBar_Scroll);
            // 
            // collectionLengthTrackBar
            // 
            this.collectionLengthTrackBar.LargeChange = 2;
            this.collectionLengthTrackBar.Location = new System.Drawing.Point(340, 111);
            this.collectionLengthTrackBar.Maximum = 20;
            this.collectionLengthTrackBar.Minimum = 2;
            this.collectionLengthTrackBar.Name = "collectionLengthTrackBar";
            this.collectionLengthTrackBar.Size = new System.Drawing.Size(111, 45);
            this.collectionLengthTrackBar.TabIndex = 21;
            this.collectionLengthTrackBar.Value = 9;
            this.collectionLengthTrackBar.Scroll += new System.EventHandler(this.collectionLengthTrackBar_Scroll);
            // 
            // titleFactorTrackBar
            // 
            this.titleFactorTrackBar.LargeChange = 2;
            this.titleFactorTrackBar.Location = new System.Drawing.Point(463, 61);
            this.titleFactorTrackBar.Maximum = 20;
            this.titleFactorTrackBar.Minimum = 2;
            this.titleFactorTrackBar.Name = "titleFactorTrackBar";
            this.titleFactorTrackBar.Size = new System.Drawing.Size(111, 45);
            this.titleFactorTrackBar.TabIndex = 22;
            this.titleFactorTrackBar.Value = 7;
            this.titleFactorTrackBar.Scroll += new System.EventHandler(this.titleFactorTrackBar_Scroll);
            // 
            // subtitleFactorTrackBar
            // 
            this.subtitleFactorTrackBar.LargeChange = 2;
            this.subtitleFactorTrackBar.Location = new System.Drawing.Point(561, 61);
            this.subtitleFactorTrackBar.Maximum = 20;
            this.subtitleFactorTrackBar.Minimum = 2;
            this.subtitleFactorTrackBar.Name = "subtitleFactorTrackBar";
            this.subtitleFactorTrackBar.Size = new System.Drawing.Size(111, 45);
            this.subtitleFactorTrackBar.TabIndex = 23;
            this.subtitleFactorTrackBar.Value = 7;
            this.subtitleFactorTrackBar.Scroll += new System.EventHandler(this.subtitleFactorTrackBar_Scroll);
            // 
            // titleLengthTrackBar
            // 
            this.titleLengthTrackBar.LargeChange = 2;
            this.titleLengthTrackBar.Location = new System.Drawing.Point(463, 111);
            this.titleLengthTrackBar.Maximum = 20;
            this.titleLengthTrackBar.Minimum = 2;
            this.titleLengthTrackBar.Name = "titleLengthTrackBar";
            this.titleLengthTrackBar.Size = new System.Drawing.Size(111, 45);
            this.titleLengthTrackBar.TabIndex = 24;
            this.titleLengthTrackBar.Value = 18;
            this.titleLengthTrackBar.Scroll += new System.EventHandler(this.titleLengthTrackBar_Scroll);
            // 
            // collectionFactorValueLabel
            // 
            this.collectionFactorValueLabel.AutoSize = true;
            this.collectionFactorValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.collectionFactorValueLabel.Location = new System.Drawing.Point(428, 48);
            this.collectionFactorValueLabel.Name = "collectionFactorValueLabel";
            this.collectionFactorValueLabel.Size = new System.Drawing.Size(18, 20);
            this.collectionFactorValueLabel.TabIndex = 25;
            this.collectionFactorValueLabel.Text = "7";
            // 
            // titleFactorValueLabel
            // 
            this.titleFactorValueLabel.AutoSize = true;
            this.titleFactorValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleFactorValueLabel.Location = new System.Drawing.Point(546, 48);
            this.titleFactorValueLabel.Name = "titleFactorValueLabel";
            this.titleFactorValueLabel.Size = new System.Drawing.Size(18, 20);
            this.titleFactorValueLabel.TabIndex = 26;
            this.titleFactorValueLabel.Text = "7";
            // 
            // subtitleFactorValueLabel
            // 
            this.subtitleFactorValueLabel.AutoSize = true;
            this.subtitleFactorValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subtitleFactorValueLabel.Location = new System.Drawing.Point(649, 48);
            this.subtitleFactorValueLabel.Name = "subtitleFactorValueLabel";
            this.subtitleFactorValueLabel.Size = new System.Drawing.Size(18, 20);
            this.subtitleFactorValueLabel.TabIndex = 27;
            this.subtitleFactorValueLabel.Text = "7";
            // 
            // collectionLengthValueLabel
            // 
            this.collectionLengthValueLabel.AutoSize = true;
            this.collectionLengthValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.collectionLengthValueLabel.Location = new System.Drawing.Point(428, 95);
            this.collectionLengthValueLabel.Name = "collectionLengthValueLabel";
            this.collectionLengthValueLabel.Size = new System.Drawing.Size(18, 20);
            this.collectionLengthValueLabel.TabIndex = 28;
            this.collectionLengthValueLabel.Text = "9";
            // 
            // titleLengthValueLabel
            // 
            this.titleLengthValueLabel.AutoSize = true;
            this.titleLengthValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLengthValueLabel.Location = new System.Drawing.Point(546, 95);
            this.titleLengthValueLabel.Name = "titleLengthValueLabel";
            this.titleLengthValueLabel.Size = new System.Drawing.Size(27, 20);
            this.titleLengthValueLabel.TabIndex = 29;
            this.titleLengthValueLabel.Text = "18";
            // 
            // convertColorsLabel
            // 
            this.convertColorsLabel.AutoSize = true;
            this.convertColorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convertColorsLabel.Location = new System.Drawing.Point(336, 156);
            this.convertColorsLabel.Name = "convertColorsLabel";
            this.convertColorsLabel.Size = new System.Drawing.Size(117, 20);
            this.convertColorsLabel.TabIndex = 30;
            this.convertColorsLabel.Text = "Convert Colors:";
            // 
            // dmcConvertColorRadioButton
            // 
            this.dmcConvertColorRadioButton.AutoSize = true;
            this.dmcConvertColorRadioButton.Location = new System.Drawing.Point(340, 180);
            this.dmcConvertColorRadioButton.Name = "dmcConvertColorRadioButton";
            this.dmcConvertColorRadioButton.Size = new System.Drawing.Size(49, 17);
            this.dmcConvertColorRadioButton.TabIndex = 31;
            this.dmcConvertColorRadioButton.TabStop = true;
            this.dmcConvertColorRadioButton.Text = "DMC";
            this.dmcConvertColorRadioButton.UseVisualStyleBackColor = true;
            this.dmcConvertColorRadioButton.CheckedChanged += new System.EventHandler(this.dmcConvertColorRadioButton_CheckedChanged);
            // 
            // anchorConvertColorRadioButton
            // 
            this.anchorConvertColorRadioButton.AutoSize = true;
            this.anchorConvertColorRadioButton.Location = new System.Drawing.Point(408, 180);
            this.anchorConvertColorRadioButton.Name = "anchorConvertColorRadioButton";
            this.anchorConvertColorRadioButton.Size = new System.Drawing.Size(59, 17);
            this.anchorConvertColorRadioButton.TabIndex = 32;
            this.anchorConvertColorRadioButton.TabStop = true;
            this.anchorConvertColorRadioButton.Text = "Anchor";
            this.anchorConvertColorRadioButton.UseVisualStyleBackColor = true;
            this.anchorConvertColorRadioButton.CheckedChanged += new System.EventHandler(this.anchorConvertColorRadioButton_CheckedChanged);
            // 
            // noConvertColorRadioButton
            // 
            this.noConvertColorRadioButton.AutoSize = true;
            this.noConvertColorRadioButton.Location = new System.Drawing.Point(487, 180);
            this.noConvertColorRadioButton.Name = "noConvertColorRadioButton";
            this.noConvertColorRadioButton.Size = new System.Drawing.Size(39, 17);
            this.noConvertColorRadioButton.TabIndex = 33;
            this.noConvertColorRadioButton.TabStop = true;
            this.noConvertColorRadioButton.Text = "No";
            this.noConvertColorRadioButton.UseVisualStyleBackColor = true;
            this.noConvertColorRadioButton.CheckedChanged += new System.EventHandler(this.noConvertColorRadioButton_CheckedChanged);
            // 
            // SavePdfDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 209);
            this.Controls.Add(this.noConvertColorRadioButton);
            this.Controls.Add(this.anchorConvertColorRadioButton);
            this.Controls.Add(this.dmcConvertColorRadioButton);
            this.Controls.Add(this.convertColorsLabel);
            this.Controls.Add(this.titleLengthValueLabel);
            this.Controls.Add(this.collectionLengthValueLabel);
            this.Controls.Add(this.subtitleFactorValueLabel);
            this.Controls.Add(this.titleFactorValueLabel);
            this.Controls.Add(this.collectionFactorValueLabel);
            this.Controls.Add(this.titleLengthTrackBar);
            this.Controls.Add(this.subtitleFactorTrackBar);
            this.Controls.Add(this.titleFactorTrackBar);
            this.Controls.Add(this.collectionLengthTrackBar);
            this.Controls.Add(this.collectionFactorTrackBar);
            this.Controls.Add(this.subtitleFactorLabel);
            this.Controls.Add(this.titleFactorLabel);
            this.Controls.Add(this.collectionFactorLabel);
            this.Controls.Add(this.formattingLabel);
            this.Controls.Add(this.showMoreCheckBox);
            this.Controls.Add(this.alternativeTitleTextBox);
            this.Controls.Add(this.alternativeTitleLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.subtitleTextBox);
            this.Controls.Add(this.subtitleLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.collectionLabel);
            this.Controls.Add(this.collectionTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SavePdfDialog";
            this.Text = "SavePdfDialog";
            ((System.ComponentModel.ISupportInitialize)(this.collectionFactorTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionLengthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleFactorTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subtitleFactorTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleLengthTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox collectionTextBox;
        private System.Windows.Forms.Label collectionLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.TextBox subtitleTextBox;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label alternativeTitleLabel;
        private System.Windows.Forms.TextBox alternativeTitleTextBox;
        private System.Windows.Forms.CheckBox showMoreCheckBox;
        private System.Windows.Forms.Label formattingLabel;
        private System.Windows.Forms.Label collectionFactorLabel;
        private System.Windows.Forms.Label titleFactorLabel;
        private System.Windows.Forms.Label subtitleFactorLabel;
        private System.Windows.Forms.TrackBar collectionFactorTrackBar;
        private System.Windows.Forms.TrackBar collectionLengthTrackBar;
        private System.Windows.Forms.TrackBar titleFactorTrackBar;
        private System.Windows.Forms.TrackBar subtitleFactorTrackBar;
        private System.Windows.Forms.TrackBar titleLengthTrackBar;
        private System.Windows.Forms.Label collectionFactorValueLabel;
        private System.Windows.Forms.Label titleFactorValueLabel;
        private System.Windows.Forms.Label subtitleFactorValueLabel;
        private System.Windows.Forms.Label collectionLengthValueLabel;
        private System.Windows.Forms.Label titleLengthValueLabel;
        private System.Windows.Forms.Label convertColorsLabel;
        private System.Windows.Forms.RadioButton dmcConvertColorRadioButton;
        private System.Windows.Forms.RadioButton anchorConvertColorRadioButton;
        private System.Windows.Forms.RadioButton noConvertColorRadioButton;
    }
}

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
            this.flowLayoutPanelListOfCrossStitchColors = new System.Windows.Forms.FlowLayoutPanel();
            this.ListOfCrossStitchColorsLabel = new System.Windows.Forms.Label();
            this.mergeCrossStitchColorsButton = new System.Windows.Forms.Button();
            this.addCrossStitchColorButton = new System.Windows.Forms.Button();
            this.addColorDialog = new System.Windows.Forms.ColorDialog();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.ColorsListsLabel = new System.Windows.Forms.Label();
            this.ListOfBackstitchColorsLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanelListOfBackstitchColors = new System.Windows.Forms.FlowLayoutPanel();
            this.mergeBackStitchColorsButton = new System.Windows.Forms.Button();
            this.addBackstitchColorButton = new System.Windows.Forms.Button();
            this.deleteBackstitchColorButton = new System.Windows.Forms.Button();
            this.crossStitchColorsRadioButton = new System.Windows.Forms.RadioButton();
            this.backStitchColorsRadioButton = new System.Windows.Forms.RadioButton();
            this.currentStitchModeTextLabel = new System.Windows.Forms.Label();
            this.currentStitchModeLabel = new System.Windows.Forms.Label();
            this.RetrieveSavedFileButton = new System.Windows.Forms.Button();
            this.retrieveSavedFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.backstitchPictureBox = new System.Windows.Forms.PictureBox();
            this.gridPictureBox = new System.Windows.Forms.PictureBox();
            this.borderPictureBox = new System.Windows.Forms.PictureBox();
            this.gridVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.borderVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.backstitchVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.baseLayerPictureBox = new System.Windows.Forms.PictureBox();
            this.mainImageVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.threadImageVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.threadPictureBox = new System.Windows.Forms.PictureBox();
            this.symbolsPictureBox = new System.Windows.Forms.PictureBox();
            this.symbolsVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.imagesContainerPanel = new System.Windows.Forms.Panel();
            this.removeAlonePixelsButton = new System.Windows.Forms.Button();
            this.removeAlonePixelsTrackBar = new System.Windows.Forms.TrackBar();
            this.removeAlonePixelsLabel = new System.Windows.Forms.Label();
            this.processImageExactToSourceCheckBox = new System.Windows.Forms.CheckBox();
            this.newPixelSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.newPixelSizeLabel = new System.Windows.Forms.Label();
            this.newPixelSizeTrackBarLabel = new System.Windows.Forms.Label();
            this.RepaintCrossesButton = new System.Windows.Forms.Button();
            this.drawingToolsControl = new EmbroideryCreator.DrawingToolsControl();
            this.newCanvasButton = new System.Windows.Forms.Button();
            this.changeCanvasSizeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfColorsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfIterationsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backstitchPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseLayerPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.symbolsPictureBox)).BeginInit();
            this.imagesContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.removeAlonePixelsTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newPixelSizeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // widthSizeTrackBar
            // 
            this.widthSizeTrackBar.Location = new System.Drawing.Point(12, 160);
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
            this.numberOfColorsTrackBar.Location = new System.Drawing.Point(8, 236);
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
            this.widthSizeLabel.Location = new System.Drawing.Point(12, 119);
            this.widthSizeLabel.Name = "widthSizeLabel";
            this.widthSizeLabel.Size = new System.Drawing.Size(113, 25);
            this.widthSizeLabel.TabIndex = 5;
            this.widthSizeLabel.Text = "New Width:";
            // 
            // numberOfColorsLabel
            // 
            this.numberOfColorsLabel.AutoSize = true;
            this.numberOfColorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numberOfColorsLabel.Location = new System.Drawing.Point(12, 208);
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
            this.numberOfColorsTrackBarLabel.Location = new System.Drawing.Point(190, 208);
            this.numberOfColorsTrackBarLabel.Name = "numberOfColorsTrackBarLabel";
            this.numberOfColorsTrackBarLabel.Size = new System.Drawing.Size(34, 25);
            this.numberOfColorsTrackBarLabel.TabIndex = 8;
            this.numberOfColorsTrackBarLabel.Text = "10";
            // 
            // widthTrackBarLabel
            // 
            this.widthTrackBarLabel.AutoSize = true;
            this.widthTrackBarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.widthTrackBarLabel.Location = new System.Drawing.Point(128, 119);
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
            this.ProcessAtAllChangesCheckBox.Location = new System.Drawing.Point(12, 12);
            this.ProcessAtAllChangesCheckBox.Name = "ProcessAtAllChangesCheckBox";
            this.ProcessAtAllChangesCheckBox.Size = new System.Drawing.Size(212, 21);
            this.ProcessAtAllChangesCheckBox.TabIndex = 13;
            this.ProcessAtAllChangesCheckBox.Text = "Process image at all changes";
            this.ProcessAtAllChangesCheckBox.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelListOfCrossStitchColors
            // 
            this.flowLayoutPanelListOfCrossStitchColors.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.flowLayoutPanelListOfCrossStitchColors.Location = new System.Drawing.Point(847, 119);
            this.flowLayoutPanelListOfCrossStitchColors.Name = "flowLayoutPanelListOfCrossStitchColors";
            this.flowLayoutPanelListOfCrossStitchColors.Size = new System.Drawing.Size(165, 364);
            this.flowLayoutPanelListOfCrossStitchColors.TabIndex = 14;
            // 
            // ListOfCrossStitchColorsLabel
            // 
            this.ListOfCrossStitchColorsLabel.AutoSize = true;
            this.ListOfCrossStitchColorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListOfCrossStitchColorsLabel.Location = new System.Drawing.Point(836, 85);
            this.ListOfCrossStitchColorsLabel.Name = "ListOfCrossStitchColorsLabel";
            this.ListOfCrossStitchColorsLabel.Size = new System.Drawing.Size(124, 25);
            this.ListOfCrossStitchColorsLabel.TabIndex = 15;
            this.ListOfCrossStitchColorsLabel.Text = "Cross Stitch:";
            // 
            // mergeCrossStitchColorsButton
            // 
            this.mergeCrossStitchColorsButton.Location = new System.Drawing.Point(848, 489);
            this.mergeCrossStitchColorsButton.Name = "mergeCrossStitchColorsButton";
            this.mergeCrossStitchColorsButton.Size = new System.Drawing.Size(98, 23);
            this.mergeCrossStitchColorsButton.TabIndex = 16;
            this.mergeCrossStitchColorsButton.Text = "Merge colors";
            this.mergeCrossStitchColorsButton.UseVisualStyleBackColor = true;
            this.mergeCrossStitchColorsButton.Click += new System.EventHandler(this.mergeColorsButton_Click);
            // 
            // addCrossStitchColorButton
            // 
            this.addCrossStitchColorButton.Location = new System.Drawing.Point(848, 518);
            this.addCrossStitchColorButton.Name = "addCrossStitchColorButton";
            this.addCrossStitchColorButton.Size = new System.Drawing.Size(98, 23);
            this.addCrossStitchColorButton.TabIndex = 17;
            this.addCrossStitchColorButton.Text = "Add color";
            this.addCrossStitchColorButton.UseVisualStyleBackColor = true;
            this.addCrossStitchColorButton.Click += new System.EventHandler(this.addColorButton_Click);
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.mainPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(529, 529);
            this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            this.mainPictureBox.Click += new System.EventHandler(this.mainPictureBox_Click);
            this.mainPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mainPictureBox_Paint);
            this.mainPictureBox.DoubleClick += new System.EventHandler(this.mainPictureBox_DoubleClick);
            this.mainPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseDown);
            this.mainPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseMove);
            this.mainPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mainPictureBox_MouseUp);
            // 
            // ColorsListsLabel
            // 
            this.ColorsListsLabel.AutoSize = true;
            this.ColorsListsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColorsListsLabel.Location = new System.Drawing.Point(836, 61);
            this.ColorsListsLabel.Name = "ColorsListsLabel";
            this.ColorsListsLabel.Size = new System.Drawing.Size(69, 25);
            this.ColorsListsLabel.TabIndex = 19;
            this.ColorsListsLabel.Text = "Colors";
            // 
            // ListOfBackstitchColorsLabel
            // 
            this.ListOfBackstitchColorsLabel.AutoSize = true;
            this.ListOfBackstitchColorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListOfBackstitchColorsLabel.Location = new System.Drawing.Point(1025, 85);
            this.ListOfBackstitchColorsLabel.Name = "ListOfBackstitchColorsLabel";
            this.ListOfBackstitchColorsLabel.Size = new System.Drawing.Size(107, 25);
            this.ListOfBackstitchColorsLabel.TabIndex = 20;
            this.ListOfBackstitchColorsLabel.Text = "Backstitch:";
            // 
            // flowLayoutPanelListOfBackstitchColors
            // 
            this.flowLayoutPanelListOfBackstitchColors.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.flowLayoutPanelListOfBackstitchColors.Location = new System.Drawing.Point(1034, 119);
            this.flowLayoutPanelListOfBackstitchColors.Name = "flowLayoutPanelListOfBackstitchColors";
            this.flowLayoutPanelListOfBackstitchColors.Size = new System.Drawing.Size(98, 364);
            this.flowLayoutPanelListOfBackstitchColors.TabIndex = 15;
            // 
            // mergeBackStitchColorsButton
            // 
            this.mergeBackStitchColorsButton.Location = new System.Drawing.Point(1034, 489);
            this.mergeBackStitchColorsButton.Name = "mergeBackStitchColorsButton";
            this.mergeBackStitchColorsButton.Size = new System.Drawing.Size(98, 23);
            this.mergeBackStitchColorsButton.TabIndex = 21;
            this.mergeBackStitchColorsButton.Text = "Merge colors";
            this.mergeBackStitchColorsButton.UseVisualStyleBackColor = true;
            this.mergeBackStitchColorsButton.Click += new System.EventHandler(this.mergeBackStitchColorsButton_Click);
            // 
            // addBackstitchColorButton
            // 
            this.addBackstitchColorButton.Location = new System.Drawing.Point(1034, 518);
            this.addBackstitchColorButton.Name = "addBackstitchColorButton";
            this.addBackstitchColorButton.Size = new System.Drawing.Size(98, 23);
            this.addBackstitchColorButton.TabIndex = 22;
            this.addBackstitchColorButton.Text = "Add color";
            this.addBackstitchColorButton.UseVisualStyleBackColor = true;
            this.addBackstitchColorButton.Click += new System.EventHandler(this.addBackstitchColorButton_Click);
            // 
            // deleteBackstitchColorButton
            // 
            this.deleteBackstitchColorButton.Location = new System.Drawing.Point(1034, 547);
            this.deleteBackstitchColorButton.Name = "deleteBackstitchColorButton";
            this.deleteBackstitchColorButton.Size = new System.Drawing.Size(98, 23);
            this.deleteBackstitchColorButton.TabIndex = 23;
            this.deleteBackstitchColorButton.Text = "Delete color";
            this.deleteBackstitchColorButton.UseVisualStyleBackColor = true;
            this.deleteBackstitchColorButton.Click += new System.EventHandler(this.deleteBackstitchColorButton_Click);
            // 
            // crossStitchColorsRadioButton
            // 
            this.crossStitchColorsRadioButton.AutoSize = true;
            this.crossStitchColorsRadioButton.Location = new System.Drawing.Point(834, 119);
            this.crossStitchColorsRadioButton.Name = "crossStitchColorsRadioButton";
            this.crossStitchColorsRadioButton.Size = new System.Drawing.Size(14, 13);
            this.crossStitchColorsRadioButton.TabIndex = 24;
            this.crossStitchColorsRadioButton.TabStop = true;
            this.crossStitchColorsRadioButton.UseVisualStyleBackColor = true;
            this.crossStitchColorsRadioButton.Click += new System.EventHandler(this.crossStitchColorsRadioButton_Clicked);
            // 
            // backStitchColorsRadioButton
            // 
            this.backStitchColorsRadioButton.AutoSize = true;
            this.backStitchColorsRadioButton.Location = new System.Drawing.Point(1021, 119);
            this.backStitchColorsRadioButton.Name = "backStitchColorsRadioButton";
            this.backStitchColorsRadioButton.Size = new System.Drawing.Size(14, 13);
            this.backStitchColorsRadioButton.TabIndex = 25;
            this.backStitchColorsRadioButton.TabStop = true;
            this.backStitchColorsRadioButton.UseVisualStyleBackColor = true;
            this.backStitchColorsRadioButton.Click += new System.EventHandler(this.backStitchColorsRadioButton_Clicked);
            // 
            // currentStitchModeTextLabel
            // 
            this.currentStitchModeTextLabel.AutoSize = true;
            this.currentStitchModeTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentStitchModeTextLabel.Location = new System.Drawing.Point(296, 547);
            this.currentStitchModeTextLabel.Name = "currentStitchModeTextLabel";
            this.currentStitchModeTextLabel.Size = new System.Drawing.Size(197, 25);
            this.currentStitchModeTextLabel.TabIndex = 15;
            this.currentStitchModeTextLabel.Text = "Current Stitch Mode: ";
            // 
            // currentStitchModeLabel
            // 
            this.currentStitchModeLabel.AutoSize = true;
            this.currentStitchModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentStitchModeLabel.Location = new System.Drawing.Point(486, 547);
            this.currentStitchModeLabel.Name = "currentStitchModeLabel";
            this.currentStitchModeLabel.Size = new System.Drawing.Size(118, 25);
            this.currentStitchModeLabel.TabIndex = 15;
            this.currentStitchModeLabel.Text = "Cross Stitch";
            // 
            // RetrieveSavedFileButton
            // 
            this.RetrieveSavedFileButton.Location = new System.Drawing.Point(13, 489);
            this.RetrieveSavedFileButton.Name = "RetrieveSavedFileButton";
            this.RetrieveSavedFileButton.Size = new System.Drawing.Size(139, 23);
            this.RetrieveSavedFileButton.TabIndex = 26;
            this.RetrieveSavedFileButton.Text = "RetrieveSavedFile ";
            this.RetrieveSavedFileButton.UseVisualStyleBackColor = true;
            this.RetrieveSavedFileButton.Click += new System.EventHandler(this.RetrieveSavedFileButton_Click);
            // 
            // retrieveSavedFileDialog
            // 
            this.retrieveSavedFileDialog.FileName = "openFileDialog1";
            // 
            // backstitchPictureBox
            // 
            this.backstitchPictureBox.Enabled = false;
            this.backstitchPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.backstitchPictureBox.Location = new System.Drawing.Point(0, 0);
            this.backstitchPictureBox.Name = "backstitchPictureBox";
            this.backstitchPictureBox.Size = new System.Drawing.Size(529, 529);
            this.backstitchPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.backstitchPictureBox.TabIndex = 27;
            this.backstitchPictureBox.TabStop = false;
            // 
            // gridPictureBox
            // 
            this.gridPictureBox.Enabled = false;
            this.gridPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.gridPictureBox.Location = new System.Drawing.Point(0, 0);
            this.gridPictureBox.Name = "gridPictureBox";
            this.gridPictureBox.Size = new System.Drawing.Size(529, 529);
            this.gridPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.gridPictureBox.TabIndex = 28;
            this.gridPictureBox.TabStop = false;
            // 
            // borderPictureBox
            // 
            this.borderPictureBox.Enabled = false;
            this.borderPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.borderPictureBox.Location = new System.Drawing.Point(0, 0);
            this.borderPictureBox.Name = "borderPictureBox";
            this.borderPictureBox.Size = new System.Drawing.Size(529, 529);
            this.borderPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.borderPictureBox.TabIndex = 29;
            this.borderPictureBox.TabStop = false;
            // 
            // gridVisibleCheckBox
            // 
            this.gridVisibleCheckBox.AutoSize = true;
            this.gridVisibleCheckBox.Checked = true;
            this.gridVisibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridVisibleCheckBox.Location = new System.Drawing.Point(215, 420);
            this.gridVisibleCheckBox.Name = "gridVisibleCheckBox";
            this.gridVisibleCheckBox.Size = new System.Drawing.Size(45, 17);
            this.gridVisibleCheckBox.TabIndex = 30;
            this.gridVisibleCheckBox.Text = "Grid";
            this.gridVisibleCheckBox.UseVisualStyleBackColor = true;
            this.gridVisibleCheckBox.CheckedChanged += new System.EventHandler(this.gridVisibleCheckBox_CheckedChanged);
            // 
            // borderVisibleCheckBox
            // 
            this.borderVisibleCheckBox.AutoSize = true;
            this.borderVisibleCheckBox.Checked = true;
            this.borderVisibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.borderVisibleCheckBox.Location = new System.Drawing.Point(215, 443);
            this.borderVisibleCheckBox.Name = "borderVisibleCheckBox";
            this.borderVisibleCheckBox.Size = new System.Drawing.Size(57, 17);
            this.borderVisibleCheckBox.TabIndex = 31;
            this.borderVisibleCheckBox.Text = "Border";
            this.borderVisibleCheckBox.UseVisualStyleBackColor = true;
            this.borderVisibleCheckBox.CheckedChanged += new System.EventHandler(this.borderCheckBox_CheckedChanged);
            // 
            // backstitchVisibleCheckBox
            // 
            this.backstitchVisibleCheckBox.AutoSize = true;
            this.backstitchVisibleCheckBox.Checked = true;
            this.backstitchVisibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backstitchVisibleCheckBox.Location = new System.Drawing.Point(215, 466);
            this.backstitchVisibleCheckBox.Name = "backstitchVisibleCheckBox";
            this.backstitchVisibleCheckBox.Size = new System.Drawing.Size(76, 17);
            this.backstitchVisibleCheckBox.TabIndex = 32;
            this.backstitchVisibleCheckBox.Text = "Backstitch";
            this.backstitchVisibleCheckBox.UseVisualStyleBackColor = true;
            this.backstitchVisibleCheckBox.CheckedChanged += new System.EventHandler(this.backstitchVisibleCheckBox_CheckedChanged);
            // 
            // baseLayerPictureBox
            // 
            this.baseLayerPictureBox.Image = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.baseLayerPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.baseLayerPictureBox.Location = new System.Drawing.Point(0, 0);
            this.baseLayerPictureBox.Name = "baseLayerPictureBox";
            this.baseLayerPictureBox.Size = new System.Drawing.Size(529, 529);
            this.baseLayerPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.baseLayerPictureBox.TabIndex = 33;
            this.baseLayerPictureBox.TabStop = false;
            // 
            // mainImageVisibleCheckBox
            // 
            this.mainImageVisibleCheckBox.AutoSize = true;
            this.mainImageVisibleCheckBox.Checked = true;
            this.mainImageVisibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mainImageVisibleCheckBox.Location = new System.Drawing.Point(215, 349);
            this.mainImageVisibleCheckBox.Name = "mainImageVisibleCheckBox";
            this.mainImageVisibleCheckBox.Size = new System.Drawing.Size(55, 17);
            this.mainImageVisibleCheckBox.TabIndex = 34;
            this.mainImageVisibleCheckBox.Text = "Colors";
            this.mainImageVisibleCheckBox.UseVisualStyleBackColor = true;
            this.mainImageVisibleCheckBox.CheckedChanged += new System.EventHandler(this.mainImageVisibleCheckBox_CheckedChanged);
            // 
            // threadImageVisibleCheckBox
            // 
            this.threadImageVisibleCheckBox.AutoSize = true;
            this.threadImageVisibleCheckBox.Location = new System.Drawing.Point(215, 374);
            this.threadImageVisibleCheckBox.Name = "threadImageVisibleCheckBox";
            this.threadImageVisibleCheckBox.Size = new System.Drawing.Size(60, 17);
            this.threadImageVisibleCheckBox.TabIndex = 36;
            this.threadImageVisibleCheckBox.Text = "Thread";
            this.threadImageVisibleCheckBox.UseVisualStyleBackColor = true;
            this.threadImageVisibleCheckBox.CheckedChanged += new System.EventHandler(this.threadImageVisibleCheckBox_CheckedChanged);
            // 
            // threadPictureBox
            // 
            this.threadPictureBox.Enabled = false;
            this.threadPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.threadPictureBox.Location = new System.Drawing.Point(0, 0);
            this.threadPictureBox.Name = "threadPictureBox";
            this.threadPictureBox.Size = new System.Drawing.Size(529, 529);
            this.threadPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.threadPictureBox.TabIndex = 37;
            this.threadPictureBox.TabStop = false;
            this.threadPictureBox.Visible = false;
            // 
            // symbolsPictureBox
            // 
            this.symbolsPictureBox.Enabled = false;
            this.symbolsPictureBox.InitialImage = global::EmbroideryCreator.Properties.Resources.ChooseImagePicture;
            this.symbolsPictureBox.Location = new System.Drawing.Point(0, 0);
            this.symbolsPictureBox.Name = "symbolsPictureBox";
            this.symbolsPictureBox.Size = new System.Drawing.Size(529, 529);
            this.symbolsPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.symbolsPictureBox.TabIndex = 38;
            this.symbolsPictureBox.TabStop = false;
            this.symbolsPictureBox.Visible = false;
            // 
            // symbolsVisibleCheckBox
            // 
            this.symbolsVisibleCheckBox.AutoSize = true;
            this.symbolsVisibleCheckBox.Location = new System.Drawing.Point(215, 397);
            this.symbolsVisibleCheckBox.Name = "symbolsVisibleCheckBox";
            this.symbolsVisibleCheckBox.Size = new System.Drawing.Size(65, 17);
            this.symbolsVisibleCheckBox.TabIndex = 39;
            this.symbolsVisibleCheckBox.Text = "Symbols";
            this.symbolsVisibleCheckBox.UseVisualStyleBackColor = true;
            this.symbolsVisibleCheckBox.CheckedChanged += new System.EventHandler(this.symbolsVisibleCheckBox_CheckedChanged);
            // 
            // imagesContainerPanel
            // 
            this.imagesContainerPanel.Controls.Add(this.borderPictureBox);
            this.imagesContainerPanel.Controls.Add(this.backstitchPictureBox);
            this.imagesContainerPanel.Controls.Add(this.gridPictureBox);
            this.imagesContainerPanel.Controls.Add(this.symbolsPictureBox);
            this.imagesContainerPanel.Controls.Add(this.threadPictureBox);
            this.imagesContainerPanel.Controls.Add(this.mainPictureBox);
            this.imagesContainerPanel.Controls.Add(this.baseLayerPictureBox);
            this.imagesContainerPanel.Location = new System.Drawing.Point(301, 12);
            this.imagesContainerPanel.Name = "imagesContainerPanel";
            this.imagesContainerPanel.Size = new System.Drawing.Size(529, 529);
            this.imagesContainerPanel.TabIndex = 40;
            // 
            // removeAlonePixelsButton
            // 
            this.removeAlonePixelsButton.Location = new System.Drawing.Point(13, 363);
            this.removeAlonePixelsButton.Name = "removeAlonePixelsButton";
            this.removeAlonePixelsButton.Size = new System.Drawing.Size(139, 23);
            this.removeAlonePixelsButton.TabIndex = 41;
            this.removeAlonePixelsButton.Text = "Remove alone pixels";
            this.removeAlonePixelsButton.UseVisualStyleBackColor = true;
            this.removeAlonePixelsButton.Click += new System.EventHandler(this.removeAlonePixelsButton_Click);
            // 
            // removeAlonePixelsTrackBar
            // 
            this.removeAlonePixelsTrackBar.Location = new System.Drawing.Point(17, 397);
            this.removeAlonePixelsTrackBar.Maximum = 9;
            this.removeAlonePixelsTrackBar.Name = "removeAlonePixelsTrackBar";
            this.removeAlonePixelsTrackBar.Size = new System.Drawing.Size(135, 45);
            this.removeAlonePixelsTrackBar.TabIndex = 42;
            this.removeAlonePixelsTrackBar.Value = 1;
            this.removeAlonePixelsTrackBar.Scroll += new System.EventHandler(this.removeAlonePixelsTrackBar_Scroll);
            // 
            // removeAlonePixelsLabel
            // 
            this.removeAlonePixelsLabel.AutoSize = true;
            this.removeAlonePixelsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeAlonePixelsLabel.Location = new System.Drawing.Point(153, 363);
            this.removeAlonePixelsLabel.Name = "removeAlonePixelsLabel";
            this.removeAlonePixelsLabel.Size = new System.Drawing.Size(20, 22);
            this.removeAlonePixelsLabel.TabIndex = 43;
            this.removeAlonePixelsLabel.Text = "1";
            // 
            // processImageExactToSourceCheckBox
            // 
            this.processImageExactToSourceCheckBox.AutoSize = true;
            this.processImageExactToSourceCheckBox.Location = new System.Drawing.Point(158, 547);
            this.processImageExactToSourceCheckBox.Name = "processImageExactToSourceCheckBox";
            this.processImageExactToSourceCheckBox.Size = new System.Drawing.Size(106, 17);
            this.processImageExactToSourceCheckBox.TabIndex = 44;
            this.processImageExactToSourceCheckBox.Text = "Exact To Source";
            this.processImageExactToSourceCheckBox.UseVisualStyleBackColor = true;
            // 
            // newPixelSizeTrackBar
            // 
            this.newPixelSizeTrackBar.Location = new System.Drawing.Point(17, 71);
            this.newPixelSizeTrackBar.Maximum = 64;
            this.newPixelSizeTrackBar.Minimum = 10;
            this.newPixelSizeTrackBar.Name = "newPixelSizeTrackBar";
            this.newPixelSizeTrackBar.Size = new System.Drawing.Size(135, 45);
            this.newPixelSizeTrackBar.TabIndex = 45;
            this.newPixelSizeTrackBar.Value = 10;
            this.newPixelSizeTrackBar.Scroll += new System.EventHandler(this.newPixelSizeTrackBar_Scroll);
            // 
            // newPixelSizeLabel
            // 
            this.newPixelSizeLabel.AutoSize = true;
            this.newPixelSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newPixelSizeLabel.Location = new System.Drawing.Point(12, 43);
            this.newPixelSizeLabel.Name = "newPixelSizeLabel";
            this.newPixelSizeLabel.Size = new System.Drawing.Size(148, 25);
            this.newPixelSizeLabel.TabIndex = 46;
            this.newPixelSizeLabel.Text = "New Pixel Size:";
            // 
            // newPixelSizeTrackBarLabel
            // 
            this.newPixelSizeTrackBarLabel.AutoSize = true;
            this.newPixelSizeTrackBarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newPixelSizeTrackBarLabel.Location = new System.Drawing.Point(159, 43);
            this.newPixelSizeTrackBarLabel.Name = "newPixelSizeTrackBarLabel";
            this.newPixelSizeTrackBarLabel.Size = new System.Drawing.Size(34, 25);
            this.newPixelSizeTrackBarLabel.TabIndex = 47;
            this.newPixelSizeTrackBarLabel.Text = "10";
            // 
            // RepaintCrossesButton
            // 
            this.RepaintCrossesButton.Location = new System.Drawing.Point(13, 460);
            this.RepaintCrossesButton.Name = "RepaintCrossesButton";
            this.RepaintCrossesButton.Size = new System.Drawing.Size(139, 23);
            this.RepaintCrossesButton.TabIndex = 48;
            this.RepaintCrossesButton.Text = "Repaint Crosses";
            this.RepaintCrossesButton.UseVisualStyleBackColor = true;
            this.RepaintCrossesButton.Click += new System.EventHandler(this.RepaintCrossesButton_Click);
            // 
            // drawingToolsControl
            // 
            this.drawingToolsControl.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.drawingToolsControl.Location = new System.Drawing.Point(848, 13);
            this.drawingToolsControl.Name = "drawingToolsControl";
            this.drawingToolsControl.Padding = new System.Windows.Forms.Padding(3);
            this.drawingToolsControl.Size = new System.Drawing.Size(215, 45);
            this.drawingToolsControl.TabIndex = 18;
            // 
            // newCanvasButton
            // 
            this.newCanvasButton.Location = new System.Drawing.Point(13, 431);
            this.newCanvasButton.Name = "newCanvasButton";
            this.newCanvasButton.Size = new System.Drawing.Size(139, 23);
            this.newCanvasButton.TabIndex = 49;
            this.newCanvasButton.Text = "New Canvas";
            this.newCanvasButton.UseVisualStyleBackColor = true;
            this.newCanvasButton.Click += new System.EventHandler(this.newCanvasButton_Click);
            // 
            // changeCanvasSizeButton
            // 
            this.changeCanvasSizeButton.Location = new System.Drawing.Point(12, 547);
            this.changeCanvasSizeButton.Name = "changeCanvasSizeButton";
            this.changeCanvasSizeButton.Size = new System.Drawing.Size(139, 23);
            this.changeCanvasSizeButton.TabIndex = 50;
            this.changeCanvasSizeButton.Text = "Change Canvas Size";
            this.changeCanvasSizeButton.UseVisualStyleBackColor = true;
            this.changeCanvasSizeButton.Click += new System.EventHandler(this.changeCanvasSizeButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 579);
            this.Controls.Add(this.changeCanvasSizeButton);
            this.Controls.Add(this.newCanvasButton);
            this.Controls.Add(this.RepaintCrossesButton);
            this.Controls.Add(this.newPixelSizeTrackBarLabel);
            this.Controls.Add(this.newPixelSizeLabel);
            this.Controls.Add(this.newPixelSizeTrackBar);
            this.Controls.Add(this.processImageExactToSourceCheckBox);
            this.Controls.Add(this.removeAlonePixelsLabel);
            this.Controls.Add(this.removeAlonePixelsTrackBar);
            this.Controls.Add(this.removeAlonePixelsButton);
            this.Controls.Add(this.symbolsVisibleCheckBox);
            this.Controls.Add(this.threadImageVisibleCheckBox);
            this.Controls.Add(this.mainImageVisibleCheckBox);
            this.Controls.Add(this.backstitchVisibleCheckBox);
            this.Controls.Add(this.borderVisibleCheckBox);
            this.Controls.Add(this.gridVisibleCheckBox);
            this.Controls.Add(this.RetrieveSavedFileButton);
            this.Controls.Add(this.currentStitchModeLabel);
            this.Controls.Add(this.currentStitchModeTextLabel);
            this.Controls.Add(this.backStitchColorsRadioButton);
            this.Controls.Add(this.crossStitchColorsRadioButton);
            this.Controls.Add(this.deleteBackstitchColorButton);
            this.Controls.Add(this.addBackstitchColorButton);
            this.Controls.Add(this.mergeBackStitchColorsButton);
            this.Controls.Add(this.flowLayoutPanelListOfBackstitchColors);
            this.Controls.Add(this.ListOfBackstitchColorsLabel);
            this.Controls.Add(this.ColorsListsLabel);
            this.Controls.Add(this.drawingToolsControl);
            this.Controls.Add(this.addCrossStitchColorButton);
            this.Controls.Add(this.mergeCrossStitchColorsButton);
            this.Controls.Add(this.ListOfCrossStitchColorsLabel);
            this.Controls.Add(this.flowLayoutPanelListOfCrossStitchColors);
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
            this.Controls.Add(this.imagesContainerPanel);
            this.Name = "MainForm";
            this.Text = "EmbroideryCreator";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.widthSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfColorsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfIterationsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backstitchPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseLayerPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.symbolsPictureBox)).EndInit();
            this.imagesContainerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.removeAlonePixelsTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newPixelSizeTrackBar)).EndInit();
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
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelListOfCrossStitchColors;
        private System.Windows.Forms.Label ListOfCrossStitchColorsLabel;
        private System.Windows.Forms.Button mergeCrossStitchColorsButton;
        private System.Windows.Forms.Button addCrossStitchColorButton;
        private System.Windows.Forms.ColorDialog addColorDialog;
        private DrawingToolsControl drawingToolsControl;
        private System.Windows.Forms.Label ColorsListsLabel;
        private System.Windows.Forms.Label ListOfBackstitchColorsLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelListOfBackstitchColors;
        private System.Windows.Forms.Button mergeBackStitchColorsButton;
        private System.Windows.Forms.Button addBackstitchColorButton;
        private System.Windows.Forms.Button deleteBackstitchColorButton;
        private System.Windows.Forms.RadioButton crossStitchColorsRadioButton;
        private System.Windows.Forms.RadioButton backStitchColorsRadioButton;
        private System.Windows.Forms.Label currentStitchModeTextLabel;
        private System.Windows.Forms.Label currentStitchModeLabel;
        private System.Windows.Forms.Button RetrieveSavedFileButton;
        private System.Windows.Forms.OpenFileDialog retrieveSavedFileDialog;
        private System.Windows.Forms.PictureBox backstitchPictureBox;
        private System.Windows.Forms.PictureBox gridPictureBox;
        private System.Windows.Forms.PictureBox borderPictureBox;
        private System.Windows.Forms.CheckBox gridVisibleCheckBox;
        private System.Windows.Forms.CheckBox borderVisibleCheckBox;
        private System.Windows.Forms.CheckBox backstitchVisibleCheckBox;
        private System.Windows.Forms.PictureBox baseLayerPictureBox;
        private System.Windows.Forms.CheckBox mainImageVisibleCheckBox;
        private System.Windows.Forms.CheckBox threadImageVisibleCheckBox;
        private System.Windows.Forms.PictureBox threadPictureBox;
        private System.Windows.Forms.PictureBox symbolsPictureBox;
        private System.Windows.Forms.CheckBox symbolsVisibleCheckBox;
        private System.Windows.Forms.Panel imagesContainerPanel;
        private System.Windows.Forms.Button removeAlonePixelsButton;
        private System.Windows.Forms.TrackBar removeAlonePixelsTrackBar;
        private System.Windows.Forms.Label removeAlonePixelsLabel;
        private System.Windows.Forms.CheckBox processImageExactToSourceCheckBox;
        private System.Windows.Forms.TrackBar newPixelSizeTrackBar;
        private System.Windows.Forms.Label newPixelSizeLabel;
        private System.Windows.Forms.Label newPixelSizeTrackBarLabel;
        private System.Windows.Forms.Button RepaintCrossesButton;
        private System.Windows.Forms.Button newCanvasButton;
        private System.Windows.Forms.Button changeCanvasSizeButton;
    }
}


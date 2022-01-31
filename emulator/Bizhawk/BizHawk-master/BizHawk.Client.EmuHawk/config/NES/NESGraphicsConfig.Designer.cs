﻿namespace BizHawk.Client.EmuHawk
{
	partial class NESGraphicsConfig
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
			this.OK = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.AllowMoreSprites = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pictureBoxPalette = new System.Windows.Forms.PictureBox();
			this.AutoLoadPalette = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.PalettePath = new System.Windows.Forms.TextBox();
			this.BrowsePalette = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.PAL_LastLineNumeric = new System.Windows.Forms.NumericUpDown();
			this.PAL_FirstLineNumeric = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.btnAreaFull = new System.Windows.Forms.Button();
			this.btnAreaStandard = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.NTSC_LastLineNumeric = new System.Windows.Forms.NumericUpDown();
			this.NTSC_FirstLineNumeric = new System.Windows.Forms.NumericUpDown();
			this.ClipLeftAndRightCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkUseBackdropColor = new System.Windows.Forms.CheckBox();
			this.ChangeBGColor = new System.Windows.Forms.Button();
			this.BackGroundColorNumber = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.BackgroundColorPanel = new System.Windows.Forms.Panel();
			this.DispBackground = new System.Windows.Forms.CheckBox();
			this.DispSprites = new System.Windows.Forms.CheckBox();
			this.BGColorDialog = new System.Windows.Forms.ColorDialog();
			this.RestoreDefaultsButton = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxPalette)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PAL_LastLineNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PAL_FirstLineNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NTSC_LastLineNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NTSC_FirstLineNumeric)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// OK
			// 
			this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OK.Location = new System.Drawing.Point(213, 477);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(75, 23);
			this.OK.TabIndex = 70;
			this.OK.Text = "&OK";
			this.OK.UseVisualStyleBackColor = true;
			this.OK.Click += new System.EventHandler(this.Ok_Click);
			// 
			// Cancel
			// 
			this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(294, 477);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 23);
			this.Cancel.TabIndex = 75;
			this.Cancel.Text = "&Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// AllowMoreSprites
			// 
			this.AllowMoreSprites.AutoSize = true;
			this.AllowMoreSprites.Location = new System.Drawing.Point(134, 96);
			this.AllowMoreSprites.Name = "AllowMoreSprites";
			this.AllowMoreSprites.Size = new System.Drawing.Size(203, 17);
			this.AllowMoreSprites.TabIndex = 23;
			this.AllowMoreSprites.Text = "Allow more than 8 sprites per scanline";
			this.AllowMoreSprites.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.pictureBoxPalette);
			this.groupBox1.Controls.Add(this.AutoLoadPalette);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.PalettePath);
			this.groupBox1.Controls.Add(this.BrowsePalette);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(352, 184);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette Config";
			// 
			// pictureBoxPalette
			// 
			this.pictureBoxPalette.Location = new System.Drawing.Point(6, 89);
			this.pictureBoxPalette.Name = "pictureBoxPalette";
			this.pictureBoxPalette.Size = new System.Drawing.Size(340, 89);
			this.pictureBoxPalette.TabIndex = 11;
			this.pictureBoxPalette.TabStop = false;
			// 
			// AutoLoadPalette
			// 
			this.AutoLoadPalette.AutoSize = true;
			this.AutoLoadPalette.Location = new System.Drawing.Point(6, 66);
			this.AutoLoadPalette.Name = "AutoLoadPalette";
			this.AutoLoadPalette.Size = new System.Drawing.Size(129, 17);
			this.AutoLoadPalette.TabIndex = 10;
			this.AutoLoadPalette.Text = "Change to this palette";
			this.AutoLoadPalette.UseVisualStyleBackColor = true;
			this.AutoLoadPalette.Click += new System.EventHandler(this.AutoLoadPalette_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Use palette from file";
			// 
			// PalettePath
			// 
			this.PalettePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PalettePath.Location = new System.Drawing.Point(6, 40);
			this.PalettePath.Name = "PalettePath";
			this.PalettePath.Size = new System.Drawing.Size(259, 20);
			this.PalettePath.TabIndex = 1;
			// 
			// BrowsePalette
			// 
			this.BrowsePalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowsePalette.Location = new System.Drawing.Point(271, 37);
			this.BrowsePalette.Name = "BrowsePalette";
			this.BrowsePalette.Size = new System.Drawing.Size(75, 23);
			this.BrowsePalette.TabIndex = 5;
			this.BrowsePalette.Text = "&Browse...";
			this.BrowsePalette.UseVisualStyleBackColor = true;
			this.BrowsePalette.Click += new System.EventHandler(this.BrowsePalette_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.PAL_LastLineNumeric);
			this.groupBox2.Controls.Add(this.PAL_FirstLineNumeric);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.btnAreaFull);
			this.groupBox2.Controls.Add(this.btnAreaStandard);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.NTSC_LastLineNumeric);
			this.groupBox2.Controls.Add(this.NTSC_FirstLineNumeric);
			this.groupBox2.Controls.Add(this.ClipLeftAndRightCheckBox);
			this.groupBox2.Controls.Add(this.AllowMoreSprites);
			this.groupBox2.Location = new System.Drawing.Point(12, 202);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(352, 150);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Drawing Area";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(131, 22);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(27, 13);
			this.label6.TabIndex = 44;
			this.label6.Text = "PAL";
			// 
			// PAL_LastLineNumeric
			// 
			this.PAL_LastLineNumeric.Location = new System.Drawing.Point(124, 67);
			this.PAL_LastLineNumeric.Maximum = new decimal(new int[] {
            239,
            0,
            0,
            0});
			this.PAL_LastLineNumeric.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.PAL_LastLineNumeric.Name = "PAL_LastLineNumeric";
			this.PAL_LastLineNumeric.Size = new System.Drawing.Size(47, 20);
			this.PAL_LastLineNumeric.TabIndex = 43;
			this.PAL_LastLineNumeric.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// PAL_FirstLineNumeric
			// 
			this.PAL_FirstLineNumeric.Location = new System.Drawing.Point(124, 41);
			this.PAL_FirstLineNumeric.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.PAL_FirstLineNumeric.Name = "PAL_FirstLineNumeric";
			this.PAL_FirstLineNumeric.Size = new System.Drawing.Size(47, 20);
			this.PAL_FirstLineNumeric.TabIndex = 42;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(62, 22);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(36, 13);
			this.label5.TabIndex = 41;
			this.label5.Text = "NTSC";
			// 
			// btnAreaFull
			// 
			this.btnAreaFull.Location = new System.Drawing.Point(6, 115);
			this.btnAreaFull.Name = "btnAreaFull";
			this.btnAreaFull.Size = new System.Drawing.Size(96, 23);
			this.btnAreaFull.TabIndex = 40;
			this.btnAreaFull.Text = "Full [0,239]";
			this.btnAreaFull.UseVisualStyleBackColor = true;
			this.btnAreaFull.Click += new System.EventHandler(this.BtnAreaFull_Click);
			// 
			// btnAreaStandard
			// 
			this.btnAreaStandard.Location = new System.Drawing.Point(6, 92);
			this.btnAreaStandard.Name = "btnAreaStandard";
			this.btnAreaStandard.Size = new System.Drawing.Size(96, 23);
			this.btnAreaStandard.TabIndex = 35;
			this.btnAreaStandard.Text = "Standard [8,231]";
			this.btnAreaStandard.UseVisualStyleBackColor = true;
			this.btnAreaStandard.Click += new System.EventHandler(this.BtnAreaStandard_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(4, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(49, 13);
			this.label4.TabIndex = 24;
			this.label4.Text = "Last line:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(5, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 23;
			this.label3.Text = "First line:";
			// 
			// NTSC_LastLineNumeric
			// 
			this.NTSC_LastLineNumeric.Location = new System.Drawing.Point(59, 67);
			this.NTSC_LastLineNumeric.Maximum = new decimal(new int[] {
            239,
            0,
            0,
            0});
			this.NTSC_LastLineNumeric.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
			this.NTSC_LastLineNumeric.Name = "NTSC_LastLineNumeric";
			this.NTSC_LastLineNumeric.Size = new System.Drawing.Size(47, 20);
			this.NTSC_LastLineNumeric.TabIndex = 28;
			this.NTSC_LastLineNumeric.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
			// 
			// NTSC_FirstLineNumeric
			// 
			this.NTSC_FirstLineNumeric.Location = new System.Drawing.Point(59, 41);
			this.NTSC_FirstLineNumeric.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.NTSC_FirstLineNumeric.Name = "NTSC_FirstLineNumeric";
			this.NTSC_FirstLineNumeric.Size = new System.Drawing.Size(47, 20);
			this.NTSC_FirstLineNumeric.TabIndex = 21;
			// 
			// ClipLeftAndRightCheckBox
			// 
			this.ClipLeftAndRightCheckBox.AutoSize = true;
			this.ClipLeftAndRightCheckBox.Location = new System.Drawing.Point(134, 119);
			this.ClipLeftAndRightCheckBox.Name = "ClipLeftAndRightCheckBox";
			this.ClipLeftAndRightCheckBox.Size = new System.Drawing.Size(186, 17);
			this.ClipLeftAndRightCheckBox.TabIndex = 30;
			this.ClipLeftAndRightCheckBox.Text = "Clip Left and Right Sides (8 pixels)";
			this.ClipLeftAndRightCheckBox.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.checkUseBackdropColor);
			this.groupBox3.Controls.Add(this.ChangeBGColor);
			this.groupBox3.Controls.Add(this.BackGroundColorNumber);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Controls.Add(this.DispBackground);
			this.groupBox3.Controls.Add(this.DispSprites);
			this.groupBox3.Location = new System.Drawing.Point(12, 358);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(352, 113);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "BG and Sprites";
			// 
			// checkUseBackdropColor
			// 
			this.checkUseBackdropColor.AutoSize = true;
			this.checkUseBackdropColor.Checked = true;
			this.checkUseBackdropColor.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkUseBackdropColor.Location = new System.Drawing.Point(169, 87);
			this.checkUseBackdropColor.Name = "checkUseBackdropColor";
			this.checkUseBackdropColor.Size = new System.Drawing.Size(59, 17);
			this.checkUseBackdropColor.TabIndex = 60;
			this.checkUseBackdropColor.Text = "Enable";
			this.checkUseBackdropColor.UseVisualStyleBackColor = true;
			// 
			// ChangeBGColor
			// 
			this.ChangeBGColor.Location = new System.Drawing.Point(111, 83);
			this.ChangeBGColor.Name = "ChangeBGColor";
			this.ChangeBGColor.Size = new System.Drawing.Size(52, 23);
			this.ChangeBGColor.TabIndex = 55;
			this.ChangeBGColor.Text = "Change";
			this.ChangeBGColor.UseVisualStyleBackColor = true;
			this.ChangeBGColor.Click += new System.EventHandler(this.ChangeBGColor_Click);
			// 
			// BackGroundColorNumber
			// 
			this.BackGroundColorNumber.Location = new System.Drawing.Point(46, 85);
			this.BackGroundColorNumber.MaxLength = 8;
			this.BackGroundColorNumber.Name = "BackGroundColorNumber";
			this.BackGroundColorNumber.ReadOnly = true;
			this.BackGroundColorNumber.Size = new System.Drawing.Size(59, 20);
			this.BackGroundColorNumber.TabIndex = 53;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(178, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Backdrop color when BG is disabled";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.BackgroundColorPanel);
			this.groupBox4.Location = new System.Drawing.Point(8, 79);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(28, 28);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			// 
			// BackgroundColorPanel
			// 
			this.BackgroundColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BackgroundColorPanel.Location = new System.Drawing.Point(4, 8);
			this.BackgroundColorPanel.Name = "BackgroundColorPanel";
			this.BackgroundColorPanel.Size = new System.Drawing.Size(20, 16);
			this.BackgroundColorPanel.TabIndex = 0;
			this.BackgroundColorPanel.DoubleClick += new System.EventHandler(this.BackgroundColorPanel_DoubleClick);
			// 
			// DispBackground
			// 
			this.DispBackground.AutoSize = true;
			this.DispBackground.Checked = true;
			this.DispBackground.CheckState = System.Windows.Forms.CheckState.Checked;
			this.DispBackground.Location = new System.Drawing.Point(7, 42);
			this.DispBackground.Name = "DispBackground";
			this.DispBackground.Size = new System.Drawing.Size(78, 17);
			this.DispBackground.TabIndex = 50;
			this.DispBackground.Text = "Display BG";
			this.DispBackground.UseVisualStyleBackColor = true;
			// 
			// DispSprites
			// 
			this.DispSprites.AutoSize = true;
			this.DispSprites.Checked = true;
			this.DispSprites.CheckState = System.Windows.Forms.CheckState.Checked;
			this.DispSprites.Location = new System.Drawing.Point(7, 19);
			this.DispSprites.Name = "DispSprites";
			this.DispSprites.Size = new System.Drawing.Size(95, 17);
			this.DispSprites.TabIndex = 45;
			this.DispSprites.Text = "Display Sprites";
			this.DispSprites.UseVisualStyleBackColor = true;
			// 
			// RestoreDefaultsButton
			// 
			this.RestoreDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.RestoreDefaultsButton.Location = new System.Drawing.Point(12, 477);
			this.RestoreDefaultsButton.Name = "RestoreDefaultsButton";
			this.RestoreDefaultsButton.Size = new System.Drawing.Size(102, 23);
			this.RestoreDefaultsButton.TabIndex = 65;
			this.RestoreDefaultsButton.Text = "&Restore Defaults";
			this.RestoreDefaultsButton.UseVisualStyleBackColor = true;
			this.RestoreDefaultsButton.Click += new System.EventHandler(this.RestoreDefaultsButton_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(142, 67);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(183, 13);
			this.label7.TabIndex = 61;
			this.label7.Text = "(The QuickNES palette is the default)";
			// 
			// NESGraphicsConfig
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(381, 512);
			this.Controls.Add(this.RestoreDefaultsButton);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(389, 434);
			this.Name = "NESGraphicsConfig";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NES Graphics Settings";
			this.Load += new System.EventHandler(this.NESGraphicsConfig_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxPalette)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PAL_LastLineNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PAL_FirstLineNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NTSC_LastLineNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NTSC_FirstLineNumeric)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.CheckBox AllowMoreSprites;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox PalettePath;
		private System.Windows.Forms.Button BrowsePalette;
		private System.Windows.Forms.CheckBox AutoLoadPalette;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox ClipLeftAndRightCheckBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox DispSprites;
		private System.Windows.Forms.CheckBox DispBackground;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Panel BackgroundColorPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox BackGroundColorNumber;
		private System.Windows.Forms.Button ChangeBGColor;
		private System.Windows.Forms.ColorDialog BGColorDialog;
		private System.Windows.Forms.CheckBox checkUseBackdropColor;
		private System.Windows.Forms.NumericUpDown NTSC_FirstLineNumeric;
		private System.Windows.Forms.NumericUpDown NTSC_LastLineNumeric;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnAreaFull;
		private System.Windows.Forms.Button btnAreaStandard;
		private System.Windows.Forms.Button RestoreDefaultsButton;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown PAL_LastLineNumeric;
		private System.Windows.Forms.NumericUpDown PAL_FirstLineNumeric;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox pictureBoxPalette;
		private System.Windows.Forms.Label label7;
	}
}
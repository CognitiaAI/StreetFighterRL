﻿namespace BizHawk.Client.EmuHawk
{
	partial class BookmarksBranchesBox
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
			this.components = new System.ComponentModel.Container();
			this.BookmarksBranchesGroupBox = new System.Windows.Forms.GroupBox();
			this.UndoBranchButton = new System.Windows.Forms.Button();
			this.JumpToBranchButton = new System.Windows.Forms.Button();
			this.UpdateBranchButton = new System.Windows.Forms.Button();
			this.AddWithTextBranchButton = new System.Windows.Forms.Button();
			this.AddBranchButton = new System.Windows.Forms.Button();
			this.LoadBranchButton = new System.Windows.Forms.Button();
			this.BranchView = new BizHawk.Client.EmuHawk.InputRoll();
			this.BranchesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.AddBranchContextMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.AddBranchWithTextContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LoadBranchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UpdateBranchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EditBranchTextContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.JumpToBranchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UndoBranchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.RemoveBranchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.BookmarksBranchesGroupBox.SuspendLayout();
			this.BranchesContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// BookmarksBranchesGroupBox
			// 
			this.BookmarksBranchesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BookmarksBranchesGroupBox.Controls.Add(this.UndoBranchButton);
			this.BookmarksBranchesGroupBox.Controls.Add(this.JumpToBranchButton);
			this.BookmarksBranchesGroupBox.Controls.Add(this.UpdateBranchButton);
			this.BookmarksBranchesGroupBox.Controls.Add(this.AddWithTextBranchButton);
			this.BookmarksBranchesGroupBox.Controls.Add(this.AddBranchButton);
			this.BookmarksBranchesGroupBox.Controls.Add(this.LoadBranchButton);
			this.BookmarksBranchesGroupBox.Controls.Add(this.BranchView);
			this.BookmarksBranchesGroupBox.Location = new System.Drawing.Point(3, 0);
			this.BookmarksBranchesGroupBox.Name = "BookmarksBranchesGroupBox";
			this.BookmarksBranchesGroupBox.Size = new System.Drawing.Size(198, 278);
			this.BookmarksBranchesGroupBox.TabIndex = 0;
			this.BookmarksBranchesGroupBox.TabStop = false;
			this.BookmarksBranchesGroupBox.Text = "Branches";
			// 
			// UndoBranchButton
			// 
			this.UndoBranchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.UndoBranchButton.Enabled = false;
			this.UndoBranchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.undo;
			this.UndoBranchButton.Location = new System.Drawing.Point(159, 249);
			this.UndoBranchButton.Name = "UndoBranchButton";
			this.UndoBranchButton.Size = new System.Drawing.Size(23, 23);
			this.UndoBranchButton.TabIndex = 10;
			this.UndoBranchButton.UseVisualStyleBackColor = true;
			this.UndoBranchButton.Click += new System.EventHandler(this.UndoBranchToolStripMenuItem_Click);
			// 
			// JumpToBranchButton
			// 
			this.JumpToBranchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.JumpToBranchButton.Enabled = false;
			this.JumpToBranchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.JumpTo;
			this.JumpToBranchButton.Location = new System.Drawing.Point(130, 249);
			this.JumpToBranchButton.Name = "JumpToBranchButton";
			this.JumpToBranchButton.Size = new System.Drawing.Size(23, 23);
			this.JumpToBranchButton.TabIndex = 9;
			this.toolTip1.SetToolTip(this.JumpToBranchButton, "Jump To Branch Frame");
			this.JumpToBranchButton.UseVisualStyleBackColor = true;
			this.JumpToBranchButton.Click += new System.EventHandler(this.JumpToBranchToolStripMenuItem_Click);
			// 
			// UpdateBranchButton
			// 
			this.UpdateBranchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.UpdateBranchButton.Enabled = false;
			this.UpdateBranchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.reboot;
			this.UpdateBranchButton.Location = new System.Drawing.Point(99, 249);
			this.UpdateBranchButton.Name = "UpdateBranchButton";
			this.UpdateBranchButton.Size = new System.Drawing.Size(25, 23);
			this.UpdateBranchButton.TabIndex = 4;
			this.toolTip1.SetToolTip(this.UpdateBranchButton, "Update Branch");
			this.UpdateBranchButton.UseVisualStyleBackColor = true;
			this.UpdateBranchButton.Click += new System.EventHandler(this.UpdateBranchToolStripMenuItem_Click);
			// 
			// AddWithTextBranchButton
			// 
			this.AddWithTextBranchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.AddWithTextBranchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.AddEdit;
			this.AddWithTextBranchButton.Location = new System.Drawing.Point(37, 249);
			this.AddWithTextBranchButton.Name = "AddWithTextBranchButton";
			this.AddWithTextBranchButton.Size = new System.Drawing.Size(25, 23);
			this.AddWithTextBranchButton.TabIndex = 3;
			this.toolTip1.SetToolTip(this.AddWithTextBranchButton, "Add Branch with Text");
			this.AddWithTextBranchButton.UseVisualStyleBackColor = true;
			this.AddWithTextBranchButton.Click += new System.EventHandler(this.AddBranchWithTexToolStripMenuItem_Click);
			// 
			// AddBranchButton
			// 
			this.AddBranchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.AddBranchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.add;
			this.AddBranchButton.Location = new System.Drawing.Point(6, 249);
			this.AddBranchButton.Name = "AddBranchButton";
			this.AddBranchButton.Size = new System.Drawing.Size(25, 23);
			this.AddBranchButton.TabIndex = 2;
			this.toolTip1.SetToolTip(this.AddBranchButton, "Add Branch");
			this.AddBranchButton.UseVisualStyleBackColor = true;
			this.AddBranchButton.Click += new System.EventHandler(this.AddBranchToolStripMenuItem_Click);
			// 
			// LoadBranchButton
			// 
			this.LoadBranchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.LoadBranchButton.Enabled = false;
			this.LoadBranchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Debugger;
			this.LoadBranchButton.Location = new System.Drawing.Point(68, 249);
			this.LoadBranchButton.Name = "LoadBranchButton";
			this.LoadBranchButton.Size = new System.Drawing.Size(25, 23);
			this.LoadBranchButton.TabIndex = 1;
			this.toolTip1.SetToolTip(this.LoadBranchButton, "Load Branch");
			this.LoadBranchButton.UseVisualStyleBackColor = true;
			this.LoadBranchButton.Click += new System.EventHandler(this.LoadBranchToolStripMenuItem_Click);
			// 
			// BranchView
			// 
			this.BranchView.AllowColumnReorder = false;
			this.BranchView.AllowColumnResize = false;
			this.BranchView.AllowRightClickSelecton = true;
			this.BranchView.AlwaysScroll = false;
			this.BranchView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.BranchView.CellHeightPadding = 0;
			this.BranchView.ContextMenuStrip = this.BranchesContextMenu;
			this.BranchView.FullRowSelect = true;
			this.BranchView.HideWasLagFrames = false;
			this.BranchView.HorizontalOrientation = false;
			this.BranchView.LagFramesToHide = 0;
			this.BranchView.LetKeysModifySelection = false;
			this.BranchView.Location = new System.Drawing.Point(6, 19);
			this.BranchView.MaxCharactersInHorizontal = 1;
			this.BranchView.MultiSelect = false;
			this.BranchView.Name = "BranchView";
			this.BranchView.RowCount = 0;
			this.BranchView.ScrollSpeed = 13;
			this.BranchView.SeekingCutoffInterval = 0;
			this.BranchView.Size = new System.Drawing.Size(186, 224);
			this.BranchView.SuspendHotkeys = false;
			this.BranchView.TabIndex = 0;
			this.BranchView.PointedCellChanged += new BizHawk.Client.EmuHawk.InputRoll.CellChangeEventHandler(this.BranchView_PointedCellChanged);
			this.BranchView.CellDropped += new BizHawk.Client.EmuHawk.InputRoll.CellDroppedEvent(this.BranchView_CellDropped);
			this.BranchView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.BranchView_MouseDoubleClick);
			this.BranchView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BranchView_MouseDown);
			this.BranchView.MouseLeave += new System.EventHandler(this.BranchView_MouseLeave);
			this.BranchView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BranchView_MouseMove);
			this.BranchView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BranchView_MouseUp);
			// 
			// BranchesContextMenu
			// 
			this.BranchesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBranchContextMenu,
            this.AddBranchWithTextContextMenuItem,
            this.LoadBranchContextMenuItem,
            this.UpdateBranchContextMenuItem,
            this.EditBranchTextContextMenuItem,
            this.JumpToBranchContextMenuItem,
            this.UndoBranchToolStripMenuItem,
            this.toolStripSeparator2,
            this.RemoveBranchContextMenuItem});
			this.BranchesContextMenu.Name = "BranchesContextMenu";
			this.BranchesContextMenu.Size = new System.Drawing.Size(153, 208);
			this.BranchesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.BranchesContextMenu_Opening);
			// 
			// AddBranchContextMenu
			// 
			this.AddBranchContextMenu.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.add;
			this.AddBranchContextMenu.Name = "AddBranchContextMenu";
			this.AddBranchContextMenu.Size = new System.Drawing.Size(152, 22);
			this.AddBranchContextMenu.Text = "Add";
			this.AddBranchContextMenu.Click += new System.EventHandler(this.AddBranchToolStripMenuItem_Click);
			// 
			// AddBranchWithTextContextMenuItem
			// 
			this.AddBranchWithTextContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.AddEdit;
			this.AddBranchWithTextContextMenuItem.Name = "AddBranchWithTextContextMenuItem";
			this.AddBranchWithTextContextMenuItem.Size = new System.Drawing.Size(152, 22);
			this.AddBranchWithTextContextMenuItem.Text = "Add with Text";
			this.AddBranchWithTextContextMenuItem.Click += new System.EventHandler(this.AddBranchWithTexToolStripMenuItem_Click);
			// 
			// LoadBranchContextMenuItem
			// 
			this.LoadBranchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Debugger;
			this.LoadBranchContextMenuItem.Name = "LoadBranchContextMenuItem";
			this.LoadBranchContextMenuItem.Size = new System.Drawing.Size(152, 22);
			this.LoadBranchContextMenuItem.Text = "Load";
			this.LoadBranchContextMenuItem.Click += new System.EventHandler(this.LoadBranchToolStripMenuItem_Click);
			// 
			// UpdateBranchContextMenuItem
			// 
			this.UpdateBranchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.reboot;
			this.UpdateBranchContextMenuItem.Name = "UpdateBranchContextMenuItem";
			this.UpdateBranchContextMenuItem.Size = new System.Drawing.Size(152, 22);
			this.UpdateBranchContextMenuItem.Text = "&Update";
			this.UpdateBranchContextMenuItem.Click += new System.EventHandler(this.UpdateBranchToolStripMenuItem_Click);
			// 
			// EditBranchTextContextMenuItem
			// 
			this.EditBranchTextContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.pencil;
			this.EditBranchTextContextMenuItem.Name = "EditBranchTextContextMenuItem";
			this.EditBranchTextContextMenuItem.Size = new System.Drawing.Size(152, 22);
			this.EditBranchTextContextMenuItem.Text = "Edit Text";
			this.EditBranchTextContextMenuItem.Click += new System.EventHandler(this.EditBranchTextToolStripMenuItem_Click);
			// 
			// JumpToBranchContextMenuItem
			// 
			this.JumpToBranchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.JumpTo;
			this.JumpToBranchContextMenuItem.Name = "JumpToBranchContextMenuItem";
			this.JumpToBranchContextMenuItem.Size = new System.Drawing.Size(152, 22);
			this.JumpToBranchContextMenuItem.Text = "Jump To";
			this.JumpToBranchContextMenuItem.Click += new System.EventHandler(this.JumpToBranchToolStripMenuItem_Click);
			// 
			// UndoBranchToolStripMenuItem
			// 
			this.UndoBranchToolStripMenuItem.Enabled = false;
			this.UndoBranchToolStripMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.undo;
			this.UndoBranchToolStripMenuItem.Name = "UndoBranchToolStripMenuItem";
			this.UndoBranchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.UndoBranchToolStripMenuItem.Text = "Undo";
			this.UndoBranchToolStripMenuItem.Click += new System.EventHandler(this.UndoBranchToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// RemoveBranchContextMenuItem
			// 
			this.RemoveBranchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Delete;
			this.RemoveBranchContextMenuItem.Name = "RemoveBranchContextMenuItem";
			this.RemoveBranchContextMenuItem.Size = new System.Drawing.Size(152, 22);
			this.RemoveBranchContextMenuItem.Text = "Remove";
			this.RemoveBranchContextMenuItem.Click += new System.EventHandler(this.RemoveBranchToolStripMenuItem_Click);
			// 
			// BookmarksBranchesBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.Controls.Add(this.BookmarksBranchesGroupBox);
			this.Name = "BookmarksBranchesBox";
			this.Size = new System.Drawing.Size(204, 281);
			this.BookmarksBranchesGroupBox.ResumeLayout(false);
			this.BranchesContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox BookmarksBranchesGroupBox;
		private InputRoll BranchView;
		private System.Windows.Forms.ContextMenuStrip BranchesContextMenu;
		private System.Windows.Forms.ToolStripMenuItem AddBranchContextMenu;
		private System.Windows.Forms.ToolStripMenuItem RemoveBranchContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem LoadBranchContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem UpdateBranchContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EditBranchTextContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AddBranchWithTextContextMenuItem;
		private System.Windows.Forms.Button UpdateBranchButton;
		private System.Windows.Forms.Button AddWithTextBranchButton;
		private System.Windows.Forms.Button AddBranchButton;
		private System.Windows.Forms.Button LoadBranchButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Button JumpToBranchButton;
		private System.Windows.Forms.ToolStripMenuItem JumpToBranchContextMenuItem;
		private System.Windows.Forms.Button UndoBranchButton;
		private System.Windows.Forms.ToolStripMenuItem UndoBranchToolStripMenuItem;
	}
}

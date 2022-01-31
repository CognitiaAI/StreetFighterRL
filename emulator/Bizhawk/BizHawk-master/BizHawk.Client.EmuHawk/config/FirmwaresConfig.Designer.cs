﻿namespace BizHawk.Client.EmuHawk
{
    partial class FirmwaresConfig
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirmwaresConfig));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.lvFirmwares = new System.Windows.Forms.ListView();
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvFirmwaresContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiSetCustomization = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiClearCustomization = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolStrip1 = new ToolStripEx();
			this.tbbGroup = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tbbScan = new System.Windows.Forms.ToolStripButton();
			this.tbbOrganize = new System.Windows.Forms.ToolStripButton();
			this.tbbImport = new System.Windows.Forms.ToolStripButton();
			this.tbbClose = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbbCloseReload = new System.Windows.Forms.ToolStripButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.linkBasePath = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.lvFirmwaresContextMenuStrip.SuspendLayout();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// lvFirmwares
			// 
			this.lvFirmwares.AllowDrop = true;
			this.lvFirmwares.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader7});
			this.lvFirmwares.ContextMenuStrip = this.lvFirmwaresContextMenuStrip;
			this.lvFirmwares.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvFirmwares.FullRowSelect = true;
			this.lvFirmwares.GridLines = true;
			this.lvFirmwares.Location = new System.Drawing.Point(0, 25);
			this.lvFirmwares.Name = "lvFirmwares";
			this.lvFirmwares.ShowItemToolTips = true;
			this.lvFirmwares.Size = new System.Drawing.Size(824, 404);
			this.lvFirmwares.SmallImageList = this.imageList1;
			this.lvFirmwares.TabIndex = 24;
			this.lvFirmwares.UseCompatibleStateImageBehavior = false;
			this.lvFirmwares.View = System.Windows.Forms.View.Details;
			this.lvFirmwares.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFirmwares_ColumnClick);
			this.lvFirmwares.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFirmwares_DragDrop);
			this.lvFirmwares.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvFirmwares_DragEnter);
			this.lvFirmwares.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFirmwares_KeyDown);
			this.lvFirmwares.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvFirmwares_MouseClick);
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "";
			this.columnHeader5.Width = 31;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "System";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Id";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Description";
			this.columnHeader4.Width = 165;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Resolved With";
			this.columnHeader2.Width = 116;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Location";
			this.columnHeader3.Width = 252;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Size";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Hash";
			this.columnHeader7.Width = 250;
			// 
			// lvFirmwaresContextMenuStrip
			// 
			this.lvFirmwaresContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetCustomization,
            this.tsmiClearCustomization,
            this.tsmiInfo,
            this.tsmiCopy});
			this.lvFirmwaresContextMenuStrip.Name = "lvFirmwaresContextMenuStrip";
			this.lvFirmwaresContextMenuStrip.Size = new System.Drawing.Size(182, 92);
			this.lvFirmwaresContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.lvFirmwaresContextMenuStrip_Opening);
			// 
			// tsmiSetCustomization
			// 
			this.tsmiSetCustomization.Name = "tsmiSetCustomization";
			this.tsmiSetCustomization.Size = new System.Drawing.Size(181, 22);
			this.tsmiSetCustomization.Text = "&Set Customization";
			this.tsmiSetCustomization.Click += new System.EventHandler(this.tsmiSetCustomization_Click);
			// 
			// tsmiClearCustomization
			// 
			this.tsmiClearCustomization.Name = "tsmiClearCustomization";
			this.tsmiClearCustomization.Size = new System.Drawing.Size(181, 22);
			this.tsmiClearCustomization.Text = "C&lear Customization";
			this.tsmiClearCustomization.Click += new System.EventHandler(this.tsmiClearCustomization_Click);
			// 
			// tsmiInfo
			// 
			this.tsmiInfo.Name = "tsmiInfo";
			this.tsmiInfo.Size = new System.Drawing.Size(181, 22);
			this.tsmiInfo.Text = "&Info";
			this.tsmiInfo.Click += new System.EventHandler(this.tsmiInfo_Click);
			// 
			// tsmiCopy
			// 
			this.tsmiCopy.Name = "tsmiCopy";
			this.tsmiCopy.Size = new System.Drawing.Size(181, 22);
			this.tsmiCopy.Text = "&Copy";
			this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lvFirmwares);
			this.panel1.Controls.Add(this.toolStrip1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 23);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(824, 429);
			this.panel1.TabIndex = 24;
			// 
			// toolStrip1
			// 
			this.toolStrip1.ClickThrough = true;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbGroup,
            this.toolStripSeparator2,
            this.tbbScan,
            this.tbbOrganize,
            this.tbbImport,
            this.tbbClose,
            this.toolStripSeparator1,
            this.tbbCloseReload});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(824, 25);
			this.toolStrip1.TabIndex = 23;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tbbGroup
			// 
			this.tbbGroup.Checked = true;
			this.tbbGroup.CheckOnClick = true;
			this.tbbGroup.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tbbGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbGroup.Image = ((System.Drawing.Image)(resources.GetObject("tbbGroup.Image")));
			this.tbbGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbGroup.Name = "tbbGroup";
			this.tbbGroup.Size = new System.Drawing.Size(44, 22);
			this.tbbGroup.Text = "Group";
			this.tbbGroup.Click += new System.EventHandler(this.tbbGroup_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tbbScan
			// 
			this.tbbScan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbScan.Image = ((System.Drawing.Image)(resources.GetObject("tbbScan.Image")));
			this.tbbScan.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbScan.Name = "tbbScan";
			this.tbbScan.Size = new System.Drawing.Size(36, 22);
			this.tbbScan.Text = "Scan";
			this.tbbScan.Click += new System.EventHandler(this.tbbScan_Click);
			// 
			// tbbOrganize
			// 
			this.tbbOrganize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbOrganize.Image = ((System.Drawing.Image)(resources.GetObject("tbbOrganize.Image")));
			this.tbbOrganize.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbOrganize.Name = "tbbOrganize";
			this.tbbOrganize.Size = new System.Drawing.Size(58, 22);
			this.tbbOrganize.Text = "Organize";
			this.tbbOrganize.Click += new System.EventHandler(this.tbbOrganize_Click);
			// 
			// tbbImport
			// 
			this.tbbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbImport.Image = ((System.Drawing.Image)(resources.GetObject("tbbImport.Image")));
			this.tbbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbImport.Name = "tbbImport";
			this.tbbImport.Size = new System.Drawing.Size(47, 22);
			this.tbbImport.Text = "Import";
			this.tbbImport.Click += new System.EventHandler(this.tbbImport_Click);
			// 
			// tbbClose
			// 
			this.tbbClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tbbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbClose.Image = ((System.Drawing.Image)(resources.GetObject("tbbClose.Image")));
			this.tbbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbClose.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
			this.tbbClose.Name = "tbbClose";
			this.tbbClose.Size = new System.Drawing.Size(40, 22);
			this.tbbClose.Text = "Close";
			this.tbbClose.Click += new System.EventHandler(this.tbbClose_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			this.toolStripSeparator1.Visible = false;
			// 
			// tbbCloseReload
			// 
			this.tbbCloseReload.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tbbCloseReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbbCloseReload.Enabled = false;
			this.tbbCloseReload.Image = ((System.Drawing.Image)(resources.GetObject("tbbCloseReload.Image")));
			this.tbbCloseReload.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbCloseReload.Name = "tbbCloseReload";
			this.tbbCloseReload.Size = new System.Drawing.Size(129, 22);
			this.tbbCloseReload.Text = "Close and reload ROM";
			this.tbbCloseReload.ToolTipText = "Close and reload ROM";
			this.tbbCloseReload.Visible = false;
			this.tbbCloseReload.Click += new System.EventHandler(this.tbbCloseReload_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(830, 478);
			this.tableLayoutPanel1.TabIndex = 25;
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.linkBasePath);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 458);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(824, 17);
			this.panel2.TabIndex = 26;
			// 
			// linkBasePath
			// 
			this.linkBasePath.AutoSize = true;
			this.linkBasePath.Location = new System.Drawing.Point(125, 0);
			this.linkBasePath.Name = "linkBasePath";
			this.linkBasePath.Size = new System.Drawing.Size(55, 13);
			this.linkBasePath.TabIndex = 27;
			this.linkBasePath.TabStop = true;
			this.linkBasePath.Text = "linkLabel1";
			this.linkBasePath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBasePath_LinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(119, 13);
			this.label1.TabIndex = 25;
			this.label1.Text = "Firmwares Search Path:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 5);
			this.label2.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(383, 13);
			this.label2.TabIndex = 27;
			this.label2.Text = "Firmware such as BIOS files are copyrighted material and not provided by BizHawk";
			// 
			// FirmwaresConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(830, 478);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FirmwaresConfig";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Firmwares";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FirmwaresConfig_FormClosed);
			this.Load += new System.EventHandler(this.FirmwaresConfig_Load);
			this.lvFirmwaresContextMenuStrip.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

				private System.Windows.Forms.ImageList imageList1;
				private System.Windows.Forms.ListView lvFirmwares;
				private System.Windows.Forms.ColumnHeader columnHeader5;
				private System.Windows.Forms.ColumnHeader columnHeader1;
				private System.Windows.Forms.ColumnHeader columnHeader4;
				private System.Windows.Forms.Panel panel1;
				private ToolStripEx toolStrip1;
				private System.Windows.Forms.ToolStripButton tbbGroup;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
				private System.Windows.Forms.ToolStripButton tbbScan;
				private System.Windows.Forms.ToolStripButton tbbOrganize;
				private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
				private System.Windows.Forms.ColumnHeader columnHeader6;
				private System.Windows.Forms.ColumnHeader columnHeader2;
				private System.Windows.Forms.ToolTip toolTip1;
				private System.Windows.Forms.ColumnHeader columnHeader3;
				private System.Windows.Forms.ColumnHeader columnHeader7;
				private System.Windows.Forms.ContextMenuStrip lvFirmwaresContextMenuStrip;
				private System.Windows.Forms.ToolStripMenuItem tsmiSetCustomization;
				private System.Windows.Forms.ToolStripMenuItem tsmiClearCustomization;
				private System.Windows.Forms.ToolStripMenuItem tsmiInfo;
				private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
				private System.Windows.Forms.Panel panel2;
				private System.Windows.Forms.LinkLabel linkBasePath;
				private System.Windows.Forms.Label label1;
				private System.Windows.Forms.ToolStripButton tbbImport;
				private System.Windows.Forms.ColumnHeader columnHeader8;
				private System.Windows.Forms.ToolStripButton tbbClose;
				private System.Windows.Forms.ToolStripButton tbbCloseReload;
				private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Label label2;
	}
}
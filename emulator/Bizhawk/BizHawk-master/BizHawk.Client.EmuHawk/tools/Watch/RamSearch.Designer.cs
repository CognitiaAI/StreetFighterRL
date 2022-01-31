﻿namespace BizHawk.Client.EmuHawk
{
	partial class RamSearch
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
			System.Windows.Forms.ToolStripMenuItem SearchMenuItem;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RamSearch));
			this.TotalSearchLabel = new System.Windows.Forms.Label();
			this.WatchListView = new BizHawk.Client.EmuHawk.VirtualListView();
			this.AddressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PreviousColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChangesColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DiffColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ListViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.DoSearchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewSearchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.RemoveContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddToRamWatchContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PokeContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FreezeContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UnfreezeAllContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.ViewInHexEditorContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.ClearPreviewContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RamSearchMenu = new MenuStripEx();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AppendFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TruncateFromFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RecentSubMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DetailedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MemoryDomainsSubMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ByteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.WordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DWordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CheckMisalignedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.BigEndianMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DisplayTypeSubMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.DefinePreviousValueSubMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Previous_LastSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PreviousFrameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Previous_OriginalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Previous_LastChangeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.UndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RedoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyValueToPrevMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearChangeCountsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.GoToAddressMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddToRamWatchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PokeAddressMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FreezeAddressMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.ClearUndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PreviewModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AutoSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AutoSearchAccountForLagMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.ExcludeRamWatchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UseUndoHistoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.AutoloadDialogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveWinPositionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AlwaysOnTopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FloatingWindowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.RestoreDefaultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MemDomainLabel = new System.Windows.Forms.Label();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.AutoSearchCheckBox = new System.Windows.Forms.CheckBox();
			this.CompareToBox = new System.Windows.Forms.GroupBox();
			this.DifferenceBox = new BizHawk.Client.EmuHawk.WatchValueBox();
			this.DifferenceRadio = new System.Windows.Forms.RadioButton();
			this.NumberOfChangesBox = new BizHawk.Client.EmuHawk.UnsignedIntegerBox();
			this.SpecificAddressBox = new BizHawk.Client.EmuHawk.HexTextBox();
			this.SpecificValueBox = new BizHawk.Client.EmuHawk.WatchValueBox();
			this.NumberOfChangesRadio = new System.Windows.Forms.RadioButton();
			this.SpecificAddressRadio = new System.Windows.Forms.RadioButton();
			this.SpecificValueRadio = new System.Windows.Forms.RadioButton();
			this.PreviousValueRadio = new System.Windows.Forms.RadioButton();
			this.toolStrip1 = new ToolStripEx();
			this.DoSearchToolButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.NewSearchToolButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.CopyValueToPrevToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.ClearChangeCountsToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.RemoveToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.AddToRamWatchToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.PokeAddressToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.FreezeAddressToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.UndoToolBarButton = new System.Windows.Forms.ToolStripButton();
			this.RedoToolBarItem = new System.Windows.Forms.ToolStripButton();
			this.RebootToolBarSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.RebootToolbarButton = new System.Windows.Forms.ToolStripButton();
			this.ErrorIconButton = new System.Windows.Forms.ToolStripButton();
			this.ComparisonBox = new System.Windows.Forms.GroupBox();
			this.DifferentByBox = new BizHawk.Client.EmuHawk.WatchValueBox();
			this.DifferentByRadio = new System.Windows.Forms.RadioButton();
			this.NotEqualToRadio = new System.Windows.Forms.RadioButton();
			this.EqualToRadio = new System.Windows.Forms.RadioButton();
			this.GreaterThanOrEqualToRadio = new System.Windows.Forms.RadioButton();
			this.LessThanOrEqualToRadio = new System.Windows.Forms.RadioButton();
			this.GreaterThanRadio = new System.Windows.Forms.RadioButton();
			this.LessThanRadio = new System.Windows.Forms.RadioButton();
			this.SearchButton = new System.Windows.Forms.Button();
			this.SizeDropdown = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.DisplayTypeDropdown = new System.Windows.Forms.ComboBox();
			SearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ListViewContextMenu.SuspendLayout();
			this.RamSearchMenu.SuspendLayout();
			this.CompareToBox.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.ComparisonBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// SearchMenuItem
			// 
			SearchMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.search;
			SearchMenuItem.Name = "SearchMenuItem";
			SearchMenuItem.Size = new System.Drawing.Size(221, 22);
			SearchMenuItem.Text = "&Search";
			SearchMenuItem.Click += new System.EventHandler(this.SearchMenuItem_Click);
			// 
			// TotalSearchLabel
			// 
			this.TotalSearchLabel.AutoSize = true;
			this.TotalSearchLabel.Location = new System.Drawing.Point(12, 49);
			this.TotalSearchLabel.Name = "TotalSearchLabel";
			this.TotalSearchLabel.Size = new System.Drawing.Size(64, 13);
			this.TotalSearchLabel.TabIndex = 2;
			this.TotalSearchLabel.Text = "0 addresses";
			// 
			// WatchListView
			// 
			this.WatchListView.AllowColumnReorder = true;
			this.WatchListView.AllowDrop = true;
			this.WatchListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.WatchListView.BlazingFast = false;
			this.WatchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AddressColumn,
            this.ValueColumn,
            this.PreviousColumn,
            this.ChangesColumn,
            this.DiffColumn});
			this.WatchListView.ContextMenuStrip = this.ListViewContextMenu;
			this.WatchListView.FullRowSelect = true;
			this.WatchListView.GridLines = true;
			this.WatchListView.HideSelection = false;
			this.WatchListView.ItemCount = 0;
			this.WatchListView.Location = new System.Drawing.Point(9, 65);
			this.WatchListView.Name = "WatchListView";
			this.WatchListView.SelectAllInProgress = false;
			this.WatchListView.selectedItem = -1;
			this.WatchListView.Size = new System.Drawing.Size(230, 366);
			this.WatchListView.TabIndex = 1;
			this.WatchListView.UseCompatibleStateImageBehavior = false;
			this.WatchListView.UseCustomBackground = true;
			this.WatchListView.View = System.Windows.Forms.View.Details;
			this.WatchListView.VirtualMode = true;
			this.WatchListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.WatchListView_ColumnClick);
			this.WatchListView.SelectedIndexChanged += new System.EventHandler(this.WatchListView_SelectedIndexChanged);
			this.WatchListView.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.WatchListView_VirtualItemsSelectionRangeChanged);
			this.WatchListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.NewRamSearch_DragDrop);
			this.WatchListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterWrapper);
			this.WatchListView.Enter += new System.EventHandler(this.WatchListView_Enter);
			this.WatchListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WatchListView_KeyDown);
			this.WatchListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.WatchListView_MouseDoubleClick);
			// 
			// AddressColumn
			// 
			this.AddressColumn.Text = "Address";
			this.AddressColumn.Width = 61;
			// 
			// ValueColumn
			// 
			this.ValueColumn.Text = "Value";
			this.ValueColumn.Width = 48;
			// 
			// PreviousColumn
			// 
			this.PreviousColumn.Text = "Prev";
			this.PreviousColumn.Width = 48;
			// 
			// ChangesColumn
			// 
			this.ChangesColumn.Text = "Changes";
			this.ChangesColumn.Width = 55;
			// 
			// DiffColumn
			// 
			this.DiffColumn.Text = "Diff";
			// 
			// ListViewContextMenu
			// 
			this.ListViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DoSearchContextMenuItem,
            this.NewSearchContextMenuItem,
            this.ContextMenuSeparator1,
            this.RemoveContextMenuItem,
            this.AddToRamWatchContextMenuItem,
            this.PokeContextMenuItem,
            this.FreezeContextMenuItem,
            this.UnfreezeAllContextMenuItem,
            this.ContextMenuSeparator2,
            this.ViewInHexEditorContextMenuItem,
            this.ContextMenuSeparator3,
            this.ClearPreviewContextMenuItem});
			this.ListViewContextMenu.Name = "contextMenuStrip1";
			this.ListViewContextMenu.Size = new System.Drawing.Size(218, 220);
			this.ListViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ListViewContextMenu_Opening);
			// 
			// DoSearchContextMenuItem
			// 
			this.DoSearchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.search;
			this.DoSearchContextMenuItem.Name = "DoSearchContextMenuItem";
			this.DoSearchContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.DoSearchContextMenuItem.Text = "&Search";
			this.DoSearchContextMenuItem.Click += new System.EventHandler(this.SearchMenuItem_Click);
			// 
			// NewSearchContextMenuItem
			// 
			this.NewSearchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.restart;
			this.NewSearchContextMenuItem.Name = "NewSearchContextMenuItem";
			this.NewSearchContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.NewSearchContextMenuItem.Text = "&Start New Search";
			this.NewSearchContextMenuItem.Click += new System.EventHandler(this.NewSearchMenuMenuItem_Click);
			// 
			// ContextMenuSeparator1
			// 
			this.ContextMenuSeparator1.Name = "ContextMenuSeparator1";
			this.ContextMenuSeparator1.Size = new System.Drawing.Size(214, 6);
			// 
			// RemoveContextMenuItem
			// 
			this.RemoveContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Delete;
			this.RemoveContextMenuItem.Name = "RemoveContextMenuItem";
			this.RemoveContextMenuItem.ShortcutKeyDisplayString = "Del";
			this.RemoveContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.RemoveContextMenuItem.Text = "Remove Selected";
			this.RemoveContextMenuItem.Click += new System.EventHandler(this.RemoveMenuItem_Click);
			// 
			// AddToRamWatchContextMenuItem
			// 
			this.AddToRamWatchContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.FindHS;
			this.AddToRamWatchContextMenuItem.Name = "AddToRamWatchContextMenuItem";
			this.AddToRamWatchContextMenuItem.ShortcutKeyDisplayString = "Ctrl+R";
			this.AddToRamWatchContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.AddToRamWatchContextMenuItem.Text = "Add to RAM Watch";
			this.AddToRamWatchContextMenuItem.Click += new System.EventHandler(this.AddToRamWatchMenuItem_Click);
			// 
			// PokeContextMenuItem
			// 
			this.PokeContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.poke;
			this.PokeContextMenuItem.Name = "PokeContextMenuItem";
			this.PokeContextMenuItem.ShortcutKeyDisplayString = "Ctrl+P";
			this.PokeContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.PokeContextMenuItem.Text = "Poke Address";
			this.PokeContextMenuItem.Click += new System.EventHandler(this.PokeAddressMenuItem_Click);
			// 
			// FreezeContextMenuItem
			// 
			this.FreezeContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Freeze;
			this.FreezeContextMenuItem.Name = "FreezeContextMenuItem";
			this.FreezeContextMenuItem.ShortcutKeyDisplayString = "Ctrl+F";
			this.FreezeContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.FreezeContextMenuItem.Text = "Freeze Address";
			this.FreezeContextMenuItem.Click += new System.EventHandler(this.FreezeAddressMenuItem_Click);
			// 
			// UnfreezeAllContextMenuItem
			// 
			this.UnfreezeAllContextMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Unfreeze;
			this.UnfreezeAllContextMenuItem.Name = "UnfreezeAllContextMenuItem";
			this.UnfreezeAllContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.UnfreezeAllContextMenuItem.Text = "Unfreeze &All";
			this.UnfreezeAllContextMenuItem.Click += new System.EventHandler(this.UnfreezeAllContextMenuItem_Click);
			// 
			// ContextMenuSeparator2
			// 
			this.ContextMenuSeparator2.Name = "ContextMenuSeparator2";
			this.ContextMenuSeparator2.Size = new System.Drawing.Size(214, 6);
			// 
			// ViewInHexEditorContextMenuItem
			// 
			this.ViewInHexEditorContextMenuItem.Name = "ViewInHexEditorContextMenuItem";
			this.ViewInHexEditorContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.ViewInHexEditorContextMenuItem.Text = "View in Hex Editor";
			this.ViewInHexEditorContextMenuItem.Click += new System.EventHandler(this.ViewInHexEditorContextMenuItem_Click);
			// 
			// ContextMenuSeparator3
			// 
			this.ContextMenuSeparator3.Name = "ContextMenuSeparator3";
			this.ContextMenuSeparator3.Size = new System.Drawing.Size(214, 6);
			// 
			// ClearPreviewContextMenuItem
			// 
			this.ClearPreviewContextMenuItem.Name = "ClearPreviewContextMenuItem";
			this.ClearPreviewContextMenuItem.Size = new System.Drawing.Size(217, 22);
			this.ClearPreviewContextMenuItem.Text = "&Clear Preview";
			this.ClearPreviewContextMenuItem.Click += new System.EventHandler(this.ClearPreviewContextMenuItem_Click);
			// 
			// RamSearchMenu
			// 
			this.RamSearchMenu.ClickThrough = true;
			this.RamSearchMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.RamSearchMenu.Location = new System.Drawing.Point(0, 0);
			this.RamSearchMenu.Name = "RamSearchMenu";
			this.RamSearchMenu.Size = new System.Drawing.Size(445, 24);
			this.RamSearchMenu.TabIndex = 4;
			this.RamSearchMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.SaveMenuItem,
            this.SaveAsMenuItem,
            this.AppendFileMenuItem,
            this.TruncateFromFileMenuItem,
            this.RecentSubMenu,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			this.fileToolStripMenuItem.DropDownOpened += new System.EventHandler(this.FileSubMenu_DropDownOpened);
			// 
			// OpenMenuItem
			// 
			this.OpenMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.OpenFile;
			this.OpenMenuItem.Name = "OpenMenuItem";
			this.OpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.OpenMenuItem.Size = new System.Drawing.Size(195, 22);
			this.OpenMenuItem.Text = "&Open...";
			this.OpenMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
			// 
			// SaveMenuItem
			// 
			this.SaveMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.SaveAs;
			this.SaveMenuItem.Name = "SaveMenuItem";
			this.SaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.SaveMenuItem.Size = new System.Drawing.Size(195, 22);
			this.SaveMenuItem.Text = "&Save";
			this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
			// 
			// SaveAsMenuItem
			// 
			this.SaveAsMenuItem.Name = "SaveAsMenuItem";
			this.SaveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.SaveAsMenuItem.Size = new System.Drawing.Size(195, 22);
			this.SaveAsMenuItem.Text = "Save As...";
			this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
			// 
			// AppendFileMenuItem
			// 
			this.AppendFileMenuItem.Name = "AppendFileMenuItem";
			this.AppendFileMenuItem.Size = new System.Drawing.Size(195, 22);
			this.AppendFileMenuItem.Text = "&Append File...";
			this.AppendFileMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
			// 
			// TruncateFromFileMenuItem
			// 
			this.TruncateFromFileMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.TruncateFromFile;
			this.TruncateFromFileMenuItem.Name = "TruncateFromFileMenuItem";
			this.TruncateFromFileMenuItem.Size = new System.Drawing.Size(195, 22);
			this.TruncateFromFileMenuItem.Text = "&Truncate from File...";
			this.TruncateFromFileMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
			// 
			// RecentSubMenu
			// 
			this.RecentSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2});
			this.RecentSubMenu.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Recent;
			this.RecentSubMenu.Name = "RecentSubMenu";
			this.RecentSubMenu.Size = new System.Drawing.Size(195, 22);
			this.RecentSubMenu.Text = "Recent";
			this.RecentSubMenu.DropDownOpened += new System.EventHandler(this.RecentSubMenu_DropDownOpened);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(57, 6);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.exitToolStripMenuItem.Text = "&Close";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modeToolStripMenuItem,
            this.MemoryDomainsSubMenu,
            this.sizeToolStripMenuItem,
            this.CheckMisalignedMenuItem,
            this.toolStripSeparator8,
            this.BigEndianMenuItem,
            this.DisplayTypeSubMenu,
            this.DefinePreviousValueSubMenu});
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.settingsToolStripMenuItem.Text = "&Settings";
			this.settingsToolStripMenuItem.DropDownOpened += new System.EventHandler(this.SettingsSubMenu_DropDownOpened);
			// 
			// modeToolStripMenuItem
			// 
			this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DetailedMenuItem,
            this.FastMenuItem});
			this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
			this.modeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.modeToolStripMenuItem.Text = "&Mode";
			this.modeToolStripMenuItem.DropDownOpened += new System.EventHandler(this.ModeSubMenu_DropDownOpened);
			// 
			// DetailedMenuItem
			// 
			this.DetailedMenuItem.Name = "DetailedMenuItem";
			this.DetailedMenuItem.Size = new System.Drawing.Size(117, 22);
			this.DetailedMenuItem.Text = "&Detailed";
			this.DetailedMenuItem.Click += new System.EventHandler(this.DetailedMenuItem_Click);
			// 
			// FastMenuItem
			// 
			this.FastMenuItem.Name = "FastMenuItem";
			this.FastMenuItem.Size = new System.Drawing.Size(117, 22);
			this.FastMenuItem.Text = "&Fast";
			this.FastMenuItem.Click += new System.EventHandler(this.FastMenuItem_Click);
			// 
			// MemoryDomainsSubMenu
			// 
			this.MemoryDomainsSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator6});
			this.MemoryDomainsSubMenu.Name = "MemoryDomainsSubMenu";
			this.MemoryDomainsSubMenu.Size = new System.Drawing.Size(187, 22);
			this.MemoryDomainsSubMenu.Text = "&Memory Domains";
			this.MemoryDomainsSubMenu.DropDownOpened += new System.EventHandler(this.MemoryDomainsSubMenu_DropDownOpened);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(57, 6);
			// 
			// sizeToolStripMenuItem
			// 
			this.sizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ByteMenuItem,
            this.WordMenuItem,
            this.DWordMenuItem});
			this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
			this.sizeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.sizeToolStripMenuItem.Text = "&Size";
			this.sizeToolStripMenuItem.DropDownOpened += new System.EventHandler(this.SizeSubMenu_DropDownOpened);
			// 
			// ByteMenuItem
			// 
			this.ByteMenuItem.Name = "ByteMenuItem";
			this.ByteMenuItem.Size = new System.Drawing.Size(106, 22);
			this.ByteMenuItem.Text = "&1 Byte";
			this.ByteMenuItem.Click += new System.EventHandler(this.ByteMenuItem_Click);
			// 
			// WordMenuItem
			// 
			this.WordMenuItem.Name = "WordMenuItem";
			this.WordMenuItem.Size = new System.Drawing.Size(106, 22);
			this.WordMenuItem.Text = "&2 Byte";
			this.WordMenuItem.Click += new System.EventHandler(this.WordMenuItem_Click);
			// 
			// DWordMenuItem
			// 
			this.DWordMenuItem.Name = "DWordMenuItem";
			this.DWordMenuItem.Size = new System.Drawing.Size(106, 22);
			this.DWordMenuItem.Text = "&4 Byte";
			this.DWordMenuItem.Click += new System.EventHandler(this.DWordMenuItem_Click_Click);
			// 
			// CheckMisalignedMenuItem
			// 
			this.CheckMisalignedMenuItem.Name = "CheckMisalignedMenuItem";
			this.CheckMisalignedMenuItem.Size = new System.Drawing.Size(187, 22);
			this.CheckMisalignedMenuItem.Text = "Check Mis-aligned";
			this.CheckMisalignedMenuItem.Click += new System.EventHandler(this.CheckMisalignedMenuItem_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(184, 6);
			// 
			// BigEndianMenuItem
			// 
			this.BigEndianMenuItem.Name = "BigEndianMenuItem";
			this.BigEndianMenuItem.Size = new System.Drawing.Size(187, 22);
			this.BigEndianMenuItem.Text = "&Big Endian";
			this.BigEndianMenuItem.Click += new System.EventHandler(this.BigEndianMenuItem_Click);
			// 
			// DisplayTypeSubMenu
			// 
			this.DisplayTypeSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1});
			this.DisplayTypeSubMenu.Name = "DisplayTypeSubMenu";
			this.DisplayTypeSubMenu.Size = new System.Drawing.Size(187, 22);
			this.DisplayTypeSubMenu.Text = "&Display Type";
			this.DisplayTypeSubMenu.DropDownOpened += new System.EventHandler(this.DisplayTypeSubMenu_DropDownOpened);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(57, 6);
			// 
			// DefinePreviousValueSubMenu
			// 
			this.DefinePreviousValueSubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Previous_LastSearchMenuItem,
            this.PreviousFrameMenuItem,
            this.Previous_OriginalMenuItem,
            this.Previous_LastChangeMenuItem});
			this.DefinePreviousValueSubMenu.Name = "DefinePreviousValueSubMenu";
			this.DefinePreviousValueSubMenu.Size = new System.Drawing.Size(187, 22);
			this.DefinePreviousValueSubMenu.Text = "Define Previous Value";
			this.DefinePreviousValueSubMenu.DropDownOpened += new System.EventHandler(this.DefinePreviousValueSubMenu_DropDownOpened);
			// 
			// Previous_LastSearchMenuItem
			// 
			this.Previous_LastSearchMenuItem.Name = "Previous_LastSearchMenuItem";
			this.Previous_LastSearchMenuItem.Size = new System.Drawing.Size(155, 22);
			this.Previous_LastSearchMenuItem.Text = "Last &Search";
			this.Previous_LastSearchMenuItem.Click += new System.EventHandler(this.Previous_LastSearchMenuItem_Click);
			// 
			// PreviousFrameMenuItem
			// 
			this.PreviousFrameMenuItem.Name = "PreviousFrameMenuItem";
			this.PreviousFrameMenuItem.Size = new System.Drawing.Size(155, 22);
			this.PreviousFrameMenuItem.Text = "&Previous Frame";
			this.PreviousFrameMenuItem.Click += new System.EventHandler(this.Previous_LastFrameMenuItem_Click);
			// 
			// Previous_OriginalMenuItem
			// 
			this.Previous_OriginalMenuItem.Name = "Previous_OriginalMenuItem";
			this.Previous_OriginalMenuItem.Size = new System.Drawing.Size(155, 22);
			this.Previous_OriginalMenuItem.Text = "&Original";
			this.Previous_OriginalMenuItem.Click += new System.EventHandler(this.Previous_OriginalMenuItem_Click);
			// 
			// Previous_LastChangeMenuItem
			// 
			this.Previous_LastChangeMenuItem.Name = "Previous_LastChangeMenuItem";
			this.Previous_LastChangeMenuItem.Size = new System.Drawing.Size(155, 22);
			this.Previous_LastChangeMenuItem.Text = "Last &Change";
			this.Previous_LastChangeMenuItem.Click += new System.EventHandler(this.Previous_LastChangeMenuItem_Click);
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSearchToolStripMenuItem,
            this.toolStripSeparator7,
            SearchMenuItem,
            this.UndoMenuItem,
            this.RedoMenuItem,
            this.CopyValueToPrevMenuItem,
            this.ClearChangeCountsMenuItem,
            this.RemoveMenuItem,
            this.toolStripSeparator5,
            this.GoToAddressMenuItem,
            this.AddToRamWatchMenuItem,
            this.PokeAddressMenuItem,
            this.FreezeAddressMenuItem,
            this.toolStripSeparator13,
            this.ClearUndoMenuItem});
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.searchToolStripMenuItem.Text = "&Search";
			this.searchToolStripMenuItem.DropDownOpened += new System.EventHandler(this.SearchSubMenu_DropDownOpened);
			// 
			// newSearchToolStripMenuItem
			// 
			this.newSearchToolStripMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.restart;
			this.newSearchToolStripMenuItem.Name = "newSearchToolStripMenuItem";
			this.newSearchToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
			this.newSearchToolStripMenuItem.Text = "&New Search";
			this.newSearchToolStripMenuItem.Click += new System.EventHandler(this.NewSearchMenuMenuItem_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(218, 6);
			// 
			// UndoMenuItem
			// 
			this.UndoMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.undo;
			this.UndoMenuItem.Name = "UndoMenuItem";
			this.UndoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.UndoMenuItem.Size = new System.Drawing.Size(221, 22);
			this.UndoMenuItem.Text = "&Undo";
			this.UndoMenuItem.Click += new System.EventHandler(this.UndoMenuItem_Click);
			// 
			// RedoMenuItem
			// 
			this.RedoMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.redo;
			this.RedoMenuItem.Name = "RedoMenuItem";
			this.RedoMenuItem.ShortcutKeyDisplayString = "";
			this.RedoMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.RedoMenuItem.Size = new System.Drawing.Size(221, 22);
			this.RedoMenuItem.Text = "&Redo";
			this.RedoMenuItem.Click += new System.EventHandler(this.RedoMenuItem_Click);
			// 
			// CopyValueToPrevMenuItem
			// 
			this.CopyValueToPrevMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Previous;
			this.CopyValueToPrevMenuItem.Name = "CopyValueToPrevMenuItem";
			this.CopyValueToPrevMenuItem.Size = new System.Drawing.Size(221, 22);
			this.CopyValueToPrevMenuItem.Text = "Copy Value to Prev";
			this.CopyValueToPrevMenuItem.Click += new System.EventHandler(this.CopyValueToPrevMenuItem_Click);
			// 
			// ClearChangeCountsMenuItem
			// 
			this.ClearChangeCountsMenuItem.Name = "ClearChangeCountsMenuItem";
			this.ClearChangeCountsMenuItem.Size = new System.Drawing.Size(221, 22);
			this.ClearChangeCountsMenuItem.Text = "&Clear Change Counts";
			this.ClearChangeCountsMenuItem.Click += new System.EventHandler(this.ClearChangeCountsMenuItem_Click);
			// 
			// RemoveMenuItem
			// 
			this.RemoveMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Delete;
			this.RemoveMenuItem.Name = "RemoveMenuItem";
			this.RemoveMenuItem.ShortcutKeyDisplayString = "Delete";
			this.RemoveMenuItem.Size = new System.Drawing.Size(221, 22);
			this.RemoveMenuItem.Text = "&Remove selected";
			this.RemoveMenuItem.Click += new System.EventHandler(this.RemoveMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(218, 6);
			// 
			// GoToAddressMenuItem
			// 
			this.GoToAddressMenuItem.Name = "GoToAddressMenuItem";
			this.GoToAddressMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.GoToAddressMenuItem.Size = new System.Drawing.Size(221, 22);
			this.GoToAddressMenuItem.Text = "&Go to Address...";
			this.GoToAddressMenuItem.Click += new System.EventHandler(this.GoToAddressMenuItem_Click);
			// 
			// AddToRamWatchMenuItem
			// 
			this.AddToRamWatchMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.FindHS;
			this.AddToRamWatchMenuItem.Name = "AddToRamWatchMenuItem";
			this.AddToRamWatchMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.AddToRamWatchMenuItem.Size = new System.Drawing.Size(221, 22);
			this.AddToRamWatchMenuItem.Text = "&Add to RAM Watch";
			this.AddToRamWatchMenuItem.Click += new System.EventHandler(this.AddToRamWatchMenuItem_Click);
			// 
			// PokeAddressMenuItem
			// 
			this.PokeAddressMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.poke;
			this.PokeAddressMenuItem.Name = "PokeAddressMenuItem";
			this.PokeAddressMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.PokeAddressMenuItem.Size = new System.Drawing.Size(221, 22);
			this.PokeAddressMenuItem.Text = "&Poke Address";
			this.PokeAddressMenuItem.Click += new System.EventHandler(this.PokeAddressMenuItem_Click);
			// 
			// FreezeAddressMenuItem
			// 
			this.FreezeAddressMenuItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Freeze;
			this.FreezeAddressMenuItem.Name = "FreezeAddressMenuItem";
			this.FreezeAddressMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.FreezeAddressMenuItem.Size = new System.Drawing.Size(221, 22);
			this.FreezeAddressMenuItem.Text = "Freeze Address";
			this.FreezeAddressMenuItem.Click += new System.EventHandler(this.FreezeAddressMenuItem_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(218, 6);
			// 
			// ClearUndoMenuItem
			// 
			this.ClearUndoMenuItem.Name = "ClearUndoMenuItem";
			this.ClearUndoMenuItem.Size = new System.Drawing.Size(221, 22);
			this.ClearUndoMenuItem.Text = "Clear Undo History";
			this.ClearUndoMenuItem.Click += new System.EventHandler(this.ClearUndoMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PreviewModeMenuItem,
            this.AutoSearchMenuItem,
            this.AutoSearchAccountForLagMenuItem,
            this.toolStripSeparator9,
            this.ExcludeRamWatchMenuItem,
            this.UseUndoHistoryMenuItem,
            this.toolStripSeparator11,
            this.AutoloadDialogMenuItem,
            this.SaveWinPositionMenuItem,
            this.AlwaysOnTopMenuItem,
            this.FloatingWindowMenuItem,
            this.toolStripSeparator3,
            this.RestoreDefaultsMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "&Options";
			this.optionsToolStripMenuItem.DropDownOpened += new System.EventHandler(this.OptionsSubMenu_DropDownOpened);
			// 
			// PreviewModeMenuItem
			// 
			this.PreviewModeMenuItem.Name = "PreviewModeMenuItem";
			this.PreviewModeMenuItem.Size = new System.Drawing.Size(242, 22);
			this.PreviewModeMenuItem.Text = "&Preview Mode";
			this.PreviewModeMenuItem.Click += new System.EventHandler(this.PreviewModeMenuItem_Click);
			// 
			// AutoSearchMenuItem
			// 
			this.AutoSearchMenuItem.Name = "AutoSearchMenuItem";
			this.AutoSearchMenuItem.Size = new System.Drawing.Size(242, 22);
			this.AutoSearchMenuItem.Text = "&Auto-Search";
			this.AutoSearchMenuItem.Click += new System.EventHandler(this.AutoSearchMenuItem_Click);
			// 
			// AutoSearchAccountForLagMenuItem
			// 
			this.AutoSearchAccountForLagMenuItem.Name = "AutoSearchAccountForLagMenuItem";
			this.AutoSearchAccountForLagMenuItem.Size = new System.Drawing.Size(242, 22);
			this.AutoSearchAccountForLagMenuItem.Text = "&Auto-Search Account for Lag";
			this.AutoSearchAccountForLagMenuItem.Click += new System.EventHandler(this.AutoSearchAccountForLagMenuItem_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(239, 6);
			// 
			// ExcludeRamWatchMenuItem
			// 
			this.ExcludeRamWatchMenuItem.Name = "ExcludeRamWatchMenuItem";
			this.ExcludeRamWatchMenuItem.Size = new System.Drawing.Size(242, 22);
			this.ExcludeRamWatchMenuItem.Text = "Always E&xclude RAM Search List";
			this.ExcludeRamWatchMenuItem.Click += new System.EventHandler(this.ExcludeRamWatchMenuItem_Click);
			// 
			// UseUndoHistoryMenuItem
			// 
			this.UseUndoHistoryMenuItem.Name = "UseUndoHistoryMenuItem";
			this.UseUndoHistoryMenuItem.Size = new System.Drawing.Size(242, 22);
			this.UseUndoHistoryMenuItem.Text = "&Use Undo History";
			this.UseUndoHistoryMenuItem.Click += new System.EventHandler(this.UseUndoHistoryMenuItem_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(239, 6);
			// 
			// AutoloadDialogMenuItem
			// 
			this.AutoloadDialogMenuItem.Name = "AutoloadDialogMenuItem";
			this.AutoloadDialogMenuItem.Size = new System.Drawing.Size(242, 22);
			this.AutoloadDialogMenuItem.Text = "Auto&load";
			this.AutoloadDialogMenuItem.Click += new System.EventHandler(this.AutoloadDialogMenuItem_Click);
			// 
			// SaveWinPositionMenuItem
			// 
			this.SaveWinPositionMenuItem.Name = "SaveWinPositionMenuItem";
			this.SaveWinPositionMenuItem.Size = new System.Drawing.Size(242, 22);
			this.SaveWinPositionMenuItem.Text = "&Save Window Position";
			this.SaveWinPositionMenuItem.Click += new System.EventHandler(this.SaveWinPositionMenuItem_Click);
			// 
			// AlwaysOnTopMenuItem
			// 
			this.AlwaysOnTopMenuItem.Name = "AlwaysOnTopMenuItem";
			this.AlwaysOnTopMenuItem.Size = new System.Drawing.Size(242, 22);
			this.AlwaysOnTopMenuItem.Text = "Always On &Top";
			this.AlwaysOnTopMenuItem.Click += new System.EventHandler(this.AlwaysOnTopMenuItem_Click);
			// 
			// FloatingWindowMenuItem
			// 
			this.FloatingWindowMenuItem.Name = "FloatingWindowMenuItem";
			this.FloatingWindowMenuItem.Size = new System.Drawing.Size(242, 22);
			this.FloatingWindowMenuItem.Text = "&Floating Window";
			this.FloatingWindowMenuItem.Click += new System.EventHandler(this.FloatingWindowMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(239, 6);
			// 
			// RestoreDefaultsMenuItem
			// 
			this.RestoreDefaultsMenuItem.Name = "RestoreDefaultsMenuItem";
			this.RestoreDefaultsMenuItem.Size = new System.Drawing.Size(242, 22);
			this.RestoreDefaultsMenuItem.Text = "&Restore Default Settings";
			this.RestoreDefaultsMenuItem.Click += new System.EventHandler(this.RestoreDefaultsMenuItem_Click);
			// 
			// MemDomainLabel
			// 
			this.MemDomainLabel.AutoSize = true;
			this.MemDomainLabel.Location = new System.Drawing.Point(135, 49);
			this.MemDomainLabel.Name = "MemDomainLabel";
			this.MemDomainLabel.Size = new System.Drawing.Size(70, 13);
			this.MemDomainLabel.TabIndex = 8;
			this.MemDomainLabel.Text = "Main Memory";
			// 
			// MessageLabel
			// 
			this.MessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.MessageLabel.AutoSize = true;
			this.MessageLabel.Location = new System.Drawing.Point(9, 434);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(106, 13);
			this.MessageLabel.TabIndex = 9;
			this.MessageLabel.Text = " todo                         ";
			// 
			// AutoSearchCheckBox
			// 
			this.AutoSearchCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AutoSearchCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
			this.AutoSearchCheckBox.AutoSize = true;
			this.AutoSearchCheckBox.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.AutoSearch;
			this.AutoSearchCheckBox.Location = new System.Drawing.Point(316, 410);
			this.AutoSearchCheckBox.Name = "AutoSearchCheckBox";
			this.AutoSearchCheckBox.Size = new System.Drawing.Size(38, 22);
			this.AutoSearchCheckBox.TabIndex = 105;
			this.AutoSearchCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.toolTip1.SetToolTip(this.AutoSearchCheckBox, "Automatically search each frame");
			this.AutoSearchCheckBox.UseVisualStyleBackColor = true;
			this.AutoSearchCheckBox.Click += new System.EventHandler(this.AutoSearchMenuItem_Click);
			// 
			// CompareToBox
			// 
			this.CompareToBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CompareToBox.Controls.Add(this.DifferenceBox);
			this.CompareToBox.Controls.Add(this.DifferenceRadio);
			this.CompareToBox.Controls.Add(this.NumberOfChangesBox);
			this.CompareToBox.Controls.Add(this.SpecificAddressBox);
			this.CompareToBox.Controls.Add(this.SpecificValueBox);
			this.CompareToBox.Controls.Add(this.NumberOfChangesRadio);
			this.CompareToBox.Controls.Add(this.SpecificAddressRadio);
			this.CompareToBox.Controls.Add(this.SpecificValueRadio);
			this.CompareToBox.Controls.Add(this.PreviousValueRadio);
			this.CompareToBox.Location = new System.Drawing.Point(244, 65);
			this.CompareToBox.Name = "CompareToBox";
			this.CompareToBox.Size = new System.Drawing.Size(190, 125);
			this.CompareToBox.TabIndex = 10;
			this.CompareToBox.TabStop = false;
			this.CompareToBox.Text = "Compare To / By";
			// 
			// DifferenceBox
			// 
			this.DifferenceBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DifferenceBox.ByteSize = BizHawk.Client.Common.WatchSize.Byte;
			this.DifferenceBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.DifferenceBox.Enabled = false;
			this.DifferenceBox.Location = new System.Drawing.Point(114, 98);
			this.DifferenceBox.MaxLength = 8;
			this.DifferenceBox.Name = "DifferenceBox";
			this.DifferenceBox.Nullable = false;
			this.DifferenceBox.Size = new System.Drawing.Size(72, 20);
			this.DifferenceBox.TabIndex = 45;
			this.DifferenceBox.Type = BizHawk.Client.Common.DisplayType.Hex;
			this.DifferenceBox.TextChanged += new System.EventHandler(this.CompareToValue_TextChanged);
			// 
			// DifferenceRadio
			// 
			this.DifferenceRadio.AutoSize = true;
			this.DifferenceRadio.Location = new System.Drawing.Point(6, 100);
			this.DifferenceRadio.Name = "DifferenceRadio";
			this.DifferenceRadio.Size = new System.Drawing.Size(89, 17);
			this.DifferenceRadio.TabIndex = 40;
			this.DifferenceRadio.Text = "Difference of:";
			this.DifferenceRadio.UseVisualStyleBackColor = true;
			this.DifferenceRadio.Click += new System.EventHandler(this.DifferenceRadio_Click);
			// 
			// NumberOfChangesBox
			// 
			this.NumberOfChangesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NumberOfChangesBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.NumberOfChangesBox.Enabled = false;
			this.NumberOfChangesBox.Location = new System.Drawing.Point(114, 78);
			this.NumberOfChangesBox.MaxLength = 8;
			this.NumberOfChangesBox.Name = "NumberOfChangesBox";
			this.NumberOfChangesBox.Nullable = false;
			this.NumberOfChangesBox.Size = new System.Drawing.Size(72, 20);
			this.NumberOfChangesBox.TabIndex = 35;
			this.NumberOfChangesBox.TextChanged += new System.EventHandler(this.CompareToValue_TextChanged);
			// 
			// SpecificAddressBox
			// 
			this.SpecificAddressBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SpecificAddressBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.SpecificAddressBox.Enabled = false;
			this.SpecificAddressBox.Location = new System.Drawing.Point(114, 58);
			this.SpecificAddressBox.MaxLength = 8;
			this.SpecificAddressBox.Name = "SpecificAddressBox";
			this.SpecificAddressBox.Nullable = false;
			this.SpecificAddressBox.Size = new System.Drawing.Size(72, 20);
			this.SpecificAddressBox.TabIndex = 25;
			this.SpecificAddressBox.TextChanged += new System.EventHandler(this.CompareToValue_TextChanged);
			// 
			// SpecificValueBox
			// 
			this.SpecificValueBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SpecificValueBox.ByteSize = BizHawk.Client.Common.WatchSize.Byte;
			this.SpecificValueBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.SpecificValueBox.Enabled = false;
			this.SpecificValueBox.Location = new System.Drawing.Point(114, 38);
			this.SpecificValueBox.MaxLength = 2;
			this.SpecificValueBox.Name = "SpecificValueBox";
			this.SpecificValueBox.Nullable = false;
			this.SpecificValueBox.Size = new System.Drawing.Size(72, 20);
			this.SpecificValueBox.TabIndex = 15;
			this.SpecificValueBox.Text = "00";
			this.SpecificValueBox.Type = BizHawk.Client.Common.DisplayType.Hex;
			this.SpecificValueBox.TextChanged += new System.EventHandler(this.CompareToValue_TextChanged);
			// 
			// NumberOfChangesRadio
			// 
			this.NumberOfChangesRadio.AutoSize = true;
			this.NumberOfChangesRadio.Location = new System.Drawing.Point(7, 80);
			this.NumberOfChangesRadio.Name = "NumberOfChangesRadio";
			this.NumberOfChangesRadio.Size = new System.Drawing.Size(111, 17);
			this.NumberOfChangesRadio.TabIndex = 30;
			this.NumberOfChangesRadio.Text = "Specific Changes:";
			this.NumberOfChangesRadio.UseVisualStyleBackColor = true;
			this.NumberOfChangesRadio.Click += new System.EventHandler(this.NumberOfChangesRadio_Click);
			// 
			// SpecificAddressRadio
			// 
			this.SpecificAddressRadio.AutoSize = true;
			this.SpecificAddressRadio.Location = new System.Drawing.Point(7, 60);
			this.SpecificAddressRadio.Name = "SpecificAddressRadio";
			this.SpecificAddressRadio.Size = new System.Drawing.Size(107, 17);
			this.SpecificAddressRadio.TabIndex = 20;
			this.SpecificAddressRadio.Text = "Specific Address:";
			this.SpecificAddressRadio.UseVisualStyleBackColor = true;
			this.SpecificAddressRadio.Click += new System.EventHandler(this.SpecificAddressRadio_Click);
			// 
			// SpecificValueRadio
			// 
			this.SpecificValueRadio.AutoSize = true;
			this.SpecificValueRadio.Location = new System.Drawing.Point(7, 40);
			this.SpecificValueRadio.Name = "SpecificValueRadio";
			this.SpecificValueRadio.Size = new System.Drawing.Size(96, 17);
			this.SpecificValueRadio.TabIndex = 10;
			this.SpecificValueRadio.Text = "Specific Value:";
			this.SpecificValueRadio.UseVisualStyleBackColor = true;
			this.SpecificValueRadio.Click += new System.EventHandler(this.SpecificValueRadio_Click);
			// 
			// PreviousValueRadio
			// 
			this.PreviousValueRadio.AutoSize = true;
			this.PreviousValueRadio.Checked = true;
			this.PreviousValueRadio.Location = new System.Drawing.Point(7, 20);
			this.PreviousValueRadio.Name = "PreviousValueRadio";
			this.PreviousValueRadio.Size = new System.Drawing.Size(96, 17);
			this.PreviousValueRadio.TabIndex = 5;
			this.PreviousValueRadio.TabStop = true;
			this.PreviousValueRadio.Text = "Previous Value";
			this.PreviousValueRadio.UseVisualStyleBackColor = true;
			this.PreviousValueRadio.Click += new System.EventHandler(this.PreviousValueRadio_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.ClickThrough = true;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DoSearchToolButton,
            this.toolStripSeparator10,
            this.NewSearchToolButton,
            this.toolStripSeparator15,
            this.CopyValueToPrevToolBarItem,
            this.ClearChangeCountsToolBarItem,
            this.toolStripSeparator16,
            this.RemoveToolBarItem,
            this.AddToRamWatchToolBarItem,
            this.PokeAddressToolBarItem,
            this.FreezeAddressToolBarItem,
            this.toolStripSeparator12,
            this.UndoToolBarButton,
            this.RedoToolBarItem,
            this.RebootToolBarSeparator,
            this.RebootToolbarButton,
            this.ErrorIconButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(445, 25);
			this.toolStrip1.TabIndex = 11;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// DoSearchToolButton
			// 
			this.DoSearchToolButton.Enabled = false;
			this.DoSearchToolButton.Image = ((System.Drawing.Image)(resources.GetObject("DoSearchToolButton.Image")));
			this.DoSearchToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DoSearchToolButton.Name = "DoSearchToolButton";
			this.DoSearchToolButton.Size = new System.Drawing.Size(65, 22);
			this.DoSearchToolButton.Text = "Search ";
			this.DoSearchToolButton.Click += new System.EventHandler(this.SearchMenuItem_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
			// 
			// NewSearchToolButton
			// 
			this.NewSearchToolButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.restart;
			this.NewSearchToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.NewSearchToolButton.Name = "NewSearchToolButton";
			this.NewSearchToolButton.Size = new System.Drawing.Size(51, 22);
			this.NewSearchToolButton.Text = "New";
			this.NewSearchToolButton.Click += new System.EventHandler(this.NewSearchMenuMenuItem_Click);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
			// 
			// CopyValueToPrevToolBarItem
			// 
			this.CopyValueToPrevToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CopyValueToPrevToolBarItem.Enabled = false;
			this.CopyValueToPrevToolBarItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Previous;
			this.CopyValueToPrevToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CopyValueToPrevToolBarItem.Name = "CopyValueToPrevToolBarItem";
			this.CopyValueToPrevToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.CopyValueToPrevToolBarItem.Text = "Copy Value to Previous";
			this.CopyValueToPrevToolBarItem.Click += new System.EventHandler(this.CopyValueToPrevMenuItem_Click);
			// 
			// ClearChangeCountsToolBarItem
			// 
			this.ClearChangeCountsToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.ClearChangeCountsToolBarItem.Image = ((System.Drawing.Image)(resources.GetObject("ClearChangeCountsToolBarItem.Image")));
			this.ClearChangeCountsToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ClearChangeCountsToolBarItem.Name = "ClearChangeCountsToolBarItem";
			this.ClearChangeCountsToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.ClearChangeCountsToolBarItem.Text = "C";
			this.ClearChangeCountsToolBarItem.ToolTipText = "Clear Change Counts";
			this.ClearChangeCountsToolBarItem.Click += new System.EventHandler(this.ClearChangeCountsMenuItem_Click);
			// 
			// toolStripSeparator16
			// 
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
			// 
			// RemoveToolBarItem
			// 
			this.RemoveToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RemoveToolBarItem.Enabled = false;
			this.RemoveToolBarItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Delete;
			this.RemoveToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RemoveToolBarItem.Name = "RemoveToolBarItem";
			this.RemoveToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.RemoveToolBarItem.Text = "C&ut";
			this.RemoveToolBarItem.ToolTipText = "Eliminate Selected Items";
			this.RemoveToolBarItem.Click += new System.EventHandler(this.RemoveMenuItem_Click);
			// 
			// AddToRamWatchToolBarItem
			// 
			this.AddToRamWatchToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AddToRamWatchToolBarItem.Enabled = false;
			this.AddToRamWatchToolBarItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.FindHS;
			this.AddToRamWatchToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddToRamWatchToolBarItem.Name = "AddToRamWatchToolBarItem";
			this.AddToRamWatchToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.AddToRamWatchToolBarItem.Text = "Watch";
			this.AddToRamWatchToolBarItem.Click += new System.EventHandler(this.AddToRamWatchMenuItem_Click);
			// 
			// PokeAddressToolBarItem
			// 
			this.PokeAddressToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.PokeAddressToolBarItem.Enabled = false;
			this.PokeAddressToolBarItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.poke;
			this.PokeAddressToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.PokeAddressToolBarItem.Name = "PokeAddressToolBarItem";
			this.PokeAddressToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.PokeAddressToolBarItem.Text = "Poke";
			this.PokeAddressToolBarItem.Click += new System.EventHandler(this.PokeAddressMenuItem_Click);
			// 
			// FreezeAddressToolBarItem
			// 
			this.FreezeAddressToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.FreezeAddressToolBarItem.Enabled = false;
			this.FreezeAddressToolBarItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Freeze;
			this.FreezeAddressToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.FreezeAddressToolBarItem.Name = "FreezeAddressToolBarItem";
			this.FreezeAddressToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.FreezeAddressToolBarItem.Text = "Freeze";
			this.FreezeAddressToolBarItem.Click += new System.EventHandler(this.FreezeAddressMenuItem_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
			// 
			// UndoToolBarButton
			// 
			this.UndoToolBarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.UndoToolBarButton.Enabled = false;
			this.UndoToolBarButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.undo;
			this.UndoToolBarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.UndoToolBarButton.Name = "UndoToolBarButton";
			this.UndoToolBarButton.Size = new System.Drawing.Size(23, 22);
			this.UndoToolBarButton.Text = "Undo Search";
			this.UndoToolBarButton.Click += new System.EventHandler(this.UndoMenuItem_Click);
			// 
			// RedoToolBarItem
			// 
			this.RedoToolBarItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RedoToolBarItem.Enabled = false;
			this.RedoToolBarItem.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.redo;
			this.RedoToolBarItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RedoToolBarItem.Name = "RedoToolBarItem";
			this.RedoToolBarItem.Size = new System.Drawing.Size(23, 22);
			this.RedoToolBarItem.Text = "Redo";
			this.RedoToolBarItem.Click += new System.EventHandler(this.RedoMenuItem_Click);
			// 
			// RebootToolBarSeparator
			// 
			this.RebootToolBarSeparator.Name = "RebootToolBarSeparator";
			this.RebootToolBarSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// RebootToolbarButton
			// 
			this.RebootToolbarButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RebootToolbarButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.reboot;
			this.RebootToolbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RebootToolbarButton.Name = "RebootToolbarButton";
			this.RebootToolbarButton.Size = new System.Drawing.Size(23, 22);
			this.RebootToolbarButton.Text = "A new search needs to be started in order for these changes to take effect";
			this.RebootToolbarButton.Click += new System.EventHandler(this.NewSearchMenuMenuItem_Click);
			// 
			// ErrorIconButton
			// 
			this.ErrorIconButton.BackColor = System.Drawing.Color.NavajoWhite;
			this.ErrorIconButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ErrorIconButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.ExclamationRed;
			this.ErrorIconButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ErrorIconButton.Name = "ErrorIconButton";
			this.ErrorIconButton.Size = new System.Drawing.Size(23, 22);
			this.ErrorIconButton.Text = "Warning! Out of Range Addresses in list, click to remove them";
			this.ErrorIconButton.Click += new System.EventHandler(this.ErrorIconButton_Click);
			// 
			// ComparisonBox
			// 
			this.ComparisonBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ComparisonBox.Controls.Add(this.DifferentByBox);
			this.ComparisonBox.Controls.Add(this.DifferentByRadio);
			this.ComparisonBox.Controls.Add(this.NotEqualToRadio);
			this.ComparisonBox.Controls.Add(this.EqualToRadio);
			this.ComparisonBox.Controls.Add(this.GreaterThanOrEqualToRadio);
			this.ComparisonBox.Controls.Add(this.LessThanOrEqualToRadio);
			this.ComparisonBox.Controls.Add(this.GreaterThanRadio);
			this.ComparisonBox.Controls.Add(this.LessThanRadio);
			this.ComparisonBox.Location = new System.Drawing.Point(244, 196);
			this.ComparisonBox.Name = "ComparisonBox";
			this.ComparisonBox.Size = new System.Drawing.Size(190, 159);
			this.ComparisonBox.TabIndex = 12;
			this.ComparisonBox.TabStop = false;
			this.ComparisonBox.Text = "Comparison Operator";
			// 
			// DifferentByBox
			// 
			this.DifferentByBox.ByteSize = BizHawk.Client.Common.WatchSize.Byte;
			this.DifferentByBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.DifferentByBox.Enabled = false;
			this.DifferentByBox.Location = new System.Drawing.Point(88, 132);
			this.DifferentByBox.MaxLength = 9;
			this.DifferentByBox.Name = "DifferentByBox";
			this.DifferentByBox.Nullable = false;
			this.DifferentByBox.Size = new System.Drawing.Size(55, 20);
			this.DifferentByBox.TabIndex = 85;
			this.DifferentByBox.Type = BizHawk.Client.Common.DisplayType.Hex;
			this.DifferentByBox.TextChanged += new System.EventHandler(this.DifferentByBox_TextChanged);
			// 
			// DifferentByRadio
			// 
			this.DifferentByRadio.AutoSize = true;
			this.DifferentByRadio.Location = new System.Drawing.Point(7, 134);
			this.DifferentByRadio.Name = "DifferentByRadio";
			this.DifferentByRadio.Size = new System.Drawing.Size(82, 17);
			this.DifferentByRadio.TabIndex = 80;
			this.DifferentByRadio.Text = "Different by:";
			this.DifferentByRadio.UseVisualStyleBackColor = true;
			this.DifferentByRadio.Click += new System.EventHandler(this.DifferentByRadio_Click);
			// 
			// NotEqualToRadio
			// 
			this.NotEqualToRadio.AutoSize = true;
			this.NotEqualToRadio.Location = new System.Drawing.Point(7, 35);
			this.NotEqualToRadio.Name = "NotEqualToRadio";
			this.NotEqualToRadio.Size = new System.Drawing.Size(88, 17);
			this.NotEqualToRadio.TabIndex = 55;
			this.NotEqualToRadio.Text = "Not Equal To";
			this.NotEqualToRadio.UseVisualStyleBackColor = true;
			this.NotEqualToRadio.Click += new System.EventHandler(this.NotEqualToRadio_Click);
			// 
			// EqualToRadio
			// 
			this.EqualToRadio.AutoSize = true;
			this.EqualToRadio.Checked = true;
			this.EqualToRadio.Location = new System.Drawing.Point(7, 15);
			this.EqualToRadio.Name = "EqualToRadio";
			this.EqualToRadio.Size = new System.Drawing.Size(68, 17);
			this.EqualToRadio.TabIndex = 50;
			this.EqualToRadio.TabStop = true;
			this.EqualToRadio.Text = "Equal To";
			this.EqualToRadio.UseVisualStyleBackColor = true;
			this.EqualToRadio.Click += new System.EventHandler(this.EqualToRadio_Click);
			// 
			// GreaterThanOrEqualToRadio
			// 
			this.GreaterThanOrEqualToRadio.AutoSize = true;
			this.GreaterThanOrEqualToRadio.Location = new System.Drawing.Point(7, 113);
			this.GreaterThanOrEqualToRadio.Name = "GreaterThanOrEqualToRadio";
			this.GreaterThanOrEqualToRadio.Size = new System.Drawing.Size(146, 17);
			this.GreaterThanOrEqualToRadio.TabIndex = 75;
			this.GreaterThanOrEqualToRadio.Text = "Greater Than or Equal To";
			this.GreaterThanOrEqualToRadio.UseVisualStyleBackColor = true;
			this.GreaterThanOrEqualToRadio.Click += new System.EventHandler(this.GreaterThanOrEqualToRadio_Click);
			// 
			// LessThanOrEqualToRadio
			// 
			this.LessThanOrEqualToRadio.AutoSize = true;
			this.LessThanOrEqualToRadio.Location = new System.Drawing.Point(7, 93);
			this.LessThanOrEqualToRadio.Name = "LessThanOrEqualToRadio";
			this.LessThanOrEqualToRadio.Size = new System.Drawing.Size(133, 17);
			this.LessThanOrEqualToRadio.TabIndex = 70;
			this.LessThanOrEqualToRadio.Text = "Less Than or Equal To";
			this.LessThanOrEqualToRadio.UseVisualStyleBackColor = true;
			this.LessThanOrEqualToRadio.Click += new System.EventHandler(this.LessThanOrEqualToRadio_Click);
			// 
			// GreaterThanRadio
			// 
			this.GreaterThanRadio.AutoSize = true;
			this.GreaterThanRadio.Location = new System.Drawing.Point(7, 74);
			this.GreaterThanRadio.Name = "GreaterThanRadio";
			this.GreaterThanRadio.Size = new System.Drawing.Size(88, 17);
			this.GreaterThanRadio.TabIndex = 65;
			this.GreaterThanRadio.Text = "Greater Than";
			this.GreaterThanRadio.UseVisualStyleBackColor = true;
			this.GreaterThanRadio.Click += new System.EventHandler(this.GreaterThanRadio_Click);
			// 
			// LessThanRadio
			// 
			this.LessThanRadio.AutoSize = true;
			this.LessThanRadio.Location = new System.Drawing.Point(7, 54);
			this.LessThanRadio.Name = "LessThanRadio";
			this.LessThanRadio.Size = new System.Drawing.Size(75, 17);
			this.LessThanRadio.TabIndex = 60;
			this.LessThanRadio.Text = "Less Than";
			this.LessThanRadio.UseVisualStyleBackColor = true;
			this.LessThanRadio.Click += new System.EventHandler(this.LessThanRadio_Click);
			// 
			// SearchButton
			// 
			this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchButton.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.search;
			this.SearchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.SearchButton.Location = new System.Drawing.Point(244, 409);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(70, 23);
			this.SearchButton.TabIndex = 100;
			this.SearchButton.Text = "&Search";
			this.SearchButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SearchButton.UseVisualStyleBackColor = true;
			this.SearchButton.Click += new System.EventHandler(this.SearchMenuItem_Click);
			// 
			// SizeDropdown
			// 
			this.SizeDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SizeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SizeDropdown.FormattingEnabled = true;
			this.SizeDropdown.Items.AddRange(new object[] {
            "1 Byte",
            "2 Byte",
            "4 Byte"});
			this.SizeDropdown.Location = new System.Drawing.Point(244, 374);
			this.SizeDropdown.Name = "SizeDropdown";
			this.SizeDropdown.Size = new System.Drawing.Size(73, 21);
			this.SizeDropdown.TabIndex = 90;
			this.SizeDropdown.SelectedIndexChanged += new System.EventHandler(this.SizeDropdown_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(244, 358);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 97;
			this.label1.Text = "Size";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(327, 358);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 13);
			this.label2.TabIndex = 99;
			this.label2.Text = "Display";
			// 
			// DisplayTypeDropdown
			// 
			this.DisplayTypeDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DisplayTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DisplayTypeDropdown.FormattingEnabled = true;
			this.DisplayTypeDropdown.Items.AddRange(new object[] {
            "1 Byte",
            "2 Byte",
            "4 Byte"});
			this.DisplayTypeDropdown.Location = new System.Drawing.Point(327, 374);
			this.DisplayTypeDropdown.Name = "DisplayTypeDropdown";
			this.DisplayTypeDropdown.Size = new System.Drawing.Size(107, 21);
			this.DisplayTypeDropdown.TabIndex = 95;
			this.DisplayTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.DisplayTypeDropdown_SelectedIndexChanged);
			// 
			// RamSearch
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(445, 459);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.DisplayTypeDropdown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SizeDropdown);
			this.Controls.Add(this.SearchButton);
			this.Controls.Add(this.AutoSearchCheckBox);
			this.Controls.Add(this.ComparisonBox);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.MessageLabel);
			this.Controls.Add(this.MemDomainLabel);
			this.Controls.Add(this.CompareToBox);
			this.Controls.Add(this.WatchListView);
			this.Controls.Add(this.TotalSearchLabel);
			this.Controls.Add(this.RamSearchMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.RamSearchMenu;
			this.MinimumSize = new System.Drawing.Size(290, 399);
			this.Name = "RamSearch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "RAM Search";
			this.Activated += new System.EventHandler(this.NewRamSearch_Activated);
			this.Load += new System.EventHandler(this.RamSearch_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.NewRamSearch_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnterWrapper);
			this.ListViewContextMenu.ResumeLayout(false);
			this.RamSearchMenu.ResumeLayout(false);
			this.RamSearchMenu.PerformLayout();
			this.CompareToBox.ResumeLayout(false);
			this.CompareToBox.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ComparisonBox.ResumeLayout(false);
			this.ComparisonBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label TotalSearchLabel;
		VirtualListView WatchListView;
		private System.Windows.Forms.ColumnHeader AddressColumn;
		private System.Windows.Forms.ColumnHeader ValueColumn;
		private System.Windows.Forms.ColumnHeader PreviousColumn;
		private System.Windows.Forms.ColumnHeader ChangesColumn;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveAsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RestoreDefaultsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveWinPositionMenuItem;
		private System.Windows.Forms.Label MemDomainLabel;
		private System.Windows.Forms.Label MessageLabel;
		private System.Windows.Forms.ToolStripMenuItem RecentSubMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem AppendFileMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ClearChangeCountsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem UndoMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RemoveMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem AddToRamWatchMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PokeAddressMenuItem;
		private System.Windows.Forms.ToolStripMenuItem TruncateFromFileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ExcludeRamWatchMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CopyValueToPrevMenuItem;
		private System.Windows.Forms.ContextMenuStrip ListViewContextMenu;
		private System.Windows.Forms.ToolStripMenuItem NewSearchContextMenuItem;
		private System.Windows.Forms.ToolStripSeparator ContextMenuSeparator1;
		private System.Windows.Forms.ToolStripMenuItem DoSearchContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem FreezeAddressMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RemoveContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AddToRamWatchContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PokeContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem FreezeContextMenuItem;
		private MenuStripEx RamSearchMenu;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem RedoMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ViewInHexEditorContextMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AutoloadDialogMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripMenuItem UnfreezeAllContextMenuItem;
		private System.Windows.Forms.ToolStripSeparator ContextMenuSeparator3;
		private System.Windows.Forms.ToolStripMenuItem AlwaysOnTopMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripMenuItem ClearUndoMenuItem;
		private System.Windows.Forms.ToolStripMenuItem UseUndoHistoryMenuItem;
		private System.Windows.Forms.ToolStripSeparator ContextMenuSeparator2;
		private System.Windows.Forms.ToolStripMenuItem ClearPreviewContextMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem newSearchToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DetailedMenuItem;
		private System.Windows.Forms.ToolStripMenuItem FastMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MemoryDomainsSubMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ByteMenuItem;
		private System.Windows.Forms.ToolStripMenuItem WordMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DWordMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DisplayTypeSubMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem BigEndianMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CheckMisalignedMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem DefinePreviousValueSubMenu;
		private System.Windows.Forms.ToolStripMenuItem PreviousFrameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem Previous_LastSearchMenuItem;
		private System.Windows.Forms.ToolStripMenuItem Previous_OriginalMenuItem;
		private System.Windows.Forms.GroupBox CompareToBox;
		private System.Windows.Forms.RadioButton DifferenceRadio;
		private UnsignedIntegerBox NumberOfChangesBox;
		private HexTextBox SpecificAddressBox;
		private WatchValueBox SpecificValueBox;
		private System.Windows.Forms.RadioButton NumberOfChangesRadio;
		private System.Windows.Forms.RadioButton SpecificAddressRadio;
		private System.Windows.Forms.RadioButton SpecificValueRadio;
		private System.Windows.Forms.RadioButton PreviousValueRadio;
		private ToolStripEx toolStrip1;
		private System.Windows.Forms.ToolStripButton DoSearchToolButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripButton NewSearchToolButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private System.Windows.Forms.GroupBox ComparisonBox;
		private WatchValueBox DifferentByBox;
		private System.Windows.Forms.RadioButton DifferentByRadio;
		private System.Windows.Forms.RadioButton NotEqualToRadio;
		private System.Windows.Forms.RadioButton EqualToRadio;
		private System.Windows.Forms.RadioButton GreaterThanOrEqualToRadio;
		private System.Windows.Forms.RadioButton LessThanOrEqualToRadio;
		private System.Windows.Forms.RadioButton GreaterThanRadio;
		private System.Windows.Forms.RadioButton LessThanRadio;
		private System.Windows.Forms.ToolStripButton CopyValueToPrevToolBarItem;
		private System.Windows.Forms.ToolStripButton ClearChangeCountsToolBarItem;
		private System.Windows.Forms.ToolStripMenuItem PreviewModeMenuItem;
		private System.Windows.Forms.ToolStripButton RemoveToolBarItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
		private System.Windows.Forms.ToolStripButton AddToRamWatchToolBarItem;
		private System.Windows.Forms.ToolStripButton PokeAddressToolBarItem;
		private System.Windows.Forms.ToolStripButton FreezeAddressToolBarItem;
		private WatchValueBox DifferenceBox;
		private System.Windows.Forms.ToolStripMenuItem AutoSearchMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ColumnHeader DiffColumn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripButton UndoToolBarButton;
		private System.Windows.Forms.ToolStripButton RedoToolBarItem;
		private System.Windows.Forms.CheckBox AutoSearchCheckBox;
		private System.Windows.Forms.Button SearchButton;
		private System.Windows.Forms.ToolStripSeparator RebootToolBarSeparator;
		private System.Windows.Forms.ToolStripButton RebootToolbarButton;
		private System.Windows.Forms.ComboBox SizeDropdown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox DisplayTypeDropdown;
		private System.Windows.Forms.ToolStripMenuItem GoToAddressMenuItem;
		private System.Windows.Forms.ToolStripMenuItem FloatingWindowMenuItem;
		private System.Windows.Forms.ToolStripButton ErrorIconButton;
		private System.Windows.Forms.ToolStripMenuItem Previous_LastChangeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AutoSearchAccountForLagMenuItem;
	}
}
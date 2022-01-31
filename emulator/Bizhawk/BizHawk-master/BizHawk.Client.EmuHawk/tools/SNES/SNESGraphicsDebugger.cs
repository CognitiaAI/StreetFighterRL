﻿//TODO - disable scanline controls if box is unchecked
//TODO - overhaul the BG display box if its mode7 or direct color (mode7 more important)
//TODO - draw `1024` label in red if your content is being scaled down.
//TODO - maybe draw a label (in lieu of above, also) showing what scale the content is at: 2x or 1x or 1/2x
//TODO - add eDisplayType for BG1-Tiles, BG2-Tiles, etc. which show the tiles available to a BG. more concise than viewing all tiles and illustrating the relevant accessible areas
//        . could this apply to the palette too?
//TODO - show the priority list for the current mode. make the priority list have checkboxes, and use that to control whether that item displays (does bsnes have that granularity? maybe)
//TODO - use custom checkboxes for register-viewing checkboxes to make them green and checked
//TODO - make freeze actually save the info caches, and re-render in realtime, so that you can pick something you want to see animate without having to hover your mouse just right. also add a checkbox to do the literal freeze (stop it from updating)
//TODO - sprite wrapping is not correct
//TODO - add "scroll&screen" checkbox which changes BG1/2/3/4 display modes to render scrolled and limited to 256x224 (for matching obj screen)
//         alternatively - add "BG1 Screen" as a complement to BG1
//TODO - make Sprites mode respect priority toggles
//TODO - add "mode" display to BG info (in addition to bpp) so we can readily see if its mode7 or unavailable

//DEFERRED:
//. 256bpp modes (difficult to use)
//. non-mode7 directcolor (no known examples, perhaps due to difficulty using the requisite 256bpp modes)

//http://stackoverflow.com/questions/1101149/displaying-thumbnail-icons-128x128-pixels-or-larger-in-a-grid-in-listview

//hiding the tab control headers.. once this design gets solid, ill get rid of them
//http://www.mostthingsweb.com/2011/01/hiding-tab-headers-on-a-tabcontrol-in-c/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using BizHawk.Common.NumberExtensions;
using BizHawk.Client.Common;
using BizHawk.Emulation.Cores.Nintendo.SNES;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;

namespace BizHawk.Client.EmuHawk
{
	public unsafe partial class SNESGraphicsDebugger : Form, IToolFormAutoConfig
	{
		List<DisplayTypeItem> displayTypeItems = new List<DisplayTypeItem>();

		public bool UpdateBefore { get { return false; } }
		public bool AskSaveChanges() { return true; }

		[RequiredService]
		private LibsnesCore Emulator { get; set; }

		[ConfigPersist]
		public bool UseUserBackdropColor
		{
			get { return checkBackdropColor.Checked; }
			set { checkBackdropColor.Checked = value; }
		}
		[ConfigPersist]
		public int UserBackdropColor { get; set; }


		public void Restart()
		{
			
		}

		public SNESGraphicsDebugger()
		{
			InitializeComponent();
			viewerTile.ScaleImage = true;

			viewer.ScaleImage = false;

			displayTypeItems.Add(new DisplayTypeItem("Sprites", eDisplayType.Sprites));
			displayTypeItems.Add(new DisplayTypeItem("OBJ", eDisplayType.OBJ));
			
			displayTypeItems.Add(new DisplayTypeItem("BG1 Screen", eDisplayType.BG1));
			displayTypeItems.Add(new DisplayTypeItem("BG2 Screen", eDisplayType.BG2));
			displayTypeItems.Add(new DisplayTypeItem("BG3 Screen", eDisplayType.BG3));
			displayTypeItems.Add(new DisplayTypeItem("BG4 Screen", eDisplayType.BG4));

			displayTypeItems.Add(new DisplayTypeItem("BG1", eDisplayType.BG1));
			displayTypeItems.Add(new DisplayTypeItem("BG2",eDisplayType.BG2));
			displayTypeItems.Add(new DisplayTypeItem("BG3",eDisplayType.BG3));
			displayTypeItems.Add(new DisplayTypeItem("BG4",eDisplayType.BG4));
			displayTypeItems.Add(new DisplayTypeItem("OBJ Tiles",eDisplayType.OBJTiles0));
			displayTypeItems.Add(new DisplayTypeItem("2bpp tiles",eDisplayType.Tiles2bpp));
			displayTypeItems.Add(new DisplayTypeItem("4bpp tiles",eDisplayType.Tiles4bpp));
			displayTypeItems.Add(new DisplayTypeItem("8bpp tiles",eDisplayType.Tiles8bpp));
			displayTypeItems.Add(new DisplayTypeItem("Mode7 tiles",eDisplayType.TilesMode7));
			displayTypeItems.Add(new DisplayTypeItem("Mode7Ext tiles",eDisplayType.TilesMode7Ext));
			displayTypeItems.Add(new DisplayTypeItem("Mode7 tiles (DC)", eDisplayType.TilesMode7DC));
			comboDisplayType.DataSource = displayTypeItems;
			comboDisplayType.SelectedIndex = 2;

			var paletteTypeItems = new List<PaletteTypeItem>();
			paletteTypeItems.Add(new PaletteTypeItem("BizHawk", SnesColors.ColorType.BizHawk));
			paletteTypeItems.Add(new PaletteTypeItem("bsnes", SnesColors.ColorType.BSNES));
			paletteTypeItems.Add(new PaletteTypeItem("Snes9X", SnesColors.ColorType.Snes9x));
			suppression = true;
			comboPalette.DataSource = paletteTypeItems;
			comboPalette.SelectedIndex = 0;
			suppression = false;

			comboBGProps.SelectedIndex = 0;

			SyncViewerSize();
			SyncColorSelection();

			//tabctrlDetails.SelectedIndex = 1;
			SetTab(null);

			UserBackdropColor = -1;
		}

		LibsnesCore currentSnesCore;
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if (currentSnesCore != null)
				currentSnesCore.ScanlineHookManager.Unregister(this);
			currentSnesCore = null;
		}

		string FormatBpp(int bpp)
		{
			if (bpp == 0) return "---";
			else return bpp.ToString();
		}

		string FormatVramAddress(int address)
		{
			int excess = address & 1023;
			if (excess != 0) return "@" + address.ToHexString(4);
			else return string.Format("@{0} ({1}K)", address.ToHexString(4), address / 1024);
		}

		public void NewUpdate(ToolFormUpdateType type) { }

		public void UpdateValues()
		{
			SyncCore();
			if (Visible && !checkScanlineControl.Checked)
			{
					RegenerateData();
					InternalUpdateValues();
			}
		}

		public void FastUpdate()
		{
			// To do
		}

		public void UpdateToolsLoadstate()
		{
			SyncCore();
			if (Visible)
			{
				RegenerateData();
				InternalUpdateValues();
			}
		}

		private void nudScanline_ValueChanged(object sender, EventArgs e)
		{
			if (suppression) return;
			SyncCore();
			suppression = true;
			sliderScanline.Value = 224 - (int)nudScanline.Value;
			suppression = false;
		}

		private void sliderScanline_ValueChanged(object sender, EventArgs e)
		{
			if (suppression) return;
			checkScanlineControl.Checked = true;
			SyncCore();
			suppression = true;
			nudScanline.Value = 224 - sliderScanline.Value;
			suppression = false;
		}

		void SyncCore()
		{
			if (currentSnesCore != Emulator && currentSnesCore != null)
			{
				currentSnesCore.ScanlineHookManager.Unregister(this);
			}

			if (currentSnesCore != Emulator && Emulator != null)
			{
				suppression = true;
				comboPalette.SelectedValue = Emulator.CurrPalette;
				RefreshBGENCheckStatesFromConfig();
				suppression = false;
			}

			currentSnesCore = Emulator;

			if (currentSnesCore != null)
			{
				if (Visible && checkScanlineControl.Checked)
					currentSnesCore.ScanlineHookManager.Register(this, ScanlineHook);
				else
					currentSnesCore.ScanlineHookManager.Unregister(this);
			}
		}

		void ScanlineHook(int line)
		{
			int target = (int)nudScanline.Value;
			if (target == line)
			{
				RegenerateData();
				InternalUpdateValues();
			}
		}

		SNESGraphicsDecoder gd;
		SNESGraphicsDecoder.ScreenInfo si;
		SNESGraphicsDecoder.TileEntry[] map;
		byte[,] spriteMap = new byte[256, 224];
		SNESGraphicsDecoder.BGMode viewBgMode;

		void RegenerateData()
		{
			if (gd != null) gd.Dispose();
			gd = null;
			if (currentSnesCore == null) return;
			gd = NewDecoder();
			using (gd.EnterExit())
			{
				if (checkBackdropColor.Checked)
					gd.SetBackColor(DecodeWinformsColorToSNES(pnBackdropColor.BackColor));
				gd.CacheTiles();
				si = gd.ScanScreenInfo();
			}
		}

		private void InternalUpdateValues()
		{
			if (currentSnesCore == null) return;
			using (gd.EnterExit())
			{
				txtOBSELSizeBits.Text = si.OBSEL_Size.ToString();
				txtOBSELBaseBits.Text = si.OBSEL_NameBase.ToString();
				txtOBSELT1OfsBits.Text = si.OBSEL_NameSel.ToString();
				txtOBSELSizeDescr.Text = string.Format("{0}, {1}", SNESGraphicsDecoder.ObjSizes[si.OBSEL_Size, 0], SNESGraphicsDecoder.ObjSizes[si.OBSEL_Size, 1]);
				txtOBSELBaseDescr.Text = FormatVramAddress(si.OBJTable0Addr);
				txtOBSELT1OfsDescr.Text = FormatVramAddress(si.OBJTable1Addr);

				checkScreenExtbg.Checked = si.SETINI_Mode7ExtBG;
				checkScreenHires.Checked = si.SETINI_HiRes;
				checkScreenOverscan.Checked = si.SETINI_Overscan;
				checkScreenObjInterlace.Checked = si.SETINI_ObjInterlace;
				checkScreenInterlace.Checked = si.SETINI_ScreenInterlace;

				txtScreenCGWSEL_ColorMask.Text = si.CGWSEL_ColorMask.ToString();
				txtScreenCGWSEL_ColorSubMask.Text = si.CGWSEL_ColorSubMask.ToString();
				txtScreenCGWSEL_MathFixed.Text = si.CGWSEL_AddSubMode.ToString();
				checkScreenCGWSEL_DirectColor.Checked = si.CGWSEL_DirectColor;
				txtScreenCGADSUB_AddSub.Text = si.CGWSEL_AddSubMode.ToString();
				txtScreenCGADSUB_AddSub_Descr.Text = si.CGADSUB_AddSub == 1 ? "SUB" : "ADD";
				txtScreenCGADSUB_Half.Checked = si.CGADSUB_Half;

				txtModeBits.Text = si.Mode.MODE.ToString();
				txtScreenBG1Bpp.Text = FormatBpp(si.BG.BG1.Bpp);
				txtScreenBG2Bpp.Text = FormatBpp(si.BG.BG2.Bpp);
				txtScreenBG3Bpp.Text = FormatBpp(si.BG.BG3.Bpp);
				txtScreenBG4Bpp.Text = FormatBpp(si.BG.BG4.Bpp);
				txtScreenBG1TSize.Text = FormatBpp(si.BG.BG1.TileSize);
				txtScreenBG2TSize.Text = FormatBpp(si.BG.BG2.TileSize);
				txtScreenBG3TSize.Text = FormatBpp(si.BG.BG3.TileSize);
				txtScreenBG4TSize.Text = FormatBpp(si.BG.BG4.TileSize);

				int bgnum = comboBGProps.SelectedIndex + 1;

				var bg = si.BG[bgnum];
				txtBG1TSizeBits.Text = bg.TILESIZE.ToString();
				txtBG1TSizeDescr.Text = string.Format("{0}x{0}", bg.TileSize);
				txtBG1Bpp.Text = FormatBpp(bg.Bpp);
				txtBG1SizeBits.Text = bg.SCSIZE.ToString();
				txtBG1SizeInTiles.Text = bg.ScreenSizeInTiles.ToString();
				int size = bg.ScreenSizeInTiles.Width * bg.ScreenSizeInTiles.Height * 2 / 1024;
				txtBG1MapSizeBytes.Text = string.Format("({0}K)", size);
				txtBG1SCAddrBits.Text = bg.SCADDR.ToString();
				txtBG1SCAddrDescr.Text = FormatVramAddress(bg.ScreenAddr);
				txtBG1Colors.Text = (1 << bg.Bpp).ToString();
				if (bg.Bpp == 8 && si.CGWSEL_DirectColor) txtBG1Colors.Text = "(Direct Color)";
				txtBG1TDAddrBits.Text = bg.TDADDR.ToString();
				txtBG1TDAddrDescr.Text = FormatVramAddress(bg.TiledataAddr);

				txtBG1Scroll.Text = string.Format("({0},{1})", bg.HOFS, bg.VOFS);

				if (bg.Bpp != 0)
				{
					var pi = bg.PaletteSelection;
					txtBGPaletteInfo.Text = string.Format("{0} colors from ${1:X2} - ${2:X2}", pi.size, pi.start, pi.start + pi.size - 1);
				}
				else txtBGPaletteInfo.Text = "";

				var sizeInPixels = bg.ScreenSizeInPixels;
				txtBG1SizeInPixels.Text = string.Format("{0}x{1}", sizeInPixels.Width, sizeInPixels.Height);

				checkTMOBJ.Checked = si.OBJ_MainEnabled;
				checkTMBG1.Checked = si.BG.BG1.MainEnabled;
				checkTMBG2.Checked = si.BG.BG2.MainEnabled;
				checkTMBG3.Checked = si.BG.BG3.MainEnabled;
				checkTMBG4.Checked = si.BG.BG4.MainEnabled;
				checkTMOBJ.Checked = si.OBJ_SubEnabled;
				checkTSBG1.Checked = si.BG.BG1.SubEnabled;
				checkTSBG2.Checked = si.BG.BG2.SubEnabled;
				checkTSBG3.Checked = si.BG.BG3.SubEnabled;
				checkTSBG4.Checked = si.BG.BG4.SubEnabled;
				checkTSOBJ.Checked = si.OBJ_MainEnabled;
				checkMathOBJ.Checked = si.OBJ_MathEnabled;
				checkMathBK.Checked = si.BK_MathEnabled;
				checkMathBG1.Checked = si.BG.BG1.MathEnabled;
				checkMathBG2.Checked = si.BG.BG2.MathEnabled;
				checkMathBG3.Checked = si.BG.BG3.MathEnabled;
				checkMathBG4.Checked = si.BG.BG4.MathEnabled;

				if (si.Mode.MODE == 1 && si.Mode1_BG3_Priority)
				{
					lblBG3.ForeColor = Color.Red;
					if (toolTip1.GetToolTip(lblBG3) != "Mode 1 BG3 priority toggle bit of $2105 is SET")
						toolTip1.SetToolTip(lblBG3, "Mode 1 BG3 priority toggle bit of $2105 is SET");
				}
				else
				{
					lblBG3.ForeColor = Color.Black;
					if (toolTip1.GetToolTip(lblBG3) != "Mode 1 BG3 priority toggle bit of $2105 is CLEAR")
						toolTip1.SetToolTip(lblBG3, "Mode 1 BG3 priority toggle bit of $2105 is CLEAR");
				}

				SyncColorSelection();
				RenderView();
				RenderPalette();
				RenderTileView();
				//these are likely to be changing all the time
				UpdateColorDetails();
				UpdateOBJDetails();
				//maybe bg settings changed, or something
				UpdateMapEntryDetails();
				UpdateTileDetails();
			}
		}

		eDisplayType CurrDisplaySelection { get { return (comboDisplayType.SelectedValue as eDisplayType?).Value; } }

		//todo - something smarter to cycle through bitmaps without repeatedly trashing them (use the dispose callback on the viewer)
		private void RenderView()
		{
			Bitmap bmp = null;
			System.Drawing.Imaging.BitmapData bmpdata = null;
			int* pixelptr = null;
			int stride = 0;

			Action<int, int> allocate = (w, h) =>
			{
				bmp = new Bitmap(w, h);
				bmpdata = bmp.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				pixelptr = (int*)bmpdata.Scan0.ToPointer();
				stride = bmpdata.Stride;
			};

			var selection = CurrDisplaySelection;
			if (selection == eDisplayType.OBJ)
			{
				var objBounds = si.ObjSizeBounds;
				int width = objBounds.Width * 8;
				int height = objBounds.Height * 16;
				allocate(width, height);
				for (int i = 0; i < 128; i++)
				{
					int tx = i % 8;
					int ty = i / 8;
					int x = tx * objBounds.Width;
					int y = ty * objBounds.Height;
					gd.RenderSpriteToScreen(pixelptr, stride / 4, x,y, si, i);
				}
			}
			if (selection == eDisplayType.Sprites)
			{
				//render sprites in-place
				allocate(256, 224);
				for (int y = 0; y < 224; y++) for (int x = 0; x < 256; x++) spriteMap[x, y] = 0xFF;
				for(int i=127;i>=0;i--)
				{
					var oam = new SNESGraphicsDecoder.OAMInfo(gd, si, i);
					gd.RenderSpriteToScreen(pixelptr, stride / 4, oam.X, oam.Y, si, i, oam, 256, 224, spriteMap);
				}
			}
			if (selection == eDisplayType.OBJTiles0 || selection == eDisplayType.OBJTiles1)
			{
				allocate(128, 256);
				int startTile;
				startTile = si.OBJTable0Addr / 32;
				gd.RenderTilesToScreen(pixelptr, 16, 16, stride / 4, 4, currPaletteSelection.start, startTile, 256, true);
				startTile = si.OBJTable1Addr / 32;
				gd.RenderTilesToScreen(pixelptr + (stride/4*8*16), 16, 16, stride / 4, 4, currPaletteSelection.start, startTile, 256, true);
			}
			if (selection == eDisplayType.Tiles2bpp)
			{
				allocate(512, 512);
				gd.RenderTilesToScreen(pixelptr, 64, 64, stride / 4, 2, currPaletteSelection.start);
			}
			if (selection == eDisplayType.Tiles4bpp)
			{
				allocate(512, 512);
				gd.RenderTilesToScreen(pixelptr, 64, 32, stride / 4, 4, currPaletteSelection.start);
			}
			if (selection == eDisplayType.Tiles8bpp)
			{
				allocate(256, 256);
				gd.RenderTilesToScreen(pixelptr, 32, 32, stride / 4, 8, currPaletteSelection.start);
			}
			if (selection == eDisplayType.TilesMode7)
			{
				//256 tiles
				allocate(128, 128);
				gd.RenderMode7TilesToScreen(pixelptr, stride / 4, false, false);
			}
			if (selection == eDisplayType.TilesMode7Ext)
			{
				//256 tiles
				allocate(128, 128);
				gd.RenderMode7TilesToScreen(pixelptr, stride / 4, true, false);
			}
			if (selection == eDisplayType.TilesMode7DC)
			{
				//256 tiles
				allocate(128, 128);
				gd.RenderMode7TilesToScreen(pixelptr, stride / 4, false, true);
			}
			if (IsDisplayTypeBG(selection))
			{
				int bgnum = (int)selection;
				var bg = si.BG[bgnum];

				map = new SNESGraphicsDecoder.TileEntry[0];
				viewBgMode = bg.BGMode;

				//bool handled = false;
				if (bg.Enabled)
				{
					//TODO - directColor in normal BG renderer
					bool DirectColor = si.CGWSEL_DirectColor && bg.Bpp == 8; //any exceptions?
					int numPixels = 0;
					//TODO - could use BGMode property on BG... too much chaos to deal with it now
					if (si.Mode.MODE == 7)
					{
						bool mode7 = bgnum == 1;
						bool mode7extbg = (bgnum == 2 && si.SETINI_Mode7ExtBG);
						if (mode7 || mode7extbg)
						{
							//handled = true;
							allocate(1024, 1024);
							gd.DecodeMode7BG(pixelptr, stride / 4, mode7extbg);
							numPixels = 128 * 128 * 8 * 8;
							if (DirectColor) gd.DirectColorify(pixelptr, numPixels);
							else gd.Paletteize(pixelptr, 0, 0, numPixels);

							//get a fake map, since mode7 doesnt really have a map
							map = gd.FetchMode7Tilemap();
						}
					}
					else
					{
						//handled = true;
						var dims = bg.ScreenSizeInPixels;
						dims.Height = dims.Width = Math.Max(dims.Width, dims.Height);
						allocate(dims.Width, dims.Height);
						numPixels = dims.Width * dims.Height;
						System.Diagnostics.Debug.Assert(stride / 4 == dims.Width);

						map = gd.FetchTilemap(bg.ScreenAddr, bg.ScreenSize);
						int paletteStart = 0;
						gd.DecodeBG(pixelptr, stride / 4, map, bg.TiledataAddr, bg.ScreenSize, bg.Bpp, bg.TileSize, paletteStart);
						gd.Paletteize(pixelptr, 0, 0, numPixels);
					}

					gd.Colorize(pixelptr, 0, numPixels);
				}
			}

			if (bmp != null)
			{
				bmp.UnlockBits(bmpdata);
				viewer.SetBitmap(bmp);
			}
		}

		enum eDisplayType
		{
			BG1 = 1, BG2 = 2, BG3 = 3, BG4 = 4, OBJTiles0, OBJTiles1, Tiles2bpp, Tiles4bpp, Tiles8bpp, TilesMode7, TilesMode7Ext, TilesMode7DC, Sprites, OBJ,
			BG1Screen = 101, BG2Screen = 102, BG3Screen = 103, BG4Screen = 104, 
		}
		static bool IsDisplayTypeBG(eDisplayType type) { return type == eDisplayType.BG1 || type == eDisplayType.BG2 || type == eDisplayType.BG3 || type == eDisplayType.BG4; }
		static bool IsDisplayTypeOBJ(eDisplayType type) { return type == eDisplayType.OBJTiles0 || type == eDisplayType.OBJTiles1; }
		static int DisplayTypeBGNum(eDisplayType type) { if(IsDisplayTypeBG(type)) return (int)type; else return -1; }
		static SNESGraphicsDecoder.BGMode BGModeForDisplayType(eDisplayType type)
		{
			switch (type)
			{
				case eDisplayType.Tiles2bpp: return SNESGraphicsDecoder.BGMode.Text;
				case eDisplayType.Tiles4bpp: return SNESGraphicsDecoder.BGMode.Text;
				case eDisplayType.Tiles8bpp: return SNESGraphicsDecoder.BGMode.Text;
				case eDisplayType.TilesMode7: return SNESGraphicsDecoder.BGMode.Mode7;
				case eDisplayType.TilesMode7Ext: return SNESGraphicsDecoder.BGMode.Mode7Ext;
				case eDisplayType.TilesMode7DC: return SNESGraphicsDecoder.BGMode.Mode7DC;
				case eDisplayType.OBJTiles0: return SNESGraphicsDecoder.BGMode.OBJ;
				case eDisplayType.OBJTiles1: return SNESGraphicsDecoder.BGMode.OBJ;
				default: throw new InvalidOperationException();
			}
		}
		
		class DisplayTypeItem
		{
			public eDisplayType Type { get; private set; }
			public string Descr { get; private set; }
			public DisplayTypeItem(string descr, eDisplayType type)
			{
				Type = type;
				Descr = descr;
			}
		}

		class PaletteTypeItem
		{
			public SnesColors.ColorType Type { get; private set; }
			public string Descr { get; private set; }
			public PaletteTypeItem(string descr, SnesColors.ColorType type)
			{
				Type = type;
				Descr = descr;
			}
		}

		private void comboDisplayType_SelectedIndexChanged(object sender, EventArgs e)
		{
			InternalUpdateValues();

			//change the bg props viewer to match
			if (IsDisplayTypeBG(CurrDisplaySelection))
				comboBGProps.SelectedIndex = DisplayTypeBGNum(CurrDisplaySelection) - 1;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void SNESGraphicsDebugger_Load(object sender, EventArgs e)
		{
			if (UserBackdropColor != -1)
			{
				pnBackdropColor.BackColor = Color.FromArgb(UserBackdropColor);
			}
			if (checkBackdropColor.Checked)
			{
				SyncBackdropColor();
			}

			UpdateToolsLoadstate();
		}

		bool suppression = false;
		private void rbBGX_CheckedChanged(object sender, EventArgs e)
		{
			if (suppression) return;
			//sync the comboBGProps dropdown with the result of this check
			suppression = true;
			if (rbBG1.Checked) comboBGProps.SelectedIndex = 0;
			if (rbBG2.Checked) comboBGProps.SelectedIndex = 1;
			if (rbBG3.Checked) comboBGProps.SelectedIndex = 2;
			if (rbBG4.Checked) comboBGProps.SelectedIndex = 3;
			suppression = false;
			InternalUpdateValues();
		}

		private void comboBGProps_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppression) return;

			//sync the radiobuttons with this selection
			suppression = true;
			if (comboBGProps.SelectedIndex == 0) rbBG1.Checked = true;
			if (comboBGProps.SelectedIndex == 1) rbBG2.Checked = true;
			if (comboBGProps.SelectedIndex == 2) rbBG3.Checked = true;
			if (comboBGProps.SelectedIndex == 3) rbBG4.Checked = true;
			suppression = false;
			InternalUpdateValues();
		}

		const int paletteCellSize = 16;
		const int paletteCellSpacing = 3;

		int[] lastPalette;
		int lastColorNum = 0;
		int selectedColorNum = 0;
		SNESGraphicsDecoder.PaletteSelection currPaletteSelection;

		Rectangle GetPaletteRegion(int start, int num)
		{
			var ret = new Rectangle();
			ret.X = start % 16;
			ret.Y = start / 16;
			ret.Width = num;
			ret.Height = num / 16;
			if (ret.Height == 0) ret.Height = 1;
			if (ret.Width > 16) ret.Width = 16;
			return ret;
		}

		Rectangle GetPaletteRegion(SNESGraphicsDecoder.PaletteSelection sel)
		{
			int start = sel.start, num = sel.size;
			return GetPaletteRegion(start, num);
		}

		void DrawPaletteRegion(Graphics g, Color color, Rectangle region)
		{
			int cellTotalSize = (paletteCellSize + paletteCellSpacing);

			int x = paletteCellSpacing + region.X * cellTotalSize - 2;
			int y = paletteCellSpacing + region.Y * cellTotalSize - 2;
			int width = cellTotalSize * region.Width;
			int height = cellTotalSize * region.Height;

			var rect = new Rectangle(x, y, width, height);
			using (var pen = new Pen(color))
				g.DrawRectangle(pen, rect);
		}

		//if a tile set is being displayed, this will adapt the user's color selection into a palette to be used for rendering the tiles
		SNESGraphicsDecoder.PaletteSelection GetPaletteSelectionForTileDisplay(int colorSelection)
		{
			int bpp = 0;
			var selection = CurrDisplaySelection;
			if (selection == eDisplayType.Tiles2bpp) bpp = 2;
			if (selection == eDisplayType.Tiles4bpp) bpp = 4;
			if (selection == eDisplayType.Tiles8bpp) bpp = 8;
			if (selection == eDisplayType.TilesMode7) bpp = 8;
			if (selection == eDisplayType.TilesMode7Ext) bpp = 7;
			if (selection == eDisplayType.OBJTiles0) bpp = 4;
			if (selection == eDisplayType.OBJTiles1) bpp = 4;

			SNESGraphicsDecoder.PaletteSelection ret = new SNESGraphicsDecoder.PaletteSelection();
			if(bpp == 0) return ret;

			//mode7 ext is fixed to use the top 128 colors
			if (bpp == 7)
			{
				ret.size = 128;
				ret.start = 0;
				return ret;
			}
			
			ret.size = 1 << bpp;
			ret.start = colorSelection & (~(ret.size - 1));
			return ret;
		}

		SNESGraphicsDecoder NewDecoder()
		{
			if (currentSnesCore != null)
				return new SNESGraphicsDecoder(currentSnesCore.Api, currentSnesCore.CurrPalette);
			else
				return null;
		}

		void RenderPalette()
		{
			//var gd = NewDecoder(); //??
			lastPalette = gd.GetPalette();

			int pixsize = paletteCellSize * 16 + paletteCellSpacing * 17;
			int cellTotalSize = (paletteCellSize + paletteCellSpacing);
			var bmp = new Bitmap(pixsize, pixsize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (var g = Graphics.FromImage(bmp))
			{
				for (int y = 0; y < 16; y++)
				{
					for (int x = 0; x < 16; x++)
					{
						int rgb555 = lastPalette[y * 16 + x];
						int color = gd.Colorize(rgb555);
						using (var brush = new SolidBrush(Color.FromArgb(color)))
						{
							g.FillRectangle(brush, new Rectangle(paletteCellSpacing + x * cellTotalSize, paletteCellSpacing + y * cellTotalSize, paletteCellSize, paletteCellSize));
						}
					}
				}

				//draw selection boxes:
				//first, draw the current selection
				var region = GetPaletteRegion(selectedColorNum, 1);
				DrawPaletteRegion(g, Color.Red, region);
				//next, draw the rectangle that advises you which colors could possibly be used for a bg
				if (IsDisplayTypeBG(CurrDisplaySelection))
				{
					var ps = si.BG[DisplayTypeBGNum(CurrDisplaySelection)].PaletteSelection;
					region = GetPaletteRegion(ps);
					DrawPaletteRegion(g, Color.FromArgb(192, 128, 255, 255), region);
				}
				if (IsDisplayTypeOBJ(CurrDisplaySelection))
				{
					var ps = new SNESGraphicsDecoder.PaletteSelection(128, 128);
					region = GetPaletteRegion(ps);
					DrawPaletteRegion(g, Color.FromArgb(192, 128, 255, 255), region);
				}
				//finally, draw the palette the user has chosen, in case he's viewing tiles
				if (currPaletteSelection.size != 0)
				{
					region = GetPaletteRegion(currPaletteSelection.start, currPaletteSelection.size);
					DrawPaletteRegion(g, Color.FromArgb(192,255,255,255), region);
				}
			}

			paletteViewer.SetBitmap(bmp);
		}

		static string BGModeShortName(SNESGraphicsDecoder.BGMode mode, int bpp)
		{
			if (mode == SNESGraphicsDecoder.BGMode.Unavailable) return "Unavailable";
			if (mode == SNESGraphicsDecoder.BGMode.Text) return string.Format("Text{0}bpp", bpp);
			if (mode == SNESGraphicsDecoder.BGMode.OBJ) return string.Format("OBJ", bpp);
			if (mode == SNESGraphicsDecoder.BGMode.Mode7) return "Mode7";
			if (mode == SNESGraphicsDecoder.BGMode.Mode7Ext) return "Mode7Ext";
			if (mode == SNESGraphicsDecoder.BGMode.Mode7DC) return "Mode7DC";
			throw new InvalidOperationException();
		}

		void UpdateOBJDetails()
		{
			if (currObjDataState == null) return;
			var oam = new SNESGraphicsDecoder.OAMInfo(gd, si, currObjDataState.Number);
			txtObjNumber.Text = string.Format("#${0:X2}", currObjDataState.Number);
			txtObjCoord.Text = string.Format("({0}, {1})",oam.X,oam.Y);
			cbObjHFlip.Checked = oam.HFlip;
			cbObjVFlip.Checked = oam.VFlip;
			cbObjLarge.Checked = oam.Size == 1;
			txtObjSize.Text = SNESGraphicsDecoder.ObjSizes[si.OBSEL_Size, oam.Size].ToString();
			txtObjPriority.Text = oam.Priority.ToString();
			txtObjPalette.Text = oam.Palette.ToString();
			txtObjPaletteMemo.Text = string.Format("${0:X2}", oam.Palette * 16 + 128);
			txtObjName.Text = string.Format("#${0:X3}", oam.Tile);
			txtObjNameAddr.Text = string.Format("@{0:X4}", oam.Address);
		}

		void UpdateTileDetails()
		{
			if (currTileDataState == null) return;
			var mode = BGModeForDisplayType(currTileDataState.Type);
			int bpp = currTileDataState.Bpp;
			txtTileMode.Text = BGModeShortName(mode, bpp);
			txtTileBpp.Text = currTileDataState.Bpp.ToString();
			txtTileColors.Text = (1 << currTileDataState.Bpp).ToString();
			txtTileNumber.Text = string.Format("#${0:X3}", currTileDataState.Tile);
			txtTileAddress.Text = string.Format("@{0:X4}", currTileDataState.Address);
			txtTilePalette.Text = string.Format("#{0:X2}", currTileDataState.Palette);
		}

		void UpdateMapEntryDetails()
		{
			if (currMapEntryState == null) return;
			txtMapEntryLocation.Text = string.Format("({0},{1}), @{2:X4}", currMapEntryState.Location.X, currMapEntryState.Location.Y, currMapEntryState.entry.address);
			txtMapEntryTileNum.Text = string.Format("${0:X3}", currMapEntryState.entry.tilenum);
			txtMapEntryPrio.Text = string.Format("{0}", (currMapEntryState.entry.flags & SNESGraphicsDecoder.TileEntryFlags.Priority)!=0?1:0);
			txtMapEntryPalette.Text = string.Format("{0}", currMapEntryState.entry.palette);
			checkMapEntryHFlip.Checked = (currMapEntryState.entry.flags & SNESGraphicsDecoder.TileEntryFlags.Horz) != 0;
			checkMapEntryVFlip.Checked = (currMapEntryState.entry.flags & SNESGraphicsDecoder.TileEntryFlags.Vert) != 0;

			//calculate address of tile
			var bg = si.BG[currMapEntryState.bgnum];
			int bpp = bg.Bpp;
			int tiledataBaseAddr = bg.TiledataAddr;
			int tileSizeBytes = 8 * bpp;
			int baseTileNum = tiledataBaseAddr / tileSizeBytes;
			int tileNum = baseTileNum + currMapEntryState.entry.tilenum;
			int addr = tileNum * tileSizeBytes;

			//mode7 takes up 128 bytes per tile because its interleaved with the screen data
			if (bg.BGModeIsMode7Type)
				addr *= 2;

			addr &= 0xFFFF;
			txtMapEntryTileAddr.Text = "@" + addr.ToHexString(4);
		}

		void UpdateColorDetails()
		{
			if (lastPalette == null) return;

			int rgb555 = lastPalette[lastColorNum];
			//var gd = NewDecoder(); //??
			int color = gd.Colorize(rgb555);
			pnDetailsPaletteColor.BackColor = Color.FromArgb(color);

			txtDetailsPaletteColor.Text = string.Format("${0:X4}", rgb555);
			txtDetailsPaletteColorHex.Text = string.Format("#{0:X6}", color & 0xFFFFFF);
			txtDetailsPaletteColorRGB.Text = string.Format("({0},{1},{2})", (color >> 16) & 0xFF, (color >> 8) & 0xFF, (color & 0xFF));

			txtPaletteDetailsIndexHex.Text = string.Format("${0:X2}", lastColorNum);
			txtPaletteDetailsIndex.Text = string.Format("{0}", lastColorNum);

			//not being used anymore
			//if (lastColorNum < 128) lblDetailsOBJOrBG.Text = "(BG:)"; else lblDetailsOBJOrBG.Text = "(OBJ:)";
			//txtPaletteDetailsIndexHexSpecific.Text = string.Format("${0:X2}", lastColorNum & 0x7F);
			//txtPaletteDetailsIndexSpecific.Text = string.Format("{0}", lastColorNum & 0x7F);

			txtPaletteDetailsAddress.Text = string.Format("${0:X3}", lastColorNum * 2);
		}


		bool TranslatePaletteCoord(Point pt, out Point outpoint)
		{
			pt.X -= paletteCellSpacing;
			pt.Y -= paletteCellSpacing;
			int tx = pt.X / (paletteCellSize + paletteCellSpacing);
			int ty = pt.Y / (paletteCellSize + paletteCellSpacing);
			outpoint = new Point(tx, ty);
			if (tx >= 16 || ty >= 16) return false;
			return true;
		}

		private void paletteViewer_MouseClick(object sender, MouseEventArgs e)
		{
			Point pt;
			bool valid = TranslatePaletteCoord(e.Location, out pt);
			if (!valid) return;
			selectedColorNum = pt.Y * 16 + pt.X;

			if (currTileDataState != null)
			{
				currTileDataState.Palette = currPaletteSelection.start;
			}

			SyncColorSelection();
			InternalUpdateValues();
		}

		void SyncColorSelection()
		{
			currPaletteSelection = GetPaletteSelectionForTileDisplay(selectedColorNum);
		}

		private void pnDetailsPaletteColor_DoubleClick(object sender, EventArgs e)
		{
			//not workign real well...
			//var cd = new ColorDialog();
			//cd.Color = pnDetailsPaletteColor.BackColor;
			//cd.ShowDialog(this);
		}

		private void rbQuad_CheckedChanged(object sender, EventArgs e)
		{
			SyncViewerSize();
		}

		void SyncViewerSize()
		{
			if (check2x.Checked)

				viewer.Size = new Size(1024, 1024);
			else
				viewer.Size = new Size(512, 512);
		}

		private void checkScanlineControl_CheckedChanged(object sender, EventArgs e)
		{
			SyncCore();
		}

		private void check2x_CheckedChanged(object sender, EventArgs e)
		{
			SyncViewerSize();
		}

		bool viewerPan = false;
		Point panStartLocation;
		private void viewer_MouseDown(object sender, MouseEventArgs e)
		{
			viewer.Capture = true;
			if ((e.Button & MouseButtons.Middle) != 0)
			{
				viewerPan = true;
				panStartLocation = viewer.PointToScreen(e.Location);
				Cursor = Cursors.SizeAll;
			}

			if ((e.Button & MouseButtons.Right) != 0)
				Freeze();
		}

		void Freeze()
		{
			groupFreeze.SuspendLayout();

			Win32.SendMessage(groupFreeze.Handle, 11, (IntPtr)0, IntPtr.Zero); //WM_SETREDRAW false

			var tp = tabctrlDetails.SelectedTab;

			//clone the currently selected tab page into the destination
			var oldControls = new ArrayList(pnGroupFreeze.Controls);
			pnGroupFreeze.Controls.Clear();
			foreach (var control in tp.Controls)
				pnGroupFreeze.Controls.Add((control as Control).Clone());
			foreach (var control in oldControls)
				(control as Control).Dispose();

			//set the freeze caption accordingly
			if (tp == tpMapEntry) groupFreeze.Text = "Freeze - Map Entry";
			if (tp == tpPalette) groupFreeze.Text = "Freeze - Color";
			if (tp == tpTile) groupFreeze.Text = "Freeze - Tile";
			if (tp == tpOBJ) groupFreeze.Text = "Freeze - OBJ";
			
			groupFreeze.ResumeLayout();

			Win32.SendMessage(groupFreeze.Handle, 11, (IntPtr)1, IntPtr.Zero); //WM_SETREDRAW true
			groupFreeze.Refresh();
		}

		enum eFreezeTarget
		{
			MainViewer
		}

		private void viewer_MouseUp(object sender, MouseEventArgs e)
		{
			viewerPan = false;
			viewer.Capture = false;
			Cursor = Cursors.Default;
		}

		private void viewer_MouseMove(object sender, MouseEventArgs e)
		{
			if (viewerPan)
			{
				var loc = viewer.PointToScreen(e.Location);
				int dx = loc.X - panStartLocation.X;
				int dy = loc.Y - panStartLocation.Y;
				panStartLocation = loc;

				int x = viewerPanel.AutoScrollPosition.X;
				int y = viewerPanel.AutoScrollPosition.Y;
				x += dx;
				y += dy;
				viewerPanel.AutoScrollPosition = new Point(-x, -y);
			}
			else
			{
				if(si != null)
					UpdateViewerMouseover(e.Location);
			}
		}

		class MapEntryState
		{
			public SNESGraphicsDecoder.TileEntry entry;
			public int bgnum;
			public Point Location;
		}
		MapEntryState currMapEntryState;

		class TileDataState
		{
			public eDisplayType Type;
			public int Bpp;
			public int Tile;
			public int Address;
			public int Palette;
		}
		TileDataState currTileDataState;

		class ObjDataState
		{
			public int Number;
		}
		ObjDataState currObjDataState;

		void RenderTileView()
		{
			if (currMapEntryState != null)
			{
				//view a BG tile 
				int paletteStart = 0;
				var bgs = currMapEntryState;
				var oneTileEntry = new SNESGraphicsDecoder.TileEntry[] { bgs.entry };
				int tileSize = si.BG[bgs.bgnum].TileSize;
				int pixels = tileSize * tileSize;

				var bmp = new Bitmap(tileSize, tileSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				var bmpdata = bmp.LockBits(new Rectangle(0, 0, tileSize, tileSize), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				if (viewBgMode == SNESGraphicsDecoder.BGMode.Mode7)
					gd.RenderMode7TilesToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, false, false, 1, currMapEntryState.entry.tilenum, 1);
				else if (viewBgMode == SNESGraphicsDecoder.BGMode.Mode7Ext)
					gd.RenderMode7TilesToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, true, false, 1, currMapEntryState.entry.tilenum, 1);
				else if (viewBgMode == SNESGraphicsDecoder.BGMode.Mode7DC)
					gd.RenderMode7TilesToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, false, true, 1, currMapEntryState.entry.tilenum, 1);
				else
				{
					gd.DecodeBG((int*)bmpdata.Scan0, bmpdata.Stride / 4, oneTileEntry, si.BG[bgs.bgnum].TiledataAddr, SNESGraphicsDecoder.ScreenSize.Hacky_1x1, si.BG[bgs.bgnum].Bpp, tileSize, paletteStart);
					gd.Paletteize((int*)bmpdata.Scan0, 0, 0, pixels);
					gd.Colorize((int*)bmpdata.Scan0, 0, pixels);
				}

				bmp.UnlockBits(bmpdata);
				viewerMapEntryTile.SetBitmap(bmp);
			}
			else if (currTileDataState != null)
			{
				//view a tileset tile
				int bpp = currTileDataState.Bpp;

				var bmp = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				var bmpdata = bmp.LockBits(new Rectangle(0, 0, 8, 8), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				if (currTileDataState.Type == eDisplayType.TilesMode7)
					gd.RenderMode7TilesToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, false, false, 1, currTileDataState.Tile, 1);
				else if (currTileDataState.Type == eDisplayType.TilesMode7Ext)
					gd.RenderMode7TilesToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, true, false, 1, currTileDataState.Tile, 1);
				else if (currTileDataState.Type == eDisplayType.TilesMode7DC)
					gd.RenderMode7TilesToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, false, true, 1, currTileDataState.Tile, 1);
				else if (currTileDataState.Type == eDisplayType.OBJTiles0 || currTileDataState.Type == eDisplayType.OBJTiles1)
				{ 
					//render an obj tile
					int tile = currTileDataState.Address / 32;
					gd.RenderTilesToScreen((int*)bmpdata.Scan0, 1, 1, bmpdata.Stride / 4, bpp, currPaletteSelection.start, tile, 1);
				}
				else gd.RenderTilesToScreen((int*)bmpdata.Scan0, 1, 1, bmpdata.Stride / 4, bpp, currPaletteSelection.start, currTileDataState.Tile, 1);

				bmp.UnlockBits(bmpdata);
				viewerTile.SetBitmap(bmp);
			}
			else if (currObjDataState != null)
			{
				var bounds = si.ObjSizeBoundsSquare;
				int width = bounds.Width;
				int height = bounds.Height;
				var bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				var bmpdata = bmp.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				gd.RenderSpriteToScreen((int*)bmpdata.Scan0, bmpdata.Stride / 4, 0, 0, si, currObjDataState.Number);
				bmp.UnlockBits(bmpdata);
				viewerObj.SetBitmap(bmp);
			}
			else
			{
				var bmp = new Bitmap(8, 8, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				viewerTile.SetBitmap(bmp);
			}
		}

		void HandleTileViewMouseOver(int pxacross, int pxtall, int bpp, int tx, int ty)
		{
			int tilestride = pxacross / 8;
			int tilesTall = pxtall / 8;
			if (tx < 0 || ty < 0 || tx >= tilestride || ty >= tilesTall)
				return;
			int tilenum = ty * tilestride + tx;
			currTileDataState = new TileDataState();
			currTileDataState.Bpp = bpp;
			currTileDataState.Type = CurrDisplaySelection;
			currTileDataState.Tile = tilenum;
			currTileDataState.Address = (bpp==7?8:bpp) * 8 * currTileDataState.Tile;
			currTileDataState.Palette = currPaletteSelection.start;
			if (CurrDisplaySelection == eDisplayType.OBJTiles0 || CurrDisplaySelection == eDisplayType.OBJTiles1)
			{
				if (tilenum < 256)
					currTileDataState.Address += si.OBJTable0Addr;
				else
					currTileDataState.Address += si.OBJTable1Addr - (256*32);

				currTileDataState.Address &= 0xFFFF;
			}
			else if (SNESGraphicsDecoder.BGModeIsMode7Type(BGModeForDisplayType(CurrDisplaySelection)))
			{
				currTileDataState.Address *= 2;
			}

			SetTab(tpTile);
		}

		void HandleSpriteMouseOver(int px, int py)
		{
			if (px < 0 || py < 0 || px >= 256 || py >= 224) return;

			int sprite = spriteMap[px,py];
			if(sprite == 0xFF) return;

			currObjDataState = new ObjDataState();
			currObjDataState.Number = sprite;

			SetTab(tpOBJ);
		}

		void HandleObjMouseOver(int px, int py)
		{
			int ox = px / si.ObjSizeBounds.Width;
			int oy = py / si.ObjSizeBounds.Height;
			
			if (ox < 0 || oy < 0 || ox >= 8 || oy >= 16)
				return;

			int objNum = oy * 8 + ox;

			currObjDataState = new ObjDataState();
			currObjDataState.Number = objNum;
			
			//RenderView(); //remember, we were going to highlight the selected sprite somehow as we hover over it
			SetTab(tpOBJ);
		}

		void SetTab(TabPage tpSet)
		{
			//doesnt work well
			//foreach (var tp in tabctrlDetails.TabPages)
			//  ((TabPage)tp).Visible = tpSet != null;
			if (tpSet != null)
			{
				tpSet.Visible = true;
				tabctrlDetails.SelectedTab = tpSet;
			}
		}

		void UpdateViewerMouseover(Point loc)
		{
			using (gd.EnterExit())
			{
				currMapEntryState = null;
				currTileDataState = null;
				currObjDataState = null;

				int tx = loc.X / 8;
				int ty = loc.Y / 8;

				switch (CurrDisplaySelection)
				{
					case eDisplayType.OBJTiles0:
					case eDisplayType.OBJTiles1:
						HandleTileViewMouseOver(128, 256, 4, tx, ty);
						break;
					case eDisplayType.OBJ:
						HandleObjMouseOver(loc.X, loc.Y);
						break;
					case eDisplayType.Sprites:
						HandleSpriteMouseOver(loc.X, loc.Y);
						break;
					case eDisplayType.Tiles2bpp:
						HandleTileViewMouseOver(512, 512, 2, tx, ty);
						break;
					case eDisplayType.Tiles4bpp:
						HandleTileViewMouseOver(512, 256, 4, tx, ty);
						break;
					case eDisplayType.Tiles8bpp:
						HandleTileViewMouseOver(256, 256, 8, tx, ty);
						break;
					case eDisplayType.TilesMode7:
					case eDisplayType.TilesMode7DC:
						HandleTileViewMouseOver(128, 128, 8, tx, ty);
						break;
					case eDisplayType.TilesMode7Ext:
						HandleTileViewMouseOver(128, 128, 7, tx, ty);
						break;
					case eDisplayType.BG1:
					case eDisplayType.BG2:
					case eDisplayType.BG3:
					case eDisplayType.BG4:
						{
							var bg = si.BG[(int)CurrDisplaySelection];

							//unavailable BG for this mode
							if (bg.Bpp == 0)
								break;

							if (bg.TileSize == 16) { tx /= 2; ty /= 2; } //worry about this later. need to pass a different flag into `currViewingTile`

							int tloc = ty * bg.ScreenSizeInTiles.Width + tx;
							if (tx >= bg.ScreenSizeInTiles.Width) break;
							if (ty >= bg.ScreenSizeInTiles.Height) break;
							if (tx < 0) break;
							if (ty < 0) break;

							currMapEntryState = new MapEntryState();
							currMapEntryState.bgnum = (int)CurrDisplaySelection;
							currMapEntryState.entry = map[tloc];
							currMapEntryState.Location = new Point(tx, ty);

							//public void DecodeBG(int* screen, int stride, TileEntry[] map, int tiledataBaseAddr, ScreenSize size, int bpp, int tilesize, int paletteStart)


							//var map = gd.FetchTilemap(bg.ScreenAddr, bg.ScreenSize);
							//int paletteStart = 0;
							//gd.DecodeBG(pixelptr, stride / 4, map, bg.TiledataAddr, bg.ScreenSize, bg.Bpp, bg.TileSize, paletteStart);
							//gd.Paletteize(pixelptr, 0, 0, numPixels);

							SetTab(tpMapEntry);
						}
						break;
				}

				RenderTileView();
				UpdateMapEntryDetails();
				UpdateTileDetails();
				UpdateOBJDetails();
			}
		}

		private void viewer_MouseLeave(object sender, EventArgs e)
		{
			SetTab(null);
		}

		private void paletteViewer_MouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Right) != 0)
				Freeze();
		}

		private void paletteViewer_MouseEnter(object sender, EventArgs e)
		{
			tabctrlDetails.SelectedIndex = 0;
		}

		private void paletteViewer_MouseLeave(object sender, EventArgs e)
		{
			SetTab(null);
		}

		private void paletteViewer_MouseMove(object sender, MouseEventArgs e)
		{
			Point pt;
			bool valid = TranslatePaletteCoord(e.Location, out pt);
			if (!valid) return;
			lastColorNum = pt.Y * 16 + pt.X;
			UpdateColorDetails();
			SetTab(tpPalette);
		}

		static int DecodeWinformsColorToSNES(Color winforms)
		{
			int r = winforms.R;
			int g = winforms.G;
			int b = winforms.B;
			r >>= 3;
			g >>= 3;
			b >>= 3;
			int col = r | (g << 5) | (b << 10);
			return col;
		}

		void SyncBackdropColor()
		{
			//TODO
			//if (checkBackdropColor.Checked)
			//{
			//  int col = DecodeWinformsColorToSNES(pnBackdropColor.BackColor);
			//  LibsnesDll.snes_set_backdropColor(col);
			//}
			//else
			//{
			//  LibsnesDll.snes_set_backdropColor(-1);
			//}
		}

		private void checkBackdropColor_CheckedChanged(object sender, EventArgs e)
		{
			SyncBackdropColor();
			RegenerateData();
		}

		private void pnBackdropColor_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var cd = new ColorDialog();
			cd.Color = pnBackdropColor.BackColor;
			if (cd.ShowDialog(this) == DialogResult.OK)
			{
				pnBackdropColor.BackColor = cd.Color;
				UserBackdropColor = pnBackdropColor.BackColor.ToArgb();
				SyncBackdropColor();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == (Keys.C | Keys.Control))
			{
				// find the control under the mouse
				Point m = Cursor.Position;
				Control top = this;
				Control found = null;
				do
				{
					found = top.GetChildAtPoint(top.PointToClient(m), GetChildAtPointSkip.Invisible);
					top = found;
				} while (found != null && found.HasChildren);

				if (found != null && found is SNESGraphicsViewer)
				{
					var v = found as SNESGraphicsViewer;
					lock (v)
					{
						var bmp = v.GetBitmap();
						Clipboard.SetImage(bmp);
					}
					string label = "";
					if (found.Name == "viewer")
						label = displayTypeItems.Find((x) => x.Type == CurrDisplaySelection).Descr;
					if (found.Name == "viewerTile")
						label = "Tile";
					if (found.Name == "viewerMapEntryTile")
						label = "Map Entry";
					if (found.Name == "paletteViewer")
						label = "Palette";
					labelClipboard.Text = label + " copied to clipboard.";
					messagetimer.Stop();
					messagetimer.Start();

					return true;
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}


		private void messagetimer_Tick(object sender, EventArgs e)
		{
			messagetimer.Stop();
			labelClipboard.Text = "CTRL+C copies the pane under the mouse.";
		}

		private void comboPalette_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (suppression) return;
			var pal = (SnesColors.ColorType)comboPalette.SelectedValue;
			Console.WriteLine("set {0}", pal);
			var s = Emulator.GetSettings();
			s.Palette = pal.ToString();
			if (currentSnesCore != null)
			{
				currentSnesCore.PutSettings(s);
			}
			RegenerateData();
			RenderView();
			RenderPalette();
			RenderTileView();
		}

		void RefreshBGENCheckStatesFromConfig()
		{
			var s = Emulator.GetSettings();
			checkEN0_BG1.Checked = s.ShowBG1_0;
			checkEN0_BG2.Checked = s.ShowBG2_0;
			checkEN0_BG3.Checked = s.ShowBG3_0;
			checkEN0_BG4.Checked = s.ShowBG4_0;
			checkEN1_BG1.Checked = s.ShowBG1_1;
			checkEN1_BG2.Checked = s.ShowBG2_1;
			checkEN1_BG3.Checked = s.ShowBG3_1;
			checkEN1_BG4.Checked = s.ShowBG4_1;
			checkEN0_OBJ.Checked = s.ShowOBJ_0;
			checkEN1_OBJ.Checked = s.ShowOBJ_1;
			checkEN2_OBJ.Checked = s.ShowOBJ_2;
			checkEN3_OBJ.Checked = s.ShowOBJ_3;
		}

		private void checkEN_CheckedChanged(object sender, EventArgs e)
		{
			if(suppression) return;
			var snes = Emulator;
			var s = snes.GetSettings();
			if (sender == checkEN0_BG1) s.ShowBG1_0 = checkEN0_BG1.Checked;
			if (sender == checkEN0_BG2) s.ShowBG2_0 = checkEN0_BG2.Checked;
			if (sender == checkEN0_BG3) s.ShowBG3_0 = checkEN0_BG3.Checked;
			if (sender == checkEN0_BG4) s.ShowBG4_0 = checkEN0_BG4.Checked;
			if (sender == checkEN1_BG1) s.ShowBG1_1 = checkEN1_BG1.Checked;
			if (sender == checkEN1_BG2) s.ShowBG2_1 = checkEN1_BG2.Checked;
			if (sender == checkEN1_BG3) s.ShowBG3_1 = checkEN1_BG3.Checked;
			if (sender == checkEN1_BG4) s.ShowBG4_1 = checkEN1_BG4.Checked;
			if (sender == checkEN0_OBJ) s.ShowOBJ_0 = checkEN0_OBJ.Checked;
			if (sender == checkEN1_OBJ) s.ShowOBJ_1 = checkEN1_OBJ.Checked;
			if (sender == checkEN2_OBJ) s.ShowOBJ_2 = checkEN2_OBJ.Checked;
			if (sender == checkEN3_OBJ) s.ShowOBJ_3 = checkEN3_OBJ.Checked;

			if ((ModifierKeys & Keys.Shift) != 0)
			{
				if (sender == checkEN0_BG1) s.ShowBG1_1 = s.ShowBG1_0;
				if (sender == checkEN1_BG1) s.ShowBG1_0 = s.ShowBG1_1;
				if (sender == checkEN0_BG2) s.ShowBG2_1 = s.ShowBG2_0;
				if (sender == checkEN1_BG2) s.ShowBG2_0 = s.ShowBG2_1;
				if (sender == checkEN0_BG3) s.ShowBG3_1 = s.ShowBG3_0;
				if (sender == checkEN1_BG3) s.ShowBG3_0 = s.ShowBG3_1;
				if (sender == checkEN0_BG4) s.ShowBG4_1 = s.ShowBG4_0;
				if (sender == checkEN1_BG4) s.ShowBG4_0 = s.ShowBG4_1;
				if (sender == checkEN0_OBJ) s.ShowOBJ_1 = s.ShowOBJ_2 = s.ShowOBJ_3 = s.ShowOBJ_0;
				if (sender == checkEN1_OBJ) s.ShowOBJ_0 = s.ShowOBJ_2 = s.ShowOBJ_3 = s.ShowOBJ_1;
				if (sender == checkEN2_OBJ) s.ShowOBJ_0 = s.ShowOBJ_1 = s.ShowOBJ_3 = s.ShowOBJ_2;
				if (sender == checkEN3_OBJ) s.ShowOBJ_0 = s.ShowOBJ_1 = s.ShowOBJ_2 = s.ShowOBJ_3;
				suppression = true;
				RefreshBGENCheckStatesFromConfig();
				suppression = false;
			}
			snes.PutSettings(s);
		}

		private void lblEnPrio0_Click(object sender, EventArgs e)
		{
			bool any = checkEN0_OBJ.Checked || checkEN0_BG1.Checked || checkEN0_BG2.Checked || checkEN0_BG3.Checked || checkEN0_BG4.Checked;
			bool all = checkEN0_OBJ.Checked && checkEN0_BG1.Checked && checkEN0_BG2.Checked && checkEN0_BG3.Checked && checkEN0_BG4.Checked;
			bool newval;
			if (all) newval = false;
			else newval = true;
			checkEN0_OBJ.Checked = checkEN0_BG1.Checked = checkEN0_BG2.Checked = checkEN0_BG3.Checked = checkEN0_BG4.Checked = newval;

		}

		private void lblEnPrio1_Click(object sender, EventArgs e)
		{
			bool any = checkEN1_OBJ.Checked || checkEN1_BG1.Checked || checkEN1_BG2.Checked || checkEN1_BG3.Checked || checkEN1_BG4.Checked;
			bool all = checkEN1_OBJ.Checked && checkEN1_BG1.Checked && checkEN1_BG2.Checked && checkEN1_BG3.Checked && checkEN1_BG4.Checked;
			bool newval;
			if (all) newval = false;
			else newval = true;
			checkEN1_OBJ.Checked = checkEN1_BG1.Checked = checkEN1_BG2.Checked = checkEN1_BG3.Checked = checkEN1_BG4.Checked = newval;

		}

		private void lblEnPrio2_Click(object sender, EventArgs e)
		{
			checkEN2_OBJ.Checked ^= true;
		}

		private void lblEnPrio3_Click(object sender, EventArgs e)
		{
			checkEN3_OBJ.Checked ^= true;
		}

	} //class SNESGraphicsDebugger 

	static class ControlExtensions
	{
		static string[] secondPass = new[] { "Size" };
		public static T Clone<T>(this T controlToClone)
				where T : Control
		{
			PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			Type t = controlToClone.GetType();
			T instance = Activator.CreateInstance(t) as T;

			t.GetProperty("AutoSize").SetValue(instance, false, null);

			for (int i = 0; i < 3; i++)
			{
				foreach (PropertyInfo propInfo in controlProperties)
				{
					if (!propInfo.CanWrite)
						continue;

					if (propInfo.Name == "AutoSize")
					{ }
					else if (propInfo.Name == "WindowTarget")
					{ }
					else
						propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
				}
			}

			if (instance is RetainedViewportPanel)
			{
				var clonebmp = ((controlToClone) as RetainedViewportPanel).GetBitmap().Clone() as Bitmap;
				((instance) as RetainedViewportPanel).SetBitmap(clonebmp);
			}

			return instance;
		}
	}
} //namespace BizHawk.Client.EmuHawk
